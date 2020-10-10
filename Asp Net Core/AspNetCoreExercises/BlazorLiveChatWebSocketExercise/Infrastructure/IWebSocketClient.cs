using System;
using System.Threading.Tasks;

namespace BlazorLiveChatWebSocketExercise.Infrastructure
{
    public interface IWebSocketClient : IAsyncDisposable
    {
        public event EventHandler<WebSocketMessageModel> OnMessageReceived;
        public Task StartConnection(string url, string userName);
        void SendMessageToPages(WebSocketMessageModel message);
        public Task SendMessage(WebSocketMessageModel messageModel);

        public Task CloseConnection();
    }
}