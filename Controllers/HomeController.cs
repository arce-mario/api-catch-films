using System;
using System.Web.Mvc;

namespace ApiCatchFilms.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.serverTime = DateTime.UtcNow;
            return View();
        }

    }
}