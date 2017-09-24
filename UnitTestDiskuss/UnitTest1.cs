using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Diskuss;
using System.Linq;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace UnitTestDiskuss {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void AddUserChannelObjectToGrid() {
            UserChannelObject _uc = new UserChannelObject("Test");
            UserChannelGrid _ucg = new UserChannelGrid();
            _ucg.Destination = new ConversationGrid();
            _ucg.Add(_uc);
            Assert.AreEqual(_ucg.Objects.First(), _uc);
        }

        [TestMethod]
        public void AddConversationToGrid() {
            Conversation _conv = new Conversation(new UserChannelObject("Test"));
            ConversationGrid _convgrid = new ConversationGrid();
            _convgrid.Add(_conv);
            Assert.AreEqual(_convgrid.Conversations.First(), _conv);
        }

        [TestMethod]
        public void RemoveUserChannelObjectToGrid() {
            UserChannelObject _uc = new UserChannelObject("Test");
            UserChannelGrid _ucg = new UserChannelGrid();
            _ucg.Destination = new ConversationGrid();
            _ucg.Add(_uc);
            Assert.AreEqual(_ucg.Objects.First(), _uc);
            _ucg.Remove(_uc);
            Assert.AreEqual(_ucg.Objects.Count, 0);
        }

        [TestMethod]
        public void ExecuteANotice() {
            List<Notice> _lNotices = new List<Notice>() {
                JsonConvert.DeserializeObject<Notice>("{ type: 'privateMessage', sender: 'user_1', recipient: 'user_2', message: 'Hello, world!', time: '2000-12-31T23:59:59.999Z' }"),
            };
            DiskussAPI _diskuss = new DiskussAPI(new List<UserChannelObject>(), new List<User>(), new List<Channel>());
            _diskuss.OnNewPrivateMessage += _diskuss_OnNewPrivateMessage;
            _diskuss.ExecuteNotices(_lNotices);
        }

        [TestMethod]
        private void _diskuss_OnNewPrivateMessage(object sender, Message e) {
            Assert.AreEqual(e.Text, "Hello, world!");
        }
    }
}
