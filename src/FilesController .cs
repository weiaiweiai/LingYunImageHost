using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace LingYunImageHost
{
    [ApiController]
    [Route("/api/file")]
    public class FilesController : ControllerBase
    {
        private IWebHostEnvironment env;

        public FilesController(IWebHostEnvironment env)
        {
            this.env = env;
        }

        [HttpPost, Route("upload")]
        public ResultModel UploadFile([FromForm] IFormCollection formCollection)
        {
            ResultModel result = new ResultModel();
            result.Message = "success";
            result.Code = 0;
            result.Url = "/api/file/download";

            try
            {
                string uploadPath = System.IO.Path.Combine("I:\\", "upload");
                if (!Directory.Exists(uploadPath))//判断是否存在
                {
                    Directory.CreateDirectory(uploadPath);//创建新路径
                }

                FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
                foreach (IFormFile file in fileCollection)
                {
                    var fileName = file.FileName;
                    string fileExtension = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);//获取文件名称后缀 
                    var stream = file.OpenReadStream();
                    // 把 Stream 转换成 byte[] 
                    byte[] bytes = new byte[stream.Length];
                    using (FileStream fs = new FileStream(uploadPath + "\\" + file.FileName, FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fs))
                        {
                            stream.Read(bytes, 0, bytes.Length);
                            // 设置当前流的位置为流的开始 
                            stream.Seek(0, SeekOrigin.Begin);
                            result.Url += Path.GetFileName("/api/file/?Name=" + file.FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Code = 100;
                result.Message = $"文件上传失败：{ex.Message}!";
                //Wongoing.Log.LoggingService<FilesController>.Instance.Error($"文件上传失败：{ex.Message}", ex);
                throw ex;
            }
            return result;
        }

        ///// <summary>
        ///// 下载文件
        ///// </summary>
        //[HttpGet, Route("download")]
        //[ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        //public FileResult Download(string Name)
        //{
        //    try
        //    {
        //        string filePath = System.IO.Path.Combine("I:\\", "upload", Name);
        //
        //        //string filePath = System.IO.Path.Combine(this.env.ContentRootPath, "upload");
        //        Stream stream = new System.IO.FileStream(filePath, FileMode.Open, FileAccess.Read);
        //        FileStreamResult actionresult = new FileStreamResult(stream, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream"));
        //        actionresult.FileDownloadName = Wongoing.Common.Global.BackupFileName;
        //        return actionresult;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Wongoing.Log.LoggingService<FilesController>.Instance.Error($"文件下载失败：{ex.Message}", ex);
        //        return null;
        //    }
        //}
        //
    }

    public class ResultModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
}
