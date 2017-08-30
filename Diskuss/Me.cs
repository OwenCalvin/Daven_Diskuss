using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diskuss {
    public class Me : User {
        public string ID { get; set; }

        public Me() { }

        public Me(string ID, string Nick) : base(Nick) {
            this.ID = ID;
        }
    }
}
