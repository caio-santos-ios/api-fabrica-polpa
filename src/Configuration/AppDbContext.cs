using MongoDB.Driver;
using PulpaAPI.src.Models;

namespace PulpaAPI.src.Configuration
{
    public class AppDbContext
    {
        public static string? ConnectionString { get; set; }
        public static string? DatabaseName { get; set; }
        public static bool IsSSL { get; set; }
        private IMongoDatabase Database { get; }

        public AppDbContext()
        {
            try
            {
                MongoClientSettings mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
                if (IsSSL)
                {
                    mongoClientSettings.SslSettings = new SslSettings
                    {
                        EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                    };
                }
                var mongoClient = new MongoClient(mongoClientSettings);
                Database = mongoClient.GetDatabase(DatabaseName);
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao conectar ao banco de dados. Erro: {ex.Message}");
            }
        }

        #region MASTER DATA
        public IMongoCollection<Product> Products => Database.GetCollection<Product>("products");
        public IMongoCollection<Customer> Customers => Database.GetCollection<Customer>("customers");
        public IMongoCollection<Supplier> Suppliers => Database.GetCollection<Supplier>("suppliers");
        #endregion

        #region ESTOQUE
        public IMongoCollection<StockBatch> StockBatches => Database.GetCollection<StockBatch>("stock_batches");
        public IMongoCollection<StockLoss> StockLosses => Database.GetCollection<StockLoss>("stock_losses");
        #endregion

        #region VENDAS
        public IMongoCollection<Sale> Sales => Database.GetCollection<Sale>("sales");
        public IMongoCollection<SaleCounter> SaleCounters => Database.GetCollection<SaleCounter>("sale_counters");
        #endregion
    }

    public class SaleCounter
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        public string Id { get; set; } = "sale_number";
        public long Value { get; set; } = 0;
    }
}
