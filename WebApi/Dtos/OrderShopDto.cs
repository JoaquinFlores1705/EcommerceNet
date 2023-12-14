namespace WebApi.Dtos
{
    public class OrderShopDto
    {
        public string CartShopId { get; set; }
        public int ShippingType { get; set; }
        public DirectionDto ShippingDirection { get; set; }
    }
}
