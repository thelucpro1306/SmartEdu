using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEDU_2.Models;

namespace SmartEDU_2.Controllers
{
    public class GoToSchoolController : Controller
    {
        // GET: GoToSchool

        LinQDataContext data = new LinQDataContext();

        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult RegisterSuccessful()
        {
            //var thanhtoan = data.KHOAHOCs.Select(a => a).Where(a => a.MAKHOAHOC == id);
            //Course.courseID = id.ToString();
            //return View(thanhtoan);
            return View();
        
         
        }

       

        [HttpPost]
        public ActionResult Index(FormCollection collection,PHIEUDANGKYHOC ph)
        {
           


                var tenBe = collection.Get("hotenbe");
                var namSinh = collection.Get("namsinh");
                //var tenCha = collection.Get("hotencha");
                //var tenMe = collection.Get("hotenme");
                //var ngheCha = collection.Get("nghecha");
                //var ngheMe = collection.Get("ngheme");           
                //var sdt = collection.Get("sdt");
                //var diaChi = collection.Get("diachi");
                var gioiTinh = "";
                if (collection.Get("gtNam") != "")
                {
                    gioiTinh = "Nam";
                }
                else
                {
                    gioiTinh = "Nữ";
                }
                var maKhoaHoc = Course.courseID;
                ph.HOTENBE = tenBe;
                ph.NAMSINH = DateTime.Parse(namSinh);
                //ph.HOTENCHA = tenCha;
                //ph.HOTENME = tenMe;
                //ph.NGHECUACHA = ngheCha;
                //ph.NGHECUAME = ngheMe;
                //ph.SDT = sdt;
                //ph.DIACHI = diaChi;
                ph.GIOITINH = gioiTinh;
                ph.MAKHOAHOC = int.Parse(maKhoaHoc);
                data.PHIEUDANGKYHOCs.InsertOnSubmit(ph);
                data.SubmitChanges();
                return RedirectToAction("RegisterSuccessful");
            
        }
    }
}