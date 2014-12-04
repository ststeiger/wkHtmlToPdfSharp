
using System;
using System.Collections.Generic;
using System.Text;


namespace SimpleHtmlToPdfConverter
{


    public class Legende : IDisposable
    {
        protected System.Data.DataTable m_LegendenSettings;
        protected System.Data.DataTable m_LegendenDaten;
        protected System.Data.DataTable m_MandantData;
        protected System.Data.DataTable m_dtLocationInfo;

        protected string m_Stylizer;
        protected string m_DWG;
        protected int m_User;
        protected bool m_ForPDF;

        protected bool? m_HasFlaeche;
        protected bool? m_HasAnzahl;
        protected bool? m_WithSum;
        protected bool? m_SumInData;
        protected bool? m_bIsSwissRe;

        protected string m_Mandant;
        protected string m_Language;
        protected string m_DataSourceEntity;
        protected string m_TextField;
        protected string m_ValueField;

        protected System.Globalization.CultureInfo m_UserCulture;
        private static System.Globalization.NumberFormatInfo s_SwissNumberFormat = CreateSwissNumberFormatInfo();
        protected string m_NumberFormat;
        protected string m_ValueTitle;
        protected string m_HtmlMeasurementUnit;
        protected string m_PdfMeasurementUnit;
        protected string m_HtmlUnitString;
        protected string m_PdfUnitString;

        protected string m_Title;
        protected string m_SumText;


        private static System.Globalization.NumberFormatInfo CreateSwissNumberFormatInfo()
        {
            //System.Globalization.NumberFormatInfo nfi = (System.Globalization.NumberFormatInfo)System.Globalization.CultureInfo.InvariantCulture.NumberFormat.Clone();
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberGroupSeparator = "'";
            nfi.NumberDecimalSeparator = ".";

            nfi.CurrencyGroupSeparator = "'";
            nfi.CurrencyDecimalSeparator = ".";
            nfi.CurrencySymbol = "CHF";

            return nfi;
        } // End Function SetupNumberFormatInfo


        public Legende()
        {
            this.m_Stylizer = "Nutzungsart";
            this.m_Stylizer = "Space Type";
            this.m_DWG = "0001_GB11_OG01_0000";
            this.m_DWG = "ADSW_3";
            this.m_User = 0;
        } // End Constructor


        public Legende(string stylizer, string dwg, bool forPDF)
        {
            this.m_Stylizer = stylizer;
            this.m_DWG = dwg;
            this.m_User = 0;
            this.m_ForPDF = forPDF;
        } // End Constructor


        public Legende(string stylizer, string dwg, bool forPDF, int Benutzer)
        {
            this.m_Stylizer = stylizer;
            this.m_DWG = dwg;
            this.m_User = Benutzer;
            this.m_ForPDF = forPDF;
        } // End Constructor


        protected void SelfDestruct()
        {
            if (this.m_LegendenDaten != null)
                this.m_LegendenDaten.Dispose();

            if (this.m_LegendenSettings != null)
                this.m_LegendenSettings.Dispose();

            if (this.m_MandantData != null)
                this.m_MandantData.Dispose();
        } // End SelfDestruct


        ~Legende()
        {
            SelfDestruct();
        } // End Destructor 


        // http://www.codeproject.com/Articles/15360/Implementing-IDisposable-and-the-Dispose-Pattern-P
        public void Dispose(bool disposing)
        {
            SelfDestruct();
        } // End Sub Dispose


        public void Dispose()
        {
            // Perform any object clean up here.
            Dispose(true);
            // If you are inheriting from another class that
            // also implements IDisposable, don't forget to
            // call base.Dispose() as well.
        } // End Sub Dispose


        public string Language
        {
            get
            {
                if (this.m_Language != null)
                    return this.m_Language;

                try
                {
                    if (this.m_User == 0)
                    {
                        if (StringComparer.OrdinalIgnoreCase.Equals(this.Mandant, "SwissRe"))
                            this.m_Language = "EN";
                        else
                            this.m_Language = "DE";
                    }
                    else
                        this.m_Language = "DE";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

                if (string.IsNullOrEmpty(this.m_Language))
                {
                    this.m_Language = "DE";
                }

                this.m_Language = this.m_Language.ToUpper();
                return this.m_Language;
            }
        } // End Property Language



        public string DataSourceEntity
        {

            get
            {
                if (this.m_DataSourceEntity != null)
                    return this.m_DataSourceEntity;

                if (LegendenSettings != null && LegendenSettings.Rows.Count > 0)
                    this.m_DataSourceEntity = System.Convert.ToString(LegendenSettings.Rows[0]["LY_Legende_View"]);

                if (string.IsNullOrEmpty(m_DataSourceEntity))
                    throw new Exception("LY_Legende_View not set.");

                if (this.m_DataSourceEntity.StartsWith("tfu", StringComparison.OrdinalIgnoreCase))
                    this.m_DataSourceEntity = "[" + this.m_DataSourceEntity.Replace("[", "[[" + "]") + "](' + " + this.m_DWG.Replace("'", "''") + " +')";
                else
                    this.m_DataSourceEntity = "[" + this.m_DataSourceEntity.Replace("[", "[[" + "]") + "]";

                return this.m_DataSourceEntity;
            }

        } // End Property DataSourceEntity



        public string TextField
        {

            get
            {
                if (this.m_TextField != null)
                    return this.m_TextField;

                try
                {
                    this.m_TextField = System.Convert.ToString(LegendenSettings.Rows[0]["LY_Legende_TextField"]);

                    if (this.m_TextField.EndsWith("DE", StringComparison.OrdinalIgnoreCase))
                    {
                        this.m_TextField = this.m_TextField.Substring(0, this.m_TextField.Length - 2);
                        this.m_TextField += this.Language;
                    } // End if (strTextField.EndsWith("DE", StringComparison.OrdinalIgnoreCase))

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }


                if (string.IsNullOrEmpty(this.m_TextField))
                {
                    this.m_TextField = "AP_LEG_NutzungDE";
                } // End if (string.IsNullOrEmpty(strTextField))

                return this.m_TextField;
            }

        } // End Property TextField


        public string ValueField
        {
            get
            {
                if (this.m_ValueField != null)
                    return this.m_ValueField;


                try
                {
                    this.m_ValueField = System.Convert.ToString(LegendenSettings.Rows[0]["LY_Legende_ValueField"]);
                    this.m_ValueField = "ROUND(MainView." + this.m_ValueField + ", 2)";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }


                if (string.IsNullOrEmpty(this.m_ValueField))
                {
                    if (this.HasFlaeche)
                    {

                        if (StringComparer.OrdinalIgnoreCase.Equals(this.Mandant, "SwissRe"))
                            this.m_ValueField = "ROUND(MainView.AP_Leg_Area, 2)";
                        else
                            this.m_ValueField = "ROUND(MainView.AP_Leg_Flaeche, 2)";

                    }

                    if (this.HasAnzahl)
                    {
                        if (StringComparer.OrdinalIgnoreCase.Equals(this.Mandant, "SwissRe"))
                            this.m_ValueField = "MainView.AP_LEG_Count";
                        else
                            this.m_ValueField = "MainView.AP_Leg_Anzahl";
                    }

                }


                if (string.IsNullOrEmpty(this.m_ValueField))
                {
                    if (StringComparer.OrdinalIgnoreCase.Equals(this.Mandant, "SwissRe"))
                        this.m_ValueField = "ROUND(MainView.AP_Leg_Area, 2)";
                    else
                        this.m_ValueField = "ROUND(MainView.AP_Leg_Flaeche, 2)";
                }

                return this.m_ValueField;
            }

        } // End Property ValueField



        public bool HasFlaeche
        {
            get
            {
                if (this.m_HasFlaeche.HasValue)
                    return this.m_HasFlaeche.Value;

                if (LegendenSettings != null && LegendenSettings.Rows.Count > 0)
                    this.m_HasFlaeche = System.Convert.ToBoolean(LegendenSettings.Rows[0]["LY_Legende_HasFlaeche"]);
                else
                    this.m_HasFlaeche = false;

                return this.m_HasFlaeche.Value;
            }
        } // End Property HasFlaeche 


        public bool HasAnzahl
        {
            get
            {
                if (this.m_HasAnzahl.HasValue)
                    return this.m_HasAnzahl.Value;

                if (LegendenSettings != null && LegendenSettings.Rows.Count > 0)
                    this.m_HasAnzahl = System.Convert.ToBoolean(LegendenSettings.Rows[0]["LY_Legende_HasAnzahl"]);
                else
                    this.m_HasAnzahl = false;

                return this.m_HasAnzahl.Value;
            }
        } // End Property HasAnzahl


        public string NumberFormat
        {
            get
            {
                if (this.m_NumberFormat != null)
                    return this.m_NumberFormat;

                if (this.HasAnzahl)
                    this.m_NumberFormat = "N0";
                else if (this.HasFlaeche)
                    this.m_NumberFormat = "N2";
                else if (this.HasFlaeche)
                    this.m_NumberFormat = "N0";

                return this.m_NumberFormat;
            }
        } // End Property NumberFormat


        public string FormatNumber(double dblQuantity)
        {
            return FormatNumber(dblQuantity, this.NumberFormat);
        } // End Function FormatNumber


        public string FormatNumber(double dblQuantity, IFormatProvider provider)
        {
            return FormatNumber(dblQuantity, this.NumberFormat);
        } // End Function FormatNumber


        public string FormatNumber(double dblQuantity, string Format)
        {

            return FormatNumber(dblQuantity, Format, s_SwissNumberFormat);
        }


        public string FormatNumber(double dblQuantity, string Format, IFormatProvider provider)
        {
            // http://msdn.microsoft.com/en-us/library/system.globalization.numberformatinfo%28VS.71%29.aspx

            //double dblQuantity = -123456789.123456789;
            //string strFormattedInteger = dblQuantity.ToString("N0", m_nfi);
            //strFormattedInteger = string.Format(m_nfi, "{0:N0}", dblQuantity);

            return dblQuantity.ToString(Format, provider);
        } // End Function FormatNumber



        public string ValueTitle
        {
            get
            {
                if (this.m_ValueTitle != null)
                    return this.m_ValueTitle;

                if (this.HasFlaeche)
                {
                    switch (this.Language)
                    {
                        case "FR":
                            this.m_ValueTitle = "Surface";
                            break;
                        case "IT":
                            this.m_ValueTitle = "Superficie";
                            break;
                        case "EN":
                            this.m_ValueTitle = "Area";
                            break;
                        default:
                            this.m_ValueTitle = "Fläche";
                            break;
                    } // End switch(this.Language)

                }

                if (this.HasAnzahl)
                {
                    switch (this.Language)
                    {
                        case "FR":
                            this.m_ValueTitle = "Nombre";
                            break;
                        case "IT":
                            this.m_ValueTitle = "Quantità";
                            break;
                        case "EN":
                            this.m_ValueTitle = "Count";
                            break;
                        default:
                            this.m_ValueTitle = "Anzahl";
                            break;
                    } // End switch(this.Language)

                }

                if (string.IsNullOrEmpty(this.m_ValueTitle))
                {
                    this.m_ValueTitle = "Undefined";
                }

                return this.m_ValueTitle;
            }
        } // End Property ValueTitle


        public string HtmlUnit
        {
            get
            {
                if (!string.IsNullOrEmpty(this.m_HtmlUnitString))
                    return m_HtmlUnitString;

                this.m_HtmlUnitString = "m&sup2;";
                m_HtmlUnitString = System.Web.HttpUtility.HtmlDecode(HtmlUnitHtmlEncoded);

                return this.m_HtmlUnitString;
            }
        } // End Property HtmlUnit


        public string HtmlUnitHtmlEncoded
        {
            get
            {
                if (!string.IsNullOrEmpty(this.m_HtmlMeasurementUnit))
                    return m_HtmlMeasurementUnit;

                this.m_HtmlMeasurementUnit = "m&sup2;";

                return this.m_HtmlMeasurementUnit;
            }
        } // End Property HtmlUnitHtmlEncoded


        public string PdfUnit
        {
            get
            {
                if (this.m_PdfUnitString != null)
                    return this.m_PdfUnitString;

                const string m2 = " m²";

                if (this.HasFlaeche)
                    this.m_PdfUnitString = m2;

                if (this.HasAnzahl)
                    this.m_PdfUnitString = "";


                if (string.IsNullOrEmpty(this.m_PdfUnitString))
                    this.m_PdfUnitString = m2;

                return this.m_PdfUnitString;
            }
        } // End Property PdfUnit


        public string PdfUnitHtmlEncoded
        {
            get
            {
                if (m_PdfMeasurementUnit != null)
                    return this.m_PdfMeasurementUnit;

                if (this.HasFlaeche)
                    this.m_PdfMeasurementUnit = "m&nbsp;&sup2;";

                if (this.HasAnzahl)
                    this.m_PdfMeasurementUnit = "";

                if (string.IsNullOrEmpty(this.m_PdfMeasurementUnit))
                    m_PdfMeasurementUnit = "m&nbsp;&sup2;";

                return this.m_PdfMeasurementUnit;
            }
        } // End Property PdfUnitHtmlEncoded


        public string Mandant
        {
            get
            {
                if (this.m_Mandant != null)
                    return this.m_Mandant;

                if (this.MandantData != null && this.MandantData.Rows.Count > 0)
                    this.m_Mandant = System.Convert.ToString(this.MandantData.Rows[0]["MDT_Lang_DE"]);

                if (string.IsNullOrEmpty(m_Mandant))
                    this.m_Mandant = "Global";

                return this.m_Mandant;
            }
        } // End Property Mandant


        public bool IsSwissRe
        {
            get
            {
                if (this.m_bIsSwissRe.HasValue)
                    return this.m_bIsSwissRe.Value;

                if (StringComparer.OrdinalIgnoreCase.Equals(this.Mandant, "SwissRe"))
                    this.m_bIsSwissRe = true;
                else
                    this.m_bIsSwissRe = false;

                return this.m_bIsSwissRe.Value;
            }
        } // End Property IsSwissRe


        public System.Data.DataTable MandantData
        {
            get
            {
                if (this.m_MandantData != null)
                    return m_MandantData;

                string strSQL = @"
SELECT 
	 T_AP_Ref_Mandant.MDT_Kurz_DE 
	,T_AP_Ref_Mandant.MDT_Lang_DE 
	,T_AP_Ref_Mandant.MDT_Logofile
	
	,T_AP_Ref_Mandant_Logo.LOGO_Code

	,T_AP_Ref_Mandant_Logo.LOGO_Kurz_DE
	,T_AP_Ref_Mandant_Logo.LOGO_Kurz_FR
	,T_AP_Ref_Mandant_Logo.LOGO_Kurz_IT
	,T_AP_Ref_Mandant_Logo.LOGO_Kurz_EN

	,T_AP_Ref_Mandant_Logo.LOGO_Lang_DE
	,T_AP_Ref_Mandant_Logo.LOGO_Lang_FR
	,T_AP_Ref_Mandant_Logo.LOGO_Lang_IT
	,T_AP_Ref_Mandant_Logo.LOGO_Lang_EN
	
	,T_AP_Ref_Mandant_Logo.LOGO_Width
	,T_AP_Ref_Mandant_Logo.LOGO_Height
	,T_AP_Ref_Mandant_Logo.LOGO_PaddingLeft
	,T_AP_Ref_Mandant_Logo.LOGO_PaddingRight
	,T_AP_Ref_Mandant_Logo.LOGO_PaddingTop
	,T_AP_Ref_Mandant_Logo.LOGO_PaddingBottom
	,T_AP_Ref_Mandant_Logo.LOGO_Hide
FROM T_AP_Ref_Mandant

LEFT JOIN T_AP_Ref_Mandant_Logo
	ON T_AP_Ref_Mandant_Logo.LOGO_MDT_Lang_DE = T_AP_Ref_Mandant.MDT_Lang_DE
	AND T_AP_Ref_Mandant_Logo.LOGO_Status = 1 
	AND GETDATE() BETWEEN T_AP_Ref_Mandant_Logo.LOGO_DatumVon AND T_AP_Ref_Mandant_Logo.LOGO_DatumBis 
	
WHERE (1=1) 
AND T_AP_Ref_Mandant.MDT_ID = 0 
";


                this.m_MandantData = SQL.GetDataTable(strSQL);
                return this.m_MandantData;
            } // End Get of  LegendenSettings

        } // End Property LegendenSettings


        public System.Data.DataTable LegendenSettings
        {
            get
            {
                if (this.m_LegendenSettings != null)
                    return this.m_LegendenSettings;

                string strSQL = @"
SELECT 
	 LY_UID
	,LY_Code
	,LY_Lang_" + this.Language + @" AS LY_Lang 
	,LY_Sort
	,LY_Legende_View
	,LY_Legende_TextField
	,LY_Legende_ValueField 
	,LY_Legende_HasFlaeche
	,LY_Legende_HasAnzahl
	,LY_Legende_HasLanguage
FROM T_SYS_Ref_Layerset 

WHERE LY_Status = 1 
AND LY_Code = '" + this.m_Stylizer.Replace("'", "''") + @"'
";


                this.m_LegendenSettings = SQL.GetDataTable(strSQL);
                return this.m_LegendenSettings;
            } // End Get of  LegendenSettings

        } // End Property LegendenSettings


        public string Title
        {
            get
            {
                if (this.m_Title != null)
                    return this.m_Title;

                if (LegendenSettings != null && LegendenSettings.Rows.Count > 0)
                {
                    this.m_Title = System.Convert.ToString(LegendenSettings.Rows[0]["LY_Lang"]);
                }

                if (string.IsNullOrEmpty(this.m_Title))
                    this.m_Title = "Unbekannte Legende";

                return this.m_Title;
            }

        } // End Property Titel


        public string SumText
        {
            get
            {
                if (this.m_SumText != null)
                    return this.m_SumText;

                switch (this.Language)
                {
                    case "FR":
                        this.m_SumText = "Somme";
                        break;
                    case "IT":
                        this.m_SumText = "Somma";
                        break;
                    case "EN":
                        this.m_SumText = "Total";
                        break;
                    default:
                        this.m_SumText = "Summe";
                        break;
                } // End switch(this.Language)

                if (string.IsNullOrEmpty(this.m_SumText))
                    this.m_SumText = "DE";

                return this.m_SumText;
            }

        } // End Property SumText


        public bool WithSum
        {
            get
            {
                if (this.m_WithSum.HasValue)
                    return m_WithSum.Value;

                // string str = null;
                // if (this.LegendenDaten != null && this.LegendenDaten.Rows != null)
                //     str = System.Convert.ToString(this.LegendenDaten.Rows[0]["Text"]);

                this.m_WithSum = true;

                return m_WithSum.Value;
            }
        } // End Property WithSum


        public bool SumInData
        {
            get
            {
                if (m_SumInData.HasValue)
                    return m_SumInData.Value;

                this.m_SumInData = false;

                string strSumInData = null;

                if (LegendenDaten != null && LegendenDaten.Rows.Count > 0)
                    strSumInData = System.Convert.ToString(LegendenDaten.Rows[0]["Text"]);

                if (
                       StringComparer.OrdinalIgnoreCase.Equals(strSumInData, "Total")
                    || StringComparer.OrdinalIgnoreCase.Equals(strSumInData, "Totale")
                    || StringComparer.OrdinalIgnoreCase.Equals(strSumInData, "Summe")
                    || StringComparer.OrdinalIgnoreCase.Equals(strSumInData, "Sum")
                    || StringComparer.OrdinalIgnoreCase.Equals(strSumInData, "Summ")
                    || StringComparer.OrdinalIgnoreCase.Equals(strSumInData, "Somme")
                    || StringComparer.OrdinalIgnoreCase.Equals(strSumInData, "Somma")

                    )
                    this.m_SumInData = true;
                else
                    this.m_SumInData = false;

                return this.m_SumInData.Value;
            }

        } // End Property SumInData


        public double Sum
        {
            get
            {
                double sum = 0;

                if (SumInData)
                {
                    sum = System.Convert.ToDouble(LegendenDaten.Rows[0]["Value"]);
                    return sum;
                } // End if (SumInData)


                for (int i = 0; i < LegendenDaten.Rows.Count; ++i)
                {
                    sum += System.Convert.ToDouble(LegendenDaten.Rows[i]["Value"]);
                } // Next i

                return sum;
            }

        } // End Property Sum


        public string Location
        {
            get
            {
                if (this.IsSwissRe)
                    return GetLocationField("LC_Lang_en");

                throw new NotImplementedException("Non-SwissRe-Location");
            }

        } // End Property Location


        public string Premise
        {
            get
            {
                if (this.IsSwissRe)
                    return GetLocationField("PR_Name");

                throw new NotImplementedException("Non-SwissRe-Premise");
            }

        } // End Property Premise


        public string Floor
        {
            get
            {
                if (this.IsSwissRe)
                    return GetLocationField("FloorDisplayString");

                throw new NotImplementedException("Non-SwissRe-Premise");
            }
        } // End Property Floor


        public System.Globalization.CultureInfo UserCulture
        {
            get
            {
                if (m_UserCulture != null)
                    return m_UserCulture;

                if (this.IsSwissRe)
                    this.m_UserCulture = new System.Globalization.CultureInfo("en-US");

                if (StringComparer.OrdinalIgnoreCase.Equals(this.Language, "FR"))
                    this.m_UserCulture = new System.Globalization.CultureInfo("fr-CH");
                else if (StringComparer.OrdinalIgnoreCase.Equals(this.Language, "IT"))
                    this.m_UserCulture = new System.Globalization.CultureInfo("it-CH");
                else if (StringComparer.OrdinalIgnoreCase.Equals(this.Language, "EN"))
                    this.m_UserCulture = new System.Globalization.CultureInfo("en-US");
                else
                    this.m_UserCulture = new System.Globalization.CultureInfo("de-CH");

                return this.m_UserCulture;
            }
        } // End Property UserCulture 


        public string FormatDateTime(string Format)
        {
            return FormatDateTime(System.DateTime.Now, Format);
        } // End Function FormatDateTime 


        public string FormatDateTime(System.DateTime dt, string Format)
        {
            return dt.ToString(Format, this.UserCulture);
        } // End Function FormatDateTime 


        public string GetLocationField(string FieldName)
        {
            string strRetValue = "";

            if (this.LocationInfo != null && this.LocationInfo.Rows.Count > 0)
                strRetValue = System.Convert.ToString(this.LocationInfo.Rows[0][FieldName]);

            return strRetValue;
        } // End Function GetLocationField 


        public System.Data.DataTable LocationInfo
        {
            get
            {
                if (this.m_dtLocationInfo != null)
                    return this.m_dtLocationInfo;

                string strSQL = null;

                if (this.IsSwissRe)
                {
                    strSQL = @"
SELECT TOP 1 
	 T_Ref_Location.LC_Lang_en
	 
	,T_Premises.PR_Name
	
	,ISNULL(T_Ref_FloorType.FT_Lang_en + ' ', '') + CAST(T_Floor.FL_Level AS varchar(10)) AS FloorDisplayString  
	,T_Ref_FloorType.FT_Lang_en
	,T_Floor.FL_Level
	,T_Floor.FL_Sort
	 
	,T_ZO_Premises_DWG.ZO_PRDWG_ApertureDWG
	,T_ZO_Premises_DWG.ZO_PRDWG_ApertureObjID
	 
	,T_ZO_Floor_DWG.ZO_FLDWG_ApertureDWG
	,T_ZO_Floor_DWG.ZO_FLDWG_ApertureObjID
	,T_ZO_Floor_Area.ZO_FLArea_Area
FROM T_Ref_Location 

LEFT JOIN T_Ref_Country
	ON T_Ref_Country.CTR_UID = T_Ref_Location.LC_CTR_UID 
	AND T_Ref_Country.CTR_Status = 1 
	
LEFT JOIN T_Ref_Region
	ON T_Ref_Region.RG_UID = T_Ref_Country.CTR_RG_UID 
	AND T_Ref_Region.RG_Status = 1 
	
LEFT JOIN T_Premises 
	ON T_Premises.PR_LC_UID = LC_UID 
	AND T_Premises.PR_Status = 1 
	AND {fn curdate()} BETWEEN T_Premises.PR_DateFrom AND T_Premises.PR_DateTo 
	
LEFT JOIN T_ZO_Premises_DWG
	ON T_ZO_Premises_DWG.ZO_PRDWG_PR_UID = PR_UID 
	AND T_ZO_Premises_DWG.ZO_PRDWG_Status = 1 
	AND {fn curdate()} BETWEEN  T_ZO_Premises_DWG.ZO_PRDWG_DateFrom AND T_ZO_Premises_DWG.ZO_PRDWG_DateTo 
	
LEFT JOIN T_Floor 
	ON T_Floor.FL_PR_UID = PR_UID 
	AND T_Floor.FL_Status = 1 
	AND {fn curdate()} BETWEEN  T_Floor.FL_DateFrom AND T_Floor.FL_DateTo 
	
LEFT JOIN T_Ref_FloorType
	ON T_Ref_FloorType.FT_UID = T_Floor.FL_FT_UID 
	AND T_Ref_FloorType.FT_Status = 1 
	
LEFT JOIN T_ZO_Floor_DWG 
	ON T_ZO_Floor_DWG.ZO_FLDWG_FL_UID = T_Floor.FL_UID 
	AND T_ZO_Floor_DWG.ZO_FLDWG_Status = 1 
	AND {fn curdate()} BETWEEN  T_ZO_Floor_DWG.ZO_FLDWG_DateFrom AND T_ZO_Floor_DWG.ZO_FLDWG_DateTo 
	
LEFT JOIN T_ZO_Floor_Area
	ON T_ZO_Floor_Area.ZO_FLArea_FL_UID = T_Floor.FL_UID 
	AND T_ZO_Floor_Area.ZO_FLArea_Status = 1 
	AND {fn curdate()} BETWEEN  T_ZO_Floor_Area.ZO_FLArea_DateFrom AND T_ZO_Floor_Area.ZO_FLArea_DateTo 
	
WHERE (1=1) 
AND T_Ref_Location.LC_Status = 1

--AND PR_Name LIKE '%Soodring 33%'
--AND FT_Lang_en = 'Upper floor'
--AND FL_Level = 3
AND ZO_FLDWG_ApertureDWG = '" + this.m_DWG.Replace("'", "''") + @"'
";
                }
                else
                {
                    throw new NotImplementedException("Non-SwissRe location data not yet implemented.");
                }

                this.m_dtLocationInfo = SQL.GetDataTable(strSQL);
                return this.m_dtLocationInfo;
            }

        } // End Property LocationInfo


        public System.Data.DataTable LegendenDaten
        {
            get
            {
                if (this.m_LegendenDaten != null)
                    return this.m_LegendenDaten;


                string strSQL = @"
SELECT 
	 MainView.AP_LEG_Nr
	--,MainView.AP_LEG_Mandant
	--,MainView.AP_LEG_DWG
	
	,MainView.[" + this.TextField + @"] AS [Text] 
	
	,MainView.AP_LEG_Pattern
	,MainView.AP_LEG_ForeColor 

	,'#' + ForegroudColorTable.COL_Hex AS HtmlForegroundColor 
	,'#' + BackgroudColorTable.COL_Hex AS HtmlBackgroundColor  
	,'#' + LineColorTable.COL_Hex AS HtmlLineColor  

	," + this.ValueField + @" AS [Value] 
FROM " + this.DataSourceEntity + @" AS MainView 

LEFT JOIN T_SYS_ApertureColorToHex AS ForegroudColorTable 
	ON ForegroudColorTable.COL_Aperture = AP_LEG_ForeColor
	AND ForegroudColorTable.COL_Status = 1 
	
LEFT JOIN T_SYS_ApertureColorToHex AS BackgroudColorTable 
	ON BackgroudColorTable.COL_Aperture = AP_LEG_BackColor
	AND BackgroudColorTable.COL_Status = 1 
	
LEFT JOIN T_SYS_ApertureColorToHex AS LineColorTable 
	ON LineColorTable.COL_Aperture = AP_LEG_BackColor
	AND LineColorTable.COL_Status = 1 

WHERE AP_LEG_DWG = '" + this.m_DWG.Replace("'", "''") + @"' 
--AND AP_LEG_Nr > 0 

ORDER BY AP_LEG_Nr 
";

                this.m_LegendenDaten = SQL.GetDataTable(strSQL);

                return this.m_LegendenDaten;
            } // End Get of  LegendenDaten

        } // End Property LegendenDaten


    } // End Class Legende 


}
