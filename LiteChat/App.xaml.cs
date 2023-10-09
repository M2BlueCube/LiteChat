using LiteChat.View;

namespace LiteChat;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        //string privateKey = Preferences.Get("privateKey", string.Empty);
        //if (string.IsNullOrEmpty(privateKey))
		//	MainPage = new LoginPage();
		//else
            MainPage = new MainPage("");
    }
}
