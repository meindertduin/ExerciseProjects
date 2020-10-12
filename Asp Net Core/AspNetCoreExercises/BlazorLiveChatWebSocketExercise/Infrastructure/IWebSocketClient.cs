using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace BlazorLiveChatWebSocketExercise.Infrastructure
{
    public interface IWebSocketClient : IAsyncDisposable
    {
        public event EventHandler<WebSocketState> OnWebSocketStateChanged; 
        public event EventHandler<WebSocketMessageModel> OnMessageReceived;
        public Task StartConnection(string url, string userName);
        void SendMessageToPages(WebSocketMessageModel message);
        void UpdateWebSocketStateOnPage(WebSocketState state);
        public Task SendMessage(WebSocketMessageModel messageModel);

        public Task CloseConnection();
    }
}