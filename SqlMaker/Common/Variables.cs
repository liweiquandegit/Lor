
namespace SqlMaker.Common
{
    public static class Variables
    {
        //public static string GVAR_SECRET = "";
        /// <summary>
        /// 清理过期事务的执行线程运行周期
        /// </summary>
        public static int KILL_TRANS_PERIOD = 600000;
        /// <summary>
        /// 事务的生存时间
        /// </summary>
        public static int TRANS_TTL = 300000;
        /// <summary>
        /// 数据连接字符串，程序启动时赋值到此
        /// </summary>
        public static string ConnStr = "DATA SOURCE=127.0.0.1:1521/xe;PASSWORD=master;USER ID=system";
    }
}
