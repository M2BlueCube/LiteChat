using LiteChat.Model;
using LiteChat.Services;
using LiteChat.View;
using LiteChat.ViewModel;
using Microsoft.Extensions.Logging;

namespace LiteChat;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});



        builder.Services.AddSingleton<IUserModel, UserModel>(); 
        builder.Services.AddSingleton<IClient, Client>();
        builder.Services.AddSingleton<IRsaService, RsaService>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<LoginViewModel>(); 
        builder.Services.AddSingleton<LoginPage>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
