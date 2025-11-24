using store_api.Entities;

namespace store_api.Dtos.Discounts;

public class DiscountAddDto : IBaseDto<DiscountEntity>
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public double Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DiscountAddDto(Guid productId, double percentage, DateTime startDate, DateTime endDate)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        Percentage = percentage;
        StartDate = startDate;
        EndDate = endDate;
    }

    public DiscountEntity ToEntity()
    {
        return new(Id, ProductId, Percentage, StartDate, EndDate);
    }
}