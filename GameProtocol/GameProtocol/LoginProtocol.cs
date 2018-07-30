using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;
namespace GameProtocol
{
    [ProtoContract]
    public class LoginProtocol : Protocol
    {
        public LoginProtocol(string userId, string userPassword)
        {
            this.userId = userId;
            this.userPassword = userPassword;
            this.protocolTypeEnum = ProtocolTypeEnum.Login;
        }

        [ProtoMember(1)]
        public string userId;

        [ProtoMember(2)]
        public string userPassword;
    }
}
