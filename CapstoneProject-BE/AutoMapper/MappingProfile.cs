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
            CreateMap<SupplierDTO, Supplier>().ForMember(cdto => cdto.City,
                map => map.MapFrom(
                    c => c.City == null ? "" : JsonSerializer.Serialize(c.City, new JsonSerializerOptions())
                    ))
                .ForMember(cdto => cdto.District,
                map => map.MapFrom(
                    c => c.District == null ? "" : JsonSerializer.Serialize(c.District, new JsonSerializerOptions())
                    ))
                .ForMember(cdto => cdto.Ward,
                map => map.MapFrom(
                    c => c.Ward == null ? "" : JsonSerializer.Serialize(c.Ward, new JsonSerializerOptions())
                    ));
            CreateMap<Supplier, SupplierDTO>()
                .ForMember(cdto => cdto.City,
                map => map.MapFrom(
                    c => c.City == "" ? null : JsonSerializer.Deserialize<City>(c.City,new JsonSerializerOptions())
                    ))
                .ForMember(cdto => cdto.District,
                map => map.MapFrom(
                    c => c.District == "" ? null : JsonSerializer.Deserialize<District>(c.District, new JsonSerializerOptions())
                    ))
                .ForMember(cdto => cdto.Ward,
                map => map.MapFrom(
                    c => c.Ward == "" ? null : JsonSerializer.Deserialize<Ward>(c.Ward, new JsonSerializerOptions())
                    ));
            CreateMap<ImportOrderDTO, ImportOrder>();
            CreateMap<UserDTO, User>();
            CreateMap<ImportDetailDTO, ImportOrderDetail>()
                                .ForMember(cdto => cdto.MeasuredUnitId,
                map => map.MapFrom(
                    c => c.MeasuredUnitId==0? null:c.MeasuredUnitId
                    ));
            CreateMap<ImportOrder, ImportOrderDTO>()
                .ForMember(cdto => cdto.CreatedDate,
                map => map.MapFrom(
                    c => c.Created
                    ))
                .ForMember(cdto => cdto.ApprovedDate,
                map => map.MapFrom(
                    c => c.Approved==null ? null : c.Approved
                    ))
                .ForMember(cdto => cdto.DeniedDate,
                map => map.MapFrom(
                    c => c.Denied==null ? null : c.Denied
                    ))
                .ForMember(cdto => cdto.CompletedDate,
                map => map.MapFrom(
                    c => c.Completed == null ? null : c.Completed
                    ));
            CreateMap<ExportOrder, ExportOrderDTO>()
                .ForMember(cdto => cdto.CreatedDate,
                map => map.MapFrom(
                    c => c.Created
                    ))
                .ForMember(cdto => cdto.ApprovedDate,
                map => map.MapFrom(
                    c => c.Approved == null ? null : c.Approved
                    ))
                .ForMember(cdto => cdto.DeniedDate,
                map => map.MapFrom(
                    c => c.Denied == null ? null : c.Denied
                    ))
                .ForMember(cdto => cdto.CompletedDate,
                map => map.MapFrom(
                    c => c.Completed == null ? null : c.Completed
                    ));
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<ImportOrderDetail, ImportDetailDTO>()
                                .ForMember(cdto => cdto.DefaultMeasuredUnit,
                map => map.MapFrom(
                    c => c.Product.DefaultMeasuredUnit
                    ));
            CreateMap<ExportOrderDetail, ExportDetailDTO>()
                    .ForMember(cdto => cdto.DefaultMeasuredUnit,
    map => map.MapFrom(
        c => c.Product.DefaultMeasuredUnit
        ));
            CreateMap<ExportOrderDTO, ExportOrder>();
            CreateMap<ExportDetailDTO, ExportOrderDetail>()
                                .ForMember(cdto => cdto.MeasuredUnitId,
                map => map.MapFrom(
                    c => c.MeasuredUnitId == 0 ? null : c.MeasuredUnitId
                    ));
            CreateMap<ReturnsDTO, ReturnsOrder>();
            CreateMap<ReturnsDetailDTO, ReturnsOrderDetail>()
                                .ForMember(cdto => cdto.MeasuredUnitId,
                map => map.MapFrom(
                    c => c.MeasuredUnitId == 0 ? null : c.MeasuredUnitId
                    ));
            CreateMap<StocktakeDTO, StocktakeNote>();
            CreateMap<StocktakeDetailDTO, StocktakeNoteDetail>()
                                .ForMember(cdto => cdto.MeasuredUnitId,
                map => map.MapFrom(
                    c => c.MeasuredUnitId == 0 ? null : c.MeasuredUnitId
                    ))
                                .ForMember(cdto => cdto.AmountDifferential,
                map => map.MapFrom(
                    c => Math.Abs(c.ActualStock-c.CurrentStock)
                    ));
        }
    }
        
    
}
