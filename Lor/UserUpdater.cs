using System;
using SqlMaker;

namespace Lor
{
    public class UserUpdater : DataUpdater<UserModel>
    {
        public UserUpdater(DataFetcher<UserModel> dataFetcher) : base(dataFetcher)
        {
            
        }


    }
    public class UserFetcher : DataFetcher<UserModel>
    {

    }
}
