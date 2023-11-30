using BootstrapBlazor.Components;

using LingYunImageHost.DB.Sqlite;

using System.IO;

using Console = System.Console;

namespace LingYunImageHost.Config
{
    public static class ConfigEntity
    {
        public static SysConfig sysConfig { get; set; } = new SysConfig();
    }

    public class SysConfig
    {
        private int maxFileLengthMB;
        public int MaxFileLengthMB 
        {
            get {
                try
                {
                    if (maxFileLengthMB == 0)
                    {
                        maxFileLengthMB = int.Parse(ConfigHelpClass.GetConfig("MaxFileLengthMB"));
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
            set 
            { 
                maxFileLengthMB = value; 
            }
        }
        private string webName;
        public string WebName
        {
            get
            {
                try
                {
                    if (webName == null)
                    {
                        webName = ConfigHelpClass.GetConfig("WebName");
                        if (webName == null)
                        {
                            throw new Exception("未查询到配置文件");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"读取配置出现错误！Message:{e.Message} \r\n错误堆栈：{e.ToString()}");
                    //默认给个5
                    webName = "图床";
                }
                return webName;
            }
            set
            {
                webName = value;
            }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get
            {
                try
                {
                    if (imageUrl == null)
                    {
                        imageUrl = ConfigHelpClass.GetConfig("ImageUrl");
                        if (imageUrl == null)
                        {
                            throw new Exception("未查询到配置文件");
                        }
                        if (!Directory.Exists(imageUrl))//判断是否存在
                        {
                            Directory.CreateDirectory(imageUrl);//创建新路径
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine($"读取配置出现错误！Message:{e.Message} \r\n错误堆栈：{e.ToString()}");
                    //默认给个5
                    imageUrl = WWWRoot+ "/images";
                }
                return imageUrl;
            }
            set
            {
                imageUrl = value;
            }
        }
        private string adminUserName;
        public string AdminUserName
        {
            get
            {
                try
                {
                    if (adminUserName == null)
                    {
                        adminUserName = ConfigHelpClass.GetConfig("AdminUserName");
                        if (adminUserName == null)
                        {
                            throw new Exception("未查询到配置文件");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"读取配置出现错误！Message:{e.Message} \r\n错误堆栈：{e.ToString()}");
                    //默认给个5
                    adminUserName = "Admin";
                }
                return adminUserName;
            }
            set
            {
                adminUserName = value;
            }
        }
        private string adminUserPassword;
        public string AdminUserPassword
        {
            get
            {
                try
                {
                    if (adminUserPassword == null)
                    {
                        adminUserPassword = ConfigHelpClass.GetConfig("AdminUserPassword");
                        if (adminUserPassword == null)
                        {
                            throw new Exception("未查询到配置文件");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"读取配置出现错误！Message:{e.Message} \r\n错误堆栈：{e.ToString()}");
                    //默认admin123@
                    adminUserPassword = SYS_Tool.sha256("admin123@");
                }
                return adminUserPassword;
            }
            set
            {
                adminUserPassword = value;
            }
        }
        public string WWWRoot { get; set; }

    }
}
