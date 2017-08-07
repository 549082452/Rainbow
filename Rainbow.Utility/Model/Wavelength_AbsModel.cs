using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.Utility.Model
{
    public class Wavelength_AbsModel:AbstractModel
    {
        double wavelength;
        double abs;
        public double Wavelength
        {
            get
            {
                return wavelength;
            }
            set
            {
                wavelength = value;
                NotifyPropertyChanged("Wavelength");
            }
        }
        public double Abs
        {
            get
            {
                return abs;
            }
            set
            {
                abs = value;
                NotifyPropertyChanged("Abs");
            }
        }
    }
}
