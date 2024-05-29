using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TfgDAW.SignalR.hubs
{
    public class chatHub : Hub
    {
        public void Send(string name,string mensaje)
        {
            Clients.All.EnviarMensaje(name,mensaje);

        }
    }
}