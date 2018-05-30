using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MP.Base;
using MP.Base.Enums;
using MP.Base.Models;
using MP.Models.Users;
using MP.Sugar;
using MP.Tools;
using MP.Web.Models;

namespace MP.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        public AccountController()
        {

        }
        public IActionResult Index(string returnUrl = null)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 该Action登录用户
        /// </summary>
        [HttpPost]
        public IActionResult Index(Login login)
        {
            if (ModelState.IsValid)
            {
                login.Password = EncryptHelper.MD5DesEncrypt(login.Password);
                var userInfo = SqlSugarDbContext.Instance.ExecutedSql().Queryable<Admin>().Where(w => w.Account == login.Email).First();
                if (userInfo != null)
                {
                    if (userInfo.Password != login.Password)
                    {
                        ModelState.AddModelError(string.Empty, "登录失败，密码错误");
                        return View(login);
                    }
                    var user = new BaseUserResult
                    {
                        Id = userInfo.Id,
                        Name = userInfo.Name,
                        Token = userInfo.Token,
                        Account = userInfo.Account,
                        RoleId = userInfo.RoleId
                    };

                    _Authentication.SetAuth(user, login.RememberMe);
                    return Redirect(TempData["returnUrl"] == null ? "/Home/Index" : TempData["returnUrl"].ToString());
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "登录失败，用户名或密码错误");
                    return View(login);
                }
            }
            return View(login);
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Register(UserRegister register)
        {
            var msg = new BaseResult();
            var userInfo = SqlSugarDbContext.Instance.ExecutedSql().Queryable<Admin>().Where(w => w.Account == register.Email).First();
            if (userInfo != null)
            {
                msg.Code = MsgCode.Registered;
            }
            else
            {
                var result = SqlSugarDbContext.Instance.ExecutedSql().Insertable(new Admin
                {
                    Account = register.Email,
                    Password = EncryptHelper.MD5DesEncrypt(register.Password),
                    CreateTime = DateTime.Now,
                    RoleId = 1,
                    State = 100
                }).ExecuteCommand();
                msg.Code = result > 0 ? MsgCode.Success : MsgCode.ProgramError;
            }
            return Json(msg);
        }
        /// <summary>
        /// 该Action从Asp.Net Core中注销登录的用户
        /// </summary>
        public IActionResult Logout()
        {
            _Authentication.SignOut();
            return Redirect("/Account/Index");
        }
    }
}