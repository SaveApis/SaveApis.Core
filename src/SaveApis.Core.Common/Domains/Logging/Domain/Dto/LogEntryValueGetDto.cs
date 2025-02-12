using SaveApis.Core.Common.Domains.Common.Domain.VO;

namespace SaveApis.Core.Common.Domains.Logging.Domain.Dto;

public class LogEntryValueGetDto
{
    public Id Id { get; set; } = Id.Empty;
    public Name AttributeName { get; set; } = Name.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}
