using System.Threading.Channels;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Services
{
    public class ScreenShareServiceHelper
    {
        private readonly Channel<IncomingStreamModel> _videoStreamChannel;

        public ScreenShareServiceHelper(Channel<IncomingStreamModel> videoStreamChannel)
        {
            _videoStreamChannel = videoStreamChannel;
        }

        public Task FeedDataToChannel(IncomingStreamModel incomingStream)
        {
            _videoStreamChannel.Writer.WriteAsync(incomingStream);
            return Task.CompletedTask;
        }
    }
}