using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MP.Base;
using MP.Base.Enums;
using MP.Base.Models;
using MP.Models;
using MP.Models.Extension;
using MP.Sugar;
using MP.Web.Models;

namespace MP.Web.Controllers
{
    public class RoleController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public IActionResult GetDate(int start, int length)
        {
            var sql = $@"select r.Id,r.`Name`,r.`Describe`,r.CreateTime,a.`Name` as CreaterName from SysRole as r left join Admin as a
on r.Creater = a.Id";
            var list = SqlSugarDbContext.Instance.ExecutedSql().Ado.SqlQuery<RoleResult>(sql).ToList();
            //
            var countSql = $@"select count(r.Id) from SysRole as r left join Admin as a on r.Creater = a.Id";
            var count = SqlSugarDbContext.Instance.ExecutedSql().Ado.GetInt(countSql);
            Dictionary<string, object> dic = new Dictionary<string, object>
            {
                { "recordsTotal",count},
                { "recordsFiltered", count},
                { "list", list }
            };
            return Json(dic);
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult GetDetails(int id)
        {
            var result = SqlSugarDbContext.Instance.ExecutedSql().Queryable<SysRole>().Where(w => w.Id == id).First();
            return Json(result);
        }
        /// <summary>
        /// 添加/修改角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public IActionResult Add(SysRole role)
        {
            var result = new BaseResult { Code = MsgCode.Success };
            var details = SqlSugarDbContext.Instance.ExecutedSql().Queryable<SysRole>().Where(w => w.Name == role.Name).First();
            if (details != null && role.Id == 0)
            {
                result.Code = MsgCode.IsExist;
                return Json(result);
            }
            if (role.Id > 0)
            {
                var sum = SqlSugarDbContext.Instance.ExecutedSql().Updateable(role).IgnoreColumns(it => new { it.CreateTime, it.Valid }).ExecuteCommand();
                if (sum <= 0)
                {
                    result.Code = MsgCode.ProgramError;
                }
            }
            else
            {
                role.Creater = CurrentUser.Id;
                role.CreateTime = DateTime.Now;
                var sum = SqlSugarDbContext.Instance.ExecutedSql().Insertable(role).IgnoreColumns(it => new { it.Valid }).ExecuteCommand();
                if (sum <= 0)
                {
                    result.Code = MsgCode.ProgramError;
                }
            }
            return Json(result);
        }
    }
}