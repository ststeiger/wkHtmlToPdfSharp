using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleHtmlToPdfConverter
{
    class SimpleHelper
    {
        public static System.Xml.XmlDocument GetReport(string reportPath)
        {
            // http://blogs.msdn.com/b/tolong/archive/2007/11/15/read-write-xml-in-memory-stream.aspx
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            // doc.Load(memorystream);
            // doc.Load(FILE_NAME);
            // doc.Load(strFileName);


            using (System.IO.Stream strm = System.IO.File.OpenRead(reportPath)) 
            {

                using (System.Xml.XmlTextReader xtrReader = new System.Xml.XmlTextReader(strm))
                {
                    doc.Load(xtrReader);
                    xtrReader.Close();
                } // End Using xtrReader

            } // End Using strm 

            return doc;
        } // End Function GetReport


        public static void SaveDocument(System.Xml.XmlDocument origDoc, System.IO.Stream strm, bool bDoReplace)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

            if (bDoReplace)
                doc.LoadXml(origDoc.OuterXml.Replace("xmlns=\"\"", ""));

            using (System.Xml.XmlTextWriter xtw = new System.Xml.XmlTextWriter(strm, System.Text.Encoding.UTF8))
            {
                xtw.Formatting = System.Xml.Formatting.Indented; // if you want it indented
                xtw.Indentation = 4;
                xtw.IndentChar = ' ';

                doc.Save(xtw);
                xtw.Flush();
                xtw.Close();
            } // End Using xtw 

            doc = null;
        } // End Sub SaveDocument 


        public static System.Xml.XmlNamespaceManager GetDefaultNamespace(System.Xml.XmlDocument doc)
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
