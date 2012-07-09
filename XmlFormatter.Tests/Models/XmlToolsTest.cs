using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlFormatter.Models;

namespace XmlFormatter.Tests.Models
{
    [TestClass]
    public class XmlToolsTest
    {
        [TestMethod]
        public void LoadXml_Fill_Document_When_Source_Is_Valid()
        {
            // Arrange
            var xml = new XmlViewModel
            {
                Source = "<valid />"
            };

            // Act
            xml = XmlTools.LoadXml(xml);

            // Assert
            Assert.AreNotEqual(0, xml.Document.ChildNodes.Count, "Should not have returned an empty XmlDocument");
            Assert.IsNull(xml.Message, "Message should contains a null String");
        }

        [TestMethod]
        public void LoadXml_Fill_Message_When_Source_Is_Invalid()
        {
            // Arrange
            var xml = new XmlViewModel
            {
                Source = "<invalid>"
            };

            // Act
            xml = XmlTools.LoadXml(xml);

            // Assert
            Assert.AreEqual(0, xml.Document.ChildNodes.Count, "Should have returned an empty XmlDocument");
            Assert.IsNotNull(xml.Message, "Message should contains a String");
        }
    }
}
