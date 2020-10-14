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

namespace Server.Services
{
    public class ScreenShareService: ScreenSharer.ScreenSharerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ChannelWriter<Byte[]> _videoStreamWriter;

        public ScreenShareService([FromServices] Channel<byte[]> videoStreamChannel, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _videoStreamWriter = videoStreamChannel.Writer;
        }
        
        public override async Task<ScreenStreamReply> StreamScreen(IAsyncStreamReader<ScreenStreamModel> requestStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext(CancellationToken.None))
            {
                var dataChunk = requestStream.Current.Data;
                //await _videoStreamWriter.WriteAsync(dataChunk.ToByteArray());
            }
            
            return await Task.FromResult(new ScreenStreamReply());
        }
    }
}