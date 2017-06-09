
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Portal.ASP
{


	public class NET
	{


		public static string GetParameter(string strRequestedKey)
		{
			string strValue = null;
			if (StringComparer.OrdinalIgnoreCase.Equals(HttpContext.Current.Request.HttpMethod, "GET")) {
                return HttpContext.Current.Request.QueryString[strRequestedKey];
            }
            else if (StringComparer.OrdinalIgnoreCase.Equals(HttpContext.Current.Request.HttpMethod, "POST"))
            {
                return HttpContext.Current.Request.Form[strRequestedKey];
            }
            else
            {
                throw new System.Web.HttpException(500, "Invalid request method");
            }

			return null;
		} // End Function GetParameter


		// COR.ASP.NET.StripInvalidPathChars("")
        public static string StripInvalidPathChars(string str)
        {
            string strReturnValue = null;

            if (str == null)
            {
                return strReturnValue;
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            char[] achrInvalidPathChars = System.IO.Path.GetInvalidPathChars();


            foreach (char cThisChar in str)
            {
                bool bIsValid = true;

                foreach (char cInvalid in achrInvalidPathChars)
                {
                    if (cThisChar == cInvalid)
                    {
                        bIsValid = false;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }

                if (bIsValid)
                {
                    sb.Append(cThisChar);
                }
            }

            strReturnValue = sb.ToString();
            sb = null;
            return strReturnValue;
        } // End Function StripInvalidPathChars


		public static string GetContentDisposition(string strFileName)
		{
			return GetContentDisposition(strFileName, "attachment");
		} // End Function GetContentDisposition


		// http://www.iana.org/assignments/cont-disp/cont-disp.xhtml
        public static string GetContentDisposition(string strFileName, string strDisposition)
        {
            // http://stackoverflow.com/questions/93551/how-to-encode-the-filename-parameter-of-content-disposition-header-in-http
            string contentDisposition = null;
            strFileName = StripInvalidPathChars(strFileName);

            if (string.IsNullOrEmpty(strDisposition))
            {
                strDisposition = "inline";
            }

            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request.Browser != null)
            {
                if ((System.Web.HttpContext.Current.Request.Browser.Browser == "IE" & (System.Web.HttpContext.Current.Request.Browser.Version == "7.0" | System.Web.HttpContext.Current.Request.Browser.Version == "8.0")))
                {
                    contentDisposition = strDisposition + "; filename=" + Uri.EscapeDataString(strFileName).Replace("'", Uri.HexEscape('\''));
                }
                else if ((System.Web.HttpContext.Current.Request.Browser.Browser == "Safari"))
                {
                    contentDisposition = strDisposition + "; filename=" + strFileName;
                }
                else
                {
                    contentDisposition = strDisposition + "; filename*=UTF-8''" + Uri.EscapeDataString(strFileName);
                }
            }
            else
            {
                contentDisposition = strDisposition + "; filename*=UTF-8''" + Uri.EscapeDataString(strFileName);
            }

            return contentDisposition;
        } // End Function GetContentDisposition


		public static void DownloadFile(string strFileName, byte[] Buffer)
		{
			DownloadFile(strFileName, Buffer, true);
		}
		// DownloadFile


		public static void DownloadFile(string strFileName, byte[] Buffer, bool EndRequest)
		{
			string strDefaultDisposition = "attachment";

			DownloadFile(strFileName, strDefaultDisposition, Buffer, EndRequest);
		} // End Sub DownloadFile


		public static void DownloadFile(string strFileName, string strDisposition, byte[] Buffer, bool EndRequest)
		{
			DownloadFile(strFileName, strDisposition, "application/pdf", Buffer, EndRequest);
		} // End Sub DownloadFile


		public static void DownloadFile(string strFileName, string strDisposition, string strMime, byte[] Buffer)
		{
			DownloadFile(strFileName, strDisposition, strMime, Buffer, true);
		} // End Sub DownloadFile


        public static void DownloadFile(string strFileName, string strDisposition, string strMime, byte[] Buffer, bool EndRequest)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.ClearContent();

            //System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", GetContentDisposition(strFileName))
            string strDispo = GetContentDisposition(strFileName, strDisposition);
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", strDispo);

            if (Buffer == null)
            {
                System.Web.HttpContext.Current.Response.AddHeader("Content-Length", "0");
            }
            else
            {
                System.Web.HttpContext.Current.Response.AddHeader("Content-Length", Buffer.Length.ToString());
            }



            // http://superuser.com/questions/219870/how-to-open-pdf-in-chromes-integrated-viewer-without-downloading-it#
            System.Web.HttpContext.Current.Response.ContentType = strMime;
            //  "application/octet-stream"

            //System.Web.HttpContext.Current.Response.TransmitFile(File.FullName)
            System.Web.HttpContext.Current.Response.BinaryWrite(Buffer);
            System.Web.HttpContext.Current.Response.Flush();

            if (EndRequest)
            {
                System.Web.HttpContext.Current.Response.End();
            }

        } // End Sub DownloadFile


	} // End Class NET


} // End Namespace COR.ASP
