using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace GameProtocol
{
    [ProtoContract]
    public class LoginSuccessProtocol: Protocol
    {
        [ProtoMember(1)]
        public int PassBadge { get; set; }
    }
}
