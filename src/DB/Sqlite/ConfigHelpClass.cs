﻿using LingYunImageHost.Config;
using LingYunImageHost.DB.Sqlite.Entity;
using LingYunImageHost.Pages;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System.IO;
using System.Reflection;
using System.Xml.Linq;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                config.Guid = Guid.NewGuid().ToString().ToUpper();
                db.sYS_Config.Add(config);
                db.SaveChanges();
            }
        }
        public static void Upload(SYS_Config config)
        {
            using (DataContext db = new DataContext(Url))
            {
                if (config.Key.Equals("AdminUserPassword"))
                {
                    if (string.IsNullOrEmpty(config.value)) return;
                    config.value = SYS_Tool.sha256(config.value);
                }
                List<SYS_Config> Listconfig = db.sYS_Config.Where(e => e.Key == config.Key).ToList();
                if (Listconfig == null || Listconfig.Count == 0)
                {
                    Add(config);
                }
                else
                {
                    Listconfig[0].value = config.value;
                    db.SaveChanges();
                }
            }
        }
        public static void UploadAll(Config.SysConfig sysConfig)
        {
            Type t = sysConfig.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                string name = item.Name;
                object value = item.GetValue(sysConfig, null);
                Upload(new SYS_Config {
                    Key = name,
                    value = (value??"").ToString()
                });
            }
        }

    }
}
