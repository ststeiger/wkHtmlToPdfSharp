
using System;


namespace COR_Helper
{


    public class UnitConversion
    {


        public static double mm2Points(double dSomeMillimeters)
        {
            // Point ist eine Maßeinheit, die 1/72 Zoll entspricht.
            // 1 Zoll = 1 in = 1000 Thou = 1000 mil = 1/12 ft = 1/36 yd = 25,4 mm = 2,54 cm = 0,254 dm = 0,0254 m.
            // 1 Point = 0.35277777777777777777777777777778 mm
            // --> 1mm = 2.834645669291338582677165354337 Point

            return dSomeMillimeters * 2.83464566929134;
            //Point
        }


        public static double mm2Pica(double dSomeMillimeters)
        {
            //Pica ist eine Maßeinheit, die 12 Points entspricht.
            // The contemporary computer pica is 1/72nd of the Anglo-Saxon compromise foot of 1959, i.e. 4.23_3mm or 0.166in. Not
            // 1 Pica = 4.233333333333333333333333333333333 mm
            // --> 1 mm = 0.23622047244094488188976377952758 Pica
            return dSomeMillimeters * 0.236220472440945;
        }


        public static double cm2Points(double dSomeCentiMeters)
        {
            return mm2Points(dSomeCentiMeters * 10.0);
        }


        public static double cm2Pica(double dSomeCentiMeters)
        {
            return mm2Pica(dSomeCentiMeters * 10.0);
        }


    } // End Class PdfUnitConversion


	[Serializable()]
	public class WebUnitConversion
	{


		public static System.Web.UI.WebControls.Unit mm2Points(double dSomeMillimeters)
		{
			// Point ist eine Maßeinheit, die 1/72 Zoll entspricht.
			// 1 Zoll = 1 in = 1000 Thou = 1000 mil = 1/12 ft = 1/36 yd = 25,4 mm = 2,54 cm = 0,254 dm = 0,0254 m.
			// 1 Point = 0.35277777777777777777777777777778 mm
			// --> 1mm = 2.834645669291338582677165354337 Point

			return System.Web.UI.WebControls.Unit.Point(System.Convert.ToInt32(dSomeMillimeters * 2.83464566929134));
			//Point
		}


		public static System.Web.UI.WebControls.Unit mm2Pica(double dSomeMillimeters)
		{
			//Pica ist eine Maßeinheit, die 12 Points entspricht.
			// The contemporary computer pica is 1/72nd of the Anglo-Saxon compromise foot of 1959, i.e. 4.23_3mm or 0.166in. Not
			// 1 Pica = 4.233333333333333333333333333333333 mm
			// --> 1 mm = 0.23622047244094488188976377952758 Pica
			return System.Web.UI.WebControls.Unit.Point(System.Convert.ToInt32(dSomeMillimeters * 0.236220472440945));
		}


		public static System.Web.UI.WebControls.Unit cm2Points(double dSomeCentiMeters)
		{
			return mm2Points(dSomeCentiMeters * 10.0);
		}


		public static System.Web.UI.WebControls.Unit cm2Pica(double dSomeCentiMeters)
		{
			return mm2Pica(dSomeCentiMeters * 10.0);
		}


	}


}
