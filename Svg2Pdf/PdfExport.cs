
namespace Svg2Pdf
{


    public class PdfExport
    {
        

        public static byte[] SvgToPdf(string svg, bool current_view)
        {
            if (string.IsNullOrEmpty(svg))
                throw new System.ArgumentNullException("svg");




            string styleTemplate = @"
path:not([fill]){fill: none}
path:not([stroke]){stroke: #000}
path:not([stroke-width]){stroke-width: 1}
";



            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

            doc.XmlResolver = null; // https://stackoverflow.com/questions/4445348/net-prevent-xmldocument-loadxml-from-retrieving-dtd
            doc.LoadXml(svg);



            System.Xml.XmlNamespaceManager nsmgr = XmlHelper.GetNamespaceManager(doc);
            string realDefaultNamespace = nsmgr.LookupNamespace("dft");

            // System.Xml.XmlElement style = doc.CreateElement("", "style", realDefaultNamespace);
            System.Xml.XmlElement style = doc.CreateElement("style");
            style.SetAttribute("type", "text/css");
            style.SetAttribute("media", "all");
            style.InnerXml = styleTemplate;
            doc.DocumentElement.PrependChild(style);

            // string svgContent = doc.OuterXml.Replace("xmlns=\"\"", "");
            // doc.LoadXml(svgContent);
            // System.Console.WriteLine(doc.InnerXml);
            




            // data-height="967px" data-width="1324px" 
            System.Xml.XmlAttribute dataWidth = doc.DocumentElement.Attributes["data-width"];
            System.Xml.XmlAttribute dataHeight = doc.DocumentElement.Attributes["data-height"];



            string swidth = "967px";
            string sheight = "967px";

            swidth = dataWidth.Value;
            sheight = dataHeight.Value;



            if (string.IsNullOrEmpty(swidth))
                throw new System.ArgumentNullException("swidth");

            if (string.IsNullOrEmpty(sheight))
                throw new System.ArgumentNullException("sheight");

            swidth = swidth.Trim();
            sheight = sheight.Trim();


            string templatePre = @"<!doctype html>
<html>
<head>
<title></title>
<style>
html, body, div, svg { margin: 0px; padding: 0px; }

#SVG svg 
{
    -ms-transform: translate(-50%, -50%) translateZ(0);
    -ms-transform-origin: 50% 50% 0px;
    -webkit-transform: translate(-50%, -50%) translateZ(0);
    -webkit-transform-origin: 50% 50% 0px;
    background: #fff;
    fill: #ffffff;
    left: 50%;
    position: absolute;
    user-select: none;
    top: 50%;
    transform: translate(-50%, -50%);
    transform-origin: 50% 50% 0px;
}

</style>
</head>
<body>
<div id=""SVG"" style=""display: block; width: " + swidth + "; height: " + sheight + @"; position: relative; background-color: hotpink; overflow: hidden;"">
";
            string templatePost = @"
</div>
</body>
</html>
";

            svg = doc.DocumentElement.OuterXml;


            string html = templatePre + svg + templatePost;



            if (swidth.EndsWith("px", System.StringComparison.InvariantCultureIgnoreCase))
            {
                swidth = swidth.Substring(0, swidth.Length - 2);
            }

            if (sheight.EndsWith("px", System.StringComparison.InvariantCultureIgnoreCase))
            {
                sheight = sheight.Substring(0, sheight.Length - 2);
            }

            int width = 1324;
            int height = 967;

            if (!int.TryParse(swidth, out width))
            {
                throw new System.ArgumentException("swidth");
            }

            if (!int.TryParse(sheight, out height))
            {
                throw new System.ArgumentException("sheight");
            }
            
            if(current_view)
                return Html2Pdf(html, width + 37, height + 37);


            System.Xml.XmlAttribute attWidth = doc.DocumentElement.Attributes["width"];
            System.Xml.XmlAttribute attHeight = doc.DocumentElement.Attributes["height"];
            System.Xml.XmlAttribute attViewBox = doc.DocumentElement.Attributes["viewBox"];



            swidth = attWidth.Value;
            sheight = attHeight.Value;
            string viewBox = attViewBox.Value;

            //System.Console.WriteLine(width);
            //System.Console.WriteLine(height);
            //System.Console.WriteLine(viewBox);
            string[] viewBoxValues = viewBox.Split(' ');

            string viewbox_width = viewBoxValues[2];
            string viewbox_height = viewBoxValues[3];

            //System.Console.WriteLine(viewBoxValues);


            if (viewbox_width.EndsWith("px", System.StringComparison.InvariantCultureIgnoreCase))
            {
                viewbox_width = viewbox_width.Substring(0, viewbox_width.Length - 2);
            }

            if (viewbox_height.EndsWith("px", System.StringComparison.InvariantCultureIgnoreCase))
            {
                viewbox_height = viewbox_height.Substring(0, viewbox_height.Length - 2);
            }

            double viewbox_dblwidth = 1324.0;
            double viewbox_dblHeight = 967.0;

            if (!double.TryParse(viewbox_width, out viewbox_dblwidth))
            {
                throw new System.ArgumentException("viewbox_width");
            }

            if (!double.TryParse(viewbox_height, out viewbox_dblHeight))
            {
                throw new System.ArgumentException("viewbox_height");
            }


            double r1 = 21.0/viewbox_dblwidth;
            double r2 = 29.7/viewbox_dblHeight;
            double r = System.Math.Min(r1, r2);

            var w = viewbox_dblwidth * r;
            var h = viewbox_dblHeight * r;

            attWidth.Value = w.ToString("N8", System.Globalization.CultureInfo.InvariantCulture) + "cm";
            attHeight.Value = h.ToString("N8", System.Globalization.CultureInfo.InvariantCulture) + "cm";

            w *= 0.393701 * 100;
            h *= 0.393701 * 100;

            System.IO.File.WriteAllText(@"d:\stefanlol.xml", doc.OuterXml, System.Text.Encoding.UTF8);


            svg = doc.DocumentElement.OuterXml;


            templatePre = @"<!doctype html>
<html>
<head>
<title></title>
<style>
html, body, div, svg { margin: 0px; padding: 0px; }
</style>
</head>
<body>
<div>
";

            html = templatePre + svg + templatePost;

            return Html2Pdf(html, (int)System.Math.Ceiling(w),  (int)System.Math.Ceiling(h));
        }



        public static object obj = new object();


        public static byte[] Html2Pdf(string strHTML, int width, int height)
        {
            //wkHtmlToPdfSharpStatic.InitLib(false);
            // pc.FinishAction("Library initialized");

            byte[] buf = null;

            lock (obj)
            {
                wkHtmlToPdfSharp.Synchronized.SynchronizedwkHtmlToPdfSharp sc =
                    new wkHtmlToPdfSharp.Synchronized.SynchronizedwkHtmlToPdfSharp(new wkHtmlToPdfSharp.GlobalConfig()
                    .SetMargins(new System.Drawing.Printing.Margins(0, 0, 0, 0))
                    .SetDocumentTitle("Legende")
                    .SetCopyCount(1)
                    //.SetImageQuality(50)
                    .SetImageQuality(100)
                    .SetLosslessCompression(true)
                    //.SetMaxImageDpi(20)
                    .SetMaxImageDpi(15200)
                    .SetOutlineGeneration(true)
                    .SetOutputDpi(14200)
                    .SetPaperOrientation(true)
                    //.SetPaperSize(System.Drawing.Printing.PaperKind.Letter)
                    //.SetPaperSize(250, 200)
                    //.SetPaperSize(System.Drawing.Printing.PaperKind.A4)

                    // Width/Height: Gets or sets the width/height of the paper, in hundredths of an inch.
                    // 1 inch = 2.54cm = 25.4mm
                    // 1/100 inch = 0.0254cm = 0.254mm

                    // Pixel
                    .SetPaperSize(new System.Drawing.Printing.PaperSize("SvgSize", width , height ))
                    // A4
                    // Height: 29.7 cm = 11.69291 inch
                    // Width: 21 cm = 8.26772 inch

                    // <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="21cm" height="29.7cm" viewBox="0 0 2098 2969.3" version="1.1">
                );

                buf = sc.Convert(new wkHtmlToPdfSharp.ObjectConfig()
                     .SetPrintBackground(true)
                     .SetIntelligentShrinking(false)
                    , strHTML
                );

                /*
                sc.Convert(new ObjectConfig()
                    .SetPrintBackground(true)
                    .SetProxyString("http://localhost:8888")
                    .SetAllowLocalContent(true)
                    .SetCreateExternalLinks(false)
                    .SetCreateForms(false)
                    .SetCreateInternalLinks(false)
                    .SetErrorHandlingType(ObjectConfig.ContentErrorHandlingType.Ignore)
                    .SetFallbackEncoding(System.Text.Encoding.ASCII)
                    .SetIntelligentShrinking(false)
                    .SetJavascriptDebugMode(true)
                    .SetLoadImages(true)
                    .SetMinFontSize(16)
                    .SetRenderDelay(2000)
                    .SetRunJavascript(true)
                    .SetIncludeInOutline(true)
                    .SetZoomFactor(2.2)
                    ,strHTML
                );
                */
            }

            if (buf == null)
            {
                throw new System.Exception("Error converting");
            } // End if (buf == null)

            return buf;
        } // End Function Html2Pdf 


    }


}
