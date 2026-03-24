using PulpaAPI.src.Interfaces;
using PulpaAPI.src.Models;
using PulpaAPI.src.Models.Base;
using PulpaAPI.src.Shared.DTOs;
using PulpaAPI.src.Shared.Utils;

namespace PulpaAPI.src.Services
{
    // ─── CUSTOMER ─────────────────────────────────────────────────────────────
    public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
    {
        public async Task<ResponseApi<Customer?>> CreateAsync(CreateCustomerDTO request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.Cpf))
                {
                    ResponseApi<Customer?> existing = await customerRepository.GetByCpfAsync(request.Cpf);
                    if (existing.Data is not null) return new(null, 400, "CPF já cadastrado.");
                }

                Customer customer = new()
                {
                    Name = request.Name,
                    Cpf = request.Cpf,
                    Phone = request.Phone,
                    Email = request.Email,
                    Address = new Address
                    {
                        Street = request.Address.Street,
                        Number = request.Address.Number,
                        City = request.Address.City,
                        State = request.Address.State,
                        ZipCode = request.Address.ZipCode,
                    },
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.CreatedBy,
                };

                ResponseApi<Customer?> response = await customerRepository.CreateAsync(customer);
                if (response.Data is null) return new(null, 400, "Falha ao criar cliente.");
                return new(response.Data, 201, "Cliente criado com sucesso.");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
        {
            try
            {
                PaginationUtil<Customer> pagination = new(request.QueryParams);
                ResponseApi<List<dynamic>> customers = await customerRepository.GetAllAsync(pagination);
                int count = await customerRepository.GetCountDocumentsAsync(pagination);
                return new(customers.Data, count, pagination.PageNumber, pagination.PageSize);
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<dynamic?>> GetByIdAsync(string id)
        {
            try
            {
                ResponseApi<dynamic?> customer = await customerRepository.GetByIdAggregateAsync(id);
                if (customer.Data is null) return new(null, 404, "Cliente não encontrado.");
                return new(customer.Data);
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<Customer?>> UpdateAsync(UpdateCustomerDTO request)
        {
            try
            {
                ResponseApi<Customer?> existing = await customerRepository.GetByIdAsync(request.Id);
                if (existing.Data is null) return new(null, 404, "Cliente não encontrado.");

                existing.Data.UpdatedAt = DateTime.UtcNow;
                existing.Data.Name = request.Name;
                existing.Data.Cpf = request.Cpf;
                existing.Data.Phone = request.Phone;
                existing.Data.Email = request.Email;
                existing.Data.Address = new Address { Street = request.Address.Street, Number = request.Address.Number, City = request.Address.City, State = request.Address.State, ZipCode = request.Address.ZipCode };
                existing.Data.UpdatedBy = request.UpdatedBy;

                ResponseApi<Customer?> response = await customerRepository.UpdateAsync(existing.Data);
                if (!response.IsSuccess) return new(null, 400, "Falha ao atualizar cliente.");
                return new(response.Data, 201, "Cliente atualizado com sucesso.");
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<Customer>> DeleteAsync(string id)
        {
            try
            {
                ResponseApi<Customer> response = await customerRepository.DeleteAsync(id);
                if (!response.IsSuccess) return new(null, 400, response.Message);
                return response;
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }
    }

    // ─── SUPPLIER ─────────────────────────────────────────────────────────────
    public class SupplierService(ISupplierRepository supplierRepository) : ISupplierService
    {
        public async Task<ResponseApi<Supplier?>> CreateAsync(CreateSupplierDTO request)
        {
            try
            {
                Supplier supplier = new()
                {
                    Name = request.Name,
                    Cnpj = request.Cnpj,
                    ContactName = request.ContactName,
                    Phone = request.Phone,
                    Email = request.Email,
                    Address = new Address { Street = request.Address.Street, Number = request.Address.Number, City = request.Address.City, State = request.Address.State, ZipCode = request.Address.ZipCode },
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.CreatedBy,
                };
                ResponseApi<Supplier?> response = await supplierRepository.CreateAsync(supplier);
                if (response.Data is null) return new(null, 400, "Falha ao criar fornecedor.");
                return new(response.Data, 201, "Fornecedor criado com sucesso.");
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request)
        {
            try
            {
                PaginationUtil<Supplier> pagination = new(request.QueryParams);
                ResponseApi<List<dynamic>> suppliers = await supplierRepository.GetAllAsync(pagination);
                int count = await supplierRepository.GetCountDocumentsAsync(pagination);
                return new(suppliers.Data, count, pagination.PageNumber, pagination.PageSize);
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<dynamic?>> GetByIdAsync(string id)
        {
            try
            {
                ResponseApi<dynamic?> supplier = await supplierRepository.GetByIdAggregateAsync(id);
                if (supplier.Data is null) return new(null, 404, "Fornecedor não encontrado.");
                return new(supplier.Data);
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<Supplier?>> UpdateAsync(UpdateSupplierDTO request)
        {
            try
            {
                ResponseApi<Supplier?> existing = await supplierRepository.GetByIdAsync(request.Id);
                if (existing.Data is null) return new(null, 404, "Fornecedor não encontrado.");
                existing.Data.UpdatedAt = DateTime.UtcNow;
                existing.Data.Name = request.Name;
                existing.Data.Cnpj = request.Cnpj;
                existing.Data.ContactName = request.ContactName;
                existing.Data.Phone = request.Phone;
                existing.Data.Email = request.Email;
                existing.Data.Address = new Address { Street = request.Address.Street, Number = request.Address.Number, City = request.Address.City, State = request.Address.State, ZipCode = request.Address.ZipCode };
                existing.Data.UpdatedBy = request.UpdatedBy;
                ResponseApi<Supplier?> response = await supplierRepository.UpdateAsync(existing.Data);
                if (!response.IsSuccess) return new(null, 400, "Falha ao atualizar fornecedor.");
                return new(response.Data, 201, "Fornecedor atualizado com sucesso.");
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<Supplier>> DeleteAsync(string id)
        {
            try
            {
                ResponseApi<Supplier> response = await supplierRepository.DeleteAsync(id);
                if (!response.IsSuccess) return new(null, 400, response.Message);
                return response;
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }
    }

    // ─── REPORT ───────────────────────────────────────────────────────────────
    public class ReportService(ISaleRepository saleRepository, IProductRepository productRepository) : IReportService
    {
        public async Task<ResponseApi<dynamic>> GetSalesSummaryAsync(DateTime from, DateTime to)
        {
            try
            {
                ResponseApi<List<Sale>> salesRes = await saleRepository.GetByDateRangeAsync(from, to);
                List<Sale> sales = salesRes.Data ?? [];

                decimal totalRevenue = sales.Sum(s => s.Total);
                decimal totalGrossProfit = sales.SelectMany(s => s.Items).Sum(i => i.GrossProfit);
                decimal grossMargin = totalRevenue > 0 ? Math.Round(totalGrossProfit / totalRevenue * 100, 2) : 0;

                var daily = sales
                    .GroupBy(s => s.CreatedAt.Date)
                    .OrderBy(g => g.Key)
                    .Select(g => new
                    {
                        date = g.Key.ToString("yyyy-MM-dd"),
                        salesCount = g.Count(),
                        revenue = g.Sum(s => s.Total),
                        grossProfit = g.SelectMany(s => s.Items).Sum(i => i.GrossProfit),
                    }).ToList();

                return new(new { from, to, totalSales = sales.Count, totalRevenue, totalGrossProfit, grossMarginPercent = grossMargin, dailyBreakdown = daily });
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<dynamic>> GetProductProfitAsync(DateTime from, DateTime to)
        {
            try
            {
                ResponseApi<List<Sale>> salesRes = await saleRepository.GetByDateRangeAsync(from, to);
                List<Sale> sales = salesRes.Data ?? [];

                var grouped = sales.SelectMany(s => s.Items).GroupBy(i => i.ProductId);
                var result = new List<object>();

                foreach (var g in grouped)
                {
                    ResponseApi<Product?> productRes = await productRepository.GetByIdAsync(g.Key);
                    if (productRes.Data is null) continue;

                    decimal totalRevenue = g.Sum(i => i.Subtotal);
                    decimal totalCost = g.Sum(i => i.CostPrice * i.Quantity);
                    decimal grossProfit = totalRevenue - totalCost;
                    decimal margin = totalRevenue > 0 ? Math.Round(grossProfit / totalRevenue * 100, 2) : 0;

                    result.Add(new
                    {
                        productId = g.Key,
                        productName = productRes.Data.Name,
                        category = productRes.Data.Category.ToString(),
                        weightGrams = productRes.Data.WeightGrams,
                        totalQuantitySold = g.Sum(i => i.Quantity),
                        totalRevenue,
                        totalCost,
                        grossProfit,
                        grossMarginPercent = margin,
                    });
                }

                return new(result.OrderByDescending(x => ((dynamic)x).grossProfit).ToList());
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<dynamic>> GetStockLossesAsync(DateTime from, DateTime to)
        {
            try
            {
                // Placeholder — losses seriam buscadas do StockLossRepository
                return new(new { from, to, totalLossEvents = 0, totalQuantityLost = 0, totalValueLost = 0, details = new List<object>() });
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }
    }

    // ─── AUTH ─────────────────────────────────────────────────────────────────
    public class AuthService(IProductRepository productRepository) : IAuthService
    {
        public async Task<ResponseApi<dynamic>> LoginAsync(string email, string password)
        {
            // Stub — implementar JWT conforme o modelo quando User estiver no escopo
            await Task.CompletedTask;
            return new(null, 501, "Auth não implementado neste MVP.");
        }

        public async Task<ResponseApi<dynamic>> RegisterAsync(string name, string email, string password, string phone)
        {
            await Task.CompletedTask;
            return new(null, 501, "Auth não implementado neste MVP.");
        }
    }
}
