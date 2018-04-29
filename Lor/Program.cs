using System;
using SqlMaker;
using SqlMaker.Common;
using System.Collections.Generic;

namespace Lor
{
    class Program
    {
        static void Main(string[] args)
        {
            string message = "";
            //写入数据对象并提交
            string tran = SqlProvider.CreateTransaction();
            SqlProvider.GetDbInstance<UserModel>().Insert(new UserModel() { Id = "201201002", Code = "75125", Creator = "Liwq", Flag = 1, Name = "Liweiquan" }, tran, out message);
            SqlProvider.CloseTransaction(tran, true);

            //在新的事务中，更改数据，将数据合并到持久化对象中，最后保存提交事务

            tran = SqlProvider.CreateTransaction();
            UserModel usrCp = new UserModel() { Id = "201201001", Code = "75125", Creator = "Liwq", Flag = 1, Name = "Liweiquande" };
            IList<UserModel> usrDbs = SqlProvider.GetDbInstance<UserModel>().Select(tran, new List<OrderBy>() { new OrderBy() { Field = "Id" } }, new Limit(), out message, Restrain.Eq("Id", "201201001"));
            UserModel usrDb = null;
            if (usrDbs.Count > 0)
                usrDb = usrDbs[0];
            usrDb.Merge(usrCp);
            SqlProvider.GetDbInstance<UserModel>().Update(usrCp, tran, out message);
            SqlProvider.CloseTransaction(tran, true);

            //在新事务中查询自定义的模板
            tran = SqlProvider.CreateTransaction();
            IList<UserWithPwdModel> models = SqlProvider.GetDbInstance<UserWithPwdModel>().Select(tran, new List<OrderBy>() { new OrderBy() { Field = "Id" } }, new Limit(), out message, Restrain.Lk("ID", "%2%"));
            SqlProvider.CloseTransaction(tran,true); 

            Console.Read();
        }
    }
}
