using System;
using Model;
using Common;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;

namespace SqlMaker
{
    public abstract class SqlMaker<T> where T : BaseModel, new()
    {
        protected void Debug(DbCommand command)
        {
            if (command == null)
                return;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("SQL:{0}",command.CommandText);
            foreach(DbParameter parameter in command.Parameters)
            {
                stringBuilder.AppendFormat("\r\nParam:{0}={1}", parameter.ParameterName, parameter.Value);
            }
        }
        public abstract IList<T> Select( string tran, IList<OrderBy> order, Limit limit, out string message,params Restrain[] restrain);
        public bool Update(T data, string tran, out string message)
        {
            DboNameAttribute dboName = typeof(T).GetCustomAttribute<DboNameAttribute>();
            if (dboName == null)
                throw new NotSupportedException();
            if (dboName.Readonly)
                throw new NotSupportedException();
            return _Update(data, tran, out message);
        }
        public bool Delete(T data, string tran, out string message)
        {
            DboNameAttribute dboName = typeof(T).GetCustomAttribute<DboNameAttribute>();
            if (dboName == null)
                throw new NotSupportedException();
            if (dboName.Readonly)
                throw new NotSupportedException();
            return _Delete(data, tran, out message);
        }
        public bool Insert(T data, string tran, out string message)
        {
            DboNameAttribute dboName = typeof(T).GetCustomAttribute<DboNameAttribute>();
            if (dboName == null)
                throw new NotSupportedException();
            if (dboName.Readonly)
                throw new NotSupportedException();
            return _Insert(data, tran, out message);
        }
        protected abstract bool _Update(T data, string tran, out string message);
        protected abstract bool _Delete(T data, string tran, out string message);
        protected abstract bool _Insert(T data, string tran, out string message);
    }
}
