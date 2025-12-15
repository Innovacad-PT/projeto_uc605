using store_api.Entities;

namespace store_api.Dtos.Discounts;

public class DiscountUpdateDto<T> : IBaseDto<DiscountEntity>
{
    public Guid? ProductId { get; set; }
    public int? Percentage { get; set; }
    public long? StartDate { get; set; }
    public long? EndDate { get; set; }

    public DiscountUpdateDto(Guid? productId, int? percentage, long? startDate, long? endDate)
    {
        ProductId = productId;
        Percentage = percentage;
        StartDate = startDate;
        EndDate = endDate;
    }
}