using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using log4net;
using log4net.Core;
using WebApplication1.Controllers;
using System.Linq;
using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class PerformanceActionFilter : IActionFilter
    {
        private static ILog _logger = LogManager.GetLogger(typeof(PerformanceActionFilter));
        private Stopwatch stopWatch = new Stopwatch();

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            stopWatch.Reset();
            stopWatch.Start();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            stopWatch.Stop();
            var executionTime = stopWatch.ElapsedMilliseconds;
            var httpContext = filterContext.RequestContext.HttpContext;
            _logger.LogPerf(executionTime, new Dictionary<string, object>()
            {
                { "url", httpContext.Request.RawUrl},
                { "httpVerb", httpContext.Request.RequestType},
                { "routeData", filterContext.RequestContext.RouteData.Values.ToDictionary() },
                { "query", httpContext.Request.QueryString},
                { "form", httpContext.Request.Form.ToDictionary()},
                { "browser", httpContext.Request.Browser.Browser},
                { "referer", httpContext.Request.Headers["Referer"] },
                { "userIp", httpContext.Request.UserHostAddress },
                { "useragent", httpContext.Request.UserAgent }
            });
        }
    }
}