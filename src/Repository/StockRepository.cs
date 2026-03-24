using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using PulpaAPI.src.Configuration;
using PulpaAPI.src.Interfaces;
using PulpaAPI.src.Models;
using PulpaAPI.src.Models.Base;
using PulpaAPI.src.Shared.Utils;

namespace PulpaAPI.src.Repository
{
    public class StockRepository(AppDbContext context) : IStockRepository
    {
        #region CREATE
        public async Task<ResponseApi<StockBatch?>> CreateAsync(StockBatch batch)
        {
            try
            {
                await context.StockBatches.InsertOneAsync(batch);
                return new(batch, 201, "Lote registrado com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<ResponseApi<StockLoss?>> CreateLossAsync(StockLoss loss)
        {
            try
            {
                await context.StockLosses.InsertOneAsync(loss);
                return new(loss, 201, "Perda registrada com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion

        #region READ
        public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<StockBatch> pagination)
        {
            try
            {
                List<BsonDocument> pipeline = new()
                {
                    new("$match", pagination.PipelineFilter),
                    new("$sort", pagination.PipelineSort),
                    new("$skip", pagination.Skip),
                    new("$limit", pagination.Limit),
                    new("$addFields", new BsonDocument { { "id", new BsonDocument("$toString", "$_id") } }),
                    new("$project", new BsonDocument { { "_id", 0 } }),
                };

                List<BsonDocument> results = await context.StockBatches.Aggregate<BsonDocument>(pipeline).ToListAsync();
                List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
                return new(list);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<ResponseApi<StockBatch?>> GetByIdAsync(string id)
        {
            try
            {
                StockBatch? batch = await context.StockBatches.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
                return new(batch);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        // FEFO: ordena por expiryDate ASC — vence primeiro, sai primeiro
        public async Task<ResponseApi<List<StockBatch>>> GetByProductFEFOAsync(string productId)
        {
            try
            {
                List<StockBatch> batches = await context.StockBatches
                    .Find(x => x.ProductId == productId && !x.Deleted && x.Active && x.Quantity > 0 && x.ExpiryDate >= DateTime.UtcNow)
                    .SortBy(x => x.ExpiryDate)
                    .ToListAsync();
                return new(batches);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<ResponseApi<List<StockBatch>>> GetNearExpiryAsync(int days = 7)
        {
            try
            {
                DateTime threshold = DateTime.UtcNow.AddDays(days);
                List<StockBatch> batches = await context.StockBatches
                    .Find(x => !x.Deleted && x.Active && x.Quantity > 0 && x.ExpiryDate >= DateTime.UtcNow && x.ExpiryDate <= threshold)
                    .SortBy(x => x.ExpiryDate)
                    .ToListAsync();
                return new(batches);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<ResponseApi<List<StockBatch>>> GetExpiredAsync()
        {
            try
            {
                List<StockBatch> batches = await context.StockBatches
                    .Find(x => !x.Deleted && x.Quantity > 0 && x.ExpiryDate < DateTime.UtcNow)
                    .SortBy(x => x.ExpiryDate)
                    .ToListAsync();
                return new(batches);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<int> GetCountDocumentsAsync(PaginationUtil<StockBatch> pagination)
        {
            List<BsonDocument> pipeline = new()
            {
                new("$match", pagination.PipelineFilter),
                new("$addFields", new BsonDocument { { "id", new BsonDocument("$toString", "$_id") } }),
                new("$project", new BsonDocument { { "_id", 0 } }),
            };
            List<BsonDocument> results = await context.StockBatches.Aggregate<BsonDocument>(pipeline).ToListAsync();
            return results.Count;
        }

        public async Task<int> GetTotalStockAsync(string productId)
        {
            try
            {
                List<StockBatch> batches = await context.StockBatches
                    .Find(x => x.ProductId == productId && !x.Deleted && x.Active && x.Quantity > 0 && x.ExpiryDate >= DateTime.UtcNow)
                    .ToListAsync();
                return batches.Sum(b => b.Quantity);
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region UPDATE
        public async Task<ResponseApi<StockBatch?>> UpdateAsync(StockBatch batch)
        {
            try
            {
                await context.StockBatches.ReplaceOneAsync(x => x.Id == batch.Id, batch);
                return new(batch, 201, "Lote atualizado com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
    }
}
