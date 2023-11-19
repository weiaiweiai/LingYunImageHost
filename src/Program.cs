using LingYunImageHost.Data;
using LingYunImageHost.DB;

using System.Text;


internal class Program
{
    private static void Main(string[] args)
    {


        var builder = WebApplication.CreateBuilder(args);
        //cofnig的DB文件初始化
        ConfigInitialize(builder);
        builder.Services.AddControllers().AddControllersAsServices();
        // Add services to the container.
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        builder.Services.AddBootstrapBlazor();

        builder.Services.AddSingleton<WeatherForecastService>();

        // 增加 Table 数据服务操作类
        builder.Services.AddTableDemoDataService();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
    public static void ConfigInitialize(WebApplicationBuilder builder) 
    {
        var path = ((IWebHostEnvironment)builder.Environment).ContentRootPath;
        if (!Directory.Exists(path + "/Config/")) 
        {
            Directory.CreateDirectory(path + "/Config/");//创建新路径
        }
        using (DataContext db = new DataContext(path + "/Config/"+"Config.db"))
        {
            db.Database.EnsureCreated();
        }
    }
}