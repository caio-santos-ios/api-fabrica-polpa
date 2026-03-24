using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PulpaAPI.src.Models.Base;

namespace PulpaAPI.src.Models
{
    public class Address
    {
        [BsonElement("street")]
        public string Street { get; set; } = string.Empty;

        [BsonElement("number")]
        public string Number { get; set; } = string.Empty;

        [BsonElement("city")]
        public string City { get; set; } = string.Empty;

        [BsonElement("state")]
        public string State { get; set; } = string.Empty;

        [BsonElement("zipCode")]
        public string ZipCode { get; set; } = string.Empty;
    }

    public class Customer : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("cpf")]
        public string Cpf { get; set; } = string.Empty;

        [BsonElement("phone")]
        public string Phone { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("address")]
        public Address Address { get; set; } = new();
    }

    public class Supplier : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("cnpj")]
        public string Cnpj { get; set; } = string.Empty;

        [BsonElement("contactName")]
        public string ContactName { get; set; } = string.Empty;

        [BsonElement("phone")]
        public string Phone { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("address")]
        public Address Address { get; set; } = new();
    }

    public class StockLoss : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("productId")]
        public string ProductId { get; set; } = string.Empty;

        [BsonElement("batchId")]
        public string BatchId { get; set; } = string.Empty;

        [BsonElement("quantity")]
        public int Quantity { get; set; } = 0;

        [BsonElement("reason")]
        public string Reason { get; set; } = "Vencimento";

        [BsonElement("costValue")]
        public decimal CostValue { get; set; } = 0;

        [BsonElement("notes")]
        public string Notes { get; set; } = string.Empty;
    }
}
