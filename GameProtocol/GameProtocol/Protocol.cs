using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace GameProtocol
{
    [ProtoContract]
    [ProtoInclude(1, typeof(LoginProtocol))]
    public class Protocol
    {
    }

    public enum ProtocolTypeEnum
    {
        Login = 0,
        Logout = 1,
    }
}
