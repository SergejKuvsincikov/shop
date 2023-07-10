using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

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

            CreateMap<Address,AddressDto>().ReverseMap();
        }
    }
} 