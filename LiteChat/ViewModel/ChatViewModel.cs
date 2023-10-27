using LiteChat.Model;
using LiteChat.Services;
using LiteChat.View;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Cryptography;
using System.Windows.Input;

namespace LiteChat.ViewModel;

public class ChatViewModel : BaseViewModel
{
    public ObservableCollection<MessageViewModel> Messages { get; set; } = new ObservableCollection<MessageViewModel>();
    public ICommand SendMessageCommand { get; }

    private readonly IUserModel _userModel;
    private readonly UserDto _toUserDto;
    private int LatestId = 0;

    public event EventHandler MessagesHeightChanged;


    public string ToUserName
    {
        get => _toUserName;
        set => SetField(ref _toUserName, value);
    }
    private string _toUserName;

    public ChatViewModel(IUserModel userModel, UserDto toUserDto)
    {
        _userModel = userModel;
        _toUserDto = toUserDto;
        _toUserName = toUserDto.UserName;
        SendMessageCommand = new AsyncCommandHandler(SendMessage, () => true);


        StartGetNewMessagesTimer();
    }

    [Obsolete]
    private void StartGetNewMessagesTimer()
    {
        var t = new TimeSpan(0, 0, 0, 0, 1000);
        Device.StartTimer(t,  () =>
        {
            if (waitingResponse)
                return true;
            _ = GetMessages();
            return true; // runs again, or false to stop
        });
    }
    private bool waitingResponse = false;
    private async Task GetMessages()
    {
        try
        {
            waitingResponse = true;
            var newMessages = await _userModel.GetChat(_toUserDto, LatestId);

            foreach (var item in newMessages)
            {
                LatestId = LatestId < item.Id ? item.Id : LatestId;
                Messages.Add(new(new() { Text = item.Message, IsMine = item.From == Guid.Parse(_userModel.User.UserId), Timestamp = item.OccurredAt.DateTime }));
            }

        }
        catch (Exception ex)
        {
            Messages.Add(new(new() { Text = ex.Message, IsMine = true, Timestamp = DateTime.Now }));
        }
        finally { waitingResponse = false; }
        MessagesHeightChanged.Invoke(this, new EventArgs());
    }

    private async Task SendMessage()
    {
        try
        {
            await _userModel.SendMessage(_newMessage, _toUserDto);
            //Messages.Add(new(new() { Text = _newMessage, IsMine = true, Timestamp = DateTime.Now }));
            NewMessage = "";
        }
        catch (Exception ex)
        {
            Messages.Add(new(new() { Text = ex.Message, IsMine = true, Timestamp = DateTime.Now }));
        }
        MessagesHeightChanged.Invoke(this, new EventArgs());
    }

    public string NewMessage
{
        get => _newMessage;
        set => SetField(ref _newMessage, value);
    }
    private string _newMessage;
}
