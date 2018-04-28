using System;
namespace Model
{
    /// <summary>
    /// Model对数据库的映射，可以是表、视图，也可以是SQL
    /// </summary>
    public class DboProjectionAttribute : Attribute
    {
        /// <summary>
        /// 映射的具体表、视图或SQL
        /// </summary>
        public string Projection { get; private set; }
        /// <summary>
        /// 映射对象是否支持写操作（表=True，非表=False）
        /// </summary>
        public bool Readonly { get; private set; }
        public DboProjectionAttribute(string name,bool readOnly = false)
        {
            
            //这里可以是普通的表名，也可以是连接表名（写法是(select * from a left join b on a.aid = b.bid)）
            this.Projection = name;
            this.Readonly = readOnly;
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
