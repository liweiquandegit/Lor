using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlMaker.Common;
using System.Data.OracleClient;
using System.Reflection;

namespace SqlMaker
{
    public class OraSqlMaker<T> : SqlMaker<T> where T : BaseModel, new()
    {
        protected override bool _Delete(T data, string tran, out string message)
        {

            DboProjectionAttribute tnAttr = typeof(T).GetCustomAttribute<DboProjectionAttribute>();
            if (tnAttr == null || String.IsNullOrWhiteSpace(tnAttr.Projection))
            {
                throw new Exception(String.Format("对象{0}没有设置表映射关系", typeof(T).Name));
            }
            if (data == null)
            {
                throw new ArgumentNullException("Delete的参数data不能为空");
            }

            int result = -1;
            OracleCommand sqlCmd = new OracleCommand();
            StringBuilder strParams = new StringBuilder();
            object key = null;
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                if (pi.GetCustomAttribute<NotColumnAttribute>() != null)
                    continue;
                PrimaryKeyAttribute pkAttr = pi.GetCustomAttribute<PrimaryKeyAttribute>();
                if (pkAttr != null)
                {
                    strParams.AppendFormat("AND {0}=:{0} ", pi.Name);
                    key = pi.GetValue(data);
                    sqlCmd.Parameters.Add(new OracleParameter(":" + pi.Name, key));
                }
            }
            strParams.Remove(strParams.Length - 1, 1);
            sqlCmd.CommandText = String.Format("DELETE FROM {0} WHERE 1=1 {1}", tnAttr.Projection, strParams);
            Debug(sqlCmd);
            OracleTransaction sqlTran = ((OracleTransaction)SqlProvider.GetTransaction(tran));
            if (sqlTran != null)
            {
                sqlCmd.Connection = sqlTran.Connection;
                sqlCmd.Transaction = sqlTran;
                try
                {
                    result = sqlCmd.ExecuteNonQuery();
                    if (result <= 0)
                        message = "不存在对象，或对象已被变更";
                    else
                        message = "成功";
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    SqlProvider.CloseTransaction(tran, false);
                }
                return result > 0;
            }
            else
            {
                message = String.Format("Trans={0}不存在", tran);
                return false;
            }
        }
        protected override bool _Insert(T data, string tran, out string message)
        {
            DboProjectionAttribute tnAttr = typeof(T).GetCustomAttribute<DboProjectionAttribute>();
            if (tnAttr == null || String.IsNullOrWhiteSpace(tnAttr.Projection))
            {
                throw new Exception(String.Format("对象{0}没有设置表映射关系", typeof(T).Name));
            }
            if (data == null)
            {
                throw new ArgumentNullException("Delete的参数data不能为空");
            }
            long result = -1;
            OracleCommand sqlCmd = new OracleCommand();
            StringBuilder strColumns = new StringBuilder();
            StringBuilder strParams = new StringBuilder();
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                if (pi.GetCustomAttribute<NotColumnAttribute>() != null)
                    continue;
                else if (!data.GetTracer().Contains(pi.Name))
                {
                    continue;
                }
                strColumns.AppendFormat("{0},", pi.Name);
                strParams.AppendFormat(":{0},", pi.Name);
                object paramValue = pi.GetValue(data);
                sqlCmd.Parameters.Add(new OracleParameter(":" + pi.Name, paramValue == null ? DBNull.Value : paramValue));
            }
            strColumns.Remove(strColumns.Length - 1, 1);
            strParams.Remove(strParams.Length - 1, 1);
            sqlCmd.CommandText = String.Format("INSERT INTO {0}({1}) VALUES({2}) ", tnAttr.Projection, strColumns, strParams);
            OracleTransaction sqlTran = ((OracleTransaction)SqlProvider.GetTransaction(tran));
            if (sqlTran != null)
            {
                sqlCmd.Connection = sqlTran.Connection;
                sqlCmd.Transaction = sqlTran;
                Debug(sqlCmd);
                try
                {
                    result =sqlCmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        message = "操作成功";
                    }
                    else
                    {
                        message = "操作失败";
                    }
                }
                catch (Exception ex)
                {
                    SqlProvider.CloseTransaction(tran, false);
                    message = ex.Message;
                }
                return result > 0;
            }
            else
            {
                message = String.Format("Trans={0}不存在", tran);
                return false;
            }
        }
        public override IList<T> Select(string tran, IList<OrderBy> orders, Limit limit, out string message, params Restrain[] restrain)
        {
            DboProjectionAttribute tnAttr = typeof(T).GetCustomAttribute<DboProjectionAttribute>();
            if (tnAttr == null || String.IsNullOrWhiteSpace(tnAttr.Projection))
            {
                throw new Exception(String.Format("对象{0}没有设置表映射关系", typeof(T).Name));
            }
            IList<T> result = new List<T>();
            StringBuilder restainStr = new StringBuilder();
            OracleCommand sqlCmd = new OracleCommand();
            foreach (Restrain rst in restrain)
            {
                if (rst == null)
                    continue;
                restainStr.Append(rst.SqlStr);
                foreach (KeyValuePair<string, object> kv in rst.Params)
                {
                    OracleParameter param = new OracleParameter
                    {
                        ParameterName = kv.Key.Replace("__PARAMCODE__", ":"),
                        Value = kv.Value
                    };
                    sqlCmd.Parameters.Add(param);
                }
            }
            StringBuilder ordBy = new StringBuilder();
            PropertyInfo[] pis = typeof(T).GetProperties();
            IEnumerable<string> piNames = from pi in pis select pi.Name;
            ordBy.Append(" ORDER BY ");
            if (orders != null)
            {
                foreach (OrderBy order in orders)
                {
                    if (String.IsNullOrWhiteSpace(order.Field))
                        throw new ArgumentNullException();
                    if (piNames.Contains(order.Field))
                    {
                        ordBy.AppendFormat(" {0}", order.Field);
                    }
                    if (!order.Asc)
                        ordBy.Append(" DESC");
                    else
                        ordBy.Append(" ASC");
                    ordBy.Append(" ,");

                }
            }

            if (ordBy[ordBy.Length - 1] == ',')
                ordBy.Remove(ordBy.Length - 1, 1);
            OracleTransaction sqlTran = null;
            bool runInTran = !String.IsNullOrWhiteSpace(tran);
            if (runInTran)
                sqlTran = (OracleTransaction)SqlProvider.GetTransaction(tran);
            else
            {
                tran = SqlProvider.CreateTransaction();
                sqlTran = (OracleTransaction)SqlProvider.GetTransaction(tran);
            }
            if (sqlTran == null && runInTran)
            {
                message = String.Format("Trans={0}不存在", tran);
                return new List<T>();
            }
            if (limit == null)
                limit = new Limit();
            string sql = "SELECT TB.* FROM (SELECT ROWNUM AS AUTOID__, T.* FROM {3} T WHERE ROWNUM < {1} {4} {2}) TB WHERE TB.AUTOID__ > {0}  ";
            sqlCmd.CommandText = String.Format(sql, limit.Pos, limit.Size + limit.Pos, ordBy.ToString(),
                tnAttr.Projection,  restainStr)
                .Replace("__PARAMCODE__", ":");
            sqlCmd.Transaction = sqlTran;
            sqlCmd.Connection = sqlTran.Connection;
            try
            {
                OracleDataReader reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    T item = new T();
                    PropertyInfo[] pList = typeof(T).GetProperties();
                    foreach (PropertyInfo pi in pList)
                    {
                        if (pi.GetCustomAttribute<NotColumnAttribute>() != null)
                            continue;
                        object val = reader[pi.Name];
                        if (val.GetType() != typeof(DBNull))
                        {
                            pi.SetValue(item, Convert.ChangeType(val, pi.PropertyType));
                        }
                    }
                    item.ResetTracer();
                    result.Add(item);
                }
                reader.Close();
                if (!runInTran)
                {
                    SqlProvider.CloseTransaction(tran, true);
                }
                message = String.Format("列出对象{0}笔", result.Count);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                SqlProvider.CloseTransaction(tran, false);
            }

            return result;
        }
        protected override bool _Update(T data, string tran, out string message)
        {
            object key = null;
            DboProjectionAttribute tnAttr = typeof(T).GetCustomAttribute<DboProjectionAttribute>();
            if (tnAttr == null || String.IsNullOrWhiteSpace(tnAttr.Projection))
            {
                throw new Exception(String.Format("对象{0}没有设置表映射关系", typeof(T).Name));
            }
            if (data == null)
            {
                throw new ArgumentNullException("Update的参数data不能为空");
            }
            int result = -1;
            OracleCommand sqlCmd = new OracleCommand();
            StringBuilder strColumns = new StringBuilder();
            StringBuilder strRstParam = new StringBuilder();
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                if (pi.GetCustomAttribute<NotColumnAttribute>() != null)
                    continue;
                object paramValue = pi.GetValue(data);
                PrimaryKeyAttribute pkAttr = pi.GetCustomAttribute<PrimaryKeyAttribute>();
                if (pkAttr != null)
                {
                    strRstParam.AppendFormat(" AND {0}=:{0} ", pi.Name);
                    key = paramValue;
                }
                else if (data.GetTracer().Contains(pi.Name))
                    strColumns.AppendFormat(" {0}=:{0},", pi.Name);
                else
                    continue;

                sqlCmd.Parameters.Add(new OracleParameter(":" + pi.Name, paramValue == null ? DBNull.Value : paramValue));
            }
            if (strColumns.Length == 0)
            {
                message = "数据对象没有发生变更";
                return true;
            }
            strColumns.Remove(strColumns.Length - 1, 1);
            sqlCmd.CommandText = String.Format("UPDATE {0} SET {1} WHERE 1=1 {2}", tnAttr.Projection, strColumns, strRstParam);
            Debug(sqlCmd);
            OracleTransaction sqlTran = ((OracleTransaction)SqlProvider.GetTransaction(tran));
            if (sqlTran != null)
            {
                sqlCmd.Connection = sqlTran.Connection;
                sqlCmd.Transaction = sqlTran;
                try
                {
                    result = sqlCmd.ExecuteNonQuery();
                    if (result <= 0)
                        message = "不存在对象，或对象已被变更";
                    else
                    {
                        message = "成功";
                        data.ResetTracer();
                    }
                }
                catch (Exception ex)
                {
                    SqlProvider.CloseTransaction(tran, false);
                    message = ex.Message;
                }
                return result > 0;
            }
            else
            {
                message = String.Format("Trans={0}不存在", tran);
                return false;
            }
        }
    }
}
