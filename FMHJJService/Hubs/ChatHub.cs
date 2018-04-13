using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace FMHJJService.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);            
        }

        public void AddToRoom(string groupId, string userName)
        {
            Groups.Add(Context.ConnectionId, groupId);
            Clients.Group(groupId, new string[0]).addUserIn(groupId, userName);
        }

        public void Send(string groupId, string detail, string userName)
        {
            Clients.Group(groupId, new string[0]).addSomeMessage(groupId, detail, userName);
        }
    }
}