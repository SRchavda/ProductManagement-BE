namespace CrudWithMongoDB.Model
{
    public class StockViewModel : Stocks
    {
        public int OrderQuantity { get; set; } = 0;
    }
}
