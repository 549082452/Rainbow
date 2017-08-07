using Rainbow.Utility;
using Rainbow.Utility.Functions;
using Rainbow.Utility.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Rainbow.BusinessLogic
{/// <summary>
/// 染料信息的处理逻辑
/// </summary>
    public class DyeListBLL
    {
        static string path = AppDomain.CurrentDomain.BaseDirectory + @"DATA\DyeList.xml";
        static XDocument doc = XDocument.Load(path);

        public static XDocument MachineConfigDoc
        {
            get
            {
                return doc;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ReadInitDye()
        {
            try
            {
                GlobalProvider.DyeList = (from dye in doc.Element("DyeList").Elements("Dye")
                                          select new DyeModel()
                                          {
                                              IsLock = Convert.ToBoolean(dye.Element("IsLock").Value),
                                              IsShow = Convert.ToBoolean(dye.Element("IsShow").Value),
                                              Dye = dye.Element("Dye").Value,
                                              Unit = dye.Element("Unit").Value,
                                              Coeff = Convert.ToDouble(dye.Element("Coeff").Value),
                                              AnalysisWavelength = Convert.ToDouble(dye.Element("AnalysisWavelength").Value),
                                              Correction_260nm = Convert.ToDouble(dye.Element("Correction_260nm").Value),
                                              Correction_280nm = Convert.ToDouble(dye.Element("Correction_280nm").Value),
                                          }).ToList();
            }
            catch (Exception ex)
            {
                Functions.LOG(ex.Message);
            }
        }

        public static void SaveInitDye()
        {
            try
            {
                //List<XElement> _dyeList = doc.Element("DyeList").Elements("Dye").ToList();
                doc.Elements("DyeList").Remove();
                XElement _dyeList = new XElement("DyeList");
                doc.Add(_dyeList);
                for (int i = 0; i < GlobalProvider.DyeList.Count; i++)
                {
                    DyeModel _dye = GlobalProvider.DyeList[i];
                    XElement _dyeXe = new XElement("Dye");
                    _dyeXe.SetElementValue("IsShow", _dye.IsShow.ToString());
                    _dyeXe.SetElementValue("IsLock", _dye.IsLock.ToString());
                    _dyeXe.SetElementValue("Dye", _dye.Dye);
                    _dyeXe.SetElementValue("Unit", _dye.Unit);
                    _dyeXe.SetElementValue("Coeff", _dye.Coeff.ToString());
                    _dyeXe.SetElementValue("AnalysisWavelength", _dye.AnalysisWavelength.ToString());
                    _dyeXe.SetElementValue("Correction_260nm", _dye.Correction_260nm.ToString());
                    _dyeXe.SetElementValue("Correction_280nm", _dye.Correction_280nm.ToString());
                    doc.Element("DyeList").Add(_dyeXe);
                }
                doc.Save(path);

            }
            catch (Exception ex)
            {
                Functions.LOG(ex.Message);
            }
        }

    }
}
