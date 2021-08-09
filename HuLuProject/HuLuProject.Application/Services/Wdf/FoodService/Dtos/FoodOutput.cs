using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.FoodService.Dtos
{
    public class FoodOutput
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
        /// 食材名称
        /// </summary>
        public string FoodName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
    }
}
