namespace SqlMaker
{
    /// <summary>
    /// 排序信息
    /// </summary>
    public class OrderBy
    {
        private string field;
        private bool asc;
        public OrderBy()
        {
        }
        /// <summary>
        /// 创建排序信息实例
        /// </summary>
        /// <param name="field">排序属性</param>
        /// <param name="asc">True=正序，False=逆序</param>
        public OrderBy(string field, bool asc)
        {
            this.field = field;
            this.asc = asc;
        }
        /// <summary>
        /// 排序属性
        /// </summary>
        public string Field { get { return field; } set { field = value; } }
        /// <summary>
        /// 排序方向
        /// </summary>
        public bool Asc { get { return asc; } set { asc = value; } }
    }
}
