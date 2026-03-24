using AutoMapper;
using PulpaAPI.src.Models;
using PulpaAPI.src.Shared.DTOs;

namespace PulpaAPI.src.Configuration
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region MASTER DATA
            CreateMap<CreateProductDTO, Product>().ReverseMap();
            CreateMap<UpdateProductDTO, Product>().ReverseMap();
            CreateMap<CreateCustomerDTO, Customer>().ReverseMap();
            CreateMap<UpdateCustomerDTO, Customer>().ReverseMap();
            CreateMap<CreateSupplierDTO, Supplier>().ReverseMap();
            CreateMap<UpdateSupplierDTO, Supplier>().ReverseMap();
            #endregion

            #region ESTOQUE
            CreateMap<CreateStockBatchDTO, StockBatch>().ReverseMap();
            #endregion
        }
    }
}
