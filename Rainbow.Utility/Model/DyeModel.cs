using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.Utility.Model
{
    public class DyeModel : AbstractModel
    {
        private bool _isLock = false;
        private bool _isShow = true;
        private string _dyeName;
        private string _unit;
        private double _coeff;
        private double _analysisWavelength;
        private double _260nmCorrection;
        private double _280nmCorrection;
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow
        {
            get
            { return _isShow; }
            set
            { _isShow = value;
                NotifyPropertyChanged("IsShow");
            }
        }
        /// <summary>
        /// 是否是可以删除的
        /// </summary>
        public bool IsLock
        {
            get { return _isLock; }
            set { _isLock = value;
                NotifyPropertyChanged("IsLock");
            }
        }
        public string Dye
        {
            get { return _dyeName; }
            set { _dyeName = value;
                NotifyPropertyChanged("Dye");
            }
        }
        public string Unit
        {
            get { return _unit; }
            set
            {
                _unit = value;
                NotifyPropertyChanged("Unit");
            }
        }
        public double Coeff
        {
            get { return _coeff; }
            set { _coeff = value;
                NotifyPropertyChanged("Coeff");
            }
        }
        public double AnalysisWavelength
        {
            get { return _analysisWavelength; }
            set
            {
                _analysisWavelength = value;
                NotifyPropertyChanged("AnalysisWavelength");
            } 
        }
        public double Correction_260nm
        { get { return _260nmCorrection; }
            set { _260nmCorrection = value;
                NotifyPropertyChanged("Correction_260nm");
            }
        }
        public double Correction_280nm
        {
            get { return _280nmCorrection; }
            set
            {
                _280nmCorrection = value;
                NotifyPropertyChanged("Correction_280nm");
            }
        }
    }
}
