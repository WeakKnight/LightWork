using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace GameProtocol
{
    [ProtoContract]
    public enum LoginFailedState
    {
        [ProtoEnum]
        @Unknown = 0,
        [ProtoEnum]
        WrongID = 1,
        [ProtoEnum]
        WrongPassword = 2
    }

    [ProtoContract]
    public class LoginFailedProtocol : Protocol
    {
        [ProtoMember(1, IsRequired = true)]
        public LoginFailedState State { get; set; }
    }
}
