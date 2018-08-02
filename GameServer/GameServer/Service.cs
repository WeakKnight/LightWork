using System;
using System.Collections.Generic;
using System.Text;
using GameProtocol;


namespace GameServer
{
    public class Service
    {
        public Service(Server server)
        {
            this.server = server;
        }

        public Server server;

        public virtual void Execute(Protocol protocol, ClientToken client)
        {
        }
    }
}
