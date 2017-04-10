using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace JsonToStaticTypeGenerator
{
    public class Proxy<T> : RealProxy
    {
        private T value;

        public Proxy(T value) : base(typeof(T))
        {
            this.value = value;
        }

        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage call = msg as IMethodCallMessage;
            string returnValue = "mathias says hello";
            ReturnMessage responseMessage = new ReturnMessage(returnValue, null, 0, call.LogicalCallContext, call);
            return responseMessage;
        }
    }
}
