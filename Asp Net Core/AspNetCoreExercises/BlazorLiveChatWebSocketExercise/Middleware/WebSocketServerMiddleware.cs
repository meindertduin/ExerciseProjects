using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlazorLiveChatWebSocketExercise.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace BlazorLiveChatWebSocketExercise.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketConnectionManager _connectionManager;
        
        public WebSocketServerMiddleware(RequestDelegate next, WebSocketConnectionManager connectionManager)
        {
            _next = next;
            _connectionManager = connectionManager;    
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    var connID = _connectionManager.AddSocket(webSocket);
                    await SendSocketAsync(webSocket, connID);
                    await ReceiveMessage(webSocket, async (result, buffer) =>
                    {
                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                        
                        }
                        else if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                        }
                    });
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task SendSocketAsync(WebSocket socket, string connID)
        {
            BinaryModelSerializer serializer = new BinaryModelSerializer();
            var buffer = serializer.ToByteArray(new WebSocketTextMessageModel
            {
                ConnectionId = connID,
                Message = $"connection established with connection id: {connID}",
            });
            
            await socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
        }
        
        private async Task ReceiveMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                handleMessage(result, buffer);
            }

            try
            {
                await CloseConnection(socket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task CloseConnection(WebSocket webSocket)
        {
            if (webSocket != null)
            {
                if (webSocket.State == WebSocketState.Open ||
                    webSocket.State == WebSocketState.CloseReceived ||
                    webSocket.State == WebSocketState.CloseSent)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationToken.None);
                }
            }
        }
    }
}