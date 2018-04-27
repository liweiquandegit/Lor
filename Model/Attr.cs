using System;
namespace Model
{
    public class DboNameAttribute : Attribute
    {
        public string DboName { get; private set; }
        public DboNameAttribute(string name)
        {
            //这里可以是普通的表名，也可以是连接表名（写法是(select * from a left join b on a.aid = b.bid)）
            this.DboName = name;
        }
    }
    public class NotColumnAttribute : Attribute
    {

    }
    public class PrimaryKeyAttribute : Attribute
    {

    }
    public class FlagKeyAttribute : Attribute
    {

    }
}
