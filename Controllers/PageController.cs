using AsynchronousProgramming.Infrastructure.Repositories.Interfaces;
using AsynchronousProgramming.Models.DTOs;
using AsynchronousProgramming.Models.Entities.Concrete;
using AsynchronousProgramming.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AsynchronousProgramming.Models.Entities.Abstract;
using System.Linq;

namespace AsynchronousProgramming.Controllers
{
    public class PageController : Controller
    {
        private readonly IBaseRepository<Page> _repository;
        private readonly IMapper _mapper;

        public PageController(IBaseRepository<Page> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreatePageDTO model)
        {
            if (ModelState.IsValid) //Model içerisindeki üyelere koyunlan kurallara uyuldu mu?
            {
                //Model'den gelen slug veri tabanında var mı yok mu diye baktık
                var slug = await _repository.GetByDefault(x => x.Slug == model.Slug);
                if (slug != null) //slug null değilse veri tabanında böyle bir slug var demektir. o halde ekleme işlemi gerçekleşmemeli, şayet ekleme gerçekleşirse birden fazla aynı varlıktan oluşur
                {
                    ModelState.AddModelError("", "The page is already exist...!");
                    TempData["Warning"] = "The page is already exist..!"; //Views => Shared => _NotificationPartial.cshtml eklenir.
                    return View(model);
                }
                else
                {
                    //Veri tabanındaki page tablosuna sadece "page" tipinde veri ekleyebiliriz. Bu action methoda gelen veririnin tipini "CreatePageDTO" olduğundan direk veri tabanındaki tabloya ekleyemeyiz. Bu yüzden DTO'dan gelen veriyi AutoMapper 3rd aracı ile Page varlığını üyelerini eşliyoruz.
                    var page = _mapper.Map<Page>(model);

                    //Kullanıcıdan gelen data model ile buraya taşında ve Page tipindeki page objesine dolduruldui artık veri tabanına ekleyebiliri.
                    await _repository.Add(page);
                    TempData["Success"] = "The page has been created....!";
                    return RedirectToAction("List");
                }
            }
            else
            {
                TempData["Error"] = "The page hasn't been created..!";
                return View(model);
            }
        }

        public async Task<IActionResult> List()
        {
            var pages = await _repository.GetFilteredList(
                select: x => new PageVm
                {
                    Id = x.Id,
                    Content = x.Content,
                    Slug = x.Slug,
                    Status = x.Status,
                },
                where: x => x.Status != Status.Passive,
                orderBy: x => x.OrderBy(z => z.Id));

            return View(pages);

            //var page = _repository.Where(x => x.Status != Models.Entities.Abstract.Status.Passive);
            //PageVm pageVm = new PageVm();
            //pageVm.Pages.AddRange(page);
            //return View(pageVm);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Page page = await _repository.GetById(id);
            var model = _mapper.Map<UpdatePageDTO>(page);
            return View(model);
        }
       
        [HttpPost]
        public async Task<IActionResult> Edit(UpdatePageDTO model)
        {
            if (ModelState.IsValid)
            {
                var slug = await _repository.GetByDefault(x => x.Slug == model.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError(string.Empty, "The page is already exist.!!");
                    TempData["Warning"] = "The page is already exist..!";
                    return View(model);
                }
                else
                {
                    var page = _mapper.Map<Page>(model);
                    await _repository.Update(page);
                    TempData["Success"] = "The page has been updated..!";
                    return RedirectToAction("List");
                }
            }
            else
            {
                TempData["Error"] = "The page hasn't been updated..!";
                return View(model);
            }
        }

        public async Task<IActionResult> Remove(int id)
        {
            Page page = await _repository.GetById(id);
            if (page != null)
            {
                await _repository.Delete(page);
                TempData["Success"] = "The page has been removed..!";
                return RedirectToAction("List");
            }
            else
            {
                TempData["Error"] = "The page hasn't been removed..!";
                return RedirectToAction("List");
            }
        }

        public async Task<IActionResult> Page(string slug)
        {
            if (slug == null)
            {
                //var a = await _repository.GetByDefault(x => x.Slug == "home");
                return RedirectToAction("List");//parametreden gelen slug boş ise home sayfasını aç
            }

            var page = await _repository.GetByDefault(x => x.Slug == slug);//parametrenden gelen slug ne ise o sluga ait sayfası ilgili değişkene atadık.

            if (page == null) return NotFound(); // yukarıdaki linq tu null dönerse bulunamadı sayfası burada client'ta dönülür.

            return View(page);//slug vasıtasıyla yakalanan sayfa burada client'a dönülür
        }
    }
}
