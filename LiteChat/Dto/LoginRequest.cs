using System.ComponentModel.DataAnnotations;

namespace LiteChat.Services;

public class LoginRequest
{
    [Required]
    public string PublicKey { get; init; } = string.Empty;
    [Required]
    public string Signature { get; init; } = string.Empty;
}
