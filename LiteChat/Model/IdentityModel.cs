using LiteChat.Services;

namespace LiteChat.Model;

public class IdentityModel
{
    private readonly IIdentityClient client;
    public IdentityModel()
    {
        client = new IdentityClient();
        PrivateKey = Preferences.Get("privateKey", string.Empty);
        Token = Preferences.Get("token", string.Empty);
    }
    public bool IsLoggedIn { get; set; }
    public UserDto? User { get; private set; }
    private string Token;
    private string PrivateKey;


    public async Task<UserDto> LoginAsync(string privateKey)
    {
        Token = await client.LoginAsync(privateKey);
        User = await client.GetUserAsync(Token);
        PrivateKey = privateKey;
        IsLoggedIn = true;
        Preferences.Set("privateKey", PrivateKey);
        Preferences.Set("token", Token);
        Application.Current.MainPage = new MainPage(PrivateKey);
        return User;
    }
    public async Task<UserDto> Register(string privateKey)
    {
        await client.RegisterAsync(privateKey);
        return await LoginAsync(privateKey);
    }
}
