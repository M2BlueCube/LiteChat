using System.ComponentModel.DataAnnotations;

namespace LiteChat.Dto;

public class AppendChatMessage
{
    [Required]
    [StringLength(63, MinimumLength = 1)]
    [RegularExpression(@"^([0-9A-Fa-f]{8}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{4}[-]?[0-9A-Fa-f]{12})$")]
    public string To { get; set; }

    [Required]
    [StringLength(1023, MinimumLength = 1)]
    public string Message { get; set; }
}
