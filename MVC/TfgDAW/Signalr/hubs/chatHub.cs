using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TfgDAW.SignalR.hubs
{
    public class chatHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.Remove(Context.ConnectionId, groupName);
        }

        public void Send(string groupName, string name, string mensaje)
        {
            Clients.Group(groupName).EnviarMensaje(name, mensaje);
        }
    }
}