using Furion;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Furion.UnifyResult;
using HuLuProject.Application.Services.Wdf.TypeService.Dtos;
using HuLuProject.Application.Utils;
using HuLuProject.Core.Entities.Wfd;
using HuLuProject.Core.Managers.Wfd;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.TypeService
{
    /// <summary>
    /// 分类接口
    /// </summary>
    [ApiDescriptionSettings("WhatsForDinner")]
    public class TypeService : BaseService
    {
        private readonly TypeManager typeManager = App.GetRequiredService<TypeManager>();

        /// <summary>
        /// 根据关键字获取分类列表 为空返回全部
        /// </summary>
        /// <param name="text">关键字</param>
        /// <param name="include">是否获取分类下的食谱</param>
        /// <returns></returns>
        [HttpGet, Route("type/list")]
        public async Task<List<TypeOutput>> GetTypeList(string text = "",bool include = false)
        {
            var entitys = await typeManager.GetTypeListAsync(UserId, text, include);
            var result = Mapper.Map<List<TypeOutput>>(entitys);
            return result;
        }

        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpGet, Route("type/get")]
        public async Task<TypeOutput> GetOne([Required] string typeId)
        {
            var entity = await typeManager.GetOneAsync(typeId);
            var result = Mapper.Map<TypeOutput>(entity);
            return result;
        }

        /// <summary>
        /// 新增或修改分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("type/addOrUpdate")]
        public async Task<bool> AddOrUpdateType([Required, FromBody] TypeInput input)
        {
            if (await typeManager.IsExistNameAsync(UserId, input.TypeName))
            {
                UnifyContext.Fill(new { Message = "已存在同名分类" });
                return false;
            }

            //如果id为空 或 id不存在 则新建id  防止id格式非法
            if (string.IsNullOrWhiteSpace(input.Id) || !await typeManager.IsExistAsync(input.Id))
            {
                input.Id = $"TYP-{IDGen.NextID(new { LittleEndianBinary16Format = true, TimeNow = DateTimeOffset.UtcNow })}";
            }

            var entiy = new TypeEntity
            {
                Id = input.Id,
                UserId = UserId,
                TypeName = input.TypeName,
                CreatedTime = DateTime.UtcNow
            };
            var result = await typeManager.AddOrUpdateTypeAsync(entiy);
            return result;
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("type/remove")]
        public Task<bool> RemoveType([Required, FromBody] TypeDeleteInput input)
        {
            var result = typeManager.RemoveTypeAsync(UserId, input.TypeId);
            return result;
        }
    }
}
