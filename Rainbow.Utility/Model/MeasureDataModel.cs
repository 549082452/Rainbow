using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.Utility.Model
{
    public class MeasureDataModel:AbstractModel
    {
        public string SampleID { get; set; }
        public int SampleTestTimes { get; set; }
        
        public double SWnm { get; set; }
        public double A260 { get; set; }
        public double A280 { get; set; }
        public double A230 { get; set; }
        public double Conc { get; set; }
        public string Unit { get; set; }
        public double[] BlankList { get; set; }
        public double[] MeasureList { get; set; }
        public double[] WaveList { get; set; }


        #region UVVS
        public double Wavelength { get; set; }
        public double Abs { get; set; }
        #endregion

        #region CellCulture
        public double AbsCorrection { get; set; }
        public double A600 { get; set; }
        public double UserCursorWave { get; set; }
        public double UserCursorAbs { get; set; }
        #endregion
    }
}
