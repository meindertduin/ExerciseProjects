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

        public async Task FeedDataToChannel(IncomingStreamModel incomingStream)
        {
            await _videoStreamChannel.Writer.WriteAsync(incomingStream);
        }
    }
}