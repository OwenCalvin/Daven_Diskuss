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
        public string Owner { get; set; }

        public Channel() : base() { }

        public Channel(string Name) : base(Name) {
            brdBack.Background = (Brush)(new BrushConverter().ConvertFrom("#2E3438"));
        }
    }
}
