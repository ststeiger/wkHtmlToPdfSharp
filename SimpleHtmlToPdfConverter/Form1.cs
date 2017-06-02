
using System;
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
            Console.WriteLine(num);


            string Heading = string.Format("{0} - {1} - {2} - {3}", leg.Location, leg.Premise, leg.Floor, leg.Title);
            Console.WriteLine(Heading);
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
                    Int32.TryParse(strAP_LEG_ForeColor, out AP_LEG_ForeColor);


                if (string.IsNullOrEmpty(strFgColor))
                    strFgColor = "#000000";

                if (StringComparer.Ordinal.Equals(strBgColor, "#"))
                    strBgColor = "#000000";

                if (StringComparer.Ordinal.Equals(strLineColor, "#"))
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


        private void btnConvert_Click(object sender, EventArgs e)
        {
            //this.txtHtml.Text = System.IO.File.ReadAllText(GetTemplateFile(), System.Text.Encoding.Default);

            string filenName = @"EmbeddedSVG.html";
            filenName = @"D:\Stefan.Steiger\Documents\Visual Studio 2013\Projects\SVG\COR\0001_GB01_OG01_0000.svg";
            // D:\Stefan.Steiger\Documents\Visual Studio 2013\Projects\SVG\COR\0001_GB01_OG01_0000.svg {Width = 1297.0 Height = 803.0}


            // this.txtHtml.Text = System.IO.File.ReadAllText(filenName, System.Text.Encoding.Default);
            this.txtHtml.Text = System.IO.File.ReadAllText(filenName, System.Text.Encoding.UTF8);
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(this.txtHtml.Text);




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
            //wkHtmlToPdfSharpStatic.InitLib(false);

            // pc.FinishAction("Library initialized");

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



            // D:\Stefan.Steiger\Documents\Visual Studio 2013\Projects\SVG\COR\0001_GB01_OG01_0000.svg {Width = 1297.0 Height = 803.0}



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
                .SetPaperSize(new System.Drawing.Printing.PaperSize("SvgSize", 1297, 803))
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
