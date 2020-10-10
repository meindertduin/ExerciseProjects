using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using BlazorLiveChatWebSocketExercise.Middleware;

namespace BlazorLiveChatWebSocketExercise.Infrastructure
{
    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocketConnection> _connections = new ConcurrentDictionary<string, WebSocketConnection>();

        public void AddConnection(WebSocketConnection webSocketConnection)
        {
            _connections.TryAdd(webSocketConnection.GetConnectionId, webSocketConnection);
        }

        public ConcurrentDictionary<string, WebSocketConnection> GetAllSockets()
        {
            return _connections;
        }
    }
}