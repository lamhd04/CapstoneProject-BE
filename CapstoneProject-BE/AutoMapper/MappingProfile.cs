using AutoMapper;
using CapstoneProject_BE.DTO;
using CapstoneProject_BE.Models;

namespace CapstoneProject_BE.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
            CreateMap<MeasuredUnitDTO, MeasuredUnit>();
        }
        
    }
}
