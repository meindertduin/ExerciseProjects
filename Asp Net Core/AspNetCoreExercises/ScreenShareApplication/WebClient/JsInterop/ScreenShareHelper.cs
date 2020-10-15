using System;
using System.IO;
using System.Net.Http;
using System.Threading;
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
        }
        
        [JSInvokable]
        public async Task HandleBlobUrl(string blobUrl)
        {
            var bytes = await _client.GetByteArrayAsync(blobUrl);
            using (var stream = new MemoryStream(bytes))
            {
                var byteString = await ByteString.FromStreamAsync(stream, CancellationToken.None);
                var reply = await _uploadClient.StreamScreenAsync(new ScreenStreamModel
                {
                    Data = byteString
                });
            }
        }

        [JSInvokable]
        public async Task StopStream()
        {
            if (_isStreaing)
            {
                await _call.RequestStream.CompleteAsync();
                _call.Dispose();
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