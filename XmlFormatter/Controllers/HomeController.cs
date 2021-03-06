﻿using System.Web.Mvc;
using XmlFormatter.Models;

namespace XmlFormatter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var xml = (XmlViewModel)TempData["xml"];

            if (xml == null)
            {
                xml = new XmlViewModel();
            }

            return View("Index", xml);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(XmlViewModel xml)
        {
            if (ModelState.IsValid)
            {
                xml = XmlTools.LoadXml(xml);
                if (!string.IsNullOrEmpty(xml.Message))
                {
                    ModelState.AddModelError("Source", xml.Message);
                    xml.Message = null;
                }
                else
                {
                    xml = XmlTools.FormatXml(xml);
                    TempData["xml"] = xml;

                    return RedirectToAction("Index");
                }
            }

            return View("Index", xml);
        }
    }
}
