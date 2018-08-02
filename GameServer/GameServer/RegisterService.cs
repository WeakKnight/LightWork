using System;
using System.Collections.Generic;
using System.Text;
using GameProtocol;
using LiteDB;

namespace GameServer
{
    public class RegisterService : Service
    {
        public RegisterService(Server server) : base(server)
        {
        }

        public override void Execute(Protocol protocol, ClientToken client)
        {
            if (protocol.GetType() != typeof(RegisterProtocol))
            {
                return;
            }

            RegisterProtocol registerProtocol = protocol as RegisterProtocol;

            Console.WriteLine("注册服务运行，账号为:"+ registerProtocol.userId+"，密码为:"+registerProtocol.userPassword+";");

            AccountDatabase accountDatabase = server.databases[typeof(AccountDatabase)] as AccountDatabase;

            //检查是否ID已经被注册
            if (accountDatabase.Find(x => (x.id == registerProtocol.userId)) != null)
            {
                //已被注册，返回注册失败
                RegisterFailedProtocol registerFailedProtocol = new RegisterFailedProtocol();
                registerFailedProtocol.state = RegisterFailedState.IDAlreadyExist;

                Byte[] serializedProtocol = ProtocolHelper.ConvertProtocolToBytes(registerFailedProtocol);
                Byte[] encodingData = GameProtocol.Encoder.Encode(serializedProtocol);

                client.WriteSendData(encodingData);
            }
            else
            {
                //注册成功
                RegisterSuccessProtocol registerSuccessProtocol = new RegisterSuccessProtocol();
                registerSuccessProtocol.PassBadge = 0;

                Byte[] serializedProtocol = ProtocolHelper.ConvertProtocolToBytes(registerSuccessProtocol);
                Byte[] encodingData = GameProtocol.Encoder.Encode(serializedProtocol);

                client.WriteSendData(encodingData);
            }
        }
    }
}
