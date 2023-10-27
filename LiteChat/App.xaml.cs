using LiteChat.View;

namespace LiteChat;

public partial class App : Application
{
	public App(MainPage mainPage, LoginPage loginPage)
	{
		InitializeComponent();

        string privateKey = Preferences.Get("privateKey", string.Empty);
        if (string.IsNullOrEmpty(privateKey))
			MainPage = loginPage;
		else
           Application.Current.MainPage = new NavigationPage(mainPage);
    }
}
