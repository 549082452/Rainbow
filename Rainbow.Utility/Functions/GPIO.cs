namespace Global
{
    using Microsoft.VisualBasic;
    using System;
    using System.Threading;
    using System.Windows.Forms;

    public class GPIO
    {
        private bool m_isQ6000 = Convert.ToBoolean(1);

        private void GPIO_Enabled()
        {
            Variables.gpioController = Variables.wrapper.getFeatureControllerGPIO(0);
            Variables.gpioController.setMuxBit(7, 0);
            Variables.gpioController.setDirectionBit(7, 1);
            Variables.gpioController.setValueBit(7, 0);
            Variables.gpioController.setValueBit(7, 1);
            Variables.gpioController.setMuxBit(6, 0);
            Variables.gpioController.setDirectionBit(6, 1);
            Variables.gpioController.setValueBit(6, 0);
            Variables.gpioController.setValueBit(6, 1);
        }

        public void Initialize()
        {
            if (Variables.wrapper.isFeatureSupportedGPIO(0) == Convert.ToInt16(false))
            {
                MessageBox.Show("The GPIO feature is not supported for this type of spectrometer.");
            }
            else if (!this.m_isQ6000)
            {
                this.GPIO_Enabled();
            }
            else
            {
                Variables.gpioController = Variables.wrapper.getFeatureControllerGPIO(0);
                for (int i = 0; i <= 7; i++)
                {
                    Variables.gpioController.setMuxBit(i, 0);
                }
                Variables.gpioController.setDirectionBit(0, 0);
                for (int j = 1; j <= 7; j++)
                {
                    Variables.gpioController.setDirectionBit(j, 1);
                }
                for (int k = 1; k <= 7; k++)
                {
                    Variables.gpioController.setValueBit(k, 0);
                }
            }
        }

        public bool MoveTo(int value)
        {
            DateTime now;
            int num = 30;
            if (!this.m_isQ6000)
            {
                if (value == 1)
                {
                    this.MoveTo1mm();
                }
                else if (value == 2)
                {
                    this.MoveTo02mm();
                }
                return true;
            }
            if (1 != Variables.gpioController.getValueBit(0))
            {
                now = DateTime.Now;
                switch (value)
                {
                    case 1:
                        Functions.LOG("Moving", "to 0.2 mm");
                        Variables.gpioController.setValueBit(4, 1);
                        Variables.gpioController.setValueBit(3, 1);
                        goto Label_0174;

                    case 2:
                        Functions.LOG("Moving", "to 0.5 mm");
                        Variables.gpioController.setValueBit(6, 1);
                        Variables.gpioController.setValueBit(3, 1);
                        goto Label_0174;

                    case 5:
                        Functions.LOG("Moving", "to 0.05 mm");
                        Variables.gpioController.setValueBit(5, 1);
                        Variables.gpioController.setValueBit(3, 1);
                        goto Label_0174;

                    case 9:
                        Functions.LOG("Moving", "to 0.2 mm");
                        Variables.gpioController.setValueBit(7, 1);
                        Variables.gpioController.setValueBit(3, 1);
                        goto Label_0174;
                }
                string log = "Wrong control code.";
                Functions.LOG("Err", log);
                MessageBox.Show(log, "Entry Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return false;
        Label_0174:
            while (Variables.gpioController.getValueBit(0) == 0)
            {
                if (Functions.Diff_Time(DateInterval.Second, now) >= num)
                {
                    this.Reset("Motor");
                    Functions.LOG("Moving", "Time Out!");
                    MessageBox.Show("Time Out!", "Moving problem", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
            }
            Functions.LOG("Moving", "Complated!");
            this.Reset("Motor");
            return true;
        }

        private void MoveTo005mm()
        {
        }

        private void MoveTo02mm()
        {
            Variables.gpioController = Variables.wrapper.getFeatureControllerGPIO(0);
            Variables.gpioController.setMuxBit(7, 0);
            Variables.gpioController.setDirectionBit(7, 1);
            Variables.gpioController.setValueBit(7, 0);
            Thread.Sleep(200);
            Variables.gpioController.setValueBit(7, 1);
            Variables.gpioController.setValueBit(7, 0);
            Variables.gpioController.setValueBit(7, 1);
            Variables.gpioController.setValueBit(7, 0);
            Variables.gpioController.setValueBit(7, 1);
            Variables.gpioController.setMuxBit(6, 0);
            Variables.gpioController.setDirectionBit(6, 1);
            Variables.gpioController.setValueBit(6, 0);
            Variables.gpioController.setValueBit(6, 1);
        }

        private void MoveTo1mm()
        {
            Variables.gpioController = Variables.wrapper.getFeatureControllerGPIO(0);
            Variables.gpioController.setMuxBit(7, 0);
            Variables.gpioController.setDirectionBit(7, 1);
            Variables.gpioController.setValueBit(7, 0);
            Thread.Sleep(200);
            Variables.gpioController.setValueBit(7, 1);
            Variables.gpioController.setValueBit(7, 0);
            Variables.gpioController.setValueBit(7, 1);
            Variables.gpioController.setMuxBit(6, 0);
            Variables.gpioController.setDirectionBit(6, 1);
            Variables.gpioController.setValueBit(6, 0);
            Variables.gpioController.setValueBit(6, 1);
            Thread.Sleep(0x3e8);
        }

        private void MoveToHome()
        {
            Variables.gpioController = Variables.wrapper.getFeatureControllerGPIO(0);
            Variables.gpioController.setMuxBit(7, 0);
            Variables.gpioController.setDirectionBit(7, 1);
            Variables.gpioController.setValueBit(7, 0);
            Thread.Sleep(100);
            Variables.gpioController.setValueBit(7, 1);
            Variables.gpioController.setValueBit(7, 0);
            Variables.gpioController.setValueBit(7, 1);
            Variables.gpioController.setValueBit(7, 0);
            Variables.gpioController.setValueBit(7, 1);
            Variables.gpioController.setValueBit(7, 0);
            Variables.gpioController.setValueBit(7, 1);
            Variables.gpioController.setMuxBit(6, 0);
            Variables.gpioController.setDirectionBit(6, 1);
            Variables.gpioController.setValueBit(6, 0);
            Variables.gpioController.setValueBit(6, 1);
        }

        public void Reset(string _type)
        {
            string str = _type;
            if (str != null)
            {
                if (str != "Motor")
                {
                    if (str != "Stir")
                    {
                        if (str == "Thermal")
                        {
                            Variables.gpioController.setValueBit(1, 0);
                            Variables.gpioController.setValueBit(2, 0);
                            return;
                        }
                        if (str == "Initial")
                        {
                            for (int i = 7; i > 0; i--)
                            {
                                Variables.gpioController.setValueBit(i, 0);
                            }
                        }
                        return;
                    }
                }
                else
                {
                    Variables.gpioController.setValueBit(3, 0);
                    Variables.gpioController.setValueBit(4, 0);
                    Variables.gpioController.setValueBit(5, 0);
                    Variables.gpioController.setValueBit(6, 0);
                    Variables.gpioController.setValueBit(7, 0);
                    return;
                }
                Variables.gpioController.setValueBit(5, 0);
                Variables.gpioController.setValueBit(6, 0);
                Variables.gpioController.setValueBit(7, 0);
            }
        }

        public void StirControl(int value)
        {
            this.Reset("Stir");
            switch (value)
            {
                case 1:
                    Variables.gpioController.setValueBit(6, 1);
                    Variables.gpioController.setValueBit(5, 1);
                    return;

                case 2:
                    Variables.gpioController.setValueBit(7, 1);
                    Variables.gpioController.setValueBit(5, 1);
                    return;
            }
        }

        public bool ThermalControl(bool onOFF)
        {
            if (onOFF)
            {
                if (1 == Variables.gpioController.getValueBit(0))
                {
                    return false;
                }
                Variables.gpioController.setValueBit(1, 1);
                Variables.gpioController.setValueBit(2, 1);
            }
            else
            {
                this.Reset("Thermal");
            }
            return true;
        }
    }
}

