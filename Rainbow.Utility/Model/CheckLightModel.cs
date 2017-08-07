using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.Utility.Model
{
    public class CheckLightModel
    {
        public double WaveLength { get; set; }
        public double CurrentLight { get; set; }
        public double AverageLight { get; set; }

        public double ScansToAverage { get; set; }
        public double BoxcarWidth { get; set; }
        public double IntergrationTime { get; set; }

        public double A280 { get; set; }
        public double A230 { get; set; }
    }
}
