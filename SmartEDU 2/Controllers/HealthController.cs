using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEDU_2.Models;

namespace SmartEDU_2.Controllers
{
    public class HealthController : Controller
    {
        LinQDataContext data = new LinQDataContext();

        // GET: Health
        public bool IsLoggined()
        {
            if (Session["TAIKHOAN"] != null)
            {

                return true;
            }
            else
                return false;
        }

     
        public ActionResult Index()
        {

            Session["ReturnUrl"] = Url.Action("Index", "Health");

            if (IsLoggined() == true)
            {
                TAIKHOAN tk = (TAIKHOAN)Session["TAIKHOAN"];
                int idTk = tk.ID;
      
                var health = data.HOCSINHs.Select(a => a).Where(a => a.ID == idTk);
                return View(health);
            }
            else
                return RedirectToAction("Login", "Account");
        }
     
    }
}