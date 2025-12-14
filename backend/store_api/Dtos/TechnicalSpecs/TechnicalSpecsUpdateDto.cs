using store_api.Entities;

namespace store_api.Dtos.TechnicalSpecs;

public class TechnicalSpecsUpdateDto<T> : IBaseDto<TechnicalSpecsEntity>
{
    public string? Key { get; set; }
    public string? Value { get; set; }
}
