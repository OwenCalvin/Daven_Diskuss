using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Diskuss {
    public class Channel : UserChannelObject {
        public string Description { get; set; }
        public bool Keep { get; set; }

        private User _uOwner;
        public User Owner { get { return _uOwner; } }

        public Channel() : base() { Init(); }

        public Channel(string Name) : base(Name) { Init(); }

        public void setOwner (string Name)
        {
            _uOwner = new User(Name);
        }

        private void Init()
        {
            brdBack.Background = (Brush)(new BrushConverter().ConvertFrom("#2E3438"));
        }
    }
}
