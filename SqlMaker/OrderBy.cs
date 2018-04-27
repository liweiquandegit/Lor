namespace SqlMaker
{
    public class OrderBy
    {
        private string field;
        private bool asc;
        public OrderBy()
        {
        }
        public OrderBy(string field, bool asc)
        {
            this.field = field;
            this.asc = asc;
        }
        public string Field { get { return field; } set { field = value; } }
        public bool Asc { get { return asc; } set { asc = value; } }
    }
}
