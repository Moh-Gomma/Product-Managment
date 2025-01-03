namespace ProductFF.Dtos
{
    public class ProductDto
    {
        public record AddProduct(string name , string Description, decimal price, int? CategoryId
);
        public record ViewProduct(int id , string name , string Description, decimal price, string CategoryId);
        public record UpdateProduct(int id ,string name , string Description, decimal price, int CategoryId);
    }
}
