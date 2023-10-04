using System.ComponentModel.DataAnnotations;

namespace LiteChat.Services;

internal class LoginRequest
{
    [Required]
    public string PublicKey { get; init; } = string.Empty;
    [Required]
    public string Signature { get; init; } = string.Empty;
}
