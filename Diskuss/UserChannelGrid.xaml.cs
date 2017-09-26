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
    public partial class UserChannelGrid : Grid {
        public ConversationGrid Destination { get; set; }

        public List<UserChannelObject> Objects {
            get { return Children.OfType<UserChannelObject>().ToList(); }
            set {
                grd.Children.Clear();
                grd.RowDefinitions.Clear();

                value.ForEach(_ucObject => {
                    _ucObject.MouseDown += (sender, e) => {
                        Remove(_ucObject);
                        if (Destination != null) {
                            Destination.Add(_ucObject);
                        }
                    };

                    Add(_ucObject);
                });
            }
        }

        public UserChannelGrid() {
            InitializeComponent();
        }

        public virtual void Remove(UserChannelObject Object) {
            grd.RowDefinitions.RemoveAt(Object.Y);
            grd.Children.Remove(Object);
            grd.Children.OfType<IY>().ToArray().Where(_iyE => _iyE.Y > Object.Y).ToList().ForEach(_iyE => {
                _iyE.Y -= 1;
            });
        }

        public virtual void Add(UserChannelObject Object) {
            AddObject(Object);
        }

        public virtual void Add(Conversation Conversation) {
            AddObject(Conversation.Object);
        }

        private void AddObject(UserChannelObject Object) {
            if (!grd.Children.OfType<UserChannelObject>().Any(e => e.Name == Object.Name) && !Destination.Children.OfType<Conversation>().Any(e => e.Object.Name == Object.Name)) {
                Object.Parent = this;
                grd.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
                Object.Y = grd.RowDefinitions.Count - 1;
                grd.Children.Add(Object);
            }
        }
    }
}
