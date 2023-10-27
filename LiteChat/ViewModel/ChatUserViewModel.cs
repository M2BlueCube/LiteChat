using LiteChat.Model;
using LiteChat.Services;
using LiteChat.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LiteChat.ViewModel;

public class ChatUserViewModel : BaseViewModel
{
    private readonly IUserModel _userModel;
    private readonly UserDto _toUser;

    public ICommand OpenChatBoxCommand { get; }
    public ChatUserViewModel(IUserModel userModel, UserDto toUser)
    {
        _userModel = userModel;
        _toUser = toUser;
        OpenChatBoxCommand = new AsyncCommandHandler(OpenChatBox, () => true);
    }

    private Task OpenChatBox()
    {
        Application.Current.MainPage.Navigation.PushAsync(new ChatPage(_userModel,_toUser));
        return Task.CompletedTask;
    }

    public string UserName => _toUser.UserName;
    public string UserId => _toUser.UserId;
}
