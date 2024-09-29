using CrudWithMongoDB.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CrudWithMongoDB.Service
{
    public class StocksService : BaseService<Stocks>
    {
        private readonly IMongoCollection<Stocks> _collection;
        private readonly OrderService _orderService;

        public StocksService(MongoRepository mongoRepository, IMongoCollection<Stocks> collection, OrderService orderService) : base(mongoRepository)
        {
            _collection = collection;
            _orderService = orderService;
        }

        public async override Task<List<Stocks>> GetAsync()
        {
            var stocks = await base.GetAsync();
            foreach (var stock in stocks)
            {
                if (!string.IsNullOrEmpty(stock.Id))
                {
                    var oQty = await _orderService.GetOrderByStockId(stock.Id);
                    stock.OrderQuantity = oQty == null ? 0 : oQty.OrderQuantity;
                }
            }
            return stocks;
        }

        public override async Task<string> CreateAsync(Stocks newItem)
        {
            if (newItem.Quantity <= 0)
            {
                throw new InvalidOperationException("Stock quantity cannot be zero or negative.");
            }

            var duplicateStock = await _collection.Find(s => s.Name == newItem.Name).FirstOrDefaultAsync();
            if (duplicateStock != null)
            {
                throw new InvalidOperationException("A stock with this name already exists.");
            }

            string id = await base.CreateAsync(newItem);
            return id;
        }

        public override async Task<Stocks> UpdateAsync(string id, Stocks updatedItem)
        {
            if (updatedItem.Quantity <= 0)
            {
                throw new InvalidOperationException("Stock quantity cannot be zero or negative.");
            }

            var existingStock = await GetAsync(id);
            if (existingStock == null)
            {
                throw new KeyNotFoundException($"Stock with ID {id} not found.");
            }

            var duplicateStock = await _collection.Find(s => s.Name == updatedItem.Name && s.Id != id).FirstOrDefaultAsync();
            if (duplicateStock != null)
            {
                throw new InvalidOperationException("A stock with this name already exists.");
            }

            var data = await base.UpdateAsync(id, updatedItem);
            return data;
        }

        public override async Task RemoveAsync(string id)
        {
            var stock = await GetAsync(id);

            if (stock == null)
            {
                throw new KeyNotFoundException($"Stock with ID {id} not found.");
            }

            var order = await _orderService.GetOrderByStockId(id);
            
            if (order != null)
            {
                throw new InvalidOperationException("Cannot delete stock with a non-zero order value.");
            }

            await base.RemoveAsync(id);
        }
    }
}
