namespace store_api.Entities;

public class DiscountEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DiscountEntity(Guid Id, Guid ProductId, int Percentage, DateTime StartDate, DateTime EndDate)
    {
        this.Id = Id;
        this.ProductId = ProductId;
        this.Percentage = Percentage;
        this.StartDate = StartDate;
        this.EndDate = EndDate;
    }
}