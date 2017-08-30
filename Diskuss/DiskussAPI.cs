using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Threading;

namespace Diskuss {
    public class DiskussAPI {
        HttpRequest _httpRequester = new HttpRequest("http://localhost:8081/");

        public Me Me { get; set; }
        private DispatcherTimer _tmr = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1), IsEnabled = true };

        public event EventHandler<List<UserChannelObject>> OnUsers;
        public event EventHandler<List<UserChannelObject>> OnChannels;
        public event EventHandler OnLogin;

        public DiskussAPI() {
            _tmr.Tick += async (sender, args) => {
                if (Me != null) {
                    Debug.WriteLine(new JavaScriptSerializer().Deserialize<List<Notice>>(await _httpRequester.GetAsync($"user/{Me.ID}/notices")));
                    //GetUsers();
                    //GetChannels();
                }
            };
        }

        public async void GetUsers() {
            List<UserChannelObject> _lObjects = new List<UserChannelObject>();
            // Obliger d'utiliser une autre liste sinon l'objet ne s'instancie pas correctement
            new JavaScriptSerializer().Deserialize<List<User>>(await _httpRequester.GetAsync("users")).ForEach(e => { _lObjects.Add(new User(e.Nick)); });
            OnUsers?.Invoke(this, _lObjects);
        }

        public async void GetChannels() {
            List<UserChannelObject> _lObjects = new List<UserChannelObject>();
            // Obliger d'utiliser une autre liste sinon l'objet ne s'instancie pas correctement
            new JavaScriptSerializer().Deserialize<List<Channel>>(await _httpRequester.GetAsync("channels")).ForEach(e => { _lObjects.Add(new Channel(e.Name)); });
            OnChannels?.Invoke(this, _lObjects);
        }

        public async void Login(string strNick, bool bUpdate) {
            Me = new JavaScriptSerializer().Deserialize<Me>(await _httpRequester.PostAsync($"users/register/{strNick}", strNick));
            OnLogin?.Invoke(this, EventArgs.Empty);
            if (bUpdate) {
                GetUsers();
                GetChannels();
            }
        }
    }
}
