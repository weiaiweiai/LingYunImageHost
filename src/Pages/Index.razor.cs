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
using AntDesign;

namespace LingYunImageHost.Pages
{
    public partial class Index
    {

        void OnSingleCompleted(UploadInfo fileinfo)
        {
            if (Items == null)
            {
                Items = new List<PictureInformation>();
            }
            if (fileinfo.File.State == UploadState.Success)
            {
                var result = fileinfo.File.GetResponse<ResponseModel>();
                fileinfo.File.Url = result.url;
                Items.Add(new PictureInformation()
                {
                    name = fileinfo.File.FileName,
                    URL = fileinfo.File.Url,
                    URL2 = NavigationManager.BaseUri + fileinfo.File.Url
                });

                StateHasChanged();

            }
        }

        public class ResponseModel
        {
            public string name { get; set; }

            public string status { get; set; }

            public string url { get; set; }

            public string thumbUrl { get; set; }
        }

        public class PictureInformation
        {
            [Display(Name = "√˚≥∆")]
            public string name { get; set; } = "";
            [Display(Name = "‘§¿¿")]
            public string URL { get; set; } = "";
            [Display(Name = "µÿ÷∑")]
            public string URL2 { get; set; } = "";
        }

        private List<PictureInformation>? Items { get; set; }
         
        public void Dispose()
        {
        }

    }


}