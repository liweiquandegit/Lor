namespace SqlMaker
{
    /// <summary>
    /// 当前会话的分页信息
    /// </summary>
    public class Limit
    {
        uint pos, size;
        public Limit()
        {
            pos = 0;
            size = (uint)short.MaxValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos">数据库起始位置</param>
        /// <param name="size">本次抓取数据的页大小</param>
        public Limit(uint pos, uint size)
        {
            this.pos = pos;
            this.size = size;
        }
        public uint Pos { get { return pos; } set { pos = value; } }
        public uint Size { get { return size; } set { size = value; } }
    }
}
