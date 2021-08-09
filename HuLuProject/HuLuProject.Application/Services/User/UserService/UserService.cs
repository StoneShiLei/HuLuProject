using Furion;
using Furion.DataEncryption;
using Furion.DistributedIDGenerator;
using Furion.UnifyResult;
using HuLuProject.Core.Entities.User;
using HuLuProject.Core.Managers.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.User.UserService
{
    /// <summary>
    /// 用户鉴权
    /// </summary>
    [ApiDescriptionSettings("User")]
    public class UserService : BaseService
    {
        private readonly UserManager userManager = App.GetRequiredService<UserManager>();
        private readonly string AesKey = App.Configuration["AppSettings:AesKey"];

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        [HttpGet, Route("user/login"), AllowAnonymous]
        public async ValueTask<bool> Login([Required]string userName, [Required]string passWord)
        {
            passWord = DESCEncryption.Encrypt(passWord,AesKey);
            var user = await userManager.CheckUserAsync(userName, passWord);
            if (user == null) return false;

            var payload = new Dictionary<string, object>()
            {
                { "UserId",user.Id},{ "UserName",user.UserName }
            };
            var accessToken = JWTEncryption.Encrypt(payload);
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, 43200);

            App.HttpContext.Response.Headers["access-token"] = accessToken;
            App.HttpContext.Response.Headers["x-access-token"] = refreshToken;
            return true;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        [HttpPost, Route("user/register"), AllowAnonymous]
        public async Task<bool> Register([Required,FromBody]string userName, [Required, FromBody] string passWord)
        {
            if(await userManager.IsExistAsync(userName))
            {
                UnifyContext.Fill(new { Message = "用户名已注册" });
                return false;
            }

            passWord = DESCEncryption.Encrypt(passWord, AesKey);
            var user = new UserEntity
            {
                Id = $"USR{IDGen.NextID(new { LittleEndianBinary16Format = true, TimeNow = DateTimeOffset.UtcNow })}",
                UserName = userName,
                PassWord = passWord,
                CreatedTime = DateTime.UtcNow,
            };
            return await userManager.AddOrUpdateUserAsync(user);
        }
    }
}
