namespace CrudWithMongoDB.Model
{
    public class GeneralResponse
    {
        public bool success;
        public object? data;
        public string? error;
        public int status = StatusCodes.Status404NotFound;
    }
}
