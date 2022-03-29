using Microsoft.AspNetCore.Mvc;

namespace Lms.Web.Controllers
{
    public class ActivityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
