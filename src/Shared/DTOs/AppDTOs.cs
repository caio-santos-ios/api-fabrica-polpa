namespace PulpaAPI.src.Shared.DTOs
{
    // ─── Product ────────────────────────────────────────────────────
    public class CreateProductDTO : RequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = "TropicalFruits";
        public int WeightGrams { get; set; } = 500;
        public decimal CostPrice { get; set; } = 0;
        public decimal SalePrice { get; set; } = 0;
        public int MinStockLevel { get; set; } = 10;
        public string SupplierId { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class UpdateProductDTO : RequestDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int WeightGrams { get; set; } = 0;
        public decimal CostPrice { get; set; } = 0;
        public decimal SalePrice { get; set; } = 0;
        public int MinStockLevel { get; set; } = 0;
        public string SupplierId { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
    }

    // ─── Stock ──────────────────────────────────────────────────────
    public class CreateStockBatchDTO : RequestDTO
    {
        public string ProductId { get; set; } = string.Empty;
        public string SupplierId { get; set; } = string.Empty;
        public string BatchCode { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public DateTime ExpiryDate { get; set; }
        public decimal CostPrice { get; set; } = 0;
        public string Notes { get; set; } = string.Empty;
    }

    public class RegisterLossDTO : RequestDTO
    {
        public string BatchId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public string Reason { get; set; } = "Vencimento";
        public string Notes { get; set; } = string.Empty;
    }

    // ─── Sale ───────────────────────────────────────────────────────
    public class CreateSaleItemDTO
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal? OverridePrice { get; set; }
        public decimal Discount { get; set; } = 0;
    }

    public class CreatePaymentDTO
    {
        public string Method { get; set; } = "Cash";
        public decimal Amount { get; set; } = 0;
    }

    public class CreateSaleDTO : RequestDTO
    {
        public string CustomerId { get; set; } = string.Empty;
        public List<CreateSaleItemDTO> Items { get; set; } = [];
        public List<CreatePaymentDTO> Payments { get; set; } = [];
        public string Notes { get; set; } = string.Empty;
    }

    // ─── Customer ───────────────────────────────────────────────────
    public class AddressDTO
    {
        public string Street { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }

    public class CreateCustomerDTO : RequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public AddressDTO Address { get; set; } = new();
    }

    public class UpdateCustomerDTO : RequestDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public AddressDTO Address { get; set; } = new();
    }

    // ─── Supplier ───────────────────────────────────────────────────
    public class CreateSupplierDTO : RequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public AddressDTO Address { get; set; } = new();
    }

    public class UpdateSupplierDTO : RequestDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public AddressDTO Address { get; set; } = new();
    }
}
