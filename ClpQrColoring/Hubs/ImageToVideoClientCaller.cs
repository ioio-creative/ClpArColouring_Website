using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ClpQrColoring.Hubs
{
    public class ImageToVideoClientCaller
    {
        private static IHubContext GetImageToVideoHubContext()
        {
            return GlobalHost.ConnectionManager
                .GetHubContext<ImageToVideoProgressHub>();
        }

        //public static void TestSentToClient()
        //{
        //    IHubContext hubContext = GetImageToVideoHubContext();
        //    hubContext.Clients.All.addNewMessageToPage();
        //}

        public static void WhenVideoRendering(string connectionId)
        {
            if (!String.IsNullOrEmpty(connectionId))
            {
                // In order to invoke SignalR functionality directly from server side,
                // without going through methods in Hub class, we must use this
                IHubContext hubContext = GetImageToVideoHubContext();

                // Pushing data to one specific client
                //hubContext.Clients.All.showVideoRenderingModal();
                hubContext.Clients.Client(connectionId).showVideoRenderingModal();
            }
        }
    }
}