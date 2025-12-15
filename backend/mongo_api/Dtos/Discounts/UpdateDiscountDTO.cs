using mongo_api.Entities;

namespace mongo_api.Dtos.Discounts;

public class UpdateDiscountDTO
{
    public Guid? ProductId { get; set; }
    public double? Percentage { get; set; }
    public long? StartDate { get; set; }
    public long? EndDate { get; set; }
}