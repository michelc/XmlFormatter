using System.Web.Mvc;
using XmlFormatter.Models;

namespace XmlFormatter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var xml = new XmlViewModel();

            return View(xml);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(XmlViewModel xml)
        {
            xml = XmlTools.FormatXml(xml);

            return View(xml);
        }
    }
}
