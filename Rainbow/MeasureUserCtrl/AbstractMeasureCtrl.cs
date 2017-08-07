using Rainbow.Utility.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Rainbow.MeasureUserCtrl
{
    public class AbstractMeasureCtrl:UserControl, IMeasureUser
    {
        public virtual void Blank()
        {

        }

        public virtual void Measure()
        {
            throw new NotImplementedException();
        }

        public virtual void ExportTxt()
        {
            throw new NotImplementedException();
        }
    }
}
