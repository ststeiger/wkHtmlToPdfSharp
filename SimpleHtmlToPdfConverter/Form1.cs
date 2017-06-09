
using System.Windows.Forms;

using wkHtmlToPdfSharp;
using wkHtmlToPdfSharp.Synchronized;


namespace SimpleHtmlToPdfConverter
{


    public partial class Form1 : Form
    {


        public static string GetTextFile(string FileName)
        {
            string str = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            str = System.IO.Path.Combine(str, FileName);
            
            return System.IO.File.ReadAllText(str, System.Text.Encoding.Default);
        } // End Function GetTextFile


        public Form1()
        {
            InitializeComponent();

            if (false)
            {
                // GetTextFile("footer_base64_invest.txt");
                this.txtHtml.Text = GetTextFile("Legenden.txt");
                this.txtHtml.Text = GetHtml();

                //Farben();
            }

        } // End Constructor 


        public string GetHtml()
        {
            Legende leg = new Legende();
            //System.Data.DataTable dtSet = leg.LegendenSettings;
            //System.Data.DataTable dtData = leg.LegendenDaten;

            //Console.WriteLine(dtSet.Rows.Count);
            //Console.WriteLine(dtData.Rows.Count);

            //Console.WriteLine(leg.TextField);
            //Console.WriteLine(leg.ValueField);
            //Console.WriteLine(leg.Language);
            //Console.WriteLine(leg.Sum);
            string strHtmlTemplate = GetTextFile("LegendenTemplate.txt");
            // strHtmlTemplate = strHtmlTemplate.Replace("{", "{{").Replace("}", "}}");



            string logofile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            logofile = System.IO.Path.Combine(logofile, "../../logos/gmbhco_lect.png");
            logofile = System.IO.Path.GetFullPath(logofile);

            string strLogoData = SimpleHtmlToPdfConverter.Draw.ImageAsDataString(logofile, System.Drawing.Imaging.ImageFormat.Png);
            string stra = string.Format("<img alt=\"{0}\" width=\"25%\" src=\"{1}\" />", "Logo", strLogoData);
            string num = leg.FormatNumber(123.456);
            System.Console.WriteLine(num);


            string Heading = string.Format("{0} - {1} - {2} - {3}", leg.Location, leg.Premise, leg.Floor, leg.Title);
            System.Console.WriteLine(Heading);
            string date = leg.FormatDateTime("dd-MMM-yyyy");


            string strHTML = string.Format(strHtmlTemplate, stra, GetRows(leg));
            return strHTML;
        } // End Function GetHtml


        public string GetRows(Legende leg)
        {
            string strRowTemplate = @"<tr{0}>
	<td class=""bullet {1}""{5}></td>
	<td class=""legendText"">
		<div class=""sizelimit"">
			{2}
		</div>
	</td>

	<td class=""legendValue"">
		{3}
	</td>

	<td>
		{4}
	</td>
</tr>

";


            string NumberFormat = leg.NumberFormat;
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            nfi.NumberGroupSeparator = "'";
            nfi.CurrencyDecimalSeparator = ".";
            nfi.CurrencyGroupSeparator = "'";
            nfi.CurrencySymbol = "CHF";


            string strInlineStyle = "";
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();




            string strHeader = @"<tr>
	<td class=""bullet Color0 legendTitle"" colspan=""3"">
        {0}
    </td>
</tr>

<tr>
    <td colspan=""3"">&nbsp;</td>
</tr>


<tr>
	<td class=""bullet Color0""></td>
	<td class=""legendText"">
        &nbsp;
	</td>

	<td class=""legendValue legendSubTitle"">
		{1}
	</td>

	<td>
		&nbsp;
	</td>
</tr>
";

            sb.AppendFormat(strHeader, System.Web.HttpUtility.HtmlEncode(leg.Title), System.Web.HttpUtility.HtmlEncode(leg.ValueTitle));
            

            bool b = 1 == 1;

            if (leg.WithSum && !leg.SumInData)
            {
                string strSumValue = leg.Sum.ToString(NumberFormat, nfi);

                if (b)
                    sb.AppendFormat(strRowTemplate, " class=\"sum\"", "Color0", leg.SumText, strSumValue, leg.PdfUnit, strInlineStyle);
                else
                    sb.AppendFormat(strRowTemplate, " class=\"sum\"", "Color0", leg.SumText, strSumValue, leg.HtmlUnit, strInlineStyle);
            } // End if (leg.WithSum && !leg.SumInData)


            for (int i = 0; i < leg.LegendenDaten.Rows.Count; ++i)
            {
                System.Data.DataRow dr = leg.LegendenDaten.Rows[i];

                int AP_LEG_Pattern = System.Convert.ToInt32(dr["AP_LEG_Pattern"]);
                int AP_LEG_ForeColor = 0;

                string strFgColor = System.Convert.ToString(dr["HtmlForegroundColor"]);
                string strBgColor = System.Convert.ToString(dr["HtmlBackgroundColor"]);
                string strLineColor = System.Convert.ToString(dr["HtmlLineColor"]);


                string strAP_LEG_ForeColor = System.Convert.ToString(dr["AP_LEG_ForeColor"]);
                if (!string.IsNullOrEmpty(strAP_LEG_ForeColor))
                    int.TryParse(strAP_LEG_ForeColor, out AP_LEG_ForeColor);


                if (string.IsNullOrEmpty(strFgColor))
                    strFgColor = "#000000";

                if (System.StringComparer.Ordinal.Equals(strBgColor, "#"))
                    strBgColor = "#000000";

                if (System.StringComparer.Ordinal.Equals(strLineColor, "#"))
                    strLineColor = "#000000";


                System.Drawing.Color colFC = System.Drawing.ColorTranslator.FromHtml(strFgColor);
                System.Drawing.Color colHB = System.Drawing.ColorTranslator.FromHtml(strBgColor);
                System.Drawing.Color colHL = System.Drawing.ColorTranslator.FromHtml(strLineColor);
                

                double dblValue = System.Convert.ToDouble(dr["Value"]);
                string strText = System.Convert.ToString(dr["Text"]) ?? "";
                string strValue = dblValue.ToString(NumberFormat, nfi);


                strText = System.Web.HttpUtility.HtmlEncode(strText);
                strValue = System.Web.HttpUtility.HtmlEncode(strValue);

                string ColorClass = null;
                if (AP_LEG_Pattern == 12)
                    ColorClass = "Color" + AP_LEG_ForeColor.ToString();
                else
                {
                    string src = SimpleHtmlToPdfConverter.Draw.GetHatchRectangleAsDataString(9, colFC, colHB, colHL, System.Drawing.Imaging.ImageFormat.Png);
                    strInlineStyle = string.Format(" style=\"background-image: url({0});\"", src);
                }

                string strSumClass = "";
                if (leg.WithSum && leg.SumInData && i == 0)
                {
                    strSumClass = " class=\"sum\"";
                    strText = leg.SumText;
                } // End if (leg.WithSum && leg.SumInData && i == 0)


                if (b)
                    sb.AppendFormat(strRowTemplate, strSumClass, ColorClass, strText, strValue, leg.PdfUnit, strInlineStyle);
                else
                    sb.AppendFormat(strRowTemplate, strSumClass, ColorClass, strText, strValue, leg.HtmlUnit, strInlineStyle);

                // AP_LEG_Nr
                //--,AP_LEG_Mandant
                //--,AP_LEG_DWG

            } // Next dr

            string strHTML = sb.ToString();
            sb.Length = 0;
            sb = null;
            return strHTML;
        } // End Function GetRows



        public static string GetTemplateFile()
        {
            string strPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            strPath = System.IO.Path.Combine(strPath, "../../../HtmlToPdfWeb/MusterText.htm");
            strPath = System.IO.Path.GetFullPath(strPath);
            return strPath;
        }



        public static void RemoveWoutWareCommentNodes(System.Xml.XmlDocument doc)
        {
            System.Xml.XmlNamespaceManager nsmgr = SimpleHelper.GetDefaultNamespace(doc);

            System.Xml.XmlNodeList EvalVersionTags = doc
                .SelectNodes("//dft:text[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜÉÈÊÀÁÂÒÓÔÙÚÛÇÅÏÕÑŒ', 'abcdefghijklmnopqrstuvwxyzäöüéèêàáâòóôùúûçåïõñœ'),'aspose')]", nsmgr);


            // System.Xml.XmlNode xnProd = doc.SelectSingleNode("//comment()[contains(., 'Produced by application')]", nsmgr);
            // System.Xml.XmlNode xnWout = doc.SelectSingleNode("//comment()[contains(., 'woutware.com')]", nsmgr);

            System.Xml.XmlNode xnProd = doc
                .SelectSingleNode("//comment()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜÉÈÊÀÁÂÒÓÔÙÚÛÇÅÏÕÑŒ', 'abcdefghijklmnopqrstuvwxyzäöüéèêàáâòóôùúûçåïõñœ'),'produced by application')]", nsmgr);

            System.Xml.XmlNode xnWout = doc
                .SelectSingleNode("//comment()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜÉÈÊÀÁÂÒÓÔÙÚÛÇÅÏÕÑŒ', 'abcdefghijklmnopqrstuvwxyzäöüéèêàáâòóôùúûçåïõñœ'),'woutware.com')]", nsmgr);

            if (xnProd != null)
            {
                System.Console.WriteLine(xnProd.InnerText);
                xnProd.ParentNode.RemoveChild(xnProd);

            }

            if (xnWout != null)
            {
                System.Console.WriteLine(xnWout.InnerText);
                xnWout.ParentNode.RemoveChild(xnWout);
            }

            using (System.IO.FileStream fs = new System.IO.FileStream(@"d:\Test.xml", System.IO.FileMode.Create
                , System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                SimpleHelper.SaveDocument(doc, fs, true);
            }
         
        }


        // https://tex.stackexchange.com/questions/8260/what-are-the-various-units-ex-em-in-pt-bp-dd-pc-expressed-in-mm
        public enum unit
        {
            px, cm, mm, pt, @in, pc // pc = pica
            ,em, ex
            ,percent

            // https://www.w3.org/Style/Examples/007/units.en.html
            // https://www.w3schools.com/cssref/css_units.asp
            // em	Relative to the font-size of the element (2em means 2 times the size of the current font) Try it
            // ex	Relative to the x-height of the current font (rarely used) Try it
            // ch	Relative to width of the "0" (zero)
            // rem	Relative to font-size of the root element
            // vw	Relative to 1% of the width of the viewport
            // vh	Relative to 1% of the height of the viewport
            // vmin	Relative to 1% of viewport's smaller dimension
            // vmax	Relative to 1% of viewport's larger dimension
        }

        // * Pixels (px) are relative to the viewing device. 
        // For low-dpi devices, 1px is one device pixel (dot) of the display. 
        // For printers and high resolution screens 1px implies multiple device pixels.

        // 1 point = 0.352778 mm
        // 1 pica = 4.2333 mm
        // 1 inch = 25.4 mm




        // All coordinates and lengths in SVG can be specified with or without a unit identifier.
        public static void parseFloat(string input)
        {
            if (input == null)
                throw new System.ArgumentNullException("input");
            input = input.Trim();
            if (input == string.Empty)
                throw new System.ArgumentException("input");

            input = input.ToLowerInvariant();

            if (input.EndsWith("px"))
            { }

            if (input.EndsWith("mm"))
            { }

            if (input.EndsWith("cm"))
            { }

            if (input.EndsWith("pt")) // 1 inch = 72 points
            { }

            if (input.EndsWith("pc")) // 1 pica = 12 points =appx. 1/6 inch
            { }

            if (input.EndsWith("in")) // 1 inch = 72 points
            { }


            float f = float.Parse(input, System.Globalization.CultureInfo.InvariantCulture);

        }



        private void btnConvert_Click(object sender, System.EventArgs e)
        {
            //this.txtHtml.Text = System.IO.File.ReadAllText(GetTemplateFile(), System.Text.Encoding.Default);

            string filenName = @"EmbeddedSVG.html";
            filenName = @"D:\Stefan.Steiger\Documents\Visual Studio 2013\Projects\SVG\COR\0001_GB01_OG01_0000.svg";
            filenName = @"D:\Stefan.Steiger\Documents\Visual Studio 2013\Projects\SVG\COR\0001_GB01_OG14_0000_Aperture.svg";
            // D:\Stefan.Steiger\Documents\Visual Studio 2013\Projects\SVG\COR\0001_GB01_OG01_0000.svg {Width = 1297.0 Height = 803.0}

            filenName = @"D:\stefan.steiger\Downloads\file.htm";
            // <div id="SVG" style="display: block; width: 1324px; height: 967px; 


            // this.txtHtml.Text = System.IO.File.ReadAllText(filenName, System.Text.Encoding.Default);
            this.txtHtml.Text = System.IO.File.ReadAllText(filenName, System.Text.Encoding.UTF8);
            //System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            //doc.XmlResolver = null; // https://stackoverflow.com/questions/4445348/net-prevent-xmldocument-loadxml-from-retrieving-dtd
            //doc.LoadXml(this.txtHtml.Text);


            // <?xml version="1.0"?>
            //<!--Produced by application: DwgToSvgConverter 2015.4.2.1728-->
            //<!--Library: www.woutware.com WW.Cad 4.0.36.35-->
            //<!--Creation date: 04/02/2015 22:39:46-->

            //System.Xml.XmlAttribute attWidth = doc.DocumentElement.Attributes["width"];
            //System.Xml.XmlAttribute attHeight = doc.DocumentElement.Attributes["height"];
            //System.Xml.XmlAttribute attViewBox = doc.DocumentElement.Attributes["viewBox"];

            //string width = attWidth.Value;
            //string height = attHeight.Value;
            //string viewBox = attViewBox.Value;

            //System.Console.WriteLine(width);
            //System.Console.WriteLine(height);
            //System.Console.WriteLine(viewBox);
            //string[] viewBoxValues = viewBox.Split(' ');
            //System.Console.WriteLine(viewBoxValues);




            byte[] buf = Html2Pdf(this.txtHtml.Text);
            if (buf == null)
                return;

            string fn = System.IO.Path.GetTempFileName() + ".pdf";
            System.IO.File.WriteAllBytes(fn, buf);


            //using(System.IO.FileStream fs = new System.IO.FileStream(fn, System.IO.FileMode.Create))
            //{ 
            //    fs.Write(buf, 0, buf.Length);
            //    fs.Flush();
            //    fs.Close();
            //}

            System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
            myProcess.StartInfo.FileName = fn;
            myProcess.Start();
        } // End Sub btnConvert_Click btnConvert_Click


        public byte[] Html2Pdf(string strHTML)
        {

            string strDimX = this.txtDimX.Text;
            string strDimY = this.txtDimY.Text;

            int iDimX = 325;
            int iDimY = 500;

            if (!string.IsNullOrEmpty(strDimX))
                int.TryParse(strDimX, out iDimX);
            else
                this.txtDimX.Text = iDimX.ToString();

            if (!string.IsNullOrEmpty(strDimY))
                int.TryParse(strDimY, out iDimY);
            else
                this.txtDimY.Text = iDimY.ToString();

            return Html2Pdf(strHTML, iDimX, iDimY);
        }


        public byte[] Html2Pdf(string strHTML, int width, int height)
        {
            //wkHtmlToPdfSharpStatic.InitLib(false);
            // pc.FinishAction("Library initialized");

            SynchronizedwkHtmlToPdfSharp sc = new SynchronizedwkHtmlToPdfSharp(new GlobalConfig()
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
                //.SetPaperSize(new System.Drawing.Printing.PaperSize("lala", 800, 625))
                //.SetPaperSize(new System.Drawing.Printing.PaperSize("SvgSize", 1297 + 37, 803 + 37))
                //.SetPaperSize(new System.Drawing.Printing.PaperSize("SvgSize", 1297, 803))
                
                
                // Pixel
                .SetPaperSize(new System.Drawing.Printing.PaperSize("SvgSize", width + 37, height + 37))
                //.SetPaperSize(new System.Drawing.Printing.PaperSize("SvgSize", 1170, 827))


                // <svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="21cm" height="29.7cm" viewBox="0 0 2098 2969.3" version="1.1">

                // 29.7 cm = 11.69291
                // 21 cm = 8.26772


                //.SetPaperSize(new System.Drawing.Printing.PaperSize("lala", iDimX, iDimY))
            );

            // pc.FinishAction("Converter created");

            //sc.Begin += OnScBegin;
            //sc.Error += OnScError;
            //sc.Warning += OnScWarning;
            //sc.PhaseChanged += OnScPhase;
            //sc.ProgressChanged += OnScProgress;
            //sc.Finished += OnScFinished;

            //pc.FinishAction("Event handlers installed");



            byte[] buf = sc.Convert(new ObjectConfig()
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

            //pc.FinishAction("conversion finished");

            if (buf == null)
            {
                MessageBox.Show("Error converting!");
                return null;
            } // End if (buf == null)

            return buf;
        } // End Function Html2Pdf 



        private void txtHtml_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                this.txtHtml.SelectAll();
            } // End if (e.Control && e.KeyCode == Keys.A)

        }


        /*
        public static string GetImageAsString(System.Drawing.Imaging.ImageFormat format)
        {
            // string strFC = "#" + System.Convert.ToString(dr["AP_Leg_ForeColor"]);
            // string strBC = "#" + System.Convert.ToString(dr["AP_Leg_BackColor"]);
            // string strLC = "#" + System.Convert.ToString(dr["Html_LineColor"]);

            string strFC = "#" + System.Convert.ToString("FF0000");
            string strBC = "#" + System.Convert.ToString("0000FF");
            string strLC = "#000000";


            if ("#".Equals(strFC))
                strFC = "#000000";

            if ("#".Equals(strBC))
                strBC = "#000000";

            if ("#".Equals(strLC))
                strLC = "#000000";


            System.Drawing.Color colFC = System.Drawing.ColorTranslator.FromHtml(strFC);
            System.Drawing.Color colHB = System.Drawing.ColorTranslator.FromHtml(strBC);
            System.Drawing.Color colHL = System.Drawing.ColorTranslator.FromHtml(strLC);


            //pbRectangle.Image = SimpleHtmlToPdfConverter.Draw.DrawHatchRectangle_Img(9, colFC, colHB, colHL);
            // pbRectangle.Image = SimpleHtmlToPdfConverter.Draw.DrawHatchRectangle_Img(35, colFC, colHB, colHL);
            // pbRectangle.Image = SimpleHtmlToPdfConverter.Draw.DrawHatchRectangle_Img(69, colFC, colHB, colHL);

            string str = SimpleHtmlToPdfConverter.Draw.DrawHatchRectangle(9, colFC, colHB, colHL, format);

            return "data:" + GetMimeType(format) + ";base64," + str;
        }
        */

    } // End Class Form1 : Form


} // End Namespace MyHtmlToPdfConverter 
