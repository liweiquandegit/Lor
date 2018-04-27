using System;

namespace Model
{
    public class UserModel:BaseModel
    {
        public string _Code;
        public string _Name;
        public string Code { get { return _Code; }set { Set("Code", value); } }
        public string Name { get { return _Name; }set { Set("Name", value); } }
    }
}
