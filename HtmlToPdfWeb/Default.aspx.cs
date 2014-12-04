
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using wkHtmlToPdfSharp;
using wkHtmlToPdfSharp.Synchronized;


namespace HtmlToPdfWeb
{


    public partial class _Default : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string str = Server.MapPath("~/App_Data/footer_base64_invest.txt");
                this.txtHtml.Text = System.IO.File.ReadAllText(str, System.Text.Encoding.Default);
            }
        }


        public static byte[] Html2Pdf(string strHTML)
        {

            //wkHtmlToPdfSharpStatic.InitLib(false);

            // pc.FinishAction("Library initialized");

            SynchronizedwkHtmlToPdfSharp sc = new SynchronizedwkHtmlToPdfSharp(new GlobalConfig()
                .SetMargins(new System.Drawing.Printing.Margins(0, 0, 0, 0))
                .SetDocumentTitle("Ololo")
                .SetCopyCount(1)
                .SetImageQuality(50)
                .SetLosslessCompression(true)
                .SetMaxImageDpi(20)
                .SetOutlineGeneration(true)
                .SetOutputDpi(1200)
                .SetPaperOrientation(true)
                .SetPaperSize(250, 200)
                //.SetPaperSize(PaperKind.Letter)
            );

            // pc.FinishAction("Converter created");

            //sc.Begin += OnScBegin;
            //sc.Error += OnScError;
            //sc.Warning += OnScWarning;
            //sc.PhaseChanged += OnScPhase;
            //sc.ProgressChanged += OnScProgress;
            //sc.Finished += OnScFinished;

            //pc.FinishAction("Event handlers installed");

            byte[] buf = sc.Convert(new ObjectConfig().SetPrintBackground(true), strHTML);

            /*
            sc.Convert(new ObjectConfig().SetPrintBackground(true).SetProxyString("http://localhost:8888")
                .SetAllowLocalContent(true).SetCreateExternalLinks(false).SetCreateForms(false).SetCreateInternalLinks(false)
                .SetErrorHandlingType(ObjectConfig.ContentErrorHandlingType.Ignore).SetFallbackEncoding(System.Text.Encoding.ASCII)
                .SetIntelligentShrinking(false).SetJavascriptDebugMode(true).SetLoadImages(true).SetMinFontSize(16)
                .SetRenderDelay(2000).SetRunJavascript(true).SetIncludeInOutline(true).SetZoomFactor(2.2), strHTML);
            */

            //pc.FinishAction("conversion finished");

            if (buf == null)
            {
                throw new Exception("Error converting!");
                //return null;
            }

            return buf;
        }


        protected void btnConvert_Click(object sender, EventArgs e)
        {
            byte[] buf = Html2Pdf(this.txtHtml.Text);

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
        }



    }




}
