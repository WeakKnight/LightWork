using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    public static class Mapper
    {
        static readonly Dictionary<Type, int> ProtocolToIntDic = new Dictionary<Type, int>
        {
                {
                    typeof(LoginProtocol),1
                },
                {
                    typeof(RegisterProtocol),2
                },
                {
                    typeof(LoginSuccessProtocol),3
                },
                {
                    typeof(LoginFailedProtocol),4
                },
                {
                    typeof(RegisterSuccessProtocol),5
                },
                {
                    typeof(RegisterFailedProtocol),6
                }
        };

        static readonly Dictionary<int, Type> IntToProtocolDic = new Dictionary<int, Type>
        {
                {
                    1,typeof(LoginProtocol)
                },
                {
                    2,typeof(RegisterProtocol)
                },
                {
                    3,typeof(LoginSuccessProtocol)
                },
                {
                    4,typeof(LoginFailedProtocol)
                },
                {
                    5,typeof(RegisterSuccessProtocol)
                },
                {
                    6,typeof(RegisterFailedProtocol)
                }
        };

        static public int GetProtocolIntMappingValue(Protocol protocol)
        {
            
            return ProtocolToIntDic[protocol.GetType()];
        }

        static public Type GetIntProtocolMappingValue(int value)
        {
            return IntToProtocolDic[value];
        }
    }
}
