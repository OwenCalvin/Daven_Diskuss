using System;
using System.Collections.Generic;
using System.Linq;
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
        DiskussAPI _diskuss = new DiskussAPI();

        public MainWindow() {
            InitializeComponent();

            grdChannelsList.Destination = grdConversations;
            grdUsersList.Destination = grdConversations;

            _diskuss.OnLogin += _diskuss_OnLogin;
            _diskuss.OnUsers += _diskuss_OnUsers;
            _diskuss.OnChannels += _diskuss_OnChannels;
        }

        private void _diskuss_OnLogin(object sender, EventArgs e) {
            grdLogin.Visibility = Visibility.Collapsed;
            lblNick.Content = _diskuss.Me.Nick;
        }

        private void _diskuss_OnChannels(object sender, List<UserChannelObject> _iyObjects) {
            grdChannelsList.setChildren(_iyObjects);
        }

        private void _diskuss_OnUsers(object sender, List<UserChannelObject> _lObject) {
            grdUsersList.setChildren(_lObject);
        }
        
        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            _diskuss.Login(tbxForm.Text, true);
        }
    }
}
