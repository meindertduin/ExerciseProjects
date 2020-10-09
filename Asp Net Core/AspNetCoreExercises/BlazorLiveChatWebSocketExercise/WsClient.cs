using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorLiveChatWebSocketExercise
{
    public class WsClient : IDisposable
    {
        public int ReceiveBufferSize { get; set; } = 8129;
        
        private ClientWebSocket WS;
        private CancellationTokenSource CTS;
        
        public async Task ConnectAsync(string url) {
            if (WS != null) {
                if (WS.State == WebSocketState.Open) return;
                WS.Dispose();
            }
            WS = new ClientWebSocket();
            CTS?.Dispose();

            CTS = new CancellationTokenSource();
            
            await WS.ConnectAsync(new Uri(url), CTS.Token);
            await Task.Factory.StartNew(ReceiveLoop, CTS.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
        
        private async Task ReceiveLoop() {
            var loopToken = CTS.Token;
            MemoryStream outputStream = null;
            WebSocketReceiveResult receiveResult = null;
            var buffer = new byte[ReceiveBufferSize];
            try {
                while (!loopToken.IsCancellationRequested) {
                    outputStream = new MemoryStream(ReceiveBufferSize);
                    do {
                        receiveResult = await WS.ReceiveAsync(buffer, CTS.Token);
                        if (receiveResult.MessageType != WebSocketMessageType.Close)
                            outputStream.Write(buffer, 0, receiveResult.Count);
                    }
                    while (!receiveResult.EndOfMessage);
                    if (receiveResult.MessageType == WebSocketMessageType.Close) break;
                    outputStream.Position = 0;
                    ResponseReceived(outputStream);
                }
            }
            catch (TaskCanceledException) { }
            finally {
                outputStream?.Dispose();
            }
        }

        private void ResponseReceived(MemoryStream outputStream)
        {
            throw new NotImplementedException();
        }

        public async Task DisconnectAsync() {
            if (WS is null) return;
            // TODO: requests cleanup code, sub-protocol dependent.
            if (WS.State == WebSocketState.Open) {
                CTS.CancelAfter(TimeSpan.FromSeconds(2));
                await WS.CloseOutputAsync(WebSocketCloseStatus.Empty, "", CancellationToken.None);
                await WS.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            WS.Dispose();
            WS = null;
            CTS.Dispose();
            CTS = null;
        }

        public void Dispose() => DisconnectAsync().Wait();
    }
}