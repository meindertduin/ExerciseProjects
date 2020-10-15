using Google.Protobuf;

namespace Server.Models
{
    public class IncomingStreamModel
    {
        public ByteString Data { get; set; }
    }
}