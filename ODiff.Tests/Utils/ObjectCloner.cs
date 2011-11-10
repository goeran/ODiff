using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ODiff.Tests.Utils
{
    public class ObjectCloner
    {
        public static Object Clone(object obj)
        {
            var binaryFormatter = new BinaryFormatter();
            using (var memStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memStream, obj);
                memStream.Seek(0, SeekOrigin.Begin);
                return binaryFormatter.Deserialize(memStream);
            }
        }
    }
}
