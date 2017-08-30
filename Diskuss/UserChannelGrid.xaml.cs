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
    public partial class UserChannelGrid : UserControl {
        public ConversationGrid Destination { get; set; }

        public UserChannelGrid() {
            InitializeComponent();
        }

        public virtual void remove(UserChannelObject _ucObject) {
            grd.RowDefinitions.RemoveAt(_ucObject.Y);
            grd.Children.Remove(_ucObject);
            grd.Children.OfType<IY>().ToArray().Where(_iyE => _iyE.Y > _ucObject.Y).ToList().ForEach(_iyE => {
                _iyE.Y -= 1;
            });
        }

        public virtual void add(UserChannelObject _ucObject) {
            grd.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
            _ucObject.Y = grd.RowDefinitions.Count - 1;
            grd.Children.Add(_ucObject);
        }

        public void setChildren(List<UserChannelObject> _iyObjects) {
            grd.Children.Clear();
            grd.RowDefinitions.Clear();

            _iyObjects.ForEach(_ucObject => {
                _ucObject.MouseDown += (sender, e) => {
                    remove(_ucObject);
                    if (Destination != null)
                        Destination.add(_ucObject);
                };

                add(_ucObject);
            });
        }
    }
}
