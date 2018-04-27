using System;
using System.Reflection;
using System.Collections.Generic;

namespace Model
{
    public class BaseModel
    {
        private string _Id;
        private int _Flag;
        private string _Creator;
        protected IList<string> updateTrace = new List<string>();
        public string[] GetTracer()
        {
            string[] result = new string[updateTrace.Count];
            updateTrace.CopyTo(result,0);
            return result;
        }
        public void ResetTracer()
        {
            updateTrace.Clear();
        }
        protected void Set(string key,object value)
        {
            Type typ = this.GetType();
            PropertyInfo property = typ.GetProperty("_"+key);
            object oldVal = property.GetValue(this);
            if (oldVal == value)
            {
                return;
            }
            property.SetValue(this,value);
            if (!updateTrace.Contains(key))
                updateTrace.Add(key);
        }
        public virtual string Id
        {
            get { return _Id; }
            set { Set("Id", value); }
        }
        public virtual int Flag
        {
            get { return _Flag; }
            set { Set("Flag", value); }
        }
        public virtual string Creator
        {
            get { return _Creator; }
            set { Set("Creator", value); }
        }
    }
}
