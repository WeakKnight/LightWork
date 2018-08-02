using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace GameProtocol
{
    [ProtoContract]
    [ProtoInclude(1, typeof(LoginProtocol))]
    [ProtoInclude(2, typeof(RegisterProtocol))]
    [ProtoInclude(3, typeof(LoginSuccessProtocol))]
    [ProtoInclude(4, typeof(LoginFailedProtocol))]
    [ProtoInclude(5, typeof(RegisterSuccessProtocol))]
    [ProtoInclude(6, typeof(RegisterFailedProtocol))]
    public class Protocol
    {
    }
}

