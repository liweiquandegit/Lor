
namespace Model
{
    [DboName("(SELECT USERMODEL.*,PWDMODEL.PASSWORD FROM USERMODEL LEFT JOIN PWDMODEL ON USERMODEL.CODE = PWDMODEL.USRCODE)", true)]
    public class UserWithPwdModel : BaseModel
    {
        protected string _Code;
        protected string _Name;
        protected string _Password;
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
        public string Password
        {
            get { return _Password; }
            set
            {
                if (_Password == value)
                {
                    return;
                }
                _Password = value;
                if (!updateTrace.Contains("Password"))
                    updateTrace.Add("Password");
            }
        }
    }
}
