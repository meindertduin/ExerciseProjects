using System;
using Microsoft.JSInterop;

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