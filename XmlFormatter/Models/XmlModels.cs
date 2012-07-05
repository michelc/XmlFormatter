using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace XmlFormatter.Models
{
    public class XmlViewModel
    {
        public string Source { get; set; }
        public MvcHtmlString Formatted { get; set; }
        public MvcHtmlString Message { get; set; }
    }

    public class XmlTools
    {
        public static XmlViewModel FormatXml(XmlViewModel xml)
        {
            try
            {
                // Charge le source XML
                var document = new XmlDocument();
                // document.Load(new StringReader(xml.Source));
                document.LoadXml(xml.Source);

                // http://stackoverflow.com/questions/3520230/how-to-check-for-xmldeclaration-in-xmldocument-c-sharp
                var declaration = document.ChildNodes
                                .OfType<XmlDeclaration>()
                                .FirstOrDefault();

                // declaration.Encoding

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
                    document.Save(writer);
                }

                // Gère l'indentation des commentaires
                string firstLine = null;
                if (document.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                {
                    firstLine = xml.Source.Split('\r')[0];
                }
                xml.Formatted = new MvcHtmlString(IndentComments(firstLine, sb.ToString()));
                xml.Message = null;
            }
            catch (Exception ex)
            {
                xml.Message = new MvcHtmlString(ex.Message);
            }
            return xml;
        }

        public static string IndentComments(string firstLine, string XmlText)
        {
            var sb = new StringBuilder();
            bool inComment = false;
            string leftIndent = "";

            string[] lines = XmlText.Split('\n');
            foreach (var line in lines)
            {
                var goodLine = line;
                if (firstLine != null)
                {
                    goodLine = firstLine;
                    firstLine = null;
                }
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