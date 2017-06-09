
namespace Svg2Pdf
{


    internal class XmlHelper
    {

        public static System.Xml.XmlNamespaceManager GetNamespaceManager(System.Xml.XmlDocument doc)
        {
            if (doc == null)
                throw new System.ArgumentNullException("doc");

            System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager(doc.NameTable);

            // <Report xmlns="http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition" xmlns:rd="http://schemas.microsoft.com/SQLServer/reporting/reportdesigner">

            if (doc.DocumentElement != null)
            {
                // string strNamespace = doc.DocumentElement.NamespaceURI;
                // System.Console.WriteLine(strNamespace);
                // nsmgr.AddNamespace("dft", strNamespace);

                System.Xml.XPath.XPathNavigator xNav = doc.CreateNavigator();
                while (xNav.MoveToFollowing(System.Xml.XPath.XPathNodeType.Element))
                {
                    System.Collections.Generic.IDictionary<string, string> localNamespaces =
                        xNav.GetNamespacesInScope(System.Xml.XmlNamespaceScope.Local);

                    foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in localNamespaces)
                    {
                        string prefix = kvp.Key;
                        if (string.IsNullOrEmpty(prefix))
                            prefix = "dft";

                        nsmgr.AddNamespace(prefix, kvp.Value);
                    } // Next kvp

                } // Whend

                return nsmgr;
            } // End if (doc.DocumentElement != null)

            nsmgr.AddNamespace("dft", "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition");
            // nsmgr.AddNamespace("dft", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");

            return nsmgr;
        } // End Function GetReportNamespaceManager


    }


}
