using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Core.Entities;


namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product,ProductToReturnDto1>()
            .ForMember(d => d.ProductBrand, o => o.MapFrom(p => p.ProductBrand.Name))
            .ForMember(d => d.ProductType, o => o.MapFrom(p => p.ProductType.Name))
            .ForMember(d => d.PictureUrl, o=>o.MapFrom<ProductUrlResolver>()    );

            CreateMap<Core.Entities.Identity.Address,AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto,CustomerBasket>();
            CreateMap<BasketItemDto,BasketItem>(); 
            CreateMap<AddressDto,Core.Entities.OrderAgregate.Address>();

            CreateMap<Core.Entities.OrderAgregate.Order,OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(p => p.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(p => p.DeliveryMethod.Price));
            CreateMap<Core.Entities.OrderAgregate.OrderItem,OrderItemDto>()
            .ForMember(d => d.ProductId, o => o.MapFrom(p => p.ItemOrdered.ProductItemId))
            .ForMember(d => d.ProductName, o => o.MapFrom(p => p.ItemOrdered.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(p => p.ItemOrdered.PictureUrl))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
        }
    }
} 