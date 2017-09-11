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

        public async Task<string> GetAsync(string strLink) {
            return await (await http.GetAsync(Url + strLink)).Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string strLink, string strContent) {
            return await (await http.PostAsync(Url + strLink, new StringContent(strContent))).Content.ReadAsStringAsync();
        }

        public async Task<string> PutAsync(string strLink, string strContent) {
            return await (await http.PutAsync(Url + strLink, new StringContent(strContent))).Content.ReadAsStringAsync();
        }
    }
}
