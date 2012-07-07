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
        public void Default_Action_Returns_Index_View()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Should have returned a ViewResult");
            Assert.AreEqual("Index", result.ViewName, "View name should have been 'Index'");
        }

        [TestMethod]
        public void Index_Action_Returns_XmlViewModel_To_The_View()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.Model, typeof(XmlViewModel), "Model should have been 'XmlViewModel'");
        }

        [TestMethod]
        public void Index_Action_Should_Redirect_When_Model_Is_Valid()
        {
            // Arrange
            var controller = new HomeController();
            var xml = new XmlViewModel();

            // Act
            var result = controller.Index(xml) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Index", result.RouteValues["Action"], "Action should have been 'Index'");
        }

        [TestMethod]
        public void Index_Action_Should_Redisplay_With_Errors_When_Model_Is_Invalid()
        {
            // Arrange
            var controller = new HomeController();
            var xml = new XmlViewModel();
            controller.ModelState.AddModelError("Source", "Error message");

            // Act
            var result = controller.Index(xml) as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Should have returned a ViewResult");
            Assert.AreEqual("Index", result.ViewName, "View name should have been 'Index'");
            Assert.IsTrue(result.ViewData.ModelState.Count > 0, "Should have returned errors");
        }
    }
}