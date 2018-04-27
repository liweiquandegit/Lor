namespace SqlMaker
{
    public class Limit
    {

        uint pos, size;
        public Limit()
        {
            pos = 0;
            size = 1000;
        }
        public Limit(uint pos, uint size)
        {
            this.pos = pos;
            this.size = size;
        }
        public uint Pos { get { return pos; } set { pos = value; } }
        public uint Size { get { return size; } set { size = value; } }
    }
}
