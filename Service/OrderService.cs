using CrudWithMongoDB.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudWithMongoDB.Service
{
    public class OrderService : BaseService<Orders>
    {
        private readonly IMongoCollection<Stocks> _stocksCollection;
        private readonly IMongoCollection<Orders> _orderCollection;


        public OrderService(MongoRepository mongoRepository) : base(mongoRepository)
        {
            _stocksCollection = mongoRepository.mongoDatabase.GetCollection<Stocks>(nameof(Stocks));
            _orderCollection = mongoRepository.mongoDatabase.GetCollection<Orders>(nameof(Orders));
        }

        public override async Task<List<Orders>> GetAsync()
        {
            var orders = await base.GetAsync();
            foreach (var order in orders)
            {
                if (!string.IsNullOrEmpty(order.StockId))
                {
                    order.Stock = await _stocksCollection.Find(s => s.Id == order.StockId).FirstOrDefaultAsync();
                }
            }
            return orders;
        }

        public async Task<Orders> GetOrderByStockId(string stockId)
        {
            Orders order = await _orderCollection.Find(o => o.StockId == stockId).FirstOrDefaultAsync();
            return order;
        }

        public async Task<List<Orders>> GetSortedOrdersAsync(string sortBy, bool descending)
        {
            var orders = await GetAsync();
            return descending
                ? orders.OrderByDescending(o => o.GetType().GetProperty(sortBy)?.GetValue(o, null)).ToList()
                : orders.OrderBy(o => o.GetType().GetProperty(sortBy)?.GetValue(o, null)).ToList();
        }

        public override async Task<string> CreateAsync(Orders newOrder)
        {
            var stock = await _stocksCollection.Find(s => s.Id == newOrder.StockId).FirstOrDefaultAsync();
            if (stock == null)
            {
                throw new InvalidOperationException("Stock not found.");
            }

            if (stock.Quantity < newOrder.OrderQuantity)
            {
                throw new InvalidOperationException("Insufficient stock quantity.");
            }

            stock.Quantity -= newOrder.OrderQuantity;
            await _stocksCollection.ReplaceOneAsync(s => s.Id == stock.Id, stock);
            string id = await base.CreateAsync(newOrder);
            return id;
        }

        public override async Task RemoveAsync(string id)
        {
            var order = await GetAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }

            var stock = await _stocksCollection.Find(s => s.Id == order.StockId).FirstOrDefaultAsync();
            if (stock != null)
            {
                stock.Quantity += order.OrderQuantity;
                await _stocksCollection.ReplaceOneAsync(s => s.Id == stock.Id, stock);
            }

            await base.RemoveAsync(id);
        }

        public override async Task<Orders> UpdateAsync(string id, Orders updatedItem)
        {
            throw new NotImplementedException($"Order Can not be Edited.");
        }
    }
}
