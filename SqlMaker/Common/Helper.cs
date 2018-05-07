using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlMaker.Common
{
    public class Helper
    {
        public static DBType GetDBType<T>()where T:BaseModel,new()
        {
            Type typ = typeof(T);
            object[] attrs = typ.GetCustomAttributes(true);
            foreach (Attribute attr in attrs)
            {
                if (attr.GetType() == typeof(DboProjectionAttribute))
                {
                    return ((DboProjectionAttribute)attr).DBtype;
                }
            }
            throw new Exception();
        }
        public static SqlMaker<T> GetSqlDbInstance<T>() where T:BaseModel,new()
        {
            Type typ = typeof(T);
            object[] attrs = typ.GetCustomAttributes(true);
            foreach(Attribute attr in attrs)
            {
                if(attr.GetType()==typeof(DboProjectionAttribute))
                {
                    switch(((DboProjectionAttribute)attr).DBtype)
                    {
                        case DBType.MsSql:
                            return new MsSqlMaker<T>();
                        case DBType.Oracle:
                            return new OraSqlMaker<T>();
                    }
                }
            }
            throw new Exception();
        }
    }
}
