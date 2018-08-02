using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace GameProtocol
{
    [ProtoContract]
    public enum RegisterFailedState
    {
        [ProtoEnum]
        @Unknown = 0,
        [ProtoEnum]
        IDAlreadyExist = 1
    }

    [ProtoContract]
    public class RegisterFailedProtocol : Protocol
    {
        [ProtoMember(1)]
        public RegisterFailedState state;
    }
}
