namespace order_routing.Server.Models
{
    public class OrderLineFulfillment
    {
        public int Id { get; set; }
        public DateOnly CreationDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public int OrderLineId { get; set; }
        public OrderLine OrderLine { get; set; } = null!;
        public int StoreId { get; set; }
        public Store Store { get; set; } = null!;
        public decimal Quantity { get; set; }
    }

    public class OrderlineFulfillmentCreateDTO
    {
        public int OrderLineId { get; set; }
        public int StoreId { get; set; }
        public decimal Quantity { get; set; }
    }

    public class OrderlineFulfillmentGetDTO
    {
        public int Id { get; set; }
        public DateOnly CreationDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public int OrderLineId { get; set; }
        public int StoreId { get; set; }
        public decimal Quantity { get; set; }

    }
}
