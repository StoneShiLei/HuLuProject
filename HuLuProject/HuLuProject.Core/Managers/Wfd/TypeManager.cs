using HuLuProject.Core.Entities.Wfd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Core.Managers.Wfd
{
    public class TypeManager : BaseManager
    {
        public TypeManager() : base() { }

        /// <summary>
        /// 获取用户的食谱分类列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchText">搜索关键字</param>
        /// <param name="include">是否贪婪加载type下的菜谱</param>
        /// <returns></returns>
        public Task<List<TypeEntity>> GetTypeListAsync(string userId,string searchText = "",bool include = false)
        {
            Expression<Func<TypeEntity, bool>> where = t => t.UserId == userId;

            if (!string.IsNullOrWhiteSpace(searchText)) where.And(t => t.TypeName.StartsWith(searchText) || t.TypeName.EndsWith(searchText));

            var select = FreeSql.Select<TypeEntity>().Where(where);
            if (include) select.IncludeMany(t => t.Menus);

            var result = select.ToListAsync();
            return result;
        }

        /// <summary>
        /// 新增或修改分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddOrUpdateTypeAsync(TypeEntity entity)
        {
            var result = await FreeSql
                .InsertOrUpdate<TypeEntity>()
                .SetSource(entity)
                .UpdateColumns(f => f.TypeName)
                .ExecuteAffrowsAsync();

            return result > 0;
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="userId">分类所属用户id</param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveTypeAsync(string userId, string typeId)
        {
            var result = await FreeSql.Delete<TypeEntity>().Where(f => f.UserId == userId && f.Id == typeId).ExecuteAffrowsAsync();

            return result > 0;
        }

        /// <summary>
        /// 判断用户是否已设置同名分类
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Task<bool> IsExistAsync(string userId, string typeName)
        {
            var result = FreeSql.Select<TypeEntity>().AnyAsync(f => f.UserId == userId && f.TypeName == typeName);
            return result;
        }
    }
}
