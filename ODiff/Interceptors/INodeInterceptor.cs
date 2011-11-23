namespace ODiff.Interceptors
{
    public interface INodeInterceptor
    {
        bool Use(string memberPath, object node);
        object Intercept(object node);
    }
}
