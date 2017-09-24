using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diskuss {
    public class Notice {
        public string Type { get; set; }
        public string Nick { get; set; }
        public Channel Channel { get; set; }
        public string Time { get; set; }
        public string Message { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }

        public Notice() { }
    }
}
