﻿using System;

namespace BlazorSignalRExercise.Client.Models
{
    public class MessageModel
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime TimeSend { get; set; }
    }
}