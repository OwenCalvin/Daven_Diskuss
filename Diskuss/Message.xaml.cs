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
        public string Text {
            get { return lblMessage.Content.ToString(); }
            set { lblMessage.Content = value; }
        }

        public Message()
        {
            InitializeComponent();
        }

        public Message(string strMessage, bool bMe)
        {
            InitializeComponent();
            Text = strMessage;
            lblMessage.HorizontalContentAlignment = bMe ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }
    }
}
