using System.ComponentModel.DataAnnotations;

namespace LiteChat.Dto;

public class GetChatQuery
{
    [Required]
    [StringLength(63, MinimumLength = 1)]
    [RegularExpression(@"^([0-9A-Fa-f]{8}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{12})$")]
    public string To { get; set; }
    public int Latest { get; set; }
    public int Count { get; set; }

    [Required]
    [StringLength(12, MinimumLength = 1)]
    [RegularExpression(@"20\d{2}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])*")]
    public string Date { get; set; }
}
