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
        private readonly ILogger _logger;
        private readonly ChannelWriter<IncomingStreamModel> _videoStreamWriter;

        public ScreenShareService(Channel<IncomingStreamModel> videoStreamChannel, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _videoStreamWriter = videoStreamChannel.Writer;
        }

        public override async Task StreamScreen(IAsyncStreamReader<ScreenStreamModel> requestStream, IServerStreamWriter<ScreenStreamReply> responseStream, ServerCallContext context)
        {
            var savePath = Path.Combine(_webHostEnvironment.WebRootPath, "ffmpeg", string.Concat(Path.GetRandomFileName(), ".mp4"));
            
            while (await requestStream.MoveNext(CancellationToken.None))
            {
                var dataChunk = requestStream.Current.Data;
                Console.WriteLine(dataChunk.Length);
                using (Stream fs = File.OpenWrite(savePath))
                {
                    fs.Write(dataChunk.ToByteArray());
                }
                
                await _videoStreamWriter.WriteAsync(new IncomingStreamModel
                {
                    Data = dataChunk,
                });
                await responseStream.WriteAsync(new ScreenStreamReply());
            }
        }
    }
}