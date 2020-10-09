using System;
using System.Threading.Tasks;

namespace BlazorLiveChatWebSocketExercise.Infrastructure
{
    public interface IWebSocketClient : IAsyncDisposable
    {
        public event EventHandler<WebSocketTextMessageModel> OnMessageReceived;
        public Task StartConnection(string url);
        public void SendMessageToPages(WebSocketTextMessageModel message);

        public Task CloseConnection();
    }
}