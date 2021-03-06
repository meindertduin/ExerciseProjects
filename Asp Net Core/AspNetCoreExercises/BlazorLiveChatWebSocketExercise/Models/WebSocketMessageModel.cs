﻿using System;

namespace BlazorLiveChatWebSocketExercise
{
    [Serializable]
    public class WebSocketMessageModel
    {
        public DateTime TimeSend { get; set; }
        public string UserName { get; set; }
        public MessageType MessageType { get; set; }
        public string Message { get; set; }
    }
}