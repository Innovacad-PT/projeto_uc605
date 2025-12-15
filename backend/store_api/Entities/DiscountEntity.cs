namespace store_api.Entities;

public class DiscountEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Percentage { get; set; }
    public long StartDate { get; set; }
    public long EndDate { get; set; }


    public DiscountEntity()
    {
        
    }
    
    public DiscountEntity(Guid id, Guid productId, int percentage, long startDate, long endDate)
    {
        Id = id;
        ProductId = productId;
        Percentage = percentage;
        StartDate = startDate;
        EndDate = endDate;
    }
}