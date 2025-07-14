using AutoMapper;
using SuperMarketApi.DTOs;
using SuperMarketApi.DTOs.Product;
using SuperMarketApi.Models;

namespace SuperMarketApi.Mapping
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.category.ToString()));
            CreateMap<CreateUserDto, User>();
            // Add other User-related mappings as needed
        }
    }
}   