using PulpaAPI.src.Models;
using PulpaAPI.src.Models.Base;
using PulpaAPI.src.Shared.DTOs;
using PulpaAPI.src.Shared.Utils;

namespace PulpaAPI.src.Interfaces
{
    // ─── AUTH ────────────────────────────────────────────────────────
    public interface IAuthService
    {
        Task<ResponseApi<dynamic>> LoginAsync(string email, string password);
        Task<ResponseApi<dynamic>> RegisterAsync(string name, string email, string password, string phone);
    }

    // ─── PRODUCT ─────────────────────────────────────────────────────
    public interface IProductRepository
    {
        Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Product> pagination);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<Product?>> GetByIdAsync(string id);
        Task<int> GetCountDocumentsAsync(PaginationUtil<Product> pagination);
        Task<ResponseApi<Product?>> CreateAsync(Product product);
        Task<ResponseApi<Product?>> UpdateAsync(Product product);
        Task<ResponseApi<Product>> DeleteAsync(string id);
    }

    public interface IProductService
    {
        Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
        Task<ResponseApi<dynamic?>> GetByIdAsync(string id);
        Task<ResponseApi<Product?>> CreateAsync(CreateProductDTO dto);
        Task<ResponseApi<Product?>> UpdateAsync(UpdateProductDTO dto);
        Task<ResponseApi<Product>> DeleteAsync(string id);
    }

    // ─── STOCK ───────────────────────────────────────────────────────
    public interface IStockRepository
    {
        Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<StockBatch> pagination);
        Task<ResponseApi<StockBatch?>> GetByIdAsync(string id);
        Task<ResponseApi<List<StockBatch>>> GetByProductFEFOAsync(string productId);
        Task<ResponseApi<List<StockBatch>>> GetNearExpiryAsync(int days = 7);
        Task<ResponseApi<List<StockBatch>>> GetExpiredAsync();
        Task<int> GetCountDocumentsAsync(PaginationUtil<StockBatch> pagination);
        Task<int> GetTotalStockAsync(string productId);
        Task<ResponseApi<StockBatch?>> CreateAsync(StockBatch batch);
        Task<ResponseApi<StockBatch?>> UpdateAsync(StockBatch batch);
        Task<ResponseApi<StockLoss?>> CreateLossAsync(StockLoss loss);
    }

    public interface IStockService
    {
        Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
        Task<ResponseApi<dynamic?>> GetAlertsAsync();
        Task<ResponseApi<StockBatch?>> AddBatchAsync(CreateStockBatchDTO dto);
        Task<ResponseApi<StockLoss?>> RegisterLossAsync(RegisterLossDTO dto);
    }

    // ─── SALE ────────────────────────────────────────────────────────
    public interface ISaleRepository
    {
        Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Sale> pagination);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<Sale?>> GetByIdAsync(string id);
        Task<int> GetCountDocumentsAsync(PaginationUtil<Sale> pagination);
        Task<long> GetNextSaleNumberAsync();
        Task<ResponseApi<Sale?>> CreateAsync(Sale sale);
        Task<ResponseApi<Sale?>> UpdateAsync(Sale sale);
        Task<ResponseApi<List<Sale>>> GetByDateRangeAsync(DateTime from, DateTime to);
    }

    public interface ISaleService
    {
        Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
        Task<ResponseApi<dynamic?>> GetByIdAsync(string id);
        Task<ResponseApi<Sale?>> CreateAsync(CreateSaleDTO dto);
        Task<ResponseApi<Sale?>> CancelAsync(string id);
    }

    // ─── CUSTOMER ────────────────────────────────────────────────────
    public interface ICustomerRepository
    {
        Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Customer> pagination);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<Customer?>> GetByIdAsync(string id);
        Task<ResponseApi<Customer?>> GetByCpfAsync(string cpf);
        Task<int> GetCountDocumentsAsync(PaginationUtil<Customer> pagination);
        Task<ResponseApi<Customer?>> CreateAsync(Customer customer);
        Task<ResponseApi<Customer?>> UpdateAsync(Customer customer);
        Task<ResponseApi<Customer>> DeleteAsync(string id);
    }

    public interface ICustomerService
    {
        Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
        Task<ResponseApi<dynamic?>> GetByIdAsync(string id);
        Task<ResponseApi<Customer?>> CreateAsync(CreateCustomerDTO dto);
        Task<ResponseApi<Customer?>> UpdateAsync(UpdateCustomerDTO dto);
        Task<ResponseApi<Customer>> DeleteAsync(string id);
    }

    // ─── SUPPLIER ────────────────────────────────────────────────────
    public interface ISupplierRepository
    {
        Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Supplier> pagination);
        Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id);
        Task<ResponseApi<Supplier?>> GetByIdAsync(string id);
        Task<int> GetCountDocumentsAsync(PaginationUtil<Supplier> pagination);
        Task<ResponseApi<Supplier?>> CreateAsync(Supplier supplier);
        Task<ResponseApi<Supplier?>> UpdateAsync(Supplier supplier);
        Task<ResponseApi<Supplier>> DeleteAsync(string id);
    }

    public interface ISupplierService
    {
        Task<PaginationApi<List<dynamic>>> GetAllAsync(GetAllDTO request);
        Task<ResponseApi<dynamic?>> GetByIdAsync(string id);
        Task<ResponseApi<Supplier?>> CreateAsync(CreateSupplierDTO dto);
        Task<ResponseApi<Supplier?>> UpdateAsync(UpdateSupplierDTO dto);
        Task<ResponseApi<Supplier>> DeleteAsync(string id);
    }

    // ─── REPORT ──────────────────────────────────────────────────────
    public interface IReportService
    {
        Task<ResponseApi<dynamic>> GetSalesSummaryAsync(DateTime from, DateTime to);
        Task<ResponseApi<dynamic>> GetProductProfitAsync(DateTime from, DateTime to);
        Task<ResponseApi<dynamic>> GetStockLossesAsync(DateTime from, DateTime to);
    }
}
