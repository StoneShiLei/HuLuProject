using HuLuProject.Core.Entities.Wfd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Core.Managers.Wfd
{
    public class MenuManager : BaseManager
    {
        public MenuManager() : base() { }

        public async Task<List<TypeEntity>> GetTest()
        {
            var result = await FreeSql.Select<TypeEntity>().IncludeMany(t => t.Menus).ToListAsync();
            return result;
        }
    }
}
