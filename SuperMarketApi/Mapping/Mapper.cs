using AutoMapper;
using SuperMarketApi.DTOs;
using SuperMarketApi.DTOs.Cart;
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
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserInfoDto, User>();
            CreateMap<CartItemCreateDto, CartItem>();
            CreateMap<CartItemUpdateDto, CartItem>();
            CreateMap<CartItem, Purchase>();
            // Add other User-related mappings as needed
        }
    }
}   