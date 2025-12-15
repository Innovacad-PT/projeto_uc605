using mongo_api.Entities;

namespace mongo_api.Dtos.TechnicalSpecs;

public class UpdateTechSpecDTO
{
    public string? Key { get; set; }
    public string? Value { get; set; }
}