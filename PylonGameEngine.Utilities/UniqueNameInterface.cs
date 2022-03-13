namespace PylonGameEngine.Utilities
{
    public class UniqueNameInterface
    {
        private string _Name = null;
        public string Name
        {
            get
            {
                if (_Name == null)
                {
                    return this.GetType().Name;
                }
                else
                {
                    return _Name;
                }
            }

            protected set
            {
                _Name = value;
            }

        }
    }
}
