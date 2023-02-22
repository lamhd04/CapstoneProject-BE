﻿using AutoMapper;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Models;
using System.Text.Json;

namespace CapstoneProject_BE.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDTO, Product>();
            CreateMap<MeasuredUnitDTO, MeasuredUnit>();
            CreateMap<SupplierDTO, Supplier>();
            CreateMap<Supplier, SupplierDTO>()
                .ForMember(cdto => cdto.City,
                map => map.MapFrom(
                    c => c.City == "" ? null : JsonSerializer.Deserialize<City>(c.City,new JsonSerializerOptions())
                    ))
                .ForMember(cdto => cdto.District,
                map => map.MapFrom(
                    c => c.District == "" ? null : JsonSerializer.Deserialize<District>(c.District, new JsonSerializerOptions())
                    ));
            CreateMap<ImportOrderDTO, ImportOrder>();
            CreateMap<ImportDetailDTO, ImportOrderDetail>()
                                .ForMember(cdto => cdto.MeasuredUnitId,
                map => map.MapFrom(
                    c => c.MeasuredUnitId==0? null:c.MeasuredUnitId
                    ));
        }
    }
        
    
}
