using LiteChat.Model;
using LiteChat.View;
using LiteChat.ViewModel;

namespace LiteChat;

public partial class MainPage : ContentPage
{
    private readonly IUserModel _userModel;
    public MainPage(IUserModel userModel)
    {
        _userModel = userModel;
        InitializeComponent();
        BindingContext = new MainViewModel(userModel);
    }

    private async  void OnCounterClicked(object sender, EventArgs e)
    {
        SemanticScreenReader.Announce(CounterBtn.Text);
        var x = await _userModel.GetAllUsers();
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        _userModel.Logout();
        Application.Current.MainPage = new LoginPage(new (_userModel));
    }
}

