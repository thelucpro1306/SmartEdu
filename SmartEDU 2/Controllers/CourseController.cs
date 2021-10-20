using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEDU_2.Models;
namespace SmartEDU_2.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        LinQDataContext data = new LinQDataContext();

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
            var course = data.KHOAHOCs.Select(a => a).Where(s => s.TRANGTHAI == true).ToList();
            return View(course);
        }

        public ActionResult Detail(int id)
        {
            var courseDetail = data.KHOAHOCs.Select(a => a).Where(a=>a.MAKHOAHOC==id);
            return View(courseDetail);
        }

        public ActionResult ApplyToACourse(int id)
        {
                var applyToACourse = data.KHOAHOCs.Select(a => a).Where(a => a.MAKHOAHOC == id);
                Course.courseID = id.ToString();
            if (IsLoggined() == true)
            {
                var tk = (TAIKHOAN)Session["TAIKHOAN"];
                ViewData["HOCSINH"] = data.HOCSINHs.Select(a => a).Where(a => a.ID == tk.ID).FirstOrDefault();
            }
            else { ViewData["HOCSINH"] = new HOCSINH(); }
            return View(applyToACourse);
            
        }

    }
}