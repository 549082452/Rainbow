using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmniDriver;
using System.Threading;
using Rainbow.Utility.Model;
using System.Windows;
using System.IO.Ports;

namespace Rainbow.Utility.Communication
{
    public class OmniProvider
    {
        

        #region 光谱仪参数
        static int numberOfPixels;                         // number of CCD elements/pixels provided by the spectrometer
        static int numberOfSpectrometers;                  // actually attached and talking to us
        static int mScansToAverage;
        static int mBoxcarWidth;
        static int mIntergrationTime;

        #endregion
        
        #region 光谱仪测量数据
        static OmniDriver.CCoSPIBus mSpibus = null;
        static private CCoWrapper mWrapper = null;

        static private double[] mBlankList_1;
        static private double[] mMeasureList_1;

        static private double[] mBlankList_2;
        static private double[] mMeasureList_2;

        static private double[] mBlankList_3;
        static private double[] mMeasureList_3;

        static private double[] mWaveList;

        #endregion

        #region 属性
        public static int ScansToAverage
        {
            get
            {
                return mScansToAverage;
            }
            set
            {
                mScansToAverage = value;
            }
        }
        public static int BoxcarWidth
        {
            get
            {
                return mBoxcarWidth;
            }
            set
            {
                mBoxcarWidth = value;
            }
        }
        public static int InterationTime
        {
            get
            {

                return mIntergrationTime;
            }
            set
            {
                mIntergrationTime = value;
            }
        }
        public static List<double[]> BlankList
        {
            get
            {
                List<double[]> list = new List<double[]>();
                list.Add(mBlankList_1);
                list.Add(mBlankList_2);
                list.Add(mBlankList_3);
                return list;
            }
        }
        public static List<double[]> MeasureList
        {
            get
            {
                List<double[]> list = new List<double[]>();
                list.Add(mMeasureList_1);
                list.Add(mMeasureList_2);
                list.Add(mMeasureList_3);
                return list;
            }
        }
        public static double[] WaveList
        {
            get
            {
                return mWaveList;
            }
        }

        static OmniDriver.CCoSPIBus SPIBus
        {
            get
            {
                if (mSpibus== null)
                {
                    if (Wrapper != null)
                    {
                        CCoGPIO tt= Wrapper.getFeatureControllerGPIO(0);


                        CCoBitSet set = new CCoBitSet();
                        
                        if (Wrapper.isFeatureSupportedSPIBus(0) == 1)
                        {
                            mSpibus = Wrapper.getFeatureControllerSPIBus(0);
                            
                        }
                        else
                        {
                            Functions.Functions.LOG("SPIBus feature is not supported");
                        }
                    }
                }
                return mSpibus;
            }
        }
        static CCoGPIO mGPIO = null;
        static OmniDriver.CCoGPIO GPIO
        {
            get
            {
                if (mGPIO == null)
                {
                    if (Wrapper != null)
                    {

                        if (Wrapper.isFeatureSupportedGPIO(0) == 1)
                        {
                            mGPIO = Wrapper.getFeatureControllerGPIO(0);
                        }
                        else
                        {
                            Functions.Functions.LOG("GPIO feature is not supported");
                        }
                    }
                }
                return mGPIO;
            }
        }
        static CCoWrapper Wrapper
        {
            get
            {
                if (mWrapper == null)
                {
                    mWrapper = new CCoWrapper();
                    numberOfSpectrometers= mWrapper.openAllSpectrometers();
                }
                return mWrapper;
            }
        }
        static SerialPort mSerialPort = new System.IO.Ports.SerialPort();
        
        #endregion 

        #region 光谱仪方法
        public static int OpenSpectrometers()
        {
            return Wrapper.openAllSpectrometers();
        }
        public static void CloseAllSpectrometers()
        {
            Wrapper.closeAllSpectrometers();
        }
        public static double[] GetSpectrum()
        {
            Wrapper.setIntegrationTime(0, mIntergrationTime);     // Sets the integration time of the first spectrometer to 500ms
            Wrapper.setStrobeEnable(0, 1);                      // Enables the strobe on the first spectrometer
            Wrapper.setAutoToggleStrobeLampEnable(0, 1);      // Enables the Auto Strobe lamp on the first spectrometer
            Wrapper.setScansToAverage(0, mScansToAverage);
            Wrapper.setBoxcarWidth(0, mBoxcarWidth);
            Wrapper.getFeatureControllerContinuousStrobe(0)
                                    .setContinuousStrobeDelay(10000);
            //Wrapper.setCorrectForElectricalDark(0, 1);
            //Wrapper.setCorrectForDetectorNonlinearity(0, 1);

            return Wrapper.getSpectrum(0);
        }
        /// <summary>
        /// 获得波长数据，不保存在对象实例中
        /// </summary>
        /// <returns></returns>
        public static double[] GetWavelengths()
        {
            return Wrapper.getWavelengths(0);
        }
        /// <summary>
        /// 获取波长数据并保存在实例
        /// </summary>
        /// <returns></returns>
        public static double[] GetWaveLengthsList()
        {
            mWaveList = GetWavelengths();
            return mWaveList;
        }
        /// <summary>
        /// 获得仪器中与指定波长最接近的波长值
        /// </summary>
        /// <returns>仪器波长值</returns>
        public static int GetBestWaveLength(double wave)
        {
            double dev = 1;
            double bestKey = 0;
            int waveIndex;
            int bestWaveIndex=0;
            for (waveIndex = 0; waveIndex < mWaveList.Length; waveIndex++)
            {
                double _key = mWaveList[waveIndex];
                double sub = _key - wave;
                if (Math.Abs(sub) > dev)
                {

                }
                else if (Math.Abs(sub) < dev)
                {
                    dev = Math.Abs(sub);
                    bestKey = _key;
                    bestWaveIndex = waveIndex;
                }
            }
            bestKey = double.Parse(bestKey.ToString("0.00"));
            return bestWaveIndex;
        }
        #endregion


        #region SerialPort


        #region 命令值
        static string mEnd = "\r";
        static string mCmdReturn = "";
        static string mReset = "AA";//复位
        static string[] mGoMarkList={"AB","AC","AD"};
        static string[] mGetMarkList = { "Zd", "Ze", "Zf" };//读取电机位置
        static string[] mSetMarkList={ "ZD", "ZE", "ZF" };//设置电机位置
        static string[] mGetParameterList = { "Zg", "Zh", "Zi", "Zj", "Zk" };//读取光谱仪参数
        static string[] mSetParameterList = { "ZG", "ZH", "ZI", "ZJ", "ZK" };//设置光谱仪参数
        static UTF8Encoding mUtf8 = new UTF8Encoding();
        static AutoResetEvent wait = new AutoResetEvent(false);
        #endregion

        #region 基本命令
        public static void Reset()
        {
            SendCommand(mReset);
            
        }
        /// <summary>
        /// 设置电机参数
        /// </summary>
        /// <param name="markOrder">位置编号1，2，3</param>
        /// <param name="parameter">参数值</param>
        public static void SaveMark(int markOrder, int parameter)
        {
            SendCommand(mSetMarkList[markOrder - 1]+parameter);
        }
        /// <summary>
        /// 获得电机位置参数
        /// </summary>
        /// <param name="markOrder">位置编号1，2，3</param>
        /// <returns></returns>
        public static string GetMark(int markOrder)
        {
            string parameter = "";
            parameter = SendCommand(mGetMarkList[markOrder - 1]);
            return parameter;
        }
        /// <summary>
        /// 电机去指定位置
        /// </summary>
        /// <param name="pointOrder">位置编号1，2，3</param>
        public static void GoMark(int markOrder)
        {
            SendCommand(mGoMarkList[markOrder - 1]);
           
        }
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="parameterOrder"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string SetParameter(int parameterOrder, int parameter)
        {
            return SendCommand(mSetParameterList[parameterOrder-1]+parameter);

        }
        /// <summary>
        /// 获得参数
        /// </summary>
        /// <param name="paraterOrder"></param>
        /// <returns></returns>
        public static int GetParameter(int paraterOrder)
        {
            string para= SendCommand(mGetParameterList[paraterOrder-1]);
            int rec = 0;
            if(para!="")
            {
                rec = int.Parse(para);
            }
            return rec;
        }
        /// <summary>
        /// 获得各项参数
        /// </summary>
        public static void GetParameters()
        {
            if (OmniProvider.numberOfSpectrometers > 0)
            {
                mScansToAverage = GetParameter(1);
                mBoxcarWidth = GetParameter(2);
                mIntergrationTime = GetParameter(3);
            }
            else
            {
                Functions.Functions.LOG("No machine!");
            }
        }
        /// <summary>
        /// 设置各项参数
        /// </summary>
        /// <param name="scnasToAverage"></param>
        /// <param name="boxcarWidth"></param>
        /// <param name="intergrationTime"></param>
        public static void SetParameters(int scnasToAverage,int boxcarWidth,int intergrationTime)
        {
            if (OmniProvider.numberOfSpectrometers > 0)
            {
                SetParameter(1, scnasToAverage);
                SetParameter(2, boxcarWidth);
                SetParameter(3, intergrationTime);
            }
            else
            {
                Functions.Functions.LOG("No machine!");
            }
        }
        #endregion

        #region sbyte和int相互转换
        /// <summary>
        /// sbyte数组转int
        /// </summary>
        /// <param name="sbytes"></param>
        /// <returns></returns>
        private static int SbyteToInt(sbyte high, sbyte low)
        {
            return high * 127 + low;
        }
        /// <summary>
        /// int转sbyte数组
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static sbyte[] IntToSbyte(int number)
        {
            sbyte[] _sbytes = new sbyte[2];
            if (number > 127)
            {
                _sbytes[0] = (sbyte)(number / 127);
                _sbytes[1] = (sbyte)(number % 127);
            }
            else
            {
                _sbytes[0] = 0;
                _sbytes[1] =(sbyte) number;
            }
            return _sbytes;
        }
        #endregion


        static List<string> mSerialPortList = new List<string>();
        public static void OpenSerialPort()
        {
            string[] str = SerialPort.GetPortNames();
            if (str == null||str.Length==0)
            {
                MessageBox.Show("本机没有串口！", "Error");
                return;
            }

            //添加串口项目
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {//获取有多少个COM口
                
                mSerialPortList.Add(s);   
            }
            //if (mSerialPortList.Count > 0)
            //    mSerialPort.PortName = mSerialPortList[0];

            mSerialPort.PortName = "COM6";

            mSerialPort.BaudRate = 115200;
            mSerialPort.DataReceived += new SerialDataReceivedEventHandler(mSerialPort_DataReceived);
            //mSerialPort.ReceivedBytesThreshold = 1;
            mSerialPort.DataBits = 8;
            mSerialPort.StopBits = StopBits.One;
            mSerialPort.Parity = Parity.None;

            //准备就绪              
            mSerialPort.DtrEnable = true;
            mSerialPort.RtsEnable = true;
            //设置数据读取超时为1秒
            mSerialPort.ReadTimeout = 1000;

            mSerialPort.Open();

            Functions.Functions.CommnucationLog("串口打开成功。");
        }
        /// <summary>
        /// 发送命令字符串
        /// </summary>
        /// <param name="cmd"></param>
        public static string SendCommand(string cmd)
        {
            if (mSerialPort.IsOpen) //如果没打开
            {
                cmd += "\r";
                mSerialPort.Write(cmd);

                Functions.Functions.CommnucationLog(cmd);
                //string recieve= mSerialPort.ReadLine();
                //mSerialPort.DiscardInBuffer();
                //Console.WriteLine(recieve);
                wait.WaitOne();
                return mCmdReturn;
            }
            else
            {
                MessageBox.Show("请先打开串口！", "Error");
                return "";
            }

        }
        /// <summary>
        /// 发送命令字符串
        /// </summary>
        /// <param name="cmd"></param>
        public static string SendCommand(string cmd,bool isWait)
        {
            if (mSerialPort.IsOpen) //如果没打开
            {
                cmd += "\r";
                mSerialPort.Write(cmd);

                Functions.Functions.CommnucationLog(cmd);
                if(isWait)
                    wait.WaitOne();
                return mCmdReturn;
            }
            else
            {
                MessageBox.Show("请先打开串口！", "Error");
                return "";
            }

        }
        /// <summary>
        /// 接收命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void mSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (mSerialPort.IsOpen)     //此处可能没有必要判断是否打开串口，但为了严谨性，我还是加上了
            {
                //输出当前时间
                DateTime dt = DateTime.Now;

                byte[] byteRead = new byte[mSerialPort.BytesToRead];    //BytesToRead:sp1接收的字符个数

                try
                {
                    Byte[] receivedData = new Byte[mSerialPort.BytesToRead];        //创建接收字节数组
                    mSerialPort.Read(receivedData, 0, receivedData.Length);         //读取数据                                                                               //string text = sp1.Read();   //Encoding.ASCII.GetString(receivedData);
                    mSerialPort.DiscardInBuffer();
                    mCmdReturn = "";
                    mCmdReturn = mUtf8.GetString(receivedData); 

                    //string tempstr,strRcv = "";
                    //for (int i = 0; i < receivedData.Length; i++) //窗体显示
                    //{
                    //    UTF8Encoding utf8 = new UTF8Encoding();


                    //    tempstr = Convert.ToChar(receivedData[i]).ToString();
                    //    mCmdReturn = mCmdReturn + tempstr;

                    //    strRcv += receivedData[i].ToString("X2");  //16进制显示

                    //}
               
                    Functions.Functions.CommnucationLog(mCmdReturn);
                    if (mCmdReturn.Contains(";"))
                    {
                        
                        switch (mCmdReturn)
                        {
                            case "AAS":

                                break;
                            case "ABS":

                                break;
                            case "ACS":

                                break;
                            case "ADS":

                                break;
                            default:
                                string para = mCmdReturn.Substring(0, 2);
                                if (mCmdReturn.Substring(0, 1) == "Z")
                                {
                                    mCmdReturn = mCmdReturn.Substring(2, mCmdReturn.Length - 3);
                                }
                                break;
                        }
                        wait.Set();
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "出错提示");
                    //txtSend.Text = "";
                }
            }
        }

        /// <summary>
        /// 电机复位
        /// </summary>
        public static void MotorReset()
        {
            SendCommand("AA");

        }
        /// <summary>
        /// 电机到指定位置 
        /// </summary>
        /// <param name="order"></param>
        public static void MotorMove(MoveEnum move)
        {
            switch(move)
            {
                case MoveEnum.Move0_5mm:
                    SendCommand("AB");
                    break;
                case MoveEnum.Move0_2mm:
                    SendCommand("AC");
                    break;
                case MoveEnum.Move0_05mm:
                    SendCommand("AD");
                    break;
            }
        }

        #endregion
        #region 组合使用方法

        /// <summary>
        /// 获取并保存空测数据到实例 
        /// </summary>
        /// <param name="markOrder">0.5mm,0.2mm,0.05mm</param>
        /// <returns></returns>
        public static double[] GetBlankList(MoveEnum markOrder)
        {
            MotorMove(markOrder);
            double[] list = GetSpectrum();
            switch(markOrder)
            {
                case MoveEnum.Move0_5mm:
                    mBlankList_1 = list;
                    break;
                case MoveEnum.Move0_2mm:
                    mBlankList_2 = list;
                    break;
                case MoveEnum.Move0_05mm:
                    mBlankList_3 = list;
                    break;

            }
            return list;
        }
        /// <summary>
        /// 获取并保存测量数据到实例
        /// </summary>
        ///<param name="markOrder">0.5mm,0.2mm,0.05mm</param>
        /// <returns></returns>
        public static double[] GetMeasureList(MoveEnum markOrder)
        {
            MotorMove(markOrder);
            double[] list = GetSpectrum();
            switch (markOrder)
            {
                case MoveEnum.Move0_5mm:
                    mMeasureList_1 = list;
                    break;
                case MoveEnum.Move0_2mm:
                    mMeasureList_2 = list;
                    break;
                case MoveEnum.Move0_05mm:
                    mMeasureList_3 = list;
                    break;

            }
            return list;
        }

        /// <summary>
        /// 计算Y轴
        /// </summary>
        /// <returns></returns>
        public static double[] CalcY(MoveEnum markOrder)
        {
            double[] blankList = null;
            double[] measureList = null;
            switch (markOrder)
            {
                case MoveEnum.Move0_5mm:
                    blankList = mBlankList_1;
                    measureList = mMeasureList_1;
                    break;
                case MoveEnum.Move0_2mm:
                    blankList = mBlankList_2;
                    measureList = mMeasureList_2;
                    break;
                case MoveEnum.Move0_05mm:
                    blankList = mBlankList_2;
                    measureList = mMeasureList_2;
                    break;
            }
            return CalcY(blankList, measureList);
        }

        private static double[] CalcY(double[] blankList, double[] measureList)
        {

            double[] logvalue = new double[2048];
            for (int i = 0; i < blankList.Length; i++)
            {
                logvalue[i] = Math.Log10(blankList[i] / measureList[i]);
            }
            return logvalue;
        }

        /// <summary>
        /// 获得指定波长的吸光值
        /// </summary>
        /// <param name="blank"></param>
        /// <param name="measure"></param>
        /// <param name="bestWave"></param>
        /// <returns></returns>
        public static double GetAbs(double wave, MoveEnum markOrder)
        {
            double[] blankList = null;
            double[] measureList = null;
            switch (markOrder)
            {
                case MoveEnum.Move0_5mm:
                    blankList = mBlankList_1;
                    measureList = mMeasureList_1;
                    break;
                case MoveEnum.Move0_2mm:
                    blankList = mBlankList_2;
                    measureList = mMeasureList_2;
                    break;
                case MoveEnum.Move0_05mm:
                    blankList = mBlankList_2;
                    measureList = mMeasureList_2;
                    break;
            }

            return GetAbs(wave, blankList, measureList);
        }
        public static double GetAbs(double wave,double[] blankList,double[] measureList)
        {
            int bestWaveIndex = GetBestWaveLength(wave);
            if (bestWaveIndex > 0)
            {
                if (blankList != null && blankList.Length > 0 && measureList != null && measureList.Length > 0)
                {
                    return Math.Log10(blankList[bestWaveIndex] / measureList[bestWaveIndex]) * 10;
                }
            }
            return 0;
        }

        /// <summary>
        /// 获得指定波长的光强值
        /// </summary>
        /// <param name="wave"></param>
        /// <param name="markOrder"></param>
        /// <returns></returns>
        public static double GetMeasureLight(double wave, int markOrder)
        {
            double[] blankList = null;
            double[] measureList = null;
            switch (markOrder)
            {
                case 1:
                    blankList = mBlankList_1;
                    measureList = mMeasureList_1;
                    break;
                case 2:
                    blankList = mBlankList_2;
                    measureList = mMeasureList_2;
                    break;
                case 3:
                    blankList = mBlankList_2;
                    measureList = mMeasureList_2;
                    break;
            }

            return GetMeasureLight(wave, measureList);
        }

        /// <summary>
        /// 获得指定波长的光强值
        /// </summary>
        /// <returns></returns>
        public static double GetMeasureLight(double wave,double[] lightList)
        {
            int bestWaveIndex = GetBestWaveLength(wave);
            if (bestWaveIndex > 0)
            {
                if (lightList != null && lightList.Length > 0)
                {
                    return lightList[bestWaveIndex];
                }
            }
            return 0;
        }

        #endregion
    }
}
