using FreeSql;
using Furion.FriendlyException;
using HuLuProject.Core.Entities.Wfd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Core.Managers.Wfd
{
    /// <summary>
    /// 菜谱
    /// </summary>
    public class MenuManager : BaseManager
    {
        public MenuManager() : base() { }

        /// <summary>
        /// 随机查询菜谱
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeDict">Dict[分类id,(出菜数量,List[食材id])]</param>
        /// <returns></returns>
        public async Task<List<MenuEntity>> GetRandomMenuListAsync(string userId, Dictionary<string, KeyValuePair<int, List<string>>> typeDict)
        {
            List<MenuEntity> result = new();
            foreach(var kv in typeDict)
            {
                string typeId = kv.Key;
                var (count, foodIds) = kv.Value;
                //该用户 某分类下包含某些食材的菜谱
                Expression<Func<MenuEntity, bool>> where = m => m.UserId == userId && m.TypeId == typeId && m.Foods.AsSelect().Any(f => foodIds.Contains(f.Id));

                //如果请求的数量大于等于符合条件的菜谱数量,则直接返回全部符合条件的结果
                var menuCount = FreeSql.Select<MenuEntity>().Where(where).Count();
                if(count >= menuCount)
                {
                    var menus = await FreeSql.Select<MenuEntity>().Where(where).ToListAsync();
                    result.AddRange(menus);
                    continue;
                }

                //添加一个时限，防止死循环
                var startTime = DateTime.UtcNow;
                var endTime = startTime.AddSeconds(3); //3秒超时
                List<MenuEntity> randomList = new();
                while (randomList.Count < count || startTime > endTime)
                {
                    var result11 = await FreeSql.Select<MenuEntity>().WithSql(@"SELECT 
                        t1.id,
                        t1.userId,
                        t1.typeId,
                        t1.menuName,
                        t3.typeName
                        FROM `wfd_menu` AS t1 JOIN 
                        (SELECT ROUND(RAND() * ((SELECT MAX(id) FROM `wfd_menu`)-(SELECT MIN(id) FROM `wfd_menu`))+(SELECT MIN(id) FROM `wfd_menu`)) AS id) AS t2
                        LEFT JOIN `wfd_type` AS t3 ON t1.typeId = t3.id 
                        WHERE t1.id >= t2.id AND 
                        ORDER BY t1.id LIMIT 1").Where(where).ToOneAsync();

                    //随机取1条数据（优化性能） 贪婪加载type
                    var randomMenu = await FreeSql.Select<MenuEntity>().As("t1")
                        .RawJoin("SELECT ROUND(RAND() * ((SELECT MAX(id) FROM `wfd_menu`)-(SELECT MIN(id) FROM `wfd_menu`))+(SELECT MIN(id) FROM `wfd_menu`)) AS id").As("t2")
                        .Include(m => m.Type)
                        .Where("t1.id >= t2.id").Where(where)
                        .OrderBy("t1.id").Limit(1).ToOneAsync();

                    //如果查询出的数据为重复数据则继续查询
                    if (randomList.Any(m => string.Equals(m.Id, randomMenu.Id))) continue;

                    randomList.Add(randomMenu);
                }

                if (randomList.Count < count) throw Oops.Oh("随机出菜发生异常");

                //加入到结果集
                result.AddRange(randomList);
            }

            return result;
        }

        /// <summary>
        /// 查询菜谱列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchText">搜索关键字</param>
        /// <param name="include">是否贪婪加载菜谱的type和food</param>
        /// <returns></returns>
        public Task<List<MenuEntity>> GetMenuListAsync(string userId,string searchText = "",bool include = false)
        {
            Expression<Func<MenuEntity, bool>> where = m => m.UserId == userId;

            if (!string.IsNullOrWhiteSpace(searchText)) where = where.And(m => m.MenuName.StartsWith(searchText) || m.MenuName.EndsWith(searchText));

            var select = FreeSql.Select<MenuEntity>().Where(where).OrderByDescending(m => m.CreatedTime);
            if (include) select.IncludeMany(m => m.Foods).Include(m => m.Type);

            var result = select.ToListAsync();
            return result;
        }

        /// <summary>
        /// 添加菜谱 级联保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddOrUpdateMenuAsync(MenuEntity entity)
        {
            using var uow = FreeSql.CreateUnitOfWork();
            var repo = FreeSql.GetRepository<MenuEntity>();
            repo.UnitOfWork = uow;

            var result = await repo.InsertOrUpdateAsync(entity);
            await repo.SaveManyAsync(entity, "Foods");
            uow.Commit();
            return result != null;
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public Task<MenuEntity> GetOneAsync(string menuId)
        {
            var result = FreeSql.Select<MenuEntity>().Where(u => u.Id == menuId).IncludeMany(m => m.Foods).ToOneAsync();
            return result;
        }

        /// <summary>
        /// 删除菜谱
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveMenuAsync(string userId,string menuId)
        {
            using var uow = FreeSql.CreateUnitOfWork();

            var result1 = await uow.Orm.Delete<MenuEntity>().Where(m => m.UserId == userId && m.Id == menuId).ExecuteAffrowsAsync();
            var result2 = await uow.Orm.Delete<MenuFoodEntity>().Where(m => m.MenuId == menuId).ExecuteAffrowsAsync();
            uow.Commit();
            return result1 > 0 && result2 > 0;
        }

        /// <summary>
        /// 判断用户是否已设置同名菜谱
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menuName"></param>
        /// <returns></returns>
        public Task<bool> IsExistNameAsync(string userId, string menuName)
        {
            var result = FreeSql.Select<MenuEntity>().AnyAsync(m => m.UserId == userId && m.MenuName == menuName);
            return result;
        }

        /// <summary>
        /// 判断id是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> IsExistAsync(string id)
        {
            var result = FreeSql.Select<MenuEntity>().AnyAsync(o => o.Id == id);
            return result;
        }
    }
}
