using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Diskuss {
    public class HttpRequest {
        public string Url { get; set; }
        private HttpClient http = new HttpClient();

        public HttpRequest(string Url) {
            this.Url = Url;
        }

        public async Task<string> GetAsync(string link) {
            return await (await http.GetAsync(Url + link)).Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string link, string content) {
            return await (await http.PostAsync(Url + link, new StringContent(content))).Content.ReadAsStringAsync();
        }
    }
}
