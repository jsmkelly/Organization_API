namespace Organization_API.Model_Classes
{
    /// <summary>
    /// A base model class that has the common meta data for all the models.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class BaseModel<T>
    {
        private List<T> data = new List<T>();
        private int offset;
        private int fetch;

        public BaseModel(int offset, int fetch)
        {
            this.offset = offset;
            this.fetch = fetch;
        }

        public List<T> Data { get => data; set => data = value; }
        public int Offset { get => offset; set => offset = value; }
        public int Fetch { get => fetch; set => fetch = value; }
    }
}
