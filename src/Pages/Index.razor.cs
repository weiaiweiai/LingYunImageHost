using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BootstrapBlazor.Components;
using LingYunImageHost;
using LingYunImageHost.Data;
using LingYunImageHost.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using LingYunImageHost.Config;
using LingYunImageHost.Shared;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Forms;

namespace LingYunImageHost.Pages
{
    public partial class Index
    {
        public class PictureInformation
        {
            [Display(Name = "名称")]
            public string name { get; set; } = "";
            [Display(Name = "预览")]
            public string URL { get; set; } = "";
            [Display(Name = "地址")]
            public string URL2 { get; set; } = "";
        }

        private List<PictureInformation>? Items { get; set; }

        private static long MaxFileLength => SysConfig.MaxFileLengthMB * 1024 * 1024;
         
        private async Task OnCardUpload(UploadFile file)
        {
            if (Items == null)
            {
                Items = new List<PictureInformation>();
            }

            if (file != null && file.File != null)
            {
                System.Console.WriteLine(file.OriginFileName);
                //服务器端验证当文件大于 200MB 时提示文件太大信息
                if (file.Size > MaxFileLength)
                {
                    //await ToastService.Information("上传文件", "文件大小超过 200MB");
                    file.Code = 1;
                    file.Error = "文件大小超过 200MB";
                }
                else
                {
                    await SaveToFile(file);
                }

                Items.Add(new PictureInformation()
                {name = file.OriginFileName, URL = file.FileName, URL2 = NavigationManager.BaseUri + file.FileName});
            }

            System.Console.WriteLine(NavigationManager.Uri);
            System.Console.WriteLine(NavigationManager.BaseUri);
            StateHasChanged();
        }

        private async Task<bool> SaveToFile(UploadFile file)
        {
            // Server Side 使用
            // Web Assembly 模式下必须使用 webapi 方式去保存文件到服务器或者数据库中
            // 生成写入文件名称
            var ret = false;
            file.FileName = $"{DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd")}/{file.OriginFileName}-{DateTimeOffset.Now:yyyyMMddHHmmss}{Path.GetExtension(file.OriginFileName)}";
            var fileName = Path.Combine(IWebHostEnvironment.WebRootPath + "/" + file.FileName);
            ret = await file.SaveToFileAsync(fileName, MaxFileLength);
            if (ret)
            {
                // 保存成功
                file.PrevUrl = file.FileName;
            }
            else
            {
                var errorMessage = $"{"保存文件失败"} {file.OriginFileName}";
                file.Code = 1;
                file.Error = errorMessage;
            }

            return ret;
        }

        public void Dispose()
        {
        }
    }
}