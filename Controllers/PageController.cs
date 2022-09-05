using AsynchronousProgramming.Infrastructure.Repositories.Interfaces;
using AsynchronousProgramming.Models.DTOs;
using AsynchronousProgramming.Models.Entities.Concrete;
using AsynchronousProgramming.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        public  IActionResult List()
        {
            var page = _repository.Where(x => x.Status != Models.Entities.Abstract.Status.Passive);
            PageVm pageVm = new PageVm();
            pageVm.Pages.AddRange(page);
            return View(pageVm);
        }

    }
}
