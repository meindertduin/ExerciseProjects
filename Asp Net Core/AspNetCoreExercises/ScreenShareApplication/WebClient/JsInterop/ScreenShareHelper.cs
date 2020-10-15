using System;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Infrastructure;
using Microsoft.JSInterop;

namespace WebClient.JsInterop
{
    public class ScreenShareHelper
    {
        private readonly HttpClient _client;
        private GrpcChannel _channel;
        private ScreenSharer.ScreenSharerClient _uploadClient;
        private AsyncDuplexStreamingCall<ScreenStreamModel, ScreenStreamReply> _call;

        private bool _isStreaing = false;
        private object _writeLock;

        public ScreenShareHelper()
        {
            _client = new HttpClient();
            _channel = GrpcChannel.ForAddress("https://localhost:5003");
            _uploadClient = new ScreenSharer.ScreenSharerClient(_channel);
            _call = _uploadClient.StreamScreen();
            _writeLock = new object();
        }
        
        [JSInvokable]
        public async Task HandleBlobUrl(string blobUrl)
        {
            if (_isStreaing == false)
            {
                _isStreaing = true;
                var readTask = Task.Run(async () =>
                {
                    await foreach (var response in _call.ResponseStream.ReadAllAsync())
                    {
                        Console.WriteLine("package delivered");
                    }
                });
            }
            var bytes = await _client.GetByteArrayAsync(blobUrl);
            var byteString = ByteString.CopyFrom(bytes);

            try
            {
                await _call.RequestStream.WriteAsync(new ScreenStreamModel
                {
                    Data = byteString,
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [JSInvokable]
        public async Task StopStream()
        {
            if (_isStreaing)
            {
                await _call.RequestStream.CompleteAsync();
                _isStreaing = false;
            }
        }
    }
}