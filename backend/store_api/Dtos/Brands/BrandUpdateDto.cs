using store_api.Entities;

namespace store_api.Dtos.Brands;

public class BrandUpdateDto<T> : IBaseDto<BrandEntity>
{
    public String? Name { get; set; }
}