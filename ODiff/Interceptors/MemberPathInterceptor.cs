using System;
using System.Text.RegularExpressions;

namespace ODiff.Interceptors
{
    public class MemberPathInterceptor<T> : INodeInterceptor
    {
        private readonly Func<T, object> rewriter;
        private readonly Regex regex;

        public MemberPathInterceptor(string memberPathPattern, Func<T, object> rewriter)
        {
            regex = new Regex(memberPathPattern);
            this.rewriter = rewriter;
        }

        public bool Use(string memberPath, object node)
        {
            return regex.IsMatch(memberPath);
        }

        public object Intercept(object node)
        {
            if (node is T)
                return rewriter((T)node);

            return node;
        }
    }
}
