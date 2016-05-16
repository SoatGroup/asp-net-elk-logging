using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using log4net;
using log4net.Core;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private Random _random = new Random((int) DateTime.Now.Ticks);
        private static ILog _logger = LogManager.GetLogger(typeof (HomeController));
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            _logger.LogBusiness("About page visited", new Dictionary<string, object>());
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            _logger.LogBusiness("Contact page visited", new Dictionary<string, object>());
            return View();
        }

        public ActionResult Exception()
        {
            ViewBag.Message = "Exception page.";
            _logger.LogBusiness("Exception page visited", new Dictionary<string, object>());
            return View();
        }

        public ActionResult Performance()
        {
            ViewBag.Message = "Performance page.";
            _logger.LogBusiness("Performance page visited", new Dictionary<string, object>());
            GenerateLoadTime();
            return View();
        }

        private void GenerateLoadTime()
        {
            Thread.Sleep(_random.Next(0, 1000));
        }
    }
}