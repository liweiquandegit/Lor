using System;
using System.Reflection;
using SqlMaker.Common;
using System.Linq;
using System.Collections.Generic;

namespace SqlMaker
{
    public class DataFetcher<T> where T: BaseModel,new()
    {
        public DataFetcher()
        {
            sqlMaker = Helper.GetSqlDbInstance<T>();
        }

        SqlMaker<T> sqlMaker;
        public virtual T GetById(string tran,object id)
        {
            string message = "";
            Type typ = typeof(T);
            string primaryKeyName = "";
            foreach(PropertyInfo pi in typ.GetProperties())
            {
                if(pi.GetCustomAttribute<PrimaryKeyAttribute>()!=null)
                {
                    primaryKeyName = pi.Name;
                    break;
                }
            }
            if(String.IsNullOrWhiteSpace(primaryKeyName))
            {
                throw new FieldAccessException();
            }
            IList<T> dataSet = sqlMaker.Select(tran, OrderBy.GetDefaultOrderBy<T>(), new Limit(), out message, Restrain.Eq(primaryKeyName, id));
            if (dataSet.Count > 0)
                return dataSet.First();
            else
                return null;
        }
        public virtual IList<T> List(string tran)
        {
            string message = "";
            Type typ = typeof(T);
            IList<T> dataSet = sqlMaker.Select(tran, OrderBy.GetDefaultOrderBy<T>(), new Limit(), out message);
            return dataSet;
        }
    }
}
