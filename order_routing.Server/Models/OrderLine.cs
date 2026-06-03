namespace order_routing.Server.Models
{
    public class OrderLine
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int StoreId { get; set; }
        public Store Store { get; set; } = null!;
        public decimal Amount { get; set; }
        public OrderLineStatus OrderLineStatus { get; set; } = OrderLineStatus.Requested;
        public ICollection<OrderLineFulfillment> OrderLineFulfillment { get; set; } = new List<OrderLineFulfillment>();
        
    }

    public enum OrderLineStatus
    {
        Requested,
        AwaitForConfirmation,
        PartiallyFullfilled,
        Completed
    }
}
