using System;
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
        }

        private void GrdConversations_OnConversationSelectedChange(object sender, Conversation e)
        {
            
        }

        private void _diskuss_OnLogin(object sender, EventArgs e) {
            grdLogin.Visibility = Visibility.Collapsed;
            lblNick.Content = _diskuss.Me.Name;
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
    }
}
