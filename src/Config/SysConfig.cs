using LingYunImageHost.DB.Sqlite;

namespace LingYunImageHost.Config
{
    public static class SysConfig
    {
        public static int maxFileLengthMB;
        public static int MaxFileLengthMB 
        {
            get {
                try
                {
                    if (maxFileLengthMB == 0)
                    {
                        maxFileLengthMB = int.Parse(ConfigHelpClass.GetConfig("maxFileLengthMB"));
                    }
                }
                catch(Exception e) 
                {
                    Console.WriteLine($"读取配置出现错误！Message:{e.Message} \r\n错误堆栈：{e.ToString()}");
                    //默认给个5
                    maxFileLengthMB = 5;
                }
                return maxFileLengthMB;
            }
        }
    }
}
