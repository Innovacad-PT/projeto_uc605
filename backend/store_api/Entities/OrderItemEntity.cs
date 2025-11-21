namespace store_api.Entities;

public class OrderItemEntity
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}