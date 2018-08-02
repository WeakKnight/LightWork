using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocol
{
    public static class Mapper
    {
        static Dictionary<Type, int> ProtocolToIntDic = new Dictionary<Type, int>
        {
                {
                    typeof(LoginProtocol),1
                },
                {
                    typeof(RegisterProtocol),2
                }
        };

        static Dictionary<int, Type> IntToProtocolDic = new Dictionary<int, Type>
        {
                {
                    1,typeof(LoginProtocol)
                },
                {
                    2,typeof(RegisterProtocol)
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
