using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PulpaAPI.src.Models.Base;

namespace PulpaAPI.src.Models
{
    public class StockBatch : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("productId")]
        public string ProductId { get; set; } = string.Empty;

        [BsonElement("supplierId")]
        public string SupplierId { get; set; } = string.Empty;

        [BsonElement("batchCode")]
        public string BatchCode { get; set; } = string.Empty;

        [BsonElement("quantity")]
        public int Quantity { get; set; } = 0;

        [BsonElement("initialQuantity")]
        public int InitialQuantity { get; set; } = 0;

        [BsonElement("expiryDate")]
        public DateTime ExpiryDate { get; set; }

        [BsonElement("entryDate")]
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [BsonElement("costPrice")]
        public decimal CostPrice { get; set; } = 0;

        [BsonElement("notes")]
        public string Notes { get; set; } = string.Empty;

        [BsonIgnore]
        public bool IsExpired => ExpiryDate < DateTime.UtcNow;

        [BsonIgnore]
        public bool IsNearExpiry => ExpiryDate <= DateTime.UtcNow.AddDays(7) && !IsExpired;

        [BsonIgnore]
        public int DaysUntilExpiry => (ExpiryDate - DateTime.UtcNow).Days;
    }
}
