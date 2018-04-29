using System;
using System.Data.Common;
using SqlMaker.Common;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.OracleClient;

namespace SqlMaker
{
    /// <summary>
    /// 提供数据库操作的帮助类
    /// </summary>
    public static class SqlProvider
    {
        static SqlProvider()
        {
            bool result = ThreadPool.QueueUserWorkItem(KillTransaction);
        }
        /// <summary>
        /// 获得数据连接实例
        /// </summary>
        /// <returns>数据连接实例</returns>
        public static DbConnection GetConnection()
        {
            DbConnection connection;
            switch (Variables.DbType)
            {
                default:
                    connection = new SqlConnection();
                    break;
                case 0:
                    connection = new SqlConnection();
                    break;
                case 1:
                    connection = new OracleConnection();
                    break;
            }
            connection.ConnectionString = Variables.ConnStr;
            return connection;
        }
        /// <summary>
        /// 创建事务
        /// </summary>
        /// <returns>事务号</returns>
        public static string CreateTransaction()
        {
            DbConnection connection = GetConnection();
            DbTransaction tran = null;
            string key = "";
            try
            {
                connection.Open();
                tran = connection.BeginTransaction();
                key = tran.GetHashCode().ToString();
                TranRecord record = new TranRecord() { LastCall = DateTime.Now, Tran = tran };
                tranPool.Add(key, record);
            }
            catch (Exception ex)
            {
            }
            return key;
        }
        /// <summary>
        /// 获取事务实例
        /// </summary>
        /// <param name="key">事务号</param>
        /// <returns>事务实例</returns>
        public static DbTransaction GetTransaction(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                return null;
            }
            if (tranPool.ContainsKey(key))
            {
                TranRecord tranRecord = (TranRecord)tranPool[key];
                tranRecord.LastCall = DateTime.Now;
                return tranRecord.Tran;
            }
            return null;
        }
        /// <summary>
        /// 关闭事务
        /// </summary>
        /// <param name="key">事务号</param>
        /// <param name="commit">True=提交；False=回滚</param>
        public static void CloseTransaction(string key, bool commit)
        {
            DbTransaction tran = GetTransaction(key);
            if (tran == null)
            {
                return;
            }
            try
            {
                if (commit)
                    tran.Commit();
                else
                    tran.Rollback();
                tranPool.Remove(key);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 获取数据库操作的实例
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <returns>数据库操作实例</returns>
        public static SqlMaker<T> GetDbInstance<T>() where T : BaseModel, new()
        {
            switch (Variables.DbType)
            {
                default:
                    return new MsSqlMaker<T>();
                case 0:
                    return new MsSqlMaker<T>();
                case 1:
                    return new OraSqlMaker<T>();
            }

        }
        static object locker = new object();
        private static void KillTransaction(object arg)
        {
            while (true)
            {
                try
                {
                    lock (locker)
                    {
                        IList<string> transWillKilled = new List<string>();
                        foreach (DictionaryEntry de in tranPool)
                        {
                            try
                            {
                                TranRecord tranRecord = de.Value as TranRecord;
                                if (tranRecord.LastCall.AddMilliseconds(Math.Abs(Variables.TRANS_TTL)) <= DateTime.Now)
                                {
                                    tranRecord.Tran.Rollback();
                                    transWillKilled.Add(de.Key.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        foreach (string dieTran in transWillKilled)
                        {
                            tranPool.Remove(dieTran);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                Thread.Sleep(Variables.KILL_TRANS_PERIOD);
            }
        }
        private static Hashtable tranPool = Hashtable.Synchronized(new Hashtable());
        private class TranRecord
        {
            public DateTime LastCall { get; set; }
            public DbTransaction Tran { get; set; }
        }
    }
}
