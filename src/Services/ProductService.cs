using PulpaAPI.src.Enums.Product;
using PulpaAPI.src.Interfaces;
using PulpaAPI.src.Models;
using PulpaAPI.src.Models.Base;
using PulpaAPI.src.Shared.DTOs;
using PulpaAPI.src.Shared.Utils;

namespace PulpaAPI.src.Services
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        #region CREATE
        public async Task<ResponseApi<Product?>> CreateAsync(CreateProductDTO request)
        {
            try
            {
                // no duplicate check needed for product name

                if (!Enum.TryParse<CategoryEnum>(request.Category, out CategoryEnum category))
                    return new(null, 400, "Categoria inválida. Use: TropicalFruits, ExoticFruits ou Mix.");

                Product product = new()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Category = category,
                    WeightGrams = request.WeightGrams,
                    CostPrice = request.CostPrice,
                    SalePrice = request.SalePrice,
                    MinStockLevel = request.MinStockLevel,
                    SupplierId = request.SupplierId,
                    ImageUrl = request.ImageUrl,
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.CreatedBy,
                };

                ResponseApi<Product?> response = await productRepository.CreateAsync(product);
                if (response.Data is null) return new(null, 400, "Falha ao criar produto.");

                return new(response.Data, 201, "Produto criado com sucesso.");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion

        #region READ
        public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
        {
            try
            {
                PaginationUtil<Product> pagination = new(request.QueryParams);
                ResponseApi<List<dynamic>> products = await productRepository.GetAllAsync(pagination);
                int count = await productRepository.GetCountDocumentsAsync(pagination);
                return new(products.Data, count, pagination.PageNumber, pagination.PageSize);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<ResponseApi<dynamic?>> GetByIdAsync(string id)
        {
            try
            {
                ResponseApi<dynamic?> product = await productRepository.GetByIdAggregateAsync(id);
                if (product.Data is null) return new(null, 404, "Produto não encontrado.");
                return new(product.Data);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion

        #region UPDATE
        public async Task<ResponseApi<Product?>> UpdateAsync(UpdateProductDTO request)
        {
            try
            {
                ResponseApi<Product?> existing = await productRepository.GetByIdAsync(request.Id);
                if (existing.Data is null) return new(null, 404, "Produto não encontrado.");

                if (!Enum.TryParse<CategoryEnum>(request.Category, out CategoryEnum category))
                    return new(null, 400, "Categoria inválida.");

                existing.Data.UpdatedAt = DateTime.UtcNow;
                existing.Data.Name = request.Name;
                existing.Data.Description = request.Description;
                existing.Data.Category = category;
                existing.Data.WeightGrams = request.WeightGrams;
                existing.Data.CostPrice = request.CostPrice;
                existing.Data.SalePrice = request.SalePrice;
                existing.Data.MinStockLevel = request.MinStockLevel;
                existing.Data.SupplierId = request.SupplierId;
                existing.Data.ImageUrl = request.ImageUrl;
                existing.Data.Active = request.Active;
                existing.Data.UpdatedBy = request.UpdatedBy;

                ResponseApi<Product?> response = await productRepository.UpdateAsync(existing.Data);
                if (!response.IsSuccess) return new(null, 400, "Falha ao atualizar produto.");
                return new(response.Data, 201, "Produto atualizado com sucesso.");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion

        #region DELETE
        public async Task<ResponseApi<Product>> DeleteAsync(string id)
        {
            try
            {
                ResponseApi<Product> response = await productRepository.DeleteAsync(id);
                if (!response.IsSuccess) return new(null, 400, response.Message);
                return response;
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
    }
}
