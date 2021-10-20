using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartEDU_2.Models;
using PagedList;
using PagedList.Mvc;
namespace SmartEDU_2.Controllers
{
    public class NotificationController : Controller
    {
        // GET: ThongBao
        LinQDataContext data = new LinQDataContext();

        public ActionResult Index(int ? page)
        {
            int pageSize = 4;
            int pageNum = (page ?? 1);
            var notification = data.THONGBAOs.Select(a => a);
            return View(notification.ToPagedList(pageNum,pageSize));
        }
        
        private List<THONGBAO> GetNewNotification(int count)
        {
            return data.THONGBAOs.OrderByDescending(a => a.NGAYTHONGBAO).Take(count).ToList();
        }

        private List<THONGBAO> GetNewNotificationException(int count,int id)
        {
            return data.THONGBAOs.OrderByDescending(a => a.NGAYTHONGBAO).Where(a=>a.MATHONGBAO!=id).Take(count).ToList();
        }

        public ActionResult NotificationForHome()
        {
            var notification = GetNewNotification(4);
            return View(notification);
        }

        public ActionResult NotificationForHomeFooter()
        {
            var notification = GetNewNotification(5);
            return View(notification);
        }

        public ActionResult NotificationForInsideNotificationDetail(int id)
        {
            var notification = GetNewNotificationException(3,id);
            return View(notification);
        }

        public ActionResult Detail(int id)
        {
            var notificationDetail = data.THONGBAOs.Select(a => a).Where(a => a.MATHONGBAO == id);
            return View(notificationDetail);
        }
    }
}