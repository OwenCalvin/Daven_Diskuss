using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Diskuss {
    public class ConversationGrid : UserChannelGrid {
        public List<Conversation> Conversations { get { return Children.OfType<Conversation>().ToList(); } }

        private Conversation _convSelectedConversation;
        public Conversation SelectedConversation {
            get { return _convSelectedConversation; }
            set {
                if (value != null) {
                    if (_convSelectedConversation != null) {
                        _convSelectedConversation.grdBack.Opacity = .5;
                    }
                    value.grdBack.Opacity = 1;
                    value.Notifications = 0;
                }
                _convSelectedConversation = value;
                OnConversationSelectedChange?.Invoke(this, SelectedConversation);
            }
        }
        
        public event EventHandler<Conversation> OnConversationSelectedChange;

        public override void Add(UserChannelObject Object) {
            Add(new Conversation(Object));
        }

        public override void Add(Conversation Object) {
            Object.Parent = this;
            grd.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
            Object.Y = grd.RowDefinitions.Count - 1;
            grd.Children.Add(Object);
        }

        public override void Remove(UserChannelObject Object) {
            Conversation _convObject = new Conversation(Object);
            RemoveObject(_convObject);
        }

        public void Remove(Conversation Object) {
            if(SelectedConversation == Object) {
                SelectedConversation = null;
            }
            RemoveObject(Object);
            Object.Destination.Add(Object);
        }

        private void RemoveObject(IY Object) {
            grd.RowDefinitions.RemoveAt(Object.Y);
            grd.Children.Remove((UIElement) Object);
            grd.Children.OfType<IY>().ToArray().Where(_iyE => _iyE.Y > Object.Y).ToList().ForEach(_iyE => {
                _iyE.Y -= 1;
            });
        }

        public Conversation GetConversation(string Name) {
            return Conversations.FirstOrDefault(x => x.Object.Name == Name);
        }
    }
}
