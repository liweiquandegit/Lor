using System;
using Model;
using SqlMaker;
using System.Collections.Generic;

namespace Lor
{
    class Program
    {
        static void Main(string[] args)
        {
            string tran = SqlProvider.CreateTransaction();
            string message = "";
            //SqlProvider.GetDbInstance<UserModel>().Insert(new UserModel() { Id = "201201001", Code = "75125", Creator = "Liwq", Flag = 1, Name = "Liweiquan" }, tran, out message);
            //SqlProvider.CloseTransaction(tran, true);
            //tran = SqlProvider.CreateTransaction();
            //UserModel usrCp = new UserModel() { Id = "201201001", Code = "75125", Creator = "Liwq", Flag = 1, Name = "Liweiquande" };
            //IList<UserModel> usrDbs = SqlProvider.GetDbInstance<UserModel>().Select(tran, new List<OrderBy>() { new OrderBy() { Field = "Id" } }, new Limit(), out message, Restrain.Eq("Id", "201201001"));
            //UserModel usrDb = null;
            //if (usrDbs.Count > 0)
            //    usrDb = usrDbs[0];
            //usrDb.Merge(usrCp);
            //SqlProvider.GetDbInstance<UserModel>().Update(usrCp, tran, out message);
            //SqlProvider.CloseTransaction(tran, true);

            //IList<UserWithPwdModel> models = SqlProvider.GetDbInstance<UserWithPwdModel>().Select(tran, new List<OrderBy>() { new OrderBy() { Field = "Id" } }, new Limit(), out message, Restrain.Lk("ID", "%2%"));
            //SqlProvider.CloseTransaction(tran,true); 
            Console.Read();
        }
    }
}
