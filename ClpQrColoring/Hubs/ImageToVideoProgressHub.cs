using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ClpQrColoring.Hubs
{
    // Our website uses methods from another class to send the data
    // from outside the hub. But we still need this class in order for
    // SignalR to communicate between server and client.
    [HubName("imageToVideoProgressHub")]
    public class ImageToVideoProgressHub : Hub
    {
        // template, not used
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}