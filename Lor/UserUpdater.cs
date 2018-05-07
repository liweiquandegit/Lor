using System;
using SqlMaker;

namespace Lor
{
    public class UserUpdater : DataUpdater<UserModel>
    {
        public UserUpdater(DataFetcher<UserModel> dataFetcher) : base(dataFetcher)
        {
            
        }

        protected override SqlMaker<UserModel> InitSqlMaker()
        {
            return new OraSqlMaker<UserModel>();
        }
    }
    public class UserFetcher : DataFetcher<UserModel>
    {
        protected override SqlMaker<UserModel> InitSqlMaker()
        {
            return new OraSqlMaker<UserModel>();
        }
    }
}
