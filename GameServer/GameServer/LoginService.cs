using System;
using System.Collections.Generic;
using System.Text;
using GameProtocol;
using LiteDB;

namespace GameServer
{
    public class LoginService : Service
    {
        public LoginService(Server server) : base(server)
        {
        }

        public override void Execute(Protocol protocol, ClientToken client)
        {
            if (protocol.GetType() != typeof(LoginProtocol))
            {
                return;
            }

            LoginProtocol loginProtocol = protocol as LoginProtocol;

            Console.WriteLine("登录服务运行，账号为:" + loginProtocol.userId + "，密码为:" + loginProtocol.userPassword + ";");

            AccountDatabase accountDatabase = server.databases[typeof(AccountDatabase)] as AccountDatabase;

            //查询是否有用户名密码相符的用户
            if (accountDatabase.Find(x => (x.id == loginProtocol.userId && x.password == loginProtocol.userPassword)) != null)
            {
                //登录成功
                LoginSuccessProtocol loginSuccessProtocol = new LoginSuccessProtocol();
                //生成令牌
                loginSuccessProtocol.PassBadge = 0;

                Byte[] serializedProtocol = ProtocolHelper.ConvertProtocolToBytes(loginSuccessProtocol);
                Byte[] encodingData = GameProtocol.Encoder.Encode(serializedProtocol);

                client.WriteSendData(encodingData);
            }
            else
            {
                //登录失败
                LoginFailedProtocol loginFailedProtocol = new LoginFailedProtocol();
                if (accountDatabase.Find(x => (x.id == loginProtocol.userId)) != null)
                {
                    loginFailedProtocol.State = LoginFailedState.WrongPassword;
                }
                else
                {
                    loginFailedProtocol.State = LoginFailedState.WrongID;
                }
                Byte[] serializedProtocol = ProtocolHelper.ConvertProtocolToBytes(loginFailedProtocol);
                Byte[] encodingData = GameProtocol.Encoder.Encode(serializedProtocol);

                client.WriteSendData(encodingData);
            }
        }
    }
}
