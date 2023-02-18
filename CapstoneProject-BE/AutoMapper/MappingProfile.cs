using AutoMapper;
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
            CreateMap<Supplier, SupplierDTO>();
            CreateMap<ImportOrderDTO, ImportOrder>();
            CreateMap<ImportDetailDTO, ImportOrderDetail>();
        }
    }
        
    
}
