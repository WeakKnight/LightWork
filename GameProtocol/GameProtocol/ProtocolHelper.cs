using ProtoBuf;
using System;
using System.IO;

namespace GameProtocol
{
    public static class ProtocolHelper
    {
        public static Byte[] ConvertProtocolToBytes<T>(T protocol)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {                    
                    Serializer.Serialize<T>(ms, protocol);
                    byte[] result = new Byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(result, 0, result.Length);
                    return result;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static T ConvertBytesToProtocol<T>(Byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Position = 0;
                    T result = Serializer.Deserialize<T>(ms);
                    return result;
                }
            }
            catch (Exception e)
            {
                return default(T);
            }
        }
    }
}
