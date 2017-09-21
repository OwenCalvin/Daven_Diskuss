using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class Conversation : UserControl, IY {
        public UserChannelObject Object { get; set; }
        public new ConversationGrid Parent { get; set; }
        public UserChannelGrid Destination { get; set; }



        public List<Message> Messages { get; set; } = new List<Message>();
        private int iNotifications { get; set; } = 0;

        public int Notifications {
            get { return iNotifications; }
            set {
                lblNotifications.Content = value;
                iNotifications = value;
                brdNotifications.Visibility = value < 1 ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public int Y {
            get { return Grid.GetRow(this); }
            set { Grid.SetRow(this, value); }
        }

        public Conversation() {
            InitializeComponent();
        }

        public Conversation(UserChannelObject _object) {
            InitializeComponent();
            Object = _object;
            Destination = _object.Parent;
            lblName.Content = Object.Name.Length > 6 ? $"{Object.Name.Substring(0, 6)}..." : Object.Name ;
            _object.lblFullName.Visibility = Visibility.Collapsed;
            grdObject.Children.Add(_object);
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Object.lblFullName.Visibility = Visibility.Visible;
            grdObject.Children.Clear();
            Parent.remove(this);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Parent.SelectedConversation = this;
        }

        public void addMessage(Message _msg) {
            lblLastMessage.Content = _msg.Text.Length > 10 ? $"{_msg.Text.Substring(0, 10)}..." : _msg.Text;
            Messages.Add(_msg);
        }
    }
}
