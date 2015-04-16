using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication6
{
  public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
           // Clients.AllExcept(Clients.Caller).broadcastMessage(name, message);
            Clients.Others.broadcastMessage(name, message);
          //  Clients.All
        }

        public void SendMsg(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            // Clients.AllExcept(Clients.Caller).broadcastMessage(name, message);

            Clients.Others.processChat("0",name, message);
            Clients.Caller.processChat("1", name, message);
            //  Clients.All
        }
    }
}
