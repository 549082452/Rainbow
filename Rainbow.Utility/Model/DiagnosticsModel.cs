using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.Utility.Model
{
    public class DiagnosticsModel
    {
        public double Target { get; set; }
        public double Current { get; set; }
        public double Average { get; set; }
        public double Maximum { get; set; }
        public double Minimum { get; set; }
        public double ErrorMax { get; set; }
        public double STD_DEV { get; set; }
    }
}
