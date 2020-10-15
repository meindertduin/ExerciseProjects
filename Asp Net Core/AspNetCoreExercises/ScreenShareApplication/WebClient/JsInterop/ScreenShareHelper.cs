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
    public class ScreenShareHelper : IDisposable
    {
        private readonly HttpClient _client;
        private GrpcChannel _channel;
        private ScreenSharer.ScreenSharerClient _uploadClient;
        private AsyncClientStreamingCall<ScreenStreamModel, ScreenStreamReply> _call;

        private bool _isStreaing = false;

        public ScreenShareHelper()
        {
            _client = new HttpClient();
            _channel = GrpcChannel.ForAddress("https://localhost:5003");
            _uploadClient = new ScreenSharer.ScreenSharerClient(_channel);
            _call = _uploadClient.StreamScreen();
        }
        
        [JSInvokable]
        public async Task HandleBlobUrl(string blobUrl)
        {
            if (_isStreaing == false)
            {
                _isStreaing = true;
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

        public void Dispose()
        {
            _client?.Dispose();
            _channel?.Dispose();
            _call?.Dispose();
        }
    }
}