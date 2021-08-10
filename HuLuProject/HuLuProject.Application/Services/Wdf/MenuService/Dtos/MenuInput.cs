using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.MenuService.Dtos
{
    public class MenuInput
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 菜谱名称
        /// </summary>
        [Required]
        public string MenuName { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        [Required]
        public string TypeId { get; set; }

        /// <summary>
        /// 食材id列表
        /// </summary>
        [Required,MinLength(1)]
        public List<string> FoodIds { get; set; }
    }
}
