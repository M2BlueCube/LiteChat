using LiteChat.Model;
using LiteChat.Services;
using System.Collections.ObjectModel;

namespace LiteChat.ViewModel;

public class MainViewModel : BaseViewModel
{
    public ObservableCollection<ChatUserViewModel> MyFriends { get; set; } = new ObservableCollection<ChatUserViewModel>();

    private readonly IUserModel _userModel;

    public MainViewModel(IUserModel userModel)
    {
        _userModel = userModel;
        _userName = userModel.User?.UserName;
        _userId = userModel.User?.UserId;
        _ = UpdateFriends();
    }

    private async Task UpdateFriends()
    {
        if (_userModel.IsLoggedIn)
        {
            UserName = _userModel.User?.UserName;
            UserId = _userModel.User?.UserId;
            //MyFriends.Clear();
            var users = await _userModel.GetAllUsers();
            foreach (var user in users)
                if (user.UserId != _userModel.User.UserId)
                    MyFriends.Add(new(_userModel, user));
        }
        else
        {
            var t = new TimeSpan(0, 0, 0, 0, 1000);
            Device.StartTimer(t, () =>
            {
                _ = UpdateFriends();
                return false; // runs again, or false to stop
            });
        }
    }
    public string UserName
    {
        get => _userName;
        set => SetField(ref _userName, value);
    }
    private string _userName;
    public string UserId
    {
        get => _userId;
        set => SetField(ref _userId, value);
    }
    private string _userId;
}
