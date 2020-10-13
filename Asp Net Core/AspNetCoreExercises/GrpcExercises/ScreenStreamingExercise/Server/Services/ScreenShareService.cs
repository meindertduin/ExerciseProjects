using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using WebClient;

namespace Server.Services
{
    public class ScreenShareService : ScreenShare.ScreenShareBase
    {
        public override async Task<ScreenShareConfirmModel> GetStreamData(IAsyncStreamReader<screenShareUploadModel> requestStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext(CancellationToken.None))
            {
                var streamData = requestStream.Current;
            }

            ScreenShareConfirmModel output = new ScreenShareConfirmModel
            {
                Connected = true
            };

            ScreenShareConfirmModel res = Task.FromResult(output);
            return res;
        }
    }
}