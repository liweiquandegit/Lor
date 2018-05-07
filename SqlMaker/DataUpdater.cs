using System;
using System.Reflection;
using SqlMaker.Common;

namespace SqlMaker
{
    public abstract class DataUpdater<T> where T:BaseModel,new()
    {
        public DataUpdater(DataFetcher<T> dataFetcher)
        {
            this.dataFetcher = dataFetcher;
        }
        SqlMaker<T> sqlMaker;
        public event DataUpdatedHandler<T> DataUpdated;
        public event DataUpdatedHandler<T> DataDeleted;
        public event DataUpdatingHandler<T> DataUpdating;
        public event DataUpdatingHandler<T> DataDeleting;
        protected abstract void InitSqlMaker();
        private DataFetcher<T> dataFetcher;
        public bool Save(string tran,T data,out string message)
        {
            message = "";
            bool result = false;
            bool ins = false;
            bool term = false;
            Type typ = typeof(T);
            PropertyInfo keyPi = null;
            PropertyInfo flagPi = null;
            foreach(PropertyInfo pi in typ.GetProperties())
            {
                if (pi.GetCustomAttribute<PrimaryKeyAttribute>() != null)
                    keyPi = pi;
                if (pi.GetCustomAttribute<FlagKeyAttribute>() != null)
                    flagPi = pi;
            }
            T dbData = dataFetcher.GetById(tran,keyPi.GetValue(data));
            T srcData = dbData == null ? null : dbData.Clone() as T;
            if(dbData==null)
            {
                ins = true;
                dbData = new T();
            }
            if (!ins&&(int)(flagPi.GetValue(data)) != (int)flagPi.GetValue(dbData))
            {
                message = "Object was updated by other user";
                return false;
            }
            dbData.Merge(data);
            if(DataUpdating!=null)
            {
                DataBaseUpdatingEventArg<T> dataBaseUpdatingEventArg = new DataBaseUpdatingEventArg<T>() { Delete = srcData, Insert = dbData };
                foreach(DataUpdatingHandler<T> dataUpdating in DataUpdating.GetInvocationList())
                {
                    dataUpdating.Invoke(this,dataBaseUpdatingEventArg);
                    if (dataBaseUpdatingEventArg.Cancle)
                    {
                        term = true;
                        message = dataBaseUpdatingEventArg.Message;
                        break;
                    }
                }
            }
            if (term)
            {
                return false;
            }
            flagPi.SetValue(dbData,(int)(flagPi.GetValue(dbData))+1);
            if(ins)
            {
                result = sqlMaker.Insert(dbData, tran, out message);
            }
            else
            {
                result = sqlMaker.Update(dbData, tran,out message);
            }
            if (DataUpdated != null)
            {
                DataBaseUpdatedEventArg<T> dataBaseUpdatedEventArg = new DataBaseUpdatedEventArg<T>() { Inserted = dbData, Success = result};
                foreach (DataUpdatedHandler<T> dataUpdated in DataUpdating.GetInvocationList())
                {
                    dataUpdated.Invoke(this, dataBaseUpdatedEventArg);
                }
            }
            return result;
        }
        public bool Delete(string tran, T data, out string message)
        {
            message = "";
            bool result = false;
            bool term = false;
            Type typ = typeof(T);
            PropertyInfo keyPi = null;
            PropertyInfo flagPi = null;
            foreach (PropertyInfo pi in typ.GetProperties())
            {
                if (pi.GetCustomAttribute<PrimaryKeyAttribute>() != null)
                    keyPi = pi;
                if (pi.GetCustomAttribute<FlagKeyAttribute>() != null)
                    flagPi = pi;
            }
            T dbData = dataFetcher.GetById(tran, keyPi.GetValue(data));
            if (dbData == null)
            {
                message = "Object not exist";
                return false;
            }
            if ((int)(flagPi.GetValue(data)) != (int)flagPi.GetValue(dbData))
            {
                message = "Object was updated by other user";
                return false;
            }
            if (DataUpdating != null)
            {
                DataBaseUpdatingEventArg<T> dataBaseUpdatingEventArg = new DataBaseUpdatingEventArg<T>() { Delete = dbData, Insert = null };
                foreach (DataUpdatingHandler<T> dataUpdating in DataUpdating.GetInvocationList())
                {
                    dataUpdating.Invoke(this, dataBaseUpdatingEventArg);
                    if (dataBaseUpdatingEventArg.Cancle)
                    {
                        term = true;
                        message = dataBaseUpdatingEventArg.Message;
                        break;
                    }
                }
            }
            if (term)
            {
                return false;
            }
            result = sqlMaker.Delete(dbData, tran, out message);

            if (DataUpdated != null)
            {
                DataBaseUpdatedEventArg<T> dataBaseUpdatedEventArg = new DataBaseUpdatedEventArg<T>() { Inserted = dbData, Success = result };
                foreach (DataUpdatedHandler<T> dataUpdated in DataUpdating.GetInvocationList())
                {
                    dataUpdated.Invoke(this, dataBaseUpdatedEventArg);
                }
            }
            return result;
        }
    }
}
