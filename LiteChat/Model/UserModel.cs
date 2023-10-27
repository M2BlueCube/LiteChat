using LiteChat.Dto;
using LiteChat.Services;
using System.Security.Cryptography;

namespace LiteChat.Model;

public class UserModel : IUserModel
{
    public bool IsLoggedIn { get; set; }
    public UserDto? User { get; private set; }

    private readonly IClient _client;
    private readonly IRsaService  _rsa;
    private string PrivateKey;

    public UserModel(IClient client, IRsaService rsa)
    {
        _client = client;
        _rsa = rsa;
        PrivateKey = Preferences.Get("privateKey", string.Empty);
        if ( !string.IsNullOrEmpty( PrivateKey))
            _ = LoginAsync(PrivateKey);
    }

    public async Task<UserDto> LoginAsync(string privateKey)
    {
        _ = await _client.LoginAsync(privateKey);
        User = await _client.GetUserAsync();
        PrivateKey = privateKey;
        IsLoggedIn = true;
        Preferences.Set("privateKey", PrivateKey);
        return User;
    }
    public async Task<UserDto> Register(string privateKey, string userName)
    {
        await _client.RegisterAsync(privateKey, userName);
        return await LoginAsync(privateKey);
    }
    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        if (!IsLoggedIn)
            throw new Exception("you are not logged in!");
        return await _client.GetAllUsersAsync();
    }

    public void Logout()
    {
        Preferences.Remove("privateKey");
        PrivateKey = string.Empty;
        User = null;
        IsLoggedIn = false;
    }

    public async Task SendMessage(string message, UserDto to)
    {
        if (!IsLoggedIn)
            throw new Exception("you are not logged in!");
        var body = new AppendChatMessage()
        {
            Message = message ,//_rsa.Encrypt(message, to.PublicKey),
            To = to.UserId,
           
        };
        await _client.AppendMessageAsync(body);
    }

    public async Task<ICollection<ChatMessageEventDto>> GetChat(UserDto to, int latestId = 0)
    {
        if (!IsLoggedIn)
            throw new Exception("you are not logged in!");
        GetChatQuery body = new()
        {
            Count = 100,
            Date = DateOnly.FromDateTime(DateTime.Now).ToString(),
            Latest = latestId,
            To = to.UserId
        };

        ICollection<ChatMessageEventDto> chatMessageEvents = await _client.GetMessagesAsync(body);
        foreach (var item in chatMessageEvents)
        {
            //item.Message = _rsa.Decrypt(item.Message, PrivateKey);
        }
        return chatMessageEvents;

    }
}
