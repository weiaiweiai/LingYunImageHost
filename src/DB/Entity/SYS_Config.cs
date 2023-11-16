using System.ComponentModel.DataAnnotations;

namespace LingYunImageHost.DB.Entity
{
    public class SYS_Config
    {
        [Key]       //主键
        public string Guid { get; set; }

        [Required]      //非空
        public string Key { get; set; } = "";

        [Required]      //非空
        public string value { get; set; } = "";

        public string Remark { get; set; } = "";

        /// <summary>
        /// 重写 ToString（） 方法，方便输出查看
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Guid：" + Guid + Environment.NewLine +
                    "Key：" + Key + Environment.NewLine +
                    "value：" + value + Environment.NewLine+
                    "Remark：" + Remark + Environment.NewLine;
        }
    }
}
