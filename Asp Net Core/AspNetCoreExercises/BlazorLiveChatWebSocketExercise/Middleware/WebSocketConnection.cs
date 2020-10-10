using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using BlazorLiveChatWebSocketExercise.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace BlazorLiveChatWebSocketExercise.Middleware
{
    public class WebSocketConnection
    {
        private WebSocket _webSocket;
        private string _connectionId;
        private string _userName;

        public string GetConnectionId => _connectionId;

        public async Task CreateConnection(HttpContext context)
        {
            _webSocket = await context.WebSockets.AcceptWebSocketAsync();
            _connectionId = Guid.NewGuid().ToString();
        }

        public async Task ListenMessages()
        {
            await LoopForMessages(_webSocket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    BinaryModelSerializer binaryModelSerializer = new BinaryModelSerializer();
                    var message = binaryModelSerializer.FromByteArray<WebSocketMessageModel>(buffer);

                    if (message.MessageType == MessageType.InitializeMessage)
                    {
                        _userName = message.UserName;
                    }
                    
                    
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    if (result.CloseStatus != null)
                    {
                        await _webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                    }
                }
            });
        }
        
        private async Task LoopForMessages(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                handleMessage(result, buffer);
            }

            try
            {
                await CloseConnection();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public async Task CloseConnection()
        {
            if (_webSocket != null)
            {
                if (_webSocket.State == WebSocketState.Open ||
                    _webSocket.State == WebSocketState.CloseReceived ||
                    _webSocket.State == WebSocketState.CloseSent)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationToken.None);
                }
            }
        }

        public async Task SendTextMessage(WebSocketMessageModel webSocketTextMessageModel)
        {
            BinaryModelSerializer serializer = new BinaryModelSerializer();
            
            var buffer = serializer.ToByteArray(webSocketTextMessageModel);
            await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}