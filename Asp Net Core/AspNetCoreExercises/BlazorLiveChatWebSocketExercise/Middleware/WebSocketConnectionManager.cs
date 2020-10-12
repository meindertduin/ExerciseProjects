using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using BlazorLiveChatWebSocketExercise.Middleware;

namespace BlazorLiveChatWebSocketExercise.Infrastructure
{
    public static class WebSocketConnectionManager
    {
        private static ConcurrentDictionary<string, WebSocketConnection> _connections = new ConcurrentDictionary<string, WebSocketConnection>();

        public static void AddConnection(WebSocketConnection webSocketConnection)
        {
            _connections.TryAdd(webSocketConnection.GetConnectionId, webSocketConnection);
        }

        public static ConcurrentDictionary<string, WebSocketConnection> GetAllSockets()
        {
            return _connections;
        }
    }
}