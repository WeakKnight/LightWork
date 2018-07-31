using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;
namespace GameProtocol
{
    [ProtoContract]
    public class LoginProtocol
    {
        [ProtoMember(1)]
        public string userId { get; set; }
        [ProtoMember(2)]
        public string userPassword { get; set; }
    }
}
