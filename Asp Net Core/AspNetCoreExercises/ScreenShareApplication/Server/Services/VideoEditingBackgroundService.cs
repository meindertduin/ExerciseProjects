using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Server.Services
{
    public class VideoEditingBackgroundService : BackgroundService
    {
        private readonly IWebHostEnvironment _env;
        private ChannelReader<byte[]> _videoFileReader;

        public VideoEditingBackgroundService(Channel<byte[]> videoFileChannel, IWebHostEnvironment env)
        {
            _env = env;
            _videoFileReader = videoFileChannel.Reader;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _videoFileReader.WaitToReadAsync(stoppingToken))
            {
                var chunk = await _videoFileReader.ReadAsync(stoppingToken);
                var savePath = Path.Combine(_env.ContentRootPath, "ffmpeg", Path.GetRandomFileName());
                using (Stream fs = File.OpenWrite(savePath))
                {
                    fs.Write(chunk);
                }
            }
        }
    }
}