using System;

namespace SqlMaker.Common
{
    public class DataBaseUpdatingEventArg<T> : EventArgs
    {
        public T Insert { get; set; }
        public T Delete { get; set; }
        public bool Cancle { get; set; }
        public string Message { get; set; }
    }
    public class DataBaseUpdatedEventArg<T>:EventArgs
    {
        public T Inserted { get; set; }
        public bool Success { get; set; }
    }
}
