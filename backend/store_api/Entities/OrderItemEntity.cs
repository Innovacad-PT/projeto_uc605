namespace store_api.Entities;

public class OrderItemEntity
{
    
    public int Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    
    public OrderItemEntity() { }

    public OrderItemEntity(int id, Guid productId, int quantity, decimal unitPrice)
    {
        Id = id;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}