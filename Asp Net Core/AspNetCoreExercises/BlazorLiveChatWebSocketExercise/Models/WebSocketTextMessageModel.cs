using System;

namespace BlazorLiveChatWebSocketExercise
{
    [Serializable]
    public class WebSocketTextMessageModel
    {
        public string ConnectionId { get; set; }
        public string Message { get; set; }
    }
}