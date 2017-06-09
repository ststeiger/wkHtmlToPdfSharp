
namespace Svg2Pdf.ajax
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für PdfExport
    /// </summary>
    public class PdfExport : System.Web.IHttpHandler
    {

        public void ProcessRequest(System.Web.HttpContext context)
        {

            // format
            // orientation
            // color
            // with_legend

            // current_view
            // adjust_to_page
            // scale 
            //   -  1_50

            string format = context.Request.QueryString["format"];
            string orientation = context.Request.QueryString["orientation"];
            string color = context.Request.QueryString["color"];
            string with_legend = context.Request.QueryString["with_legend"];

            string s_current_view = context.Request.QueryString["current_view"];
            string s_adjust_to_page = context.Request.QueryString["adjust_to_page"];
            string scale = context.Request.QueryString["scale"];


            context.Response.ContentType = "application/pdf";

            s_adjust_to_page = "true";
            bool adjust_to_page = false;
            if (!bool.TryParse(s_adjust_to_page, out adjust_to_page))
            {
                throw new System.ArgumentException("adjust_to_page is not a valid boolean");
            }


            s_current_view = "true";
            bool current_view = false;
            if (!bool.TryParse(s_current_view, out current_view))
            {
                throw new System.ArgumentException("current_view is not a valid boolean");
            }




            // string svg = System.IO.File.ReadAllText(@"D:\stefan.steiger\Downloads\1496756043764.svg", System.Text.Encoding.UTF8);
            string svg = System.IO.File.ReadAllText(@"D:\stefan.steiger\Downloads\1496760599759.svg", System.Text.Encoding.UTF8);

            byte[] buf = Svg2Pdf.PdfExport.SvgToPdf(svg, current_view);
            Portal.ASP.NET.DownloadFile("myfile.pdf", "inline", buf, false);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}