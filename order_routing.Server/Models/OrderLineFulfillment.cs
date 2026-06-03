namespace order_routing.Server.Models
{
    public class OrderLineFulfillment
    {
        public int Id { get; set; }
        public int OrderLineId { get; set; }
        public OrderLine OrderLine { get; set; } = null!;
        public int StoreId { get; set; }
        public Store Store { get; set; } = null!;
        public decimal Quantity { get; set; }
    }
}
