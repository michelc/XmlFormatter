using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace XmlFormatter.Models
{
    public class XmlViewModel
    {
        [Required(ErrorMessage="Please, provide some XML code")]
        public string Source { get; set; }
        public String Formatted { get; set; }
        public String Message { get; set; }
        public XmlDocument Document { get; set; }
    }

    public class XmlTools
    {
        public static XmlViewModel LoadXml(XmlViewModel xml)
        {
            try
            {
                // Charge le source XML
                xml.Document = new XmlDocument();
                xml.Document.LoadXml(xml.Source);
            }
            catch (Exception ex)
            {
                xml.Message = ex.Message;
            }

            return xml;
        }

        public static XmlViewModel FormatXml(XmlViewModel xml)
        {
            try
            {
                // Formatte le document XML
                var sb = new StringBuilder();
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "\t",
                    NewLineChars = "\n",
                    NewLineHandling = NewLineHandling.Replace,
                    NewLineOnAttributes = true
                };
                using (var writer = XmlWriter.Create(sb, settings))
                {
                    xml.Document.Save(writer);
                }

                // Gère l'indentation des commentaires
                xml.Formatted = IndentComments(sb.ToString());

                // Vérifie que l'encoding n'a pas changé
                xml = XmlTools.CheckEncoding(xml);

                xml.Message = null;
            }
            catch (Exception ex)
            {
                xml.Message = ex.Message;
            }
            return xml;
        }

        public static XmlViewModel CheckEncoding(XmlViewModel xml)
        {
            // Est-ce que le XML formatté contient un encoding UTF-16 ?
            var firstLine = xml.Formatted.Substring(0, xml.Formatted.IndexOf(Environment.NewLine));
            if (firstLine.Contains(" encoding=\"utf-16\""))
            {
                // Si oui, on recherche l'encoding d'origine
                // http://stackoverflow.com/questions/3520230/how-to-check-for-xmldeclaration-in-xmldocument-c-sharp
                var declaration = xml.Document.ChildNodes
                                    .OfType<XmlDeclaration>()
                                    .FirstOrDefault();

                var encoding = string.IsNullOrEmpty(declaration.Encoding)
                                ? ""
                                : " encoding=\"" + declaration.Encoding + "\"";

                // Et on remplace la déclaration d'encoding UTF-16 par celle d'origine
                var newFirstLine = firstLine.Replace(" encoding=\"utf-16\"", encoding);
                xml.Formatted = xml.Formatted.Replace(firstLine, newFirstLine);
            }

            return xml;
        }

        public static string IndentComments(string XmlText)
        {
            var sb = new StringBuilder();
            bool inComment = false;
            string leftIndent = "";

            string[] lines = XmlText.Split('\n');
            foreach (var line in lines)
            {
                var goodLine = line;
                if (line.Contains("<!--"))
                {
                    var before = line.Substring(0, line.IndexOf("<!--"));
                    if (before == new string('\t', before.Length))
                    {
                        inComment = true;
                        leftIndent = before;
                    }
                }
                if (line.Trim("\t ".ToCharArray()) == "-->")
                {
                    inComment = false;
                    goodLine = leftIndent + "-->";
                }
                if (inComment)
                {
                    if (!line.StartsWith(leftIndent))
                    {
                        goodLine = leftIndent + "\t" + line.Trim("\t ".ToCharArray());
                    }
                    else if ((leftIndent == "") && (!line.Contains("<!--")))
                    {
                        goodLine = leftIndent + "\t" + line.Trim("\t ".ToCharArray());
                    }
                }
                if (line.Trim('\t').Contains("-->"))
                {
                    inComment = false;
                }
                sb.Append(goodLine);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

   }
}