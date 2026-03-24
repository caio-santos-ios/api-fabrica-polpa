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
    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        #region CREATE
        public async Task<ResponseApi<Product?>> CreateAsync(Product product)
        {
            try
            {
                await context.Products.InsertOneAsync(product);
                return new(product, 201, "Produto criado com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion

        #region READ
        public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Product> pagination)
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

                List<BsonDocument> results = await context.Products.Aggregate<BsonDocument>(pipeline).ToListAsync();
                List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
                return new(list);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<ResponseApi<dynamic?>> GetByIdAggregateAsync(string id)
        {
            try
            {
                BsonDocument[] pipeline = [
                    new("$match", new BsonDocument { { "_id", new ObjectId(id) }, { "deleted", false } }),
                    new("$addFields", new BsonDocument { { "id", new BsonDocument("$toString", "$_id") } }),
                    new("$project", new BsonDocument { { "_id", 0 } }),
                ];

                BsonDocument? response = await context.Products.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
                dynamic? result = response is null ? null : BsonSerializer.Deserialize<dynamic>(response);
                return result is null ? new(null, 404, "Produto não encontrado") : new(result);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<ResponseApi<Product?>> GetByIdAsync(string id)
        {
            try
            {
                Product? product = await context.Products.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
                return new(product);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<int> GetCountDocumentsAsync(PaginationUtil<Product> pagination)
        {
            List<BsonDocument> pipeline = new()
            {
                new("$match", pagination.PipelineFilter),
                new("$addFields", new BsonDocument { { "id", new BsonDocument("$toString", "$_id") } }),
                new("$project", new BsonDocument { { "_id", 0 } }),
            };
            List<BsonDocument> results = await context.Products.Aggregate<BsonDocument>(pipeline).ToListAsync();
            return results.Count;
        }
        #endregion

        #region UPDATE
        public async Task<ResponseApi<Product?>> UpdateAsync(Product product)
        {
            try
            {
                await context.Products.ReplaceOneAsync(x => x.Id == product.Id, product);
                return new(product, 201, "Produto atualizado com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion

        #region DELETE
        public async Task<ResponseApi<Product>> DeleteAsync(string id)
        {
            try
            {
                Product? product = await context.Products.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
                if (product is null) return new(null, 404, "Produto não encontrado");
                product.Deleted = true;
                product.DeletedAt = DateTime.UtcNow;
                await context.Products.ReplaceOneAsync(x => x.Id == id, product);
                return new(product, 204, "Produto excluído com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
    }
}
