using LiteChat.Model;
using LiteChat.Services;
using Newtonsoft.Json;
using System.Windows.Input;

namespace LiteChat.ViewModel;

public class LoginViewModel : BaseViewModel
{
    private readonly IRsaService rsa;
    private IdentityModel Identity { get; set; }
    public ICommand LogInCommand { get; }
    public ICommand RegisterCommand { get; }
    public ICommand GenerateKeyCommand { get; }
    public LoginViewModel()
    {
        Identity = new();
        rsa = new RsaService();
        _message = "defult message";
        _messageColor = Colors.Blue;
        LogInCommand = new AsyncCommandHandler(Login, CanLogin);
        RegisterCommand = new AsyncCommandHandler(Register, CanRegister);
        GenerateKeyCommand = new AsyncCommandHandler(GenerateKey, ()=>true);
    }

    

    public string PrivateKey
    {
        get => _privateKey;
        set => SetField(ref _privateKey, value);
    }
    private string _privateKey;

    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }
    private string _message;

    public Color MessageColor
    {
        get => _messageColor;
        set => SetField(ref _messageColor, value);
    }
    private Color _messageColor;

    private void LogInfo(string message)
    {
        Message = message;
        MessageColor = Colors.Blue;
    }
    private void LogError(string message)
    {
        Message = message;
        MessageColor = Colors.DarkRed;
    }
    private void LogWarning(string message)
    {
        Message = message;
        MessageColor = Colors.DarkGoldenrod;
    }

    private async Task Register()
    {
        try
        {
            var user = await Identity.Register(_privateKey);
            LogInfo("You are successfully Registered.");
            LogInfo($"user : {JsonConvert.SerializeObject(user)}");
        }
        catch (Exception ex)
        {
            LogError(ex.Message);
        }
    }
    private async Task Login()
    {
        try
        {
            var user = await Identity.LoginAsync(_privateKey);
            LogInfo($"user : {JsonConvert.SerializeObject(user)}");
        }
        catch (Exception ex)
        {
            LogError(ex.Message);
        }
    }
    private Task GenerateKey()
    {
        (_, PrivateKey) = rsa.CreateKeys();
        return Task.CompletedTask;
    }
    private bool CanLogin() => true;
    private bool CanRegister() => true;
}
