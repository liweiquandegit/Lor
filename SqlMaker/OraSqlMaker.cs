using System;
using System.Collections.Generic;
using Model;

namespace SqlMaker
{
    public class OraSqlMaker<T> : SqlMaker<T> where T : BaseModel, new()
    {
        public override bool Delete(T data, string tran, out string message)
        {
            throw new NotImplementedException();
        }

        public override bool Insert(T data, string tran, out string message)
        {
            throw new NotImplementedException();
        }

        public override IList<T> Select(string tran, IList<OrderBy> order, Limit limit, out string message, params Restrain[] restrain)
        {
            throw new NotImplementedException();
        }

        public override bool Update(T data, string tran, out string message)
        {
            throw new NotImplementedException();
        }
    }
}
