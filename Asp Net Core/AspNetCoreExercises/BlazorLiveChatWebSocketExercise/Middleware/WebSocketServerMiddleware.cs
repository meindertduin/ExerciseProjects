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
                    var connection = new WebSocketConnection();
                    await connection.CreateConnection(context);
                    _connectionManager.AddConnection(connection);
                    await connection.ListenMessages();
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
    }
}