namespace CrudWithMongoDB.Model
{
    public class Orders : BaseEntity
    {
        public string CutomerName { get; set; }
        public string? StockId { get; set; }
        public int OrderQuantity { get; set; } = 0;
        public Stocks? Stock { get; set; }
    }
}
