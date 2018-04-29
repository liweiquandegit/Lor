using SqlMaker.Common;

namespace Lor
{
    //本实例演示一般表的映射
    [DboProjection("USERMODEL")]
    public class UserModel:BaseModel
    {
        protected string _Code;
        protected string _Name;
        public string Code
        {
            get { return _Code; }
            set
            {
                if (_Code == value)
                {
                    return;
                }
                _Code = value;
                if (!updateTrace.Contains("Code"))
                    updateTrace.Add("Code");
            }
        }
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value)
                {
                    return;
                }
                _Name = value;
                if (!updateTrace.Contains("Name"))
                    updateTrace.Add("Name");
            }
        }
    }
}
