using AutoMapper;
using Core.Entities;
using Core.Entities.OrderShop;

namespace WebApi.Dtos
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(p => p.CategoryName, x => x.MapFrom(a => a.Category.Name))
                .ForMember(p => p.BrandName, x => x.MapFrom(a => a.Brand.Name));

            CreateMap<Core.Entities.Direction, DirectionDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<DirectionDto, Core.Entities.OrderShop.Direction>();

            CreateMap<OrderShop, orderShopResponseDto>()
                .ForMember(o => o.ShippingType, x => x.MapFrom(y => y.ShippingType.Name))
                .ForMember(o => o.ShippingTypePrice, x => x.MapFrom(y => y.ShippingType.Price));

            CreateMap<OrderItem, OrderItemResponseDto>()
                .ForMember(o => o.ProductItemId, x => x.MapFrom(y => y.ItemOrder.ProductItemId))
                .ForMember(o => o.ProductName, x => x.MapFrom(y => y.ItemOrder.ProductName))
                .ForMember(o => o.ProductImage, x => x.MapFrom(y => y.ItemOrder.ImageUrl));
        }
    }
}
