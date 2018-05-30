using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MP.Base;
using MP.Base.Enums;
using MP.Base.Models;
using MP.Sugar;
using MP.Web.Models;

namespace MP.Web.Controllers
{
    public class MenuController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="start">分页开始</param>
        /// <param name="length">分页大小</param>
        /// <returns></returns>
        public IActionResult GetMenu(int start, int length)
        {
            var count = 0;
            var menu = SqlSugarDbContext.Instance.ExecutedSql().Queryable<BaseSysMenu>().Where(w => w.Valid == 1).ToPageList(start, length, ref count).ToList();
            Dictionary<string, object> dic = new Dictionary<string, object>
            {
                { "recordsTotal",count},
                { "recordsFiltered", count},
                { "list", menu }
            };
            return Json(dic);
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public IActionResult AddMenu(BaseSysMenu addMenu)
        {
            addMenu.CreateTime = DateTime.Now;
            if (addMenu.Id > 0)
            {
                var menu = SqlSugarDbContext.Instance.ExecutedSql().Updateable(addMenu).ExecuteCommand();
                return Json(new BaseResult { Code = menu > 0 ? MsgCode.Success : MsgCode.ProgramError });
            }
            else
            {
                addMenu.Valid = 1;
                var menu = SqlSugarDbContext.Instance.ExecutedSql().Insertable(addMenu).ExecuteCommand();
                return Json(new BaseResult { Code = menu > 0 ? MsgCode.Success : MsgCode.ProgramError });
            }
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id">菜单标识</param>
        /// <returns></returns>
        public IActionResult GetDetails(int id)
        {
            var result = SqlSugarDbContext.Instance.ExecutedSql().Queryable<BaseSysMenu>().Where(s => s.Id == id).First();
            return Json(result);
        }
    }
}