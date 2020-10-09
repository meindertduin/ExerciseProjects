using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlazorLiveChatWebSocketExercise.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;
        private List<WebSocket> _activeSockets = new List<WebSocket>();


        public WebSocketServerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using (var webSocket = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        if (!_activeSockets.Contains(webSocket))
                        {
                            _activeSockets.Add(webSocket);
                        }
                        await ReceiveMessage(context, webSocket);
                    }
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
        
        private async Task ReceiveMessage(HttpContext httpContext, WebSocket webSocket)
        {
            var buffer = new Byte[1024 * 4];
            while (webSocket.State.HasFlag(WebSocketState.Open))
            {
                try
                {
                    var received = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                }
                catch (Exception e)
                {
                    break;
                }
            }

            _activeSockets.Remove(webSocket);
            if (webSocket.State != WebSocketState.Closed && webSocket.State != WebSocketState.Aborted)
            {
                try
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "socket closed",
                        CancellationToken.None);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}