using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    public class Protocol
    {
        public ProtocolTypeEnum protocolTypeEnum = ProtocolTypeEnum.NeedImplemention;
    }

    public enum ProtocolTypeEnum
    {
        NeedImplemention = 0,
        Login = 1,
        Logout = 2,
    }
}
