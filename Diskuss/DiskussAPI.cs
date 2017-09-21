using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Threading;
using Newtonsoft.Json;
using System.Collections;

namespace Diskuss {
    public class DiskussAPI {
        HttpRequest _httpRequester = new HttpRequest("http://localhost:8081/");

        public Me Me { get; set; }
        private DispatcherTimer _tmr = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1), IsEnabled = true };
        private List<UserChannelObject> _lucoConversations;
        private List<User> _luUsers;
        private List<Channel> _lcChannel;

        public event EventHandler<List<UserChannelObject>> OnUsers;
        public event EventHandler<List<UserChannelObject>> OnChannels;
        public event EventHandler<Channel> OnChannelJoin;
        public event EventHandler OnLogin;
        public event EventHandler<Message> OnSendPrivateMessage;
        public event EventHandler<Message> OnNewPrivateMessage;

        public DiskussAPI(List<UserChannelObject> _lucoConversations, List<User> _luUsers, List<Channel> _lcChannel) {
            _tmr.Tick += async (sender, args) => {
                if (Me != null) {
                    string res = await _httpRequester.GetAsync($"user/{Me.ID}/notices");
                    if (res != "[]")
                    {
                        Debug.WriteLine(res);
                        List<Notice> _lNotices = JsonConvert.DeserializeObject<List<Notice>>(res);
                        ExecuteNotices(_lNotices);
                    }
                    GetUsers();
                    GetChannels();
                }
            };

            this._lucoConversations = _lucoConversations;
            this._luUsers = _luUsers;
            this._lcChannel = _lcChannel;
        }

        public async void GetUsers() {
            OnUsers?.Invoke(this, JsonConvert.DeserializeObject<List<User>>(await _httpRequester.GetAsync("users")).ToList<UserChannelObject>().Where(_e => _e.Name != Me.Name).ToList());
        }

        public async void GetChannels() {
            OnChannels?.Invoke(this, JsonConvert.DeserializeObject<List<Channel>>(await _httpRequester.GetAsync("channels")).ToList<UserChannelObject>());
        }
        
        public async void JoinChannel(string strName) {
            string obj = await _httpRequester.PutAsync($"user/{Me.ID}/channels/{strName}/join/", strName, "");
            OnChannelJoin?.Invoke(this, JsonConvert.DeserializeObject<Channel>(obj));
        }

        public async void SendPrivateMessage(string Nick, string Message) {
            string obj = await _httpRequester.PutAsync($"user/{Me.ID}/message/{Nick}/", Message, "message");
            OnSendPrivateMessage?.Invoke(this, new Message(Message, true));
        }

        public async void Login(string strNick, bool bUpdate) {
            Me = JsonConvert.DeserializeObject<Me>(await _httpRequester.PostAsync($"users/register/{strNick}", strNick));
            OnLogin?.Invoke(this, EventArgs.Empty);
            if (bUpdate) {
                GetUsers();
                GetChannels();
            }
        }

        public void ExecuteNotices(List<Notice> _lNotices)
        {
            _lNotices.ForEach(e => {
                Debug.WriteLine(e.Type);
                switch(e.Type)
                {
                    case "privateMessage":
                        Debug.WriteLine(e.Type);
                        OnNewPrivateMessage?.Invoke(this, new Message(e.Message, false, e.Sender));
                        break;
                }
            });
        }
    }
}
