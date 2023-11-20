using LingYunImageHost.DB.Sqlite.Entity;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System.IO;

namespace LingYunImageHost.DB.Sqlite
{
    public static class ConfigHelpClass
    {
        private static string Url { get; set; }
        public static void SettingUp(string url) 
        {
            Url = url;
            using (DataContext db = new DataContext(Url))
            {
                db.Database.EnsureCreated();
            }
        }
        public static string GetConfig(string key) 
        {
            using (DataContext db = new DataContext(Url))
            {
                List<SYS_Config> Listconfig = db.sYS_Config.Where(e => e.Key == key).ToList();
                if (Listconfig == null || Listconfig.Count == 0) { return null; }
                return Listconfig[0].value;
            }
        }
        public static void Add(SYS_Config config)
        {
            using (DataContext db = new DataContext(Url))
            {
                db.sYS_Config.Add(config);
                db.SaveChanges();
            }
        }

        public static void Upload(SYS_Config config)
        {
            using (DataContext db = new DataContext(Url))
            {
                List<SYS_Config> Listconfig = db.sYS_Config.Where(e => e.Key == config.Key).ToList();
                if (Listconfig == null || Listconfig.Count == 0) 
                {
                    Add(config);
                }
                Listconfig[0].value = config.value;
                db.SaveChanges();
            }
        }
    }
}
