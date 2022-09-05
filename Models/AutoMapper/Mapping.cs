using AsynchronousProgramming.Models.DTOs;
using AsynchronousProgramming.Models.Entities.Concrete;
using AsynchronousProgramming.Models.ViewModels;
using AutoMapper;

namespace AsynchronousProgramming.Models.AutoMapper
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<Page, CreatePageDTO>().ReverseMap();
            CreateMap<Page, UpdatePageDTO>().ReverseMap();
            CreateMap<Page, PageVm>().ReverseMap();

            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();

            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Product, UpdatePageDTO>().ReverseMap();
            CreateMap<Product, ProductVM>().ReverseMap();
        }
    }
}
