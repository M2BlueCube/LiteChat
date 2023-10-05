using LiteChat.Services;
using LiteChat.View;
using LiteChat.ViewModel;

namespace LiteChat;

public partial class MainPage : ContentPage
{
	private readonly string PrivateKey;

	public MainPage(string privateKey)
	{
		InitializeComponent();
        BindingContext = new MainViewModel(privateKey);
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Remove("privateKey");
        Preferences.Remove("token");
        Application.Current.MainPage = new LoginPage();
    }
}

