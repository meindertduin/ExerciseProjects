using System;
using Microsoft.JSInterop;
using WebClient.Domain;

namespace WebClient.JsInterop
{
    public class StartRecordingHelper
    {
        [JSInvokable]
        public void GiveBlobUrl(string blobUrl)
        {
            Console.WriteLine(blobUrl);
        }

        
    }
}