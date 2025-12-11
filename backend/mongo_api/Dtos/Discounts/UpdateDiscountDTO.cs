using mongo_api.Entities;

namespace mongo_api.Dtos.Discounts;

public class UpdateDiscountDTO(double? percentage, DateTime? startTime, DateTime? endTime)
{
    public readonly double? Percentage = percentage;
    public readonly DateTime? StartTime = startTime;
    public readonly DateTime? EndTime = endTime;

}