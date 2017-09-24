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
        public Me Me { get; set; }

        private HttpRequest _httpRequester = new HttpRequest("http://localhost:8081/");
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

        public DiskussAPI(List<UserChannelObject> Conversations, List<User> Users, List<Channel> Channels) {
            _tmr.Tick += async (sender, args) => {
                if (Me != null) {
                    string res = await _httpRequester.GetAsync($"user/{Me.ID}/notices");
                    if (res != "[]") {
                        ExecuteNotices(JsonConvert.DeserializeObject<List<Notice>>(res));
                    }
                    GetUsers();
                    GetChannels();
                }
            };

            _lucoConversations = Conversations;
            _luUsers = Users;
            _lcChannel = Channels;
        }

        public async void GetUsers() {
            OnUsers?.Invoke(this, JsonConvert.DeserializeObject<List<User>>(await _httpRequester.GetAsync("users")).ToList<UserChannelObject>().Where(_e => _e.Name != Me.Name).ToList());
        }

        public async void GetChannels() {
            OnChannels?.Invoke(this, JsonConvert.DeserializeObject<List<Channel>>(await _httpRequester.GetAsync("channels")).ToList<UserChannelObject>());
        }
        
        public async void JoinChannel(string Name) {
            string obj = await _httpRequester.PutAsync($"user/{Me.ID}/channels/{Name}/join/", Name, "");
            OnChannelJoin?.Invoke(this, JsonConvert.DeserializeObject<Channel>(obj));
        }

        public async void SendPrivateMessage(string Nick, string Message) {
            string obj = await _httpRequester.PutAsync($"user/{Me.ID}/message/{Nick}/", Message, "message");
            OnSendPrivateMessage?.Invoke(this, new Message(Message, true));
        }

        public async void Login(string Nick, bool Update) {
            Me = JsonConvert.DeserializeObject<Me>(await _httpRequester.PostAsync($"users/register/{Nick}", Nick));
            OnLogin?.Invoke(this, EventArgs.Empty);
            if (Update) {
                GetUsers();
                GetChannels();
            }
        }

        public void ExecuteNotices(List<Notice> Notices) {
            Notices.ForEach(e => {
                switch(e.Type) {
                    case "privateMessage":
                        OnNewPrivateMessage?.Invoke(this, new Message(e.Message, false, e.Sender));
                        break;
                }
            });
        }
    }
}
