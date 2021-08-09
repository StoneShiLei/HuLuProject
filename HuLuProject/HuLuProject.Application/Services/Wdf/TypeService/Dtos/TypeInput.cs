using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.TypeService.Dtos
{
    public class TypeInput
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }
}
