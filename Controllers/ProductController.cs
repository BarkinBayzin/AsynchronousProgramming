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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsynchronousProgramming.Controllers
{
    //[Route("Product")]
    public class ProductController : Controller
    {
        private readonly IBaseRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Category> _categoryRepository;
        private IWebHostEnvironment _webHostEnvironment;

        public ProductController(IBaseRepository<Product> repository, IMapper mapper, IWebHostEnvironment webHostEnvironment, IBaseRepository<Category> catRepo)
        {
            _productRepository = repository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _categoryRepository = catRepo;
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _categoryRepository.GetByDefaults(x => x.Status != Status.Passive), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDTO model)
        {
            bool format = false;
            if (ModelState.IsValid)
            {
                string imageName = "noimage.png";
                if (model.UploadImage != null)
                {
                    var list = model.UploadImage.FileName.Split('.');

                    if (list[list.Length - 1] == "jpeg" || list[list.Length - 1] == "jpg" || list[list.Length - 1] == "png" || list[list.Length - 1] == "img")
                    {
                        format = true;
                        string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                        imageName = $"{Guid.NewGuid()}_{model.UploadImage.FileName}";
                        string filePath = Path.Combine(uploadDir, imageName);
                        FileStream fileStream = new FileStream(filePath, FileMode.Create);
                        await model.UploadImage.CopyToAsync(fileStream);
                        fileStream.Close();
                    }
                    else
                        ModelState.AddModelError(String.Empty, "Not supported Type !!");
                }
                if (format)
                {
                    Product product = _mapper.Map<Product>(model);
                    product.Image = imageName;
                    await _productRepository.Add(product);
                    TempData["Success"] = "The product has been created..!";
                    return RedirectToAction("List");
                }
                TempData["Error"] = "Not supported Type !";
                return RedirectToAction();
            }
            else
            {
                TempData["Error"] = "The product hasn't been created..!";
                return RedirectToAction();
            }
        }
        //[Route("List")]
        public async Task<IActionResult> List()
        {
            var products = await _productRepository.GetFilteredList(
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

        //[Route("Product/Edit/{id}")]
        // localhost770912/Product/Edit/2
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await _productRepository.GetById(id);
            if (product == null)
            {
                ModelState.AddModelError(String.Empty, "Product can not be found!");
                return RedirectToAction("List");
            }
            UpdateProductDTO dto = new UpdateProductDTO()
            {
                Categories = await _categoryRepository.GetByDefaults(x => x.Status != Status.Passive),
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                UnitPrice = product.UnitPrice,
                Image = product.Image
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductDTO model)
        {
            if (ModelState.IsValid)
            {
                string imageName = "noimage.png";
                if (model.UploadImage != null)
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");

                    if (!string.Equals(model.Image, "noimage.png"))
                    {
                        string oldPath = Path.Combine(uploadDir, model.Image);

                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    imageName = $"{Guid.NewGuid()}_{model.UploadImage.FileName}";
                    string filePath = Path.Combine(uploadDir, imageName);
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    await model.UploadImage.CopyToAsync(fileStream);
                    fileStream.Close();
                }
                Product product = _mapper.Map<Product>(model);
                if (imageName != "noimage.png")
                {
                    product.Image = imageName;
                }
               
                await _productRepository.Update(product);
                TempData["Success"] = "The product has been updated..!";
                return RedirectToAction("List");
            }
            else
            {
                TempData["Error"] = "The product has not been edited..!";
                return View(model);
            }
        }
        public async Task<IActionResult> Remove(int id)
        {
            Product product = await _productRepository.GetById(id);

            if (product != null)
            {
                await _productRepository.Delete(product);
                TempData["Success"] = "The product has been removed";
                return RedirectToAction("List");
            }
            else
            {
                TempData["Error"] = "The product has not been removed!";
                return RedirectToAction("List");
            }
        }

        public async Task<IActionResult> Index() => View(await _productRepository.GetByDefaults(x => x.Status != Status.Passive));

        //[Route("{categorySlug}")]
        public async Task<IActionResult> ProductByCategory(string categorySlug)
        {
            Category category = await _categoryRepository.GetByDefault(x => x.Slug == categorySlug);

            if (category == null) return RedirectToAction("Index");

            ViewBag.CategorySlug = category.Slug;
            ViewBag.CategoryName = category.Name;

            List<Product> products = await _productRepository.GetByDefaults(x => x.CategoryId == category.Id && x.Status != Status.Passive);

            return View(products);
        }

    }
}
