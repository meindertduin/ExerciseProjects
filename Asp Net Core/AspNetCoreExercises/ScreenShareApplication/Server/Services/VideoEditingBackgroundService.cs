using System;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Server.Models;

namespace Server.Services
{
    public class VideoEditingBackgroundService : BackgroundService
    {
        private readonly IWebHostEnvironment _env;
        private Channel<IncomingStreamModel> _videoFileReader;

        public VideoEditingBackgroundService(Channel<IncomingStreamModel> videoFileChannel, 
            IWebHostEnvironment env)
        {
            _env = env;
            _videoFileReader = videoFileChannel;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string fileName = Path.GetRandomFileName();
            while (_videoFileReader.Reader.Completion.IsCompleted == false)
            {
                Console.WriteLine("background service starting up");
                var chunk = await _videoFileReader.Reader.ReadAsync(stoppingToken);
                
                var bytes = chunk.Data.ToByteArray();

                using (var ms = new MemoryStream(bytes))
                {
                    File.WriteAllBytes(Path.Combine(_env.WebRootPath, "ffmpeg", fileName, ".mp4"), bytes);
                }
            }
        }
    }
}