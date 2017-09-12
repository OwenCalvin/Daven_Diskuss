using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diskuss {
    public class User : UserChannelObject {
        public string Nick {
            get { return Name; }
            set { Name = value; }
        }

        public Channel Channel { get; set; }

        public User() : base() { }

        public User(string Nick) : base(Nick) {
            this.Nick = Nick;
        }
    }
}
