using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using LingYunImageHost.Config;

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
        [RequestFormLimits(MultipartBodyLengthLimit = 2147483648)]
        [RequestSizeLimit(2147483648)]
        public ResultModel UploadFile([FromForm] IFormCollection formCollection)
        {
            ResultModel result = new ResultModel();
            result.Message = "success";
            result.Code = 0;
            result.Url = "/api/file/download";

            try
            {
                string date = DateTime.Now.ToString("yy-MM-dd");
                string uploadPath = Path.Combine(ConfigEntity.sysConfig.ImageUrl+ "/" + date);
                if (!Directory.Exists(uploadPath))//判断是否存在
                {
                    Directory.CreateDirectory(uploadPath);//创建新路径
                }

                FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
                foreach (IFormFile file in fileCollection)
                {
                    var fileName = file.FileName;
                    string fileExtension = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);//获取文件名称后缀 
                    Stream stream = file.OpenReadStream();
                    byte[] bytes = new byte[stream.Length];
                    using (FileStream fs = new FileStream(uploadPath + "/" + file.FileName, FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fs))
                        {
                            stream.Read(bytes, 0, bytes.Length);
                            // 设置当前流的位置为流的开始 
                            stream.Seek(0, SeekOrigin.Begin);
                            stream.CopyTo(fs);
                            result.Url = $"Image/{date}/{file.FileName}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Code = 100;
                result.Message = $"文件上传失败：{ex.Message}!";
                throw ex;
            }
            return result;
        }
    }

    public class ResultModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }
}
