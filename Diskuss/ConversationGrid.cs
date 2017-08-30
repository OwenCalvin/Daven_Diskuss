using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Diskuss {
    public class ConversationGrid : UserChannelGrid {
        public override void add(UserChannelObject _ucObject) {
            Conversation _convObject = new Conversation(_ucObject);
            grd.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100) });
            _convObject.Y = grd.RowDefinitions.Count - 1;
            grd.Children.Add(_convObject);
        }

        public override void remove(UserChannelObject _ucObject) {
            Conversation _convObject = new Conversation(_ucObject);
            grd.RowDefinitions.RemoveAt(_convObject.Y);
            grd.Children.Remove(_convObject);
            grd.Children.OfType<IY>().ToArray().Where(_iyE => _iyE.Y > _convObject.Y).ToList().ForEach(_iyE => {
                _iyE.Y -= 1;
            });
        }
    }
}
