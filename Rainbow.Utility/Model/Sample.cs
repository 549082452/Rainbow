using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.Utility.Model
{
    public class Sample
    {
        int _ID;
        string _application;
        double _type;
        double _blankLight;
        double _measureLight;
        double _Conc;
        
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public string Application
        {
            get
            {
                return _application;
            }
            set
            {
                _application = value;
            }
        }
        public double Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }
        public double BlankLight
        {
            get
            {
                return _blankLight;
            }
            set
            {
                _blankLight = value;
            }
        }
        public double MeasureLight
        {
            get
            {
                return _measureLight;
            }

            set
            {
                _measureLight = value;
            }
        }
        public double Conc
        {
            get
            {
                return _Conc;
            }
            set
            {
                _Conc = value;
            }
        }
    }
}
