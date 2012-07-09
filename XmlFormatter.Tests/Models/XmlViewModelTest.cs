using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlFormatter.Models;

namespace XmlFormatter.Tests.Models
{
    [TestClass]
    public class XmlViewModelTest
    {
        [TestMethod]
        public void XmlViewModel_Source_Is_Required()
        {
            // http://bradwilson.typepad.com/blog/2009/04/dataannotations-and-aspnet-mvc.html

            // Arrange
            var propertyInfo = typeof(XmlViewModel).GetProperty("Source");

            // Act
            var attribute = propertyInfo.GetCustomAttributes(typeof(RequiredAttribute), true)
                                        .Cast<RequiredAttribute>()
                                        .FirstOrDefault();

            // Assert
            Assert.IsNotNull(attribute, "Should have returned a RequiredAttribute");
        }
    }
}
