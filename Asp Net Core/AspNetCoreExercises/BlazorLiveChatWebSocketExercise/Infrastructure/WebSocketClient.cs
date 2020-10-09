using System;
using System.Net.WebSockets;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorLiveChatWebSocketExercise.Infrastructure
{
    public class WebSocketClient : IWebSocketClient
    {
        private ClientWebSocket _clientWebSocket;
        private BinaryModelSerializer _binaryModelSerializer;
        
        public event EventHandler<WebSocketTextMessageModel> OnMessageReceived;

        public WebSocketClient()
        {
            _binaryModelSerializer = new BinaryModelSerializer();
        }
        
        public async Task StartConnection(string url)
        {
            _clientWebSocket = new ClientWebSocket();
            await _clientWebSocket.ConnectAsync(new Uri(url), CancellationToken.None);
            await ListenMessages(url);
        }

        public void SendMessageToPages(WebSocketTextMessageModel message)
        {
            OnMessageReceived?.Invoke(this, message);
        }
        
        public async Task CloseConnection()
        {
            try
            {
                await CloseConnectionAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task ListenMessages(string url)
        {
            byte[] buffer = new byte[1024];
            while (_clientWebSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                var data = _binaryModelSerializer.FromByteArray<WebSocketTextMessageModel>(buffer);
                SendMessageToPages(data);
            }

            try
            {
                await CloseConnectionAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task CloseConnectionAsync()
        {
            if (_clientWebSocket != null)
            {
                if (_clientWebSocket.State == WebSocketState.Open ||
                    _clientWebSocket.State == WebSocketState.CloseReceived ||
                    _clientWebSocket.State == WebSocketState.CloseSent)
                {
                    await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationToken.None);
                }
                
                _clientWebSocket = null;
            }
        }
        
        public async ValueTask DisposeAsync()
        {
            if (_clientWebSocket != null)
            {
                try
                {
                    await CloseConnectionAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
                GC.SuppressFinalize(this);
            }
        }
    }
}