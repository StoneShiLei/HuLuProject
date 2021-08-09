using Furion.DataEncryption;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Utils
{
    public static class JwtHelper
    {
        /// <summary>
        /// 获取jwt的负载
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<Claim> GetPayloads(HttpContext context)
        {
            var jwtToken = context.Request.Headers["Authorization"];
            var tokenStr = jwtToken.ToString().Replace("Bearer ", string.Empty);
            var tokenData = JWTEncryption.ReadJwtToken(tokenStr);

            return tokenData?.Claims?.ToList();
        }
    }
}
