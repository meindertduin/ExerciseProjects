using System;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Server.Models;

namespace Server.Services
{
    public class ScreenShareService: ScreenSharer.ScreenSharerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ScreenShareServiceHelper _serviceHelper;
        private readonly ILogger _logger;

        public ScreenShareService(IWebHostEnvironment webHostEnvironment, ScreenShareServiceHelper serviceHelper)
        {
            _webHostEnvironment = webHostEnvironment;
            _serviceHelper = serviceHelper;
        }

        public override Task<ScreenStreamReply> StreamScreen(ScreenStreamModel request, ServerCallContext context)
        {
            var dataChunk = request.Data;
            _serviceHelper.FeedDataToChannel(new IncomingStreamModel
            {
                Data = dataChunk,
            });

            return Task.FromResult(new ScreenStreamReply
            {
                Status = 200,
            });
        }
    }
}