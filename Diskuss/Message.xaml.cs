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

namespace Diskuss
{
    public partial class Message : UserControl
    {
        private bool bMe;
        public bool Me {
            get { return bMe; }
            set {
                bMe = value;
                if(bMe) {
                    brdMain.HorizontalAlignment = HorizontalAlignment.Right;
                    brdMain.Background = (Brush)(new BrushConverter().ConvertFrom("#1CFFFFFF"));
                } else {
                    brdMain.HorizontalAlignment = HorizontalAlignment.Left;
                }
            }
        }

        public string Sender { get; set; }

        public string Text {
            get { return lblMessage.Text.ToString(); }
            set { lblMessage.Text = value; }
        }

        public Message()
        {
            InitializeComponent();
        }

        public Message(string strMessage, bool bMe)
        {
            InitializeComponent();
            Text = strMessage;
            Me = bMe;
        }

        public Message(string strMessage, bool bMe, string strSender) {
            InitializeComponent();
            Text = strMessage;
            Me = bMe;
            Sender = strSender;
        }
    }
}
