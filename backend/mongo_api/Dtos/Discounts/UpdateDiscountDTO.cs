using mongo_api.Entities;

namespace mongo_api.Dtos.Discounts;

public class UpdateDiscountDTO(Guid? productId, double? percentage, DateTime? startTime, DateTime? endTime)
{
    public readonly Guid? ProductId = productId;
    public readonly double? Percentage = percentage;
    public readonly DateTime? StartTime = startTime;
    public readonly DateTime? EndTime = endTime;

}