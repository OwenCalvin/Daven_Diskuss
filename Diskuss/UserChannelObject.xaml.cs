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
    public partial class UserChannelObject : UserControl, IY{
        public new UserChannelGrid Parent { get; set; }
        
        public int Y {
            get { return Grid.GetRow(this); }
            set { Grid.SetRow(this, value); }
        }

        private string strName;
        public new string Name {
            get { return strName; }
            set {
                strName = value;
                lblFirstLetterName.Content = Name[0].ToString().ToUpper();
                lblFullName.Content = Name.Length > 6 ? $"{Name.Substring(0, 5)}..." : Name;
            }
        }

        public UserChannelObject() {
            InitializeComponent();
        }

        public UserChannelObject(string Name) {
            InitializeComponent();
            this.Name = Name;
        }
    }
}
