using LiteChat.Dto;
using LiteChat.Services;

namespace LiteChat.Model;

public interface IUserModel
{
    bool IsLoggedIn { get; set; }
    UserDto? User { get; }
    Task<UserDto> LoginAsync(string privateKey);
    Task<UserDto> Register(string privateKey);
    Task<IEnumerable<UserDto>> GetAllUsers();
    void Logout();
    Task SendMessage(string message, UserDto to);
    Task<ICollection<ChatMessageEventDto>> GetChat(UserDto to, int latestId = 0);


}