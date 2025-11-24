namespace store_api.Entities;

/*public class DiscountEntity
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public float DiscountPercentage { get; set; }
    public string DueAt { get; set; }

    public DiscountEntity(string code, float discountPercentage, string dueAt)
    {
        Id = Guid.NewGuid();
        Code = code;
        DiscountPercentage = discountPercentage;
        DueAt = dueAt;
    }

    public bool IsDue()
    {
        return DateTime.Now.CompareTo(DueAt) > 0;
    }

    public float GetValueWithDiscount(float value)
    {
        return value - (value * DiscountPercentage);
    }
}*/

public class DiscountEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public double Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DiscountEntity(Guid Id, Guid ProductId, double Percentage, DateTime StartDate, DateTime EndDate)
    {
        this.Id = Id;
        this.ProductId = ProductId;
        this.Percentage = Percentage;
        this.StartDate = StartDate;
        this.EndDate = EndDate;
    }
}