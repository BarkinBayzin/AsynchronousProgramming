using AsynchronousProgramming.Infrastructure.Repositories.Interfaces;
using AsynchronousProgramming.Models.DTOs;
using AsynchronousProgramming.Models.Entities.Abstract;
using AsynchronousProgramming.Models.Entities.Concrete;
using AsynchronousProgramming.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsynchronousProgramming.Controllers
{
    public class ProductController : Controller
    {
        private readonly IBaseRepository<Product> _repository;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _webHostEnvironment;

        public ProductController(IBaseRepository<Product> repository, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _repository.GetByDefaults(x => x.Status != Status.Passive), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDTO model)
        {
            if (ModelState.IsValid)
            {
                string imageName = "noimage.png";
                if (model.UploadImage != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    imageName = $"{Guid.NewGuid()}_{model.UploadImage.FileName}";
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    await model.UploadImage.CopyToAsync(fileStream);
                    fileStream.Close();
                }
                Product product = _mapper.Map<Product>(model);
                product.Image = imageName;
                await _repository.Add(product);
                TempData["Success"] = "The product has been created..!";
                return RedirectToAction("List");
            }
            else
            {
                TempData["Error"] = "The product hasn't been created..!";
                return View();
            }
        }

        public async Task<IActionResult> List()
        {
            var products = await _repository.GetFilteredList(
                select: x => new ProductVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Image = x.Image,
                    UnitPrice = x.UnitPrice,
                    CategoryName = x.Category.Name,
                    Status = x.Status
                },
                where: x => x.Status != Status.Passive,
                orderBy: x => x.OrderByDescending(z => z.CreateDate),
                join: x => x.Include(z => z.Category));
            return View(products);
        }
    }
}
