using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PulpaAPI.src.Enums.Product;
using PulpaAPI.src.Models.Base;

namespace PulpaAPI.src.Models
{
    public class Product : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("category")]
        [BsonRepresentation(BsonType.String)]
        public CategoryEnum Category { get; set; } = CategoryEnum.TropicalFruits;

        [BsonElement("weightGrams")]
        public int WeightGrams { get; set; } = 500;

        [BsonElement("costPrice")]
        public decimal CostPrice { get; set; } = 0;

        [BsonElement("salePrice")]
        public decimal SalePrice { get; set; } = 0;

        [BsonElement("minStockLevel")]
        public int MinStockLevel { get; set; } = 10;

        [BsonElement("supplierId")]
        public string SupplierId { get; set; } = string.Empty;

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
