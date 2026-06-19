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
        public bool FirstVerification { get; set; } = false;
        public bool SecondVerification { get; set; } = false;
        public FulfillmentTransportMethod fulfillmentTransportMethod { get; set; } = FulfillmentTransportMethod.CompanyDriver;
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
        public bool FirstVerification { get; set; }
        public bool SecondVerification { get; set; }
        public FulfillmentTransportMethod fulfillmentTransportMethod { get; set; } = FulfillmentTransportMethod.CompanyDriver;

    }

    public enum FulfillmentTransportMethod
    {
        CompanyDriver,
        SecondWay,
        ThirdWay
    }
}
