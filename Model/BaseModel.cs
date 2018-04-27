using System;
using System.Reflection;
using System.Collections.Generic;

namespace Model
{
    public abstract class BaseModel
    {
        protected string _Id { get; set; }
        protected int _Flag { get; set; }
        protected string _Creator { get; set; }
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
    }
}
