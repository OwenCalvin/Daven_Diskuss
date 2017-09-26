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
    public partial class Message : UserControl {
        private int iMe;
        public int Me {
            get { return iMe; }
            set {
                iMe = value;
                if (Me >= 1) {
                    brdMain.HorizontalAlignment = HorizontalAlignment.Right;
                    brdMain.Background = (Brush)(new BrushConverter().ConvertFrom("#1CFFFFFF"));
                } else if (Me == 0) {
                    brdMain.HorizontalAlignment = HorizontalAlignment.Left;
                } else {
                    brdMain.Background = Brushes.Transparent;
                    brdMain.HorizontalAlignment = HorizontalAlignment.Center;
                }
            }
        }

        public string Sender { get; set; }

        public string Text {
            get { return lblMessage.Text.ToString(); }
            set { lblMessage.Text = value; }
        }

        public Message() {
            InitializeComponent();
        }

        public Message(string Message, int Me) {
            InitializeComponent();
            this.Text = Message;
            this.Me = Me;
        }

        public Message(string Message, int Me, string Sender) {
            InitializeComponent();
            this.Text = Message;
            this.Me = Me;
            this.Sender = Sender;
        }
    }
}
