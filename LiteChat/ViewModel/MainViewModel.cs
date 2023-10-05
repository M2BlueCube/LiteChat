using LiteChat.Services;

namespace LiteChat.ViewModel;

public class MainViewModel : BaseViewModel
{
    private readonly IIdentityClient client;
    private readonly string _privateKey;
    public MainViewModel(UserDto user) : base()
    {
        _userName = user.UserName;
        _userId = user.UserId;
        _publicKey = user.PublicKey;
    }
    public MainViewModel(string privateKey) : base()
    {
        client = new IdentityClient();
        _privateKey = privateKey;
        _ = UpdateUserInfoAsync();
    }

    private async Task UpdateUserInfoAsync()
    {
        var token = await client.LoginAsync(_privateKey);
        var user = await client.GetUserAsync(token);
        UserName = user.UserName;
        UserId = user.UserId;
        PublicKey = user.PublicKey;
    }

    public string UserId
    {
        get => _userId;
        set => SetField(ref _userId, value);
    }
    private string _userId;

    public string UserName
    {
        get => _userName;
        set => SetField(ref _userName, value);
    }
    private string _userName;
    public string PublicKey
    {
        get => _publicKey;
        set => SetField(ref _publicKey, value);
    }
    private string _publicKey;

}
