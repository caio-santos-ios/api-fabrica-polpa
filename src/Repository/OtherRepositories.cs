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
    // ─── SALE ─────────────────────────────────────────────────────────────────
    public class SaleRepository(AppDbContext context) : ISaleRepository
    {
        #region CREATE
        public async Task<ResponseApi<Sale?>> CreateAsync(Sale sale)
        {
            try
            {
                await context.Sales.InsertOneAsync(sale);
                return new(sale, 201, "Venda realizada com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion

        #region READ
        public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Sale> pagination)
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
                List<BsonDocument> results = await context.Sales.Aggregate<BsonDocument>(pipeline).ToListAsync();
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
                BsonDocument? response = await context.Sales.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
                dynamic? result = response is null ? null : BsonSerializer.Deserialize<dynamic>(response);
                return result is null ? new(null, 404, "Venda não encontrada") : new(result);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<ResponseApi<Sale?>> GetByIdAsync(string id)
        {
            try
            {
                Sale? sale = await context.Sales.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
                return new(sale);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        public async Task<int> GetCountDocumentsAsync(PaginationUtil<Sale> pagination)
        {
            List<BsonDocument> pipeline = new()
            {
                new("$match", pagination.PipelineFilter),
                new("$addFields", new BsonDocument { { "id", new BsonDocument("$toString", "$_id") } }),
                new("$project", new BsonDocument { { "_id", 0 } }),
            };
            List<BsonDocument> results = await context.Sales.Aggregate<BsonDocument>(pipeline).ToListAsync();
            return results.Count;
        }

        public async Task<long> GetNextSaleNumberAsync()
        {
            SaleCounter result = await context.SaleCounters.FindOneAndUpdateAsync(
                Builders<SaleCounter>.Filter.Eq(c => c.Id, "sale_number"),
                Builders<SaleCounter>.Update.Inc(c => c.Value, 1L),
                new FindOneAndUpdateOptions<SaleCounter> { IsUpsert = true, ReturnDocument = ReturnDocument.After }
            );
            return result.Value;
        }

        public async Task<ResponseApi<List<Sale>>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            try
            {
                List<Sale> sales = await context.Sales
                    .Find(x => !x.Deleted && x.CreatedAt >= from && x.CreatedAt <= to.AddDays(1) && x.Status == Enums.Product.SaleStatusEnum.Completed)
                    .ToListAsync();
                return new(sales);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion

        #region UPDATE
        public async Task<ResponseApi<Sale?>> UpdateAsync(Sale sale)
        {
            try
            {
                await context.Sales.ReplaceOneAsync(x => x.Id == sale.Id, sale);
                return new(sale, 201, "Venda atualizada com sucesso");
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
        #endregion
    }

    // ─── CUSTOMER ─────────────────────────────────────────────────────────────
    public class CustomerRepository(AppDbContext context) : ICustomerRepository
    {
        public async Task<ResponseApi<Customer?>> CreateAsync(Customer customer)
        {
            try { await context.Customers.InsertOneAsync(customer); return new(customer, 201, "Cliente criado com sucesso"); }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Customer> pagination)
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
                List<BsonDocument> results = await context.Customers.Aggregate<BsonDocument>(pipeline).ToListAsync();
                List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
                return new(list);
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
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
                BsonDocument? response = await context.Customers.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
                dynamic? result = response is null ? null : BsonSerializer.Deserialize<dynamic>(response);
                return result is null ? new(null, 404, "Cliente não encontrado") : new(result);
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<Customer?>> GetByIdAsync(string id)
        {
            try { Customer? c = await context.Customers.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync(); return new(c); }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<Customer?>> GetByCpfAsync(string cpf)
        {
            try { Customer? c = await context.Customers.Find(x => x.Cpf == cpf && !x.Deleted).FirstOrDefaultAsync(); return new(c); }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<int> GetCountDocumentsAsync(PaginationUtil<Customer> pagination)
        {
            List<BsonDocument> pipeline = new() { new("$match", pagination.PipelineFilter), new("$count", "count") };
            List<BsonDocument> results = await context.Customers.Aggregate<BsonDocument>(pipeline).ToListAsync();
            return results.Count > 0 ? results[0]["count"].AsInt32 : 0;
        }

        public async Task<ResponseApi<Customer?>> UpdateAsync(Customer customer)
        {
            try { await context.Customers.ReplaceOneAsync(x => x.Id == customer.Id, customer); return new(customer, 201, "Cliente atualizado com sucesso"); }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<Customer>> DeleteAsync(string id)
        {
            try
            {
                Customer? c = await context.Customers.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
                if (c is null) return new(null, 404, "Cliente não encontrado");
                c.Deleted = true; c.DeletedAt = DateTime.UtcNow;
                await context.Customers.ReplaceOneAsync(x => x.Id == id, c);
                return new(c, 204, "Cliente excluído com sucesso");
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }
    }

    // ─── SUPPLIER ─────────────────────────────────────────────────────────────
    public class SupplierRepository(AppDbContext context) : ISupplierRepository
    {
        public async Task<ResponseApi<Supplier?>> CreateAsync(Supplier supplier)
        {
            try { await context.Suppliers.InsertOneAsync(supplier); return new(supplier, 201, "Fornecedor criado com sucesso"); }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<List<dynamic>>> GetAllAsync(PaginationUtil<Supplier> pagination)
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
                List<BsonDocument> results = await context.Suppliers.Aggregate<BsonDocument>(pipeline).ToListAsync();
                List<dynamic> list = results.Select(doc => BsonSerializer.Deserialize<dynamic>(doc)).ToList();
                return new(list);
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
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
                BsonDocument? response = await context.Suppliers.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();
                dynamic? result = response is null ? null : BsonSerializer.Deserialize<dynamic>(response);
                return result is null ? new(null, 404, "Fornecedor não encontrado") : new(result);
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<Supplier?>> GetByIdAsync(string id)
        {
            try { Supplier? s = await context.Suppliers.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync(); return new(s); }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<int> GetCountDocumentsAsync(PaginationUtil<Supplier> pagination)
        {
            List<BsonDocument> pipeline = new() { new("$match", pagination.PipelineFilter), new("$count", "count") };
            List<BsonDocument> results = await context.Suppliers.Aggregate<BsonDocument>(pipeline).ToListAsync();
            return results.Count > 0 ? results[0]["count"].AsInt32 : 0;
        }

        public async Task<ResponseApi<Supplier?>> UpdateAsync(Supplier supplier)
        {
            try { await context.Suppliers.ReplaceOneAsync(x => x.Id == supplier.Id, supplier); return new(supplier, 201, "Fornecedor atualizado com sucesso"); }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }

        public async Task<ResponseApi<Supplier>> DeleteAsync(string id)
        {
            try
            {
                Supplier? s = await context.Suppliers.Find(x => x.Id == id && !x.Deleted).FirstOrDefaultAsync();
                if (s is null) return new(null, 404, "Fornecedor não encontrado");
                s.Deleted = true; s.DeletedAt = DateTime.UtcNow;
                await context.Suppliers.ReplaceOneAsync(x => x.Id == id, s);
                return new(s, 204, "Fornecedor excluído com sucesso");
            }
            catch { return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); }
        }
    }
}
