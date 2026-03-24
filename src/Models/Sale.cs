using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PulpaAPI.src.Enums.Product;
using PulpaAPI.src.Models.Base;

namespace PulpaAPI.src.Models
{
    public class SaleItem
    {
        [BsonElement("productId")]
        public string ProductId { get; set; } = string.Empty;

        [BsonElement("productName")]
        public string ProductName { get; set; } = string.Empty;

        [BsonElement("batchId")]
        public string BatchId { get; set; } = string.Empty;

        [BsonElement("quantity")]
        public int Quantity { get; set; } = 0;

        [BsonElement("unitPrice")]
        public decimal UnitPrice { get; set; } = 0;

        [BsonElement("costPrice")]
        public decimal CostPrice { get; set; } = 0;

        [BsonElement("discount")]
        public decimal Discount { get; set; } = 0;

        [BsonIgnore]
        public decimal Subtotal => (UnitPrice * Quantity) - Discount;

        [BsonIgnore]
        public decimal GrossProfit => (UnitPrice - CostPrice) * Quantity;
    }

    public class PaymentDetail
    {
        [BsonElement("method")]
        [BsonRepresentation(BsonType.String)]
        public PaymentMethodEnum Method { get; set; } = PaymentMethodEnum.Cash;

        [BsonElement("amount")]
        public decimal Amount { get; set; } = 0;
    }

    public class Sale : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("saleNumber")]
        public long SaleNumber { get; set; }

        [BsonElement("customerId")]
        public string CustomerId { get; set; } = string.Empty;

        [BsonElement("items")]
        public List<SaleItem> Items { get; set; } = [];

        [BsonElement("payments")]
        public List<PaymentDetail> Payments { get; set; } = [];

        [BsonElement("status")]
        [BsonRepresentation(BsonType.String)]
        public SaleStatusEnum Status { get; set; } = SaleStatusEnum.Pending;

        [BsonElement("subtotal")]
        public decimal Subtotal { get; set; } = 0;

        [BsonElement("totalDiscount")]
        public decimal TotalDiscount { get; set; } = 0;

        [BsonElement("total")]
        public decimal Total { get; set; } = 0;

        [BsonElement("amountPaid")]
        public decimal AmountPaid { get; set; } = 0;

        [BsonElement("change")]
        public decimal Change { get; set; } = 0;

        [BsonElement("notes")]
        public string Notes { get; set; } = string.Empty;

        [BsonElement("completedAt")]
        public DateTime? CompletedAt { get; set; }
    }
}
