using System.ComponentModel.DataAnnotations;

namespace CrudWithMongoDB.Model
{
    public class Orders : BaseEntity
    {
        public string CutomerName { get; set; }
        public string? StockId { get; set; }
        //private int _orderQuantity;
        [Range(1, int.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public int OrderQuantity { get; set; }
        //{
        //    get => _orderQuantity;
        //    set
        //    {
        //        if (value < 0)
        //        {
        //            throw new ArgumentException("Order quantity cannot be negative.");
        //        }
        //        _orderQuantity = value;
        //    }
        //}
        public Stocks? Stock { get; set; }
    }
}
