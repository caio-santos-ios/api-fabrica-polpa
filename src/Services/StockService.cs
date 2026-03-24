using PulpaAPI.src.Interfaces;
using PulpaAPI.src.Models;
using PulpaAPI.src.Models.Base;
using PulpaAPI.src.Shared.DTOs;
using PulpaAPI.src.Shared.Utils;

namespace PulpaAPI.src.Services
{
    public class StockService(IStockRepository stockRepository, IProductRepository productRepository) : IStockService
    {
        #region CREATE
        public async Task<ResponseApi<StockBatch?>> AddBatchAsync(CreateStockBatchDTO request)
        {
            try
            {
                ResponseApi<Product?> product = await productRepository.GetByIdAsync(request.ProductId);
                if (product.Data is null) return new(null, 404, "Produto não encontrado.");

                if (request.Quantity <= 0) return new(null, 400, "Quantidade deve ser maior que zero.");
                if (request.ExpiryDate <= DateTime.UtcNow) return new(null, 400, "Data de validade deve ser futura.");

                StockBatch batch = new()
                {
                    ProductId = request.ProductId,
                    SupplierId = request.SupplierId,
                    BatchCode = request.BatchCode,
                    Quantity = request.Quantity,
                    InitialQuantity = request.Quantity,
                    ExpiryDate = request.ExpiryDate,
                    CostPrice = request.CostPrice,
                    Notes = request.Notes,
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.CreatedBy,
                };

                ResponseApi<StockBatch?> response = await stockRepository.CreateAsync(batch);
                if (response.Data is null) return new(null, 400, "Falha ao registrar lote.");
                return new(response.Data, 201, "Lote registrado com sucesso.");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<ResponseApi<StockLoss?>> RegisterLossAsync(RegisterLossDTO request)
        {
            try
            {
                ResponseApi<StockBatch?> batchRes = await stockRepository.GetByIdAsync(request.BatchId);
                if (batchRes.Data is null) return new(null, 404, "Lote não encontrado.");
                if (request.Quantity > batchRes.Data.Quantity)
                    return new(null, 400, $"Quantidade de perda ({request.Quantity}) maior que estoque do lote ({batchRes.Data.Quantity}).");

                StockLoss loss = new()
                {
                    ProductId = batchRes.Data.ProductId,
                    BatchId = request.BatchId,
                    Quantity = request.Quantity,
                    Reason = request.Reason,
                    CostValue = batchRes.Data.CostPrice * request.Quantity,
                    Notes = request.Notes,
                    CreatedBy = request.CreatedBy,
                };

                await stockRepository.CreateLossAsync(loss);

                batchRes.Data.Quantity -= request.Quantity;
                batchRes.Data.UpdatedAt = DateTime.UtcNow;
                if (batchRes.Data.Quantity == 0) batchRes.Data.Active = false;
                await stockRepository.UpdateAsync(batchRes.Data);

                return new(loss, 201, "Perda registrada com sucesso.");
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
                PaginationUtil<StockBatch> pagination = new(request.QueryParams);
                ResponseApi<List<dynamic>> batches = await stockRepository.GetAllAsync(pagination);
                int count = await stockRepository.GetCountDocumentsAsync(pagination);
                return new(batches.Data, count, pagination.PageNumber, pagination.PageSize);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<ResponseApi<dynamic>> GetAlertsAsync()
        {
            try
            {
                var nearExpiry = await stockRepository.GetNearExpiryAsync(7);
                var expired = await stockRepository.GetExpiredAsync();

                var products = new List<dynamic>();
                var allProducts = await productRepository.GetAllAsync(new PaginationUtil<Product>(new() { { "pageSize", "200" }, { "orderBy", "createdAt" } }));

                var lowStock = new List<object>();
                if (allProducts.Data is not null)
                {
                    foreach (var p in allProducts.Data)
                    {
                        string pid = p.id?.ToString() ?? "";
                        int total = await stockRepository.GetTotalStockAsync(pid);
                        int min = (int)(p.minStockLevel ?? 10);
                        if (total < min)
                        {
                            lowStock.Add(new { productId = pid, productName = p.name?.ToString() ?? "", alertType = "LowStock", currentStock = total, minStockLevel = min });
                        }
                    }
                }

                return new(new
                {
                    nearExpiry = nearExpiry.Data ?? [],
                    expired = expired.Data ?? [],
                    lowStock
                });
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
    }
}
