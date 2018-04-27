using System;
using Model;
using SqlMaker;

namespace Lor
{
    class Program
    {
        static void Main(string[] args)
        {
            string tran = SqlProvider.CreateTransaction();
            string message = "";
            SqlProvider.GetDbInstance<UserModel>().Insert(new UserModel() { Id="201201001", Code="75125", Creator="Liwq", Flag=1, Name="Liweiquan"},tran,out message);
            SqlProvider.CloseTransaction(tran,true);
            Console.WriteLine(message);
            Console.Read();
        }
    }
}
