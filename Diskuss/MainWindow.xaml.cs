using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Diskuss {
    public partial class MainWindow : Window {
        DiskussAPI _diskuss;

        public MainWindow() {
            InitializeComponent();
            _diskuss = new DiskussAPI(grdConversations.Children.OfType<UserChannelObject>().ToList(), grdUsersList.Children.OfType<User>().ToList(), grdChannelsList.Children.OfType<Channel>().ToList());

            btnLogin.Click += btnLogin_Click;

            grdChannelsList.Destination = grdConversations;
            grdUsersList.Destination = grdConversations;


            grdConversations.OnConversationSelectedChange += GrdConversations_OnConversationSelectedChange;
            _diskuss.OnLogin += _diskuss_OnLogin;
            _diskuss.OnUsers += _diskuss_OnUsers;
            _diskuss.OnChannels += _diskuss_OnChannels;
            _diskuss.OnNewPrivateMessage += _diskuss_OnNewPrivateMessage;
            _diskuss.OnSendPrivateMessage += _diskuss_OnSendPrivateMessage;
        }

        private void _diskuss_OnSendPrivateMessage(object sender, Message _msg) {
            grdConversations.SelectedConversation.addMessage(_msg);
            addMessage(_msg);
        }

        private void _diskuss_OnNewPrivateMessage(object sender, Message _msg)
        {
            UserChannelObject _ucUser = grdUsersList.Children.OfType<UserChannelObject>().FirstOrDefault(x => x.Name == _msg.Sender);
            if (_ucUser != null) {
                grdUsersList.remove(_ucUser);
                grdConversations.add(_ucUser);
            }

            Conversation _convUser = grdConversations.getConversation(_msg.Sender);
            _convUser.addMessage(_msg);
            if (_convUser != grdConversations.SelectedConversation) {
                _convUser.Notifications = ++_convUser.Notifications;
            } else {
                addMessage(_msg);
            }
        }

        private void GrdConversations_OnConversationSelectedChange(object sender, Conversation _conv) {
            grdChat.Children.Clear();
            grdChat.RowDefinitions.Clear();
            tbxMessage.Text = null;
            if ((lblChatName.Content = _conv) == null) {
                lblChatName.Content = "";
                msgForm.Visibility = Visibility.Collapsed;
            } else {
                lblChatName.Content = _conv.Object.Name.Length > 25 ? $"{_conv.Object.Name.Substring(0, 25)}..." : _conv.Object.Name;
                msgForm.Visibility = Visibility.Visible;
                _conv.Messages.ForEach(e => { addMessage(e); });
            }
        }

        private void _diskuss_OnLogin(object sender, EventArgs e) {
            grdLogin.Visibility = Visibility.Collapsed;
            lblNick.Content = _diskuss.Me.Name.Length > 13 ? $"{_diskuss.Me.Name.Substring(0, 13)}..." : _diskuss.Me.Name;
        }

        private void _diskuss_OnChannels(object sender, List<UserChannelObject> _lObjects) {
            grdChannelsList.setChildren(_lObjects);
        }

        private void _diskuss_OnUsers(object sender, List<UserChannelObject> _lObject) {
            grdUsersList.setChildren(_lObject);
        }
        
        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            _diskuss.Login(tbxForm.Text.Length > 0 ? tbxForm.Text : "Anonymous", true);
        }

        private void btn_ChannelClick(object sender, EventArgs e)
        {
            cancelForm();
            _diskuss.JoinChannel(tbxForm.Text);
        }

        private void grdAddChannel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            showForm("Join", btn_ChannelClick, true);
        }

        private void showForm(string strBtnContent, RoutedEventHandler _eventClick, bool bCross)
        {
            grdLogin.Visibility = Visibility.Visible;
            tbxForm.Text = null;
            grdBtn.Children.Clear();
            Button _btn = new Button()
            {
                Content = strBtnContent,
                Style = Resources["BtnYellow"] as Style,
                FontFamily = btnLogin.FontFamily,
                FontSize = btnLogin.FontSize,
                Foreground = btnLogin.Foreground,
                BorderBrush = btnLogin.BorderBrush,
                Background = btnLogin.Background,
                FontWeight = btnLogin.FontWeight,
                Width = btnLogin.Width,
            };

            Cross.Visibility = bCross ? Visibility.Visible : Visibility.Collapsed;

            _btn.Click += _eventClick;
            grdBtn.Children.Add(_btn);
        }

        private void Cross_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cancelForm();
        }

        private void cancelForm()
        {
            grdLogin.Visibility = Visibility.Collapsed;
        }

        private void btnSend_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _diskuss.SendPrivateMessage(grdConversations.SelectedConversation.Object.Name, tbxMessage.Text);
        }

        private void tbxMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                _diskuss.SendPrivateMessage(grdConversations.SelectedConversation.Object.Name, tbxMessage.Text);
        }

        private void addMessage(Message _msg) {
            grdChat.RowDefinitions.Add(new RowDefinition() { Height = new GridLength() });
            Grid.SetRow(_msg, grdChat.RowDefinitions.Count - 1);
            Grid.SetColumn(_msg, _msg.Me ? 1 : 0);
            grdChat.Children.Add(_msg);
        }
    }
}
