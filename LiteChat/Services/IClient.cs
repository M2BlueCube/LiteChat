using LiteChat.Dto;

namespace LiteChat.Services;

public interface IClient
{
   
    Task RegisterAsync(string privateKey, string userName, CancellationToken? cancellationToken = null);
    Task<string> LoginAsync(string privateKey, CancellationToken? cancellationToken = null);
    Task<UserDto> GetUserAsync(CancellationToken? cancellationToken = null);
    Task<IEnumerable<UserDto>> GetAllUsersAsync( CancellationToken? cancellationToken = null);


    Task<ICollection<ChatMessageEventDto>> GetMessagesAsync(GetChatQuery body, CancellationToken? cancellationToken = null);
    Task AppendMessageAsync(AppendChatMessage body, CancellationToken? cancellationToken = null);
}