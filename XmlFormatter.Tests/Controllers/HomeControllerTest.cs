using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlFormatter.Controllers;
using XmlFormatter.Models;

namespace XmlFormatter.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.Model, typeof(XmlViewModel));
        }
    }
}
