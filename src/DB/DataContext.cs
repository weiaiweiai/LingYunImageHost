using LingYunImageHost.DB.Entity;

using Microsoft.EntityFrameworkCore;

namespace LingYunImageHost.DB
{
    public class DataContext : DbContext
    {

        /// <summary>
        /// 声明一个 DbSet<Student> 属性，相当于数据源
        /// </summary>
        public DbSet<SYS_Config> sYS_Config { get; set; }

        public string ConnectionString { get; set; }

        public DataContext(string dbFile)
        {
            //这个地方是运行目录
            ConnectionString = "Data Source=" + Path.Combine(Environment.CurrentDirectory, dbFile);
        }

        /// <summary>
        /// 重写 OnConfiguring 方法，设置数据库连接 （本例中设置为程序当前目录下的Data.db）
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }
    }


}
