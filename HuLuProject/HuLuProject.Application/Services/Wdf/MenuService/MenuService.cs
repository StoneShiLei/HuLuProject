using Furion;
using HuLuProject.Core.Entities.Wfd;
using HuLuProject.Core.Managers.Wfd;
using HuLuProject.Core.Universal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.MenuService
{
    /// <summary>
    /// 菜谱接口
    /// </summary>
    [ApiDescriptionSettings("WhatsForDinner")]
    public class MenuService : BaseService
    {
        private readonly MenuManager menuManager = App.GetRequiredService<MenuManager>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //[HttpGet, Route("demo/list")]
        //public async Task<Container<DemoOutput>> GetDemoList(int index = 1, int size = 5)
        //{
        //    var entitys = await demoManager.GetDemoListAsync(index, size);
        //    var dtos = Mapper.Map<Container<DemoOutput>>(entitys);
        //    return dtos;
        //}
    }
}
