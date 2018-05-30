using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MP.Base;
using MP.Base.Enums;
using MP.Models;
using MP.Models.Extension;
using MP.Sugar;
using MP.Web.Models;

namespace MP.Web.Controllers
{
    public class ProjectController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add(int id)
        {
            var result = SqlSugarDbContext.Instance.ExecutedSql().Queryable<ProjectInfo>().Where(w => w.Valid == 1 && w.Id == id).First();
            var projectEffect = SqlSugarDbContext.Instance.ExecutedSql().Queryable<ProjectEffect>().Where(w => w.ProjectId == id && w.Valid == 1).ToList();
            return View(new ProjectResult { ProjectInfo = result == null ? new ProjectInfo() : result, EffectResults = projectEffect });
        }
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <param name="start">分页开始</param>
        /// <param name="length">分页大小</param>
        /// <returns></returns>
        public IActionResult GetProject(int start, int length)
        {
            var count = 0;
            var list = SqlSugarDbContext.Instance.ExecutedSql().Queryable<ProjectInfo>().Select(s => new
            { s.Id, s.Name, s.Company, s.Duties, s.Place, s.Scale, s.ProjectTime, s.StartTime, s.EndTime, s.Valid })
            .Where(w => w.Valid == 1).ToPageList(start, length, ref count).ToList();
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
            return View();
        }
        /// <summary>
        /// 添加/编辑
        /// </summary>
        /// <param name="info"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public IActionResult AddProject(ProjectInfo info, List<ProjectEffect> imageList)
        {
            if (info.Id > 0)
            {
                info.UpdateTime = DateTime.Now;
                var t3 = SqlSugarDbContext.Instance.ExecutedSql().Updateable(info).IgnoreColumns(it => new { it.CreateTime, it.Valid }).ExecuteCommandAsync();
                if (imageList.Count > 0)
                {
                    imageList.ForEach(f => f.ProjectId = info.Id);
                    var s9 = SqlSugarDbContext.Instance.ExecutedSql().Insertable(imageList).ExecuteCommand();
                }
                return Json(new BaseResult { Code = MsgCode.Success });
            }
            else
            {
                info.CreateTime = DateTime.Now;
                var t3 = SqlSugarDbContext.Instance.ExecutedSql().Insertable(info).ExecuteCommandIdentityIntoEntity();
                if (t3 == true && imageList.Count > 0)
                {
                    imageList.ForEach(f => f.ProjectId = info.Id);
                    var s9 = SqlSugarDbContext.Instance.ExecutedSql().Insertable(imageList).ExecuteCommand();
                }
                return Json(new BaseResult { Code = MsgCode.Success });
            }
        }
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="file">文件信息</param>
        /// <returns></returns>
        public async Task<IActionResult> Upload([FromServices]IHostingEnvironment environment, IFormFileCollection file)
        {
            var result = new BaseResult();
            string path = string.Empty;
            //获取上传文件
            //var files = Request.Form.Files;
            if (file == null || file.Count() <= 0)
            {
                result.Msg = "请选择上传的文件。";
                return Json(result);
            }
            //格式限制
            var allowType = new string[] { "image/jpg", "image/png", "image/jpeg" };
            if (file.Any(c => allowType.Contains(c.ContentType)))
            {
                var time = DateTime.Now.ToString("MMddHHmmss");
                string strpath = Path.Combine("images", time + ".png");
                path = Path.Combine(environment.WebRootPath, strpath);
                using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    await file[0].CopyToAsync(stream);
                }
                result.Data = strpath;
                result.Msg = time;
                result.Code = MsgCode.Success;
            }
            else

            {
                result.Msg = "图片格式错误";
            }
            return Json(result);
        }
    }
}