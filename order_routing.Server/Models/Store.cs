namespace order_routing.Server.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string StoreDescription { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
        public ICollection<OrderLineFulfillment> OrderLineFulfillments { get; set; } = new List<OrderLineFulfillment>();
        public ICollection<User> Users { get; set; } = new List<User>();

    }

    public class StoreCreateDTO
    {
        public string StoreDescription { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class StoreGetDTO
    {
        public int Id { get; set; }
        public string StoreDescription { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
