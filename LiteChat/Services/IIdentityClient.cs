namespace LiteChat.Services;

public interface IIdentityClient
{
    Task RegisterAsync(string privateKey, CancellationToken? cancellationToken = null);
    Task<string> LoginAsync(string privateKey, CancellationToken? cancellationToken = null);
    Task<UserDto> GetUserNameAsync(string token, CancellationToken? cancellationToken = null);
}