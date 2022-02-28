using System.Xml.Xsl;

namespace Library_network.Library
{
    public class XsltTransformer
    {
        public static void SaveAs(string xmlPath, string xsltPath, string output)
        {
            XslTransform xslt = new();

            xslt.Load(xsltPath);

            xslt.Transform(xmlPath, output);
        }
    }
}
