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
            IList<UserWithPwdModel> pwd = new DataFetcher<UserWithPwdModel>().List(SqlProvider.CreateTransaction());
            string message = "";
            UserFetcher userFetcher = new UserFetcher();
            UserUpdater userUpdater = new UserUpdater(userFetcher);
            //写入数据对象并提交
            DateTime dt = DateTime.Now;
            for (int i = 0; i < 10000; i++)
            {
                Random random = new Random(i);
                string id = (random.Next(0,10000)).ToString();
                string tran = SqlProvider.CreateTransaction();
                UserModel model = userFetcher.GetById(tran,id);
                if (model == null)
                    model = new UserModel() { Code = "liwq_" + id, Name = "liweiquan_" + id, Id = id };
                else
                    model.Code = "llwwwqqq";
                bool result = userUpdater.Save(tran,model,out message);
                SqlProvider.CloseTransaction(tran, result);
                if (result) { }
                //  Console.WriteLine(i);
                else
                    Console.WriteLine(message);
            }
            Console.WriteLine((DateTime.Now - dt).TotalMilliseconds);
            Console.Write("done");
            Console.Read();
            //在新的事务中，更改数据，将数据合并到持久化对象中，最后保存提交事务

            //tran = SqlProvider.CreateTransaction();
            //UserModel usrCp = new UserModel() { Id = "201201001", Code = "75125", Creator = "Liwq", Flag = 1, Name = "Liweiquande" };
            //IList<UserModel> usrDbs = SqlProvider.GetDbInstance<UserModel>().Select(tran, new List<OrderBy>() { new OrderBy() { Field = "Id" } }, new Limit(), out message, Restrain.Eq("Id", "201201001"));
            //UserModel usrDb = null;
            //if (usrDbs.Count > 0)
            //    usrDb = usrDbs[0];
            //usrDb.Merge(usrCp);
            //SqlProvider.GetDbInstance<UserModel>().Update(usrCp, tran, out message);
            //SqlProvider.CloseTransaction(tran, true);

            //在新事务中查询自定义的模板
            //tran = SqlProvider.CreateTransaction();
            //IList<UserWithPwdModel> models = SqlProvider.GetDbInstance<UserWithPwdModel>().Select(tran, new List<OrderBy>() { new OrderBy() { Field = "Id" } }, new Limit(), out message, Restrain.Lk("ID", "%2%"));
            //SqlProvider.CloseTransaction(tran,true); 

            //Console.Read();
        }
    }
}
