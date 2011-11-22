namespace ODiff
{
    public interface INodeInterceptor
    {
        bool Use(string memberPath, object node);
        object Intercept(object node);
    }
}
