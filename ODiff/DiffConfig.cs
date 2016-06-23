namespace ODiff
{
    public class DiffConfig
    {
        public bool AllowCyclicGraph { get; set; }

        public DiffConfig()
        {
            AllowCyclicGraph = false;
        }
    }
}
