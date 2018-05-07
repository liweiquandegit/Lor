using System;
using SqlMaker.Common;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Reflection;

namespace SqlMaker
{
    /// <summary>
    /// 数据库的操作类
    /// </summary>
    /// <typeparam name="T">需要操作的对象类型</typeparam>
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
        /// <summary>
        /// 从数据库中取得制定条件的对象
        /// </summary>
        /// <param name="tran">事务号</param>
        /// <param name="order">排序信息</param>
        /// <param name="limit">分页信息</param>
        /// <param name="message">提示信息</param>
        /// <param name="restrain">条件</param>
        /// <returns>对象列表</returns>
        public abstract IList<T> Select( string tran, IList<OrderBy> order, Limit limit, out string message,params Restrain[] restrain);
        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="data">要更新的实例</param>
        /// <param name="tran">事务号</param>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        public bool Update(T data, string tran, out string message)
        {
            DboProjectionAttribute dboName = typeof(T).GetCustomAttribute<DboProjectionAttribute>();
            if (dboName == null)
                throw new NotSupportedException();
            if (dboName.Readonly)
                throw new NotSupportedException();
            return _Update(data, tran, out message);
        }
        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="data">要删除的实例</param>
        /// <param name="tran">事务号</param>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        public bool Delete(T data, string tran, out string message)
        {
            DboProjectionAttribute dboName = typeof(T).GetCustomAttribute<DboProjectionAttribute>();
            if (dboName == null)
                throw new NotSupportedException();
            if (dboName.Readonly)
                throw new NotSupportedException();
            return _Delete(data, tran, out message);
        }
        /// <summary>
        /// 新增数据库
        /// </summary>
        /// <param name="data">要新增的实例</param>
        /// <param name="tran">事务号</param>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        public bool Insert(T data, string tran, out string message)
        {
            DboProjectionAttribute dboName = typeof(T).GetCustomAttribute<DboProjectionAttribute>();
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
