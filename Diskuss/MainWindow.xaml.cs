using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Diskuss {
    public partial class MainWindow : Window {
        private DiskussAPI _diskuss;

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

        private void _diskuss_OnSendPrivateMessage(object sender, Message Message) {
            grdConversations.SelectedConversation.AddMessage(Message);
            AddMessage(Message);
        }

        private void _diskuss_OnNewPrivateMessage(object sender, Message Message) {
            UserChannelObject _ucUser = grdUsersList.Children.OfType<UserChannelObject>().FirstOrDefault(x => x.Name == Message.Sender);
            if (_ucUser != null) {
                grdUsersList.Remove(_ucUser);
                grdConversations.Add(_ucUser);
            }

            Conversation _convUser = grdConversations.GetConversation(Message.Sender);
            _convUser.AddMessage(Message);
            if (_convUser != grdConversations.SelectedConversation) {
                _convUser.Notifications = ++_convUser.Notifications;
            } else {
                AddMessage(Message);
            }
        }

        private void GrdConversations_OnConversationSelectedChange(object sender, Conversation Conversation) {
            grdChat.Children.Clear();
            grdChat.RowDefinitions.Clear();
            tbxMessage.Text = null;
            if ((lblChatName.Content = Conversation) == null) {
                lblChatName.Content = "";
                msgForm.Visibility = Visibility.Collapsed;
            } else {
                lblChatName.Content = Conversation.Object.Name.Length > 25 ? $"{Conversation.Object.Name.Substring(0, 25)}..." : Conversation.Object.Name;
                msgForm.Visibility = Visibility.Visible;
                Conversation.Messages.ForEach(e => { AddMessage(e); });
            }
        }

        private void _diskuss_OnLogin(object sender, EventArgs e) {
            grdLogin.Visibility = Visibility.Collapsed;
            lblNick.Content = _diskuss.Me.Name.Length > 13 ? $"{_diskuss.Me.Name.Substring(0, 13)}..." : _diskuss.Me.Name;
        }

        private void _diskuss_OnChannels(object sender, List<UserChannelObject> Objects) {
            grdChannelsList.Objects = Objects;
        }

        private void _diskuss_OnUsers(object sender, List<UserChannelObject> Objects) {
            grdUsersList.Objects = Objects;
            /*grdConversations.Conversations.ForEach(_conv => {
                bool bDisconnected = false;
                Objects.ForEach(_uc => {
                    bDisconnected = _uc.Name == _conv.Object.Name;
                });
                _conv.Disconnected = bDisconnected;
            });*/
        }
        
        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            _diskuss.Login(tbxForm.Text.Length > 0 ? tbxForm.Text : "Anonymous", true);
        }

        private void btn_ChannelClick(object sender, EventArgs e) {
            cancelForm();
            _diskuss.JoinChannel(tbxForm.Text);
        }

        private void grdAddChannel_MouseDown(object sender, MouseButtonEventArgs e) {
            showForm("Join", btn_ChannelClick, true);
        }

        private void showForm(string BtnContent, RoutedEventHandler EventClick, bool Cross) {
            grdLogin.Visibility = Visibility.Visible;
            tbxForm.Text = null;
            grdBtn.Children.Clear();
            Button _btn = new Button() {
                Content = BtnContent,
                Style = Resources["BtnYellow"] as Style,
                FontFamily = btnLogin.FontFamily,
                FontSize = btnLogin.FontSize,
                Foreground = btnLogin.Foreground,
                BorderBrush = btnLogin.BorderBrush,
                Background = btnLogin.Background,
                FontWeight = btnLogin.FontWeight,
                Width = btnLogin.Width,
            };

            cnvCross.Visibility = Cross ? Visibility.Visible : Visibility.Collapsed;

            _btn.Click += EventClick;
            grdBtn.Children.Add(_btn);
        }

        private void Cross_MouseDown(object sender, MouseButtonEventArgs e) {
            cancelForm();
        }

        private void cancelForm() {
            grdLogin.Visibility = Visibility.Collapsed;
        }

        private void btnSend_MouseDown(object sender, MouseButtonEventArgs e) {
            _diskuss.SendPrivateMessage(grdConversations.SelectedConversation.Object.Name, tbxMessage.Text);
        }

        private void tbxMessage_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                _diskuss.SendPrivateMessage(grdConversations.SelectedConversation.Object.Name, tbxMessage.Text);
            }
        }

        private void AddMessage(Message Message) {
            grdChat.RowDefinitions.Add(new RowDefinition() { Height = new GridLength() });
            Grid.SetRow(Message, grdChat.RowDefinitions.Count - 1);
            Grid.SetColumn(Message, Message.Me ? 1 : 0);
            grdChat.Children.Add(Message);
        }
    }
}
