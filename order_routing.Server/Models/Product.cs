namespace order_routing.Server.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }
}
