using LiteChat.Dto;

namespace LiteChat.Services;

public interface IChatClient
{
    Task<ICollection<ChatMessageEventDto>> GetMessagesAsync(GetChatQuery body, CancellationToken? cancellationToken = null);
    Task AppendMessageAsync(AppendChatMessage body, CancellationToken? cancellationToken = null);
}