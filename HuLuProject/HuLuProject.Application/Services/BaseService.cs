using Furion;
using Furion.Cache;
using Furion.DynamicApiController;
using HuLuProject.Application.Utils;
using MapsterMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services
{
    public abstract class BaseService : IDynamicApiController
    {
        protected readonly IMapper Mapper = App.GetRequiredService<IMapper>();
        protected readonly IMemoryCache MemoryCache = App.GetRequiredService<IMemoryCache>();
        protected readonly RedisCacheHelper RedisCache = App.GetService<RedisCacheHelper>();
        protected readonly string UserId = string.Empty;

        public BaseService()
        {
            //注入userid  如果用户header没有信息则为string.empty
            var payLoads = JwtHelper.GetPayloads(App.HttpContext);
            if(payLoads != null) UserId = payLoads.Find(c => string.Equals(c.Type.ToLower(), "userid"))?.Value;

        }
    }
}
