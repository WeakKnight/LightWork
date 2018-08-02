using System;
using System.IO;

namespace GameProtocol
{
    public static class ProtocolHelper
    {
        public static Byte[] ConvertProtocolToBytes(Protocol protocol)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    //先把协议类转换成简单的字节流
                    ProtoBuf.Serializer.NonGeneric.SerializeWithLengthPrefix(ms, protocol, ProtoBuf.PrefixStyle.Base128, Mapper.GetProtocolIntMappingValue(protocol));
                    //ProtoBuf.Serializer.SerializeWithLengthPrefix<T>(ms, protocol, ProtoBuf.PrefixStyle.Base128);
                    byte[] result = new Byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(result, 0, result.Length);

                    //再在序列化的字节流前加上协议号
                    MemoryStream resultMs = new MemoryStream();
                    BinaryWriter br = new BinaryWriter(resultMs);
                    br.Write(Mapper.GetProtocolIntMappingValue(protocol));
                    br.Write(result);

                    byte[] finalResult = resultMs.ToArray();

                    br.Close();
                    resultMs.Close();

                    return finalResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static Protocol ConvertBytesToProtocol(Byte[] bytes)
        {
            try
            {
                Type protocolType;
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Position = 0;
                    BinaryReader br = new BinaryReader(ms);
                    int typeCode = br.ReadInt32();
                    protocolType = Mapper.GetIntProtocolMappingValue(typeCode);
                    br.Close();
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(bytes, 4, bytes.Length - 4);
                    ms.Position = 0;
                    Object protocol;
                    ProtoBuf.Serializer.NonGeneric.TryDeserializeWithLengthPrefix(ms, ProtoBuf.PrefixStyle.Base128, (t)=>Mapper.GetIntProtocolMappingValue(t), out protocol);
                    return protocol as Protocol;
                }
            }
            catch (Exception e)
            {
                return default(Protocol);
            }
        }
    }
}
