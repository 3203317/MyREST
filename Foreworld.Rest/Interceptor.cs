using System;
using System.Web;

namespace Foreworld.Rest
{
    public abstract class Interceptor
    {
        protected Interceptor successor;

        public void SetSuccessor(Interceptor @successor)
        {
            this.successor = @successor;
        }

        public abstract void RequestInterceptor(HttpContext @context);
    }
}
