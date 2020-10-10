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
        
        public event EventHandler<WebSocketMessageModel> OnMessageReceived;

        public WebSocketClient()
        {
            _binaryModelSerializer = new BinaryModelSerializer();
        }
        
        public async Task StartConnection(string url, string userName)
        {
            _clientWebSocket = new ClientWebSocket();
            await _clientWebSocket.ConnectAsync(new Uri(url), CancellationToken.None);
            await SendMessage(new WebSocketMessageModel
            {
                MessageType = MessageType.InitializeMessage,
                UserName = userName,
            });
            
            await ListenMessages(url);
        }

        public async Task SendMessage(WebSocketMessageModel messageModel)
        {
            var data = _binaryModelSerializer.ToByteArray(messageModel);
            await _clientWebSocket.SendAsync(data, WebSocketMessageType.Binary, true, CancellationToken.None);
        }
        public void SendMessageToPages(WebSocketMessageModel message)
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
                try
                {
                    WebSocketReceiveResult result = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                    var data = _binaryModelSerializer.FromByteArray<WebSocketMessageModel>(buffer);
                    SendMessageToPages(data);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
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