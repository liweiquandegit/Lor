using System;
using System.Collections.Generic;
using System.Text;

namespace SqlMaker
{
    /// <summary>
    /// 限定条件的实例
    /// </summary>
    public class Restrain
    {
        private Restrain() { }
        static ushort counter;
        static object locker;
        static Restrain()
        {
            locker = new object();
        }
        static ushort Counter
        {
            get
            {
                lock (locker)
                {
                    if (counter >= 10000)
                        counter = 0;
                    else
                        counter++;
                    return counter;
                }
            }
        }
        /// <summary>
        /// 与条件
        /// </summary>
        /// <param name="restrain">另一条件</param>
        /// <returns>新的条件</returns>
        public Restrain And(Restrain restrain)
        {
            if (restrain == null)
                throw new ArgumentNullException("方法Add不接受restrain为空的值");
            Restrain _restrain = Restrain.And(this, restrain);
            return _restrain;
        }
        public string SqlStr;
        private Dictionary<string, object> _Params = new Dictionary<string, object>();
        /// <summary>
        /// SQL的参数键值对
        /// </summary>
        public Dictionary<string, object> Params { get { return _Params; } private set { _Params = value; } }
        /// <summary>
        /// 等于条件
        /// </summary>
        /// <param name="key">类型的属性</param>
        /// <param name="value">条件值</param>
        /// <returns>条件</returns>
        public static Restrain Eq(string key, object value)
        {
            if (String.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Eq方法不接受空白或者空值的Key参数");
            Restrain restrain = new Restrain();
            string paramCode = String.Format("__PARAMCODE__{0}_{1}", key.Trim(), Counter);
            restrain.Params.Add(paramCode, value);
            restrain.SqlStr = String.Format("AND {0} = {1}", key, paramCode);
            return restrain;
        }
        /// <summary>
        /// 存在于条件
        /// </summary>
        /// <param name="key">类型的属性</param>
        /// <param name="values">条件值</param>
        /// <returns>条件</returns>
        public static Restrain In(string key, params object[] values)
        {
            if(String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("In方法不接受空白或者空值的Key参数");
            }
            Restrain restrain = new Restrain();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (object value in values)
            {
                string paramCode = String.Format("__PARAMCODE__{0}_{1}", key.Trim(), Counter);
                stringBuilder.AppendFormat(" {0},", paramCode);
                restrain.Params.Add(paramCode, value);
            }
            if(stringBuilder.Length>0)
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            restrain.SqlStr = String.Format("AND {0} IN ({1})", key, stringBuilder);
            return restrain;
        }
        /// <summary>
        /// 大于条件
        /// </summary>
        /// <param name="key">类型的属性</param>
        /// <param name="value">条件值</param>
        /// <returns>条件</returns>
        public static Restrain Gt(string key, object value)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Gt方法不接受空白或者空值的Key参数");
            }
            Restrain restrain = new Restrain();
            string paramCode = String.Format("__PARAMCODE__{0}_{1}", key.Trim(), Counter);
            restrain.Params.Add(paramCode, value);
            restrain.SqlStr = String.Format("AND {0} > {1}", key, paramCode);
            return restrain;
        }
        /// <summary>
        /// 小于条件
        /// </summary>
        /// <param name="key">类型的属性</param>
        /// <param name="value">条件值</param>
        /// <returns>条件</returns>
        public static Restrain Lt(string key, object value)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Gt方法不接受空白或者空值的Key参数");
            }
            Restrain restrain = new Restrain();
            string paramCode = String.Format("__PARAMCODE__{0}_{1}", key.Trim(), Counter);
            restrain.Params.Add(paramCode, value);
            restrain.SqlStr = String.Format("AND {0} < {1}", key, paramCode);
            return restrain;
        }
        /// <summary>
        /// 相似条件
        /// </summary>
        /// <param name="key">类型的属性</param>
        /// <param name="value">条件值</param>
        /// <returns>条件</returns>
        public static Restrain Lk(string key, object value)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Lk方法不接受空白或者空值的Key参数");
            }
            Restrain restrain = new Restrain();
            string paramCode = String.Format("__PARAMCODE__{0}_{1}", key.Trim(), Counter);
            restrain.Params.Add(paramCode, value);
            restrain.SqlStr = String.Format("AND {0} LIKE {1}", key, paramCode);
            return restrain;
        }
        /// <summary>
        /// 大于等于条件
        /// </summary>
        /// <param name="key">类型的属性</param>
        /// <param name="value">条件值</param>
        /// <returns>条件</returns>
        public static Restrain Ge(string key, object value)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Ge方法不接受空白或者空值的Key参数");
            }
            Restrain restrain = new Restrain();
            string paramCode = String.Format("__PARAMCODE__{0}_{1}", key.Trim(), Counter);
            restrain.Params.Add(paramCode, value);
            restrain.SqlStr = String.Format("AND {0} >= {1}", key, paramCode);
            return restrain;
        }
        /// <summary>
        /// 小于等于条件
        /// </summary>
        /// <param name="key">类型的属性</param>
        /// <param name="value">条件值</param>
        /// <returns>条件</returns>
        public static Restrain Le(string key, object value)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Le方法不接受空白或者空值的Key参数");
            }
            Restrain restrain = new Restrain();
            string paramCode = String.Format("__PARAMCODE__{0}_{1}", key.Trim(), Counter);
            restrain.Params.Add(paramCode, value);
            restrain.SqlStr = String.Format("AND {0} <= {1}", key, paramCode);
            return restrain;
        }
        /// <summary>
        /// 不存在条件
        /// </summary>
        /// <param name="key">类型的属性</param>
        /// <param name="values">条件值</param>
        /// <returns>条件</returns>
        public static Restrain NotIn(string key, params object[] values)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("NotIn方法不接受空白或者空值的Key参数");
            }
            Restrain restrain = new Restrain();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (object value in values)
            {
                string paramCode = String.Format("__PARAMCODE__{0}_{1}", key.Trim(), Counter);
                restrain.Params.Add(paramCode, value);
                stringBuilder.Append(String.Format(" {0},", paramCode));
            }
            if(stringBuilder.Length>0)
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            restrain.SqlStr = String.Format("AND {0} NOT IN ({1})", key, stringBuilder);
            return restrain;
        }
        /// <summary>
        /// 不等于条件
        /// </summary>
        /// <param name="key">类型的属性</param>
        /// <param name="value">条件值</param>
        /// <returns>条件</returns>
        public static Restrain NotEq(string key, object value)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("NotEq方法不接受空白或者空值的Key参数");
            }
            Restrain restrain = new Restrain();
            string paramCode = String.Format("__PARAMCODE__{0}_{1}", key.Trim(), Counter);
            restrain.Params.Add(paramCode, value);
            restrain.SqlStr = String.Format("AND {0} != {1}", key, paramCode);
            return restrain;
        }
        /// <summary>
        /// 非条件
        /// </summary>
        /// <param name="restrain">条件</param>
        /// <returns>新的条件</returns>
        public static Restrain Not(Restrain restrain)
        {
            if (restrain == null)
                throw new ArgumentNullException("Not方法的restrain参数不允许为空");
            Restrain _restrain = new Restrain();
            _restrain.Params = restrain.Params;
            _restrain.SqlStr = String.Format("AND NOT (1=1 {0})", restrain.SqlStr);
            return _restrain;
        }
        /// <summary>
        /// 或条件
        /// </summary>
        /// <param name="restrain1">条件1</param>
        /// <param name="restrain2">条件2</param>
        /// <returns>新的条件</returns>
        public static Restrain Or(Restrain restrain1, Restrain restrain2)
        {
            if (restrain1 == null || restrain2 == null)
                throw new ArgumentNullException("Or方法的restrain参数不允许为空");
            Restrain _restrain = new Restrain();
            foreach (KeyValuePair<string, object> de in restrain1.Params)
            {
                _restrain.Params.Add(de.Key, de.Value);
            }
            foreach (KeyValuePair<string, object> de in restrain2.Params)
            {
                _restrain.Params.Add(de.Key, de.Value);
            }
            _restrain.SqlStr = String.Format("AND (1=1 {0}) OR (1=1 {1})", restrain1.SqlStr, restrain2.SqlStr);
            return _restrain;
        }
        /// <summary>
        /// 与条件
        /// </summary>
        /// <param name="restrain1">条件1</param>
        /// <param name="restrain2">条件2</param>
        /// <returns>新的条件</returns>
        public static Restrain And(Restrain restrain1, Restrain restrain2)
        {
            if (restrain1 == null || restrain2 == null)
                throw new ArgumentNullException("And方法的restrain参数不允许为空");
            Restrain _restrain = new Restrain();
            foreach (KeyValuePair<string, object> de in restrain1.Params)
            {
                _restrain.Params.Add(de.Key, de.Value);
            }
            foreach (KeyValuePair<string, object> de in restrain2.Params)
            {
                _restrain.Params.Add(de.Key, de.Value);
            }
            _restrain.SqlStr = String.Format("AND (1=1 {0}) AND (1=1 {1})", restrain1.SqlStr, restrain2.SqlStr);
            return _restrain;
        }
    }
}
