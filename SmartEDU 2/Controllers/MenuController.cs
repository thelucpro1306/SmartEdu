using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEDU_2.Models;
namespace SmartEDU_2.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu

        LinQDataContext  data = new LinQDataContext();

        public ActionResult Index()
        {
            var menu = data.THUCDONs.Select(a => a);
            return View(menu);
        }
    }
}