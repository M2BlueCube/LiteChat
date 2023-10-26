using LiteChat.Model;
using LiteChat.Services;
using LiteChat.ViewModel;

namespace LiteChat.View;

public partial class ChatPage : ContentPage
{
    private readonly IUserModel _userModel;
    public ChatPage(IUserModel userModel, UserDto toUserDto)
    {
        _userModel = userModel;
		InitializeComponent();
        var viewModel = new ChatViewModel(_userModel, toUserDto);
        BindingContext = viewModel;
        viewModel.MessagesHeightChanged += ViewModel_MessagesHeightChanged;
    }

    private void ViewModel_MessagesHeightChanged(object sender, EventArgs e)
    {
        ScrollToEnd();
    }

    [Obsolete]
    private void ScrollToEnd()
    {
        var t = new TimeSpan(0, 0, 0, 0, 100);
        Device.StartTimer(t, () =>
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Scroller.ScrollToAsync(0, mList.Height, true);
            });
            return false; // runs again, or false to stop
        });
    }
    //private void ScrollToEnd()
    //{
    //    var t = new TimeSpan(0, 0, 0, 0, 100);
    //    Device.StartTimer(t, ScrollEnd);
    //}

    //private bool ScrollEnd()
    //{
    //    Device.BeginInvokeOnMainThread(() => Scroller.ScrollToAsync(0, mList.Height, true));
    //    return false; // runs again, or false to stop
    //}
}