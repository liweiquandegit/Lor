using System;
using System.Reflection;
using System.Collections.Generic;

namespace SqlMaker.Common
{
    public abstract class BaseModel
    {
        protected string _Id { get; set; }
        protected int _Flag { get; set; }
        protected string _Creator { get; set; }
        /// <summary>
        /// 跟踪对象被更改的属性，用于辅助生成Insert、Update语句
        /// </summary>
        protected IList<string> updateTrace = new List<string>();
        /// <summary>
        /// 获取对象被更改过的属性
        /// </summary>
        public string[] GetTracer()
        {
            string[] result = new string[updateTrace.Count];
            updateTrace.CopyTo(result,0);
            return result;
        }
        /// <summary>
        /// 重置变更记录跟踪器
        /// </summary>
        public void ResetTracer()
        {
            updateTrace.Clear();
        }
        [PrimaryKey]
        public virtual string Id
        {
            get { return _Id; }
            set
            {
                if (_Id == value)
                {
                    return;
                }
                _Id = value;
                if (!updateTrace.Contains("Id"))
                    updateTrace.Add("Id") ;
            }
        }
        [FlagKey]
        public virtual int Flag
        {
            get { return _Flag; }
            set
            {
                if (_Flag == value)
                {
                    return;
                }
                _Flag = value;
                if (!updateTrace.Contains("Flag"))
                    updateTrace.Add("Flag");
            }
        }
        public virtual string Creator
        {
            get { return _Creator; }
            set
            {
                if (_Creator == value)
                {
                    return;
                }
                _Creator = value;
                if (!updateTrace.Contains("Creator"))
                    updateTrace.Add("Creator");
            }
        }
        /// <summary>
        /// 将当前编辑的对象副本合并到持久化对象
        /// </summary>
        /// <typeparam name="T">来源对象类型</typeparam>
        /// <param name="other">来源对象</param>
        public virtual void Merge<T>(T other) where T : BaseModel, new()
        {
            if (other.GetType() != this.GetType())
            {
                throw new ArgumentException("Merge对象类型必须与属性类型保持一致");
            }
            //string[] trace = new string[updateTrace.Count];
            //this.updateTrace.CopyTo(trace, 0);
            this.updateTrace.Clear();
            //T swap = new T();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.GetCustomAttribute<NotColumnAttribute>() == null)
                {
                    //property.SetValue(swap, property.GetValue(this));
                    property.SetValue(this, property.GetValue(other));
                }
            }

            //string[] trace2 = new string[trace.Length + updateTrace.Count];
            //trace.CopyTo(trace2, 0);
            //updateTrace.CopyTo(trace2, trace.Length);
            //this.updateTrace.Clear();
            //foreach (string s in trace2)
            //{
            //    PropertyInfo property = typeof(T).GetProperty(s);
            //    property.SetValue(this, property.GetValue(swap));
            //}
        }
    }
}
