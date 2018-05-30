using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MP.Base.Filters
{
    /// <summary>
    /// 登录认证特性
    /// </summary>
    public class AuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;
            if (filterContext.Filters.Count < 5)
            {
                if (controller == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Account" }));
                    return;
                }
                if (controller.Permissions == null || controller.CurrentUser == null)
                {
                    if (filterContext.HttpContext.Request.IsHttps)
                    {
                        filterContext.Result = new RedirectResult("/404.html");
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Account" }));
                    }
                }
                else
                {
                    if (!controller.IsLogin)
                    {
                        string reutrnUrl = string.Empty;
                        foreach (var item in filterContext.RouteData.Values.Values)
                        {
                            reutrnUrl += "/" + item;
                        }
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                            new
                            {
                                controller = "Account",
                                action = "Index",
                                returnUrl = reutrnUrl
                            }));
                    }
                    else
                    {
                        var controllerName = filterContext.RouteData.Values["Controller"].ToString().ToLower();
                        if (controller.Permissions.All(it => it.Controller.ToLower() != controllerName))//&& it.Action == actionName
                        {
                            if (filterContext.HttpContext.Request.IsHttps)
                            {
                                filterContext.Result = new RedirectResult("/404.html");
                            }
                            else
                            {
                                filterContext.Result = new RedirectResult("/404.html");
                            }
                        }
                        controller.ViewBag.TopName = controller.CurrentUser.Name;
                        controller.ViewBag.Menu = controller.Permissions;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
