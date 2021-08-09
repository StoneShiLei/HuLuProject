using HuLuProject.Core.Entities.Wfd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Core.Managers.Wfd
{
    public class FoodManager : BaseManager
    {
        public FoodManager() : base() { }

        /// <summary>
        /// 根据关键字查询食材
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public Task<List<FoodEntity>> GetFoodListAsync(string userId,string searchText = "")
        {
            Expression<Func<FoodEntity, bool>> where = f => f.UserId == userId;

            if (!string.IsNullOrWhiteSpace(searchText)) where.And(f => f.FoodName.StartsWith(searchText) || f.FoodName.EndsWith(searchText));

            var result = FreeSql.Select<FoodEntity>()
                .Where(where)
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// 新增或修改食材
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddOrUpdateFoodAsync(FoodEntity entity)
        {
            var result = await FreeSql
                .InsertOrUpdate<FoodEntity>()
                .SetSource(entity)
                .UpdateColumns(f => f.FoodName)
                .ExecuteAffrowsAsync();

            return result > 0;
        }

        /// <summary>
        /// 删除食材
        /// </summary>
        /// <param name="userId">食材所属用户id</param>
        /// <param name="foodId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveFoodAsync(string userId,string foodId)
        {
            var result = await FreeSql.Delete<FoodEntity>().Where(f => f.UserId == userId && f.Id == foodId).ExecuteAffrowsAsync();

            return result > 0;
        }

        /// <summary>
        /// 判断用户是否已设置同名食材
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="foodName"></param>
        /// <returns></returns>
        public Task<bool> IsExistAsync(string userId,string foodName)
        {
            var result = FreeSql.Select<FoodEntity>().AnyAsync(f => f.UserId == userId && f.FoodName == foodName);
            return result;
        }

    }
}
