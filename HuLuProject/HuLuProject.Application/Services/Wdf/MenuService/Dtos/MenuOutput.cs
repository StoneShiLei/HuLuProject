﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.MenuService.Dtos
{
    public class MenuOutput
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
        /// 菜谱名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 食材id列表
        /// </summary>
        public List<string> FoodIds { get; set; }
    }
}
