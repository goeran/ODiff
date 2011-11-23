using System;

namespace ODiff.Interceptors
{
    public class TypeInterceptor<T> : INodeInterceptor
    {
        private readonly Func<T, object> rewriter;

        public TypeInterceptor(Func<T, object> rewriter)
        {
            this.rewriter = rewriter;
        }

        public bool Use(string memberPath, object node)
        {
            return node is T;
        }

        public object Intercept(object node)
        {
            if (node == null) return rewriter(default(T));
            return rewriter((T)node);
        }
    }
}
