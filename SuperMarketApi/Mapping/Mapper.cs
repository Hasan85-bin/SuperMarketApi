using AutoMapper;
using SuperMarketApi.DTOs;
using SuperMarketApi.DTOs.Cart;
using SuperMarketApi.DTOs.Product;
using SuperMarketApi.DTOs.Staff;
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
            CreateMap<CartItem, PurchaseItem>()
                .ForMember(dest => dest.ID, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseID, opt => opt.Ignore())
                .ForMember(dest => dest.Purchase, opt => opt.Ignore())
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product!.ProductName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product!.Price))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Product!.Brand))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Product!.Category));
                
            CreateMap<Purchase, UserPurchaseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<Purchase, PurchaseResponseDto>();
            CreateMap<User, StaffUserDto>();
            CreateMap<PurchaseItem, DTOs.Staff.PurchaseItemDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));
            
            // New mappings for cart response
            CreateMap<Product, ProductInfoDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.ToString()));
            CreateMap<CartItem, CartItemResponseDto>();
            
            // Add other User-related mappings as needed
        }
    }
}   