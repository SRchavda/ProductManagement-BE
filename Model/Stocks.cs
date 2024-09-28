namespace CrudWithMongoDB.Model
{
    public class Stocks : BaseEntity
    {
        public string Name { get; set; } = null!;
        public int Quantity { get; set; } = 0;
    }
}
