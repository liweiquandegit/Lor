using SqlMaker.Common;
namespace Lor
{
    //本实例演示自定义查询的映射
    [DboProjection("(SELECT USERMODEL.*,PWDMODEL.PASSWORD FROM USERMODEL LEFT JOIN PWDMODEL ON USERMODEL.CODE = PWDMODEL.USRCODE WHERE 1=1)", true)]
    public class UserWithPwdModel : BaseModel
    {
        //protected string _Code;
        //protected string _Name;
        //protected string _Password;
        public string Code { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
