namespace order_routing.Server.Models
{
    public class OrderLine
    {
        public int Id { get; set; }
        public DateOnly CreationDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int StoreId { get; set; }
        public Store Store { get; set; } = null!;
        public decimal Amount { get; set; }
        public OrderLineStatus OrderLineStatus { get; set; } = OrderLineStatus.Requested;
        public ICollection<OrderLineFulfillment> OrderLineFulfillment { get; set; } = new List<OrderLineFulfillment>();
        
    }

    public class OrderLineGetDTO
    {
        public int Id { get; set; }
        public DateOnly CreationDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public decimal Amount { get; set; }
        public OrderLineStatus OrderLineStatus { get; set; } = OrderLineStatus.Requested;
        public ICollection<OrderlineFulfillmentGetDTO> OrderLineFulfillment { get; set; } = new List<OrderlineFulfillmentGetDTO>();
    }

    public class OrderLineCreateDTO
    {
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public decimal Amount { get; set; }
    }

    public enum OrderLineStatus
    {
        Requested,
        AwaitForConfirmation,
        PartiallyFullfilled,
        Completed
    }
}
