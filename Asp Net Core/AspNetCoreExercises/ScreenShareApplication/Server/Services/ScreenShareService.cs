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

        public override async Task<ScreenStreamReply> StreamScreen(IAsyncStreamReader<ScreenStreamModel> requestStream, ServerCallContext context)
        {
            var savePath = Path.Combine(_webHostEnvironment.WebRootPath, "ffmpeg", string.Concat(Path.GetRandomFileName(), ".mp4"));
            
            while (await requestStream.MoveNext(CancellationToken.None))
            {
                var dataChunk = requestStream.Current.Data;
                Console.WriteLine(dataChunk.Length);
                await _serviceHelper.FeedDataToChannel(new IncomingStreamModel
                {
                    Data = dataChunk,
                });
            }

            return new ScreenStreamReply();
        }
    }
}