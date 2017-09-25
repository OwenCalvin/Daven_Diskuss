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

namespace Diskuss
{
    public partial class Conversation : UserControl, IY
    {
        public UserChannelObject Object { get; set; }
        public new ConversationGrid Parent { get; set; }
        public UserChannelGrid Destination { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();

        public int Y
        {
            get { return Grid.GetRow(this); }
            set { Grid.SetRow(this, value); }
        }

        private int iNotifications { get; set; } = 0;
        public int Notifications
        {
            get { return iNotifications; }
            set
            {
                lblNotifications.Content = value;
                iNotifications = value;
                brdNotifications.Visibility = value < 1 ? Visibility.Hidden : Visibility.Visible;
            }
        }

        private bool bDisconnected = false;
        public bool Disconnected
        {
            get { return bDisconnected; }
            set
            {
                if (value)
                {
                    Object.brdBack.Background = (Brush)(new BrushConverter().ConvertFrom("#2E3438"));
                    lblName.FontStyle = FontStyles.Italic;
                }
                else
                {
                    Object.brdBack.Background = (Brush)(new BrushConverter().ConvertFrom("#485056"));
                    lblName.FontStyle = FontStyles.Normal;
                }
                bDisconnected = value;
            }
        }

        public Conversation()
        {
            InitializeComponent();
        }

        public Conversation(UserChannelObject Object)
        {
            InitializeComponent();
            this.Object = Object;
            Destination = Object.Parent;
            lblName.Content = Object.Name.Length > 6 ? $"{Object.Name.Substring(0, 6)}..." : Object.Name;
            Object.lblFullName.Visibility = Visibility.Collapsed;
            grdObject.Children.Add(Object);
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Object.lblFullName.Visibility = Visibility.Visible;
            grdObject.Children.Clear();
            Parent.Remove(this);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Parent.SelectedConversation = this;
        }

        public void AddMessage(Message Message)
        {
            if (!Disconnected)
            {
                lblLastMessage.Content = Message.Text.Length > 10 ? $"{Message.Text.Substring(0, 10)}..." : Message.Text;
                Messages.Add(Message);
            }
        }
    }
}
