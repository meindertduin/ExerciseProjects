﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BlazorSingalRExercise.Server.Hubs
{
    public class LiveChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}