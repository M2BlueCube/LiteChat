using LiteChat.Model;
using LiteChat.ViewModel;

namespace LiteChat.View;

public partial class ChatBox : ContentView
{
    ChatBoxViewModel ViewModel { get; set; }

    public event EventHandler MessagesHeightChanged;
    public ChatBox()
    {
        InitializeComponent();
        ViewModel = new ChatBoxViewModel();
        BindingContext = ViewModel;
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
    private void Button_Clicked(object sender, EventArgs e)
    {
        var h = mList.Height;
        ViewModel.UserName = $"Start ... h = {h}";
        ViewModel.Messages.Add(new MessageViewModel(new Message() { Text = messageEntry.Text, From="Morteza" ,Timestamp = DateTime.Now }));
        messageEntry.Text = "";
        messageEntry.Focus();
        ScrollToEnd();
    }
}

