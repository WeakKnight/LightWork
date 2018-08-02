using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GameProtocol
{
    public class Encoder
    {
        public static Byte[] Encode(Byte[] data)
        {
            Byte[] result = new Byte[data.Length + 4];
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(data.Length);
            br.Write(data);
            Buffer.BlockCopy(ms.ToArray(), 0, result, 0, (int)ms.Length);
            br.Close();
            ms.Close();
            return result;
        }

        public static Byte[] Decode(ref List<Byte> cache)
        {
            if (cache.Count < 4)
            {
                return null;
            }

            MemoryStream ms = new MemoryStream(cache.ToArray());
            BinaryReader br = new BinaryReader(ms);
            int len = br.ReadInt32();
            if (len > ms.Length - ms.Position)
            {
                return null;
            }
            Byte[] result = br.ReadBytes(len);

            cache.Clear();
            cache.AddRange(br.ReadBytes((int)ms.Length - (int)ms.Position));

            return result;
        }

        public static Byte[] Decode(Byte[] data)
        {

            MemoryStream ms = new MemoryStream(data);
            BinaryReader br = new BinaryReader(ms);

            int len = br.ReadInt32();
            if (len > ms.Length - ms.Position)
            {
                return null;
            }
            Byte[] result = br.ReadBytes(len);

            return result;
        }
    }
}
