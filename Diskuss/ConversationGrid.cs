using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Diskuss {
    public class ConversationGrid : UserChannelGrid {
        public event EventHandler<Conversation> OnConversationSelectedChange;
        private Conversation _convSelectedConversation;
        public Conversation SelectedConversation {
            get { return _convSelectedConversation; }
            set {
                if (value != null)
                {
                    if (_convSelectedConversation != null)
                    {
                        _convSelectedConversation.grdBack.Opacity = .5;
                    }
                    value.grdBack.Opacity = 1;
                }
                _convSelectedConversation = value;
                OnConversationSelectedChange?.Invoke(this, SelectedConversation);
            }
        }

        public List<Conversation> Conversations { get { return Children.OfType<Conversation>().ToList(); } }

        public override void add(UserChannelObject _ucObject) {
            Conversation _convObject = new Conversation(_ucObject);
            _convObject.Parent = this;
            grd.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
            _convObject.Y = grd.RowDefinitions.Count - 1;
            grd.Children.Add(_convObject);
        }

        public override void remove(UserChannelObject _ucObject) {
            Conversation _convObject = new Conversation(_ucObject);
            removeObject(_convObject);
        }

        public void remove(Conversation _convObject)
        {
            if(SelectedConversation == _convObject)
            {
                SelectedConversation = null;
            }
            removeObject(_convObject);
            _convObject.Destination.add(_convObject);
        }

        private void removeObject(IY _iyObject)
        {
            grd.RowDefinitions.RemoveAt(_iyObject.Y);
            grd.Children.Remove((UIElement)_iyObject);
            grd.Children.OfType<IY>().ToArray().Where(_iyE => _iyE.Y > _iyObject.Y).ToList().ForEach(_iyE => {
                _iyE.Y -= 1;
            });
        }
    }
}
