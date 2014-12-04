
using System;
using System.Collections.Generic;
using System.Text;


namespace SimpleHtmlToPdfConverter
{


    public class SQL
    {

        public static System.Data.DataTable GetDataTable(string strSQL)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();
            csb.DataSource = "CORDB2008R2";
            csb.InitialCatalog = "COR_Basic_Sursee";
            csb.InitialCatalog = "SwissRe_Test_V3";

            csb.IntegratedSecurity = true;

            System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(strSQL, csb.ConnectionString);
            da.Fill(dt);

            return dt;
        } // End Function GetDataTable

    }

    public class xxx
    {

        public void Farben()
        {
            string strSQL = @"
SELECT 
	 COL_Aperture 
	,COL_Hex 
	,COL_Status 
FROM T_SYS_ApertureColorToHex 
;";


            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            using (System.Data.DataTable dt = SQL.GetDataTable(strSQL))
            {

                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    string strApertureNumber = System.Convert.ToString(dr["COL_Aperture"]);
                    string strHexCode = "#" + System.Convert.ToString(dr["COL_Hex"]);

                    sb.AppendLine(".Color" + strApertureNumber + " { background-color: " + strHexCode + "; }");
                } // Next dr 

            } // End Using dt


            string str = sb.ToString();
            sb.Length = 0;
            sb = null;

            Console.WriteLine(str);
        } // End Sub Farben


    } // End Class SQL


}
