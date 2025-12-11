using store_api.Entities;

namespace store_api.Dtos;

public class TechnicalSpecsAddDto
{
    public Guid TechnicalSpecsId { get; set; }
    public string Value { get; set; }
    public string? Key { get; set; }
    
    public TechnicalSpecsEntity ToEntity()
    {
        return new TechnicalSpecsEntity()
        {
            TechnicalSpecsId = TechnicalSpecsId,
            Key = ""
        };
    }

}