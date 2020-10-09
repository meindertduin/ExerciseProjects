using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BlazorLiveChatWebSocketExercise.Infrastructure
{
    public class BinaryModelSerializer
    {
        public byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }
            
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }

        public T FromByteArray<T>(byte[] data)
        {
            if (data == null)
            {
                return default(T);
            }
            
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (var stream = new MemoryStream(data))
            {
                var obj = binaryFormatter.Deserialize(stream);
                try
                {
                    return (T) obj;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}