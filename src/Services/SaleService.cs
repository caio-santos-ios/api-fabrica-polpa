using PulpaAPI.src.Enums.Product;
using PulpaAPI.src.Interfaces;
using PulpaAPI.src.Models;
using PulpaAPI.src.Models.Base;
using PulpaAPI.src.Shared.DTOs;
using PulpaAPI.src.Shared.Utils;

namespace PulpaAPI.src.Services
{
    public class SaleService(
        ISaleRepository saleRepository,
        IStockRepository stockRepository,
        IProductRepository productRepository) : ISaleService
    {
        #region CREATE
        public async Task<ResponseApi<Sale?>> CreateAsync(CreateSaleDTO request)
        {
            try
            {
                if (request.Items.Count == 0) return new(null, 400, "A venda deve ter pelo menos um item.");

                List<SaleItem> saleItems = [];
                decimal subtotal = 0;

                foreach (var itemDto in request.Items)
                {
                    ResponseApi<Product?> productRes = await productRepository.GetByIdAsync(itemDto.ProductId);
                    if (productRes.Data is null) return new(null, 404, $"Produto {itemDto.ProductId} não encontrado.");

                    // FEFO: pega lotes ordenados por validade ASC
                    ResponseApi<List<StockBatch>> batchesRes = await stockRepository.GetByProductFEFOAsync(itemDto.ProductId);
                    int available = batchesRes.Data?.Sum(b => b.Quantity) ?? 0;

                    if (available < itemDto.Quantity)
                        return new(null, 400, $"Estoque insuficiente para '{productRes.Data.Name}'. Disponível: {available}, solicitado: {itemDto.Quantity}.");

                    string batchId = batchesRes.Data!.First().Id;
                    decimal unitPrice = itemDto.OverridePrice ?? productRes.Data.SalePrice;

                    SaleItem saleItem = new()
                    {
                        ProductId = productRes.Data.Id,
                        ProductName = productRes.Data.Name,
                        BatchId = batchId,
                        Quantity = itemDto.Quantity,
                        UnitPrice = unitPrice,
                        CostPrice = batchesRes.Data.First().CostPrice,
                        Discount = itemDto.Discount,
                    };

                    saleItems.Add(saleItem);
                    subtotal += saleItem.Subtotal;
                }

                decimal amountPaid = request.Payments.Sum(p => p.Amount);
                decimal change = amountPaid - subtotal;

                if (change < 0) return new(null, 400, "Valor pago insuficiente.");

                long saleNumber = await saleRepository.GetNextSaleNumberAsync();

                Sale sale = new()
                {
                    SaleNumber = saleNumber,
                    CustomerId = request.CustomerId,
                    Items = saleItems,
                    Payments = request.Payments.Select(p => new PaymentDetail
                    {
                        Method = Enum.Parse<PaymentMethodEnum>(p.Method),
                        Amount = p.Amount
                    }).ToList(),
                    Status = SaleStatusEnum.Completed,
                    Subtotal = subtotal,
                    TotalDiscount = saleItems.Sum(i => i.Discount),
                    Total = subtotal,
                    AmountPaid = amountPaid,
                    Change = change,
                    Notes = request.Notes,
                    CompletedAt = DateTime.UtcNow,
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.CreatedBy,
                };

                ResponseApi<Sale?> response = await saleRepository.CreateAsync(sale);
                if (response.Data is null) return new(null, 400, "Falha ao registrar venda.");

                // Dedução FEFO no estoque
                foreach (var item in saleItems)
                {
                    ResponseApi<List<StockBatch>> batchesRes = await stockRepository.GetByProductFEFOAsync(item.ProductId);
                    int remaining = item.Quantity;

                    foreach (var batch in batchesRes.Data ?? [])
                    {
                        if (remaining <= 0) break;
                        int deduction = Math.Min(batch.Quantity, remaining);
                        batch.Quantity -= deduction;
                        batch.UpdatedAt = DateTime.UtcNow;
                        if (batch.Quantity == 0) batch.Active = false;
                        remaining -= deduction;
                        await stockRepository.UpdateAsync(batch);
                    }
                }

                return new(response.Data, 201, "Venda realizada com sucesso.");
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
                PaginationUtil<Sale> pagination = new(request.QueryParams);
                ResponseApi<List<dynamic>> sales = await saleRepository.GetAllAsync(pagination);
                int count = await saleRepository.GetCountDocumentsAsync(pagination);
                return new(sales.Data, count, pagination.PageNumber, pagination.PageSize);
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
                ResponseApi<dynamic?> sale = await saleRepository.GetByIdAggregateAsync(id);
                if (sale.Data is null) return new(null, 404, "Venda não encontrada.");
                return new(sale.Data);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion

        #region UPDATE
        public async Task<ResponseApi<Sale?>> CancelAsync(string id)
        {
            try
            {
                ResponseApi<Sale?> saleRes = await saleRepository.GetByIdAsync(id);
                if (saleRes.Data is null) return new(null, 404, "Venda não encontrada.");
                if (saleRes.Data.Status == SaleStatusEnum.Cancelled) return new(null, 400, "Venda já cancelada.");

                // Reverter estoque
                foreach (var item in saleRes.Data.Items)
                {
                    ResponseApi<StockBatch?> batchRes = await stockRepository.GetByIdAsync(item.BatchId);
                    if (batchRes.Data is not null)
                    {
                        batchRes.Data.Quantity += item.Quantity;
                        batchRes.Data.Active = true;
                        batchRes.Data.UpdatedAt = DateTime.UtcNow;
                        await stockRepository.UpdateAsync(batchRes.Data);
                    }
                }

                saleRes.Data.Status = SaleStatusEnum.Cancelled;
                saleRes.Data.UpdatedAt = DateTime.UtcNow;
                ResponseApi<Sale?> response = await saleRepository.UpdateAsync(saleRes.Data);
                if (!response.IsSuccess) return new(null, 400, "Falha ao cancelar venda.");

                return new(response.Data, 201, "Venda cancelada com sucesso.");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
    }
}
