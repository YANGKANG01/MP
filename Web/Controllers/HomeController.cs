using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MP.Base;
using MP.Models;
using MP.Sugar;
using MP.Web.Models;

namespace MP.Web.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            var result = SqlSugarDbContext.Instance.ExecutedSql().Queryable<ProjectInfo>().Where(w => w.Valid == 1).OrderBy(o => o.CreateTime, SqlSugar.OrderByType.Desc).ToList();
            ViewData["projectEffect"] = SqlSugarDbContext.Instance.ExecutedSql().Queryable<ProjectEffect>().Where(w => w.Valid == 1).ToList();
            return View(result);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
