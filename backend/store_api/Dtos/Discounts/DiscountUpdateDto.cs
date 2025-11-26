using store_api.Entities;

namespace store_api.Dtos.Discounts;

public class DiscountUpdateDto : IBaseDto<DiscountEntity>
{
    public Guid? ProductId { get; set; }
    public int? Percentage { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public DiscountUpdateDto(Guid? productId, int? percentage, DateTime? startDate, DateTime? endDate)
    {
        ProductId = productId;
        Percentage = percentage;
        StartDate = startDate;
        EndDate = endDate;
    }
}