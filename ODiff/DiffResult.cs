namespace ODiff
{
    public class DiffResult
    {
        private bool diffFound;

        public DiffResult(bool diffFound)
        {
            this.diffFound = diffFound;
        }

        public bool DiffFound
        {
            get { return diffFound; }
        }

        public void Merge(DiffResult anotherResult)
        {
            if (!diffFound)
            {
                diffFound = anotherResult.diffFound;
            }
        }
    }
}
