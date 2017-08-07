namespace Rainbow.Utility.Functions
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class Fitting
    {
        private int m_numbers;
        private double[] m_trueX;
        private double[] m_trueY;

        public int Fitting_Contents(int fit, int points, double[] dataX, double[] dataY, ref double intercept, ref double slope, ref double rSquared)
        {
            //Variables.nameCurrentMethod = MethodBase.GetCurrentMethod().Name;
            this.m_trueX = new double[points];
            this.m_trueY = new double[points];
            this.m_numbers = this.Settle(points, dataX, dataY);
            if (this.m_numbers < 2)
            {
                intercept = 0.0;
                slope = 0.0;
                rSquared = 0.0;
                return this.m_numbers;
            }
            double num = Enumerable.Sum(this.m_trueX);
            double num2 = Enumerable.Sum(this.m_trueY);
            double num3 = this.Sum_Square_Scores(this.m_trueX) - ((num * num) / ((double) this.m_numbers));
            double num4 = this.Sum_Square_Scores(this.m_trueY) - ((num2 * num2) / ((double) this.m_numbers));
            double num5 = this.Sum_Product_Scores(this.m_trueX, this.m_trueY) - ((num * num2) / ((double) this.m_numbers));
            double num6 = num5 / Math.Sqrt(num3 * num4);
            double num7 = num5 / num3;
            double num8 = (num2 / ((double) this.m_numbers)) - ((num7 * num) / ((double) this.m_numbers));
            intercept = num8;
            slope = num7;
            rSquared = num6;
            return this.m_numbers;
        }

        private int Settle(int points, double[] dataX, double[] dataY)
        {
            int num = 0;
            for (int i = 0; i < points; i++)
            {
                if ((dataY[i] != 0.0) || (i == 0))
                {
                    num++;
                    this.m_trueX[num - 1] = dataX[i];
                    this.m_trueY[num - 1] = dataY[i];
                }
            }
            return num;
        }

        private double Sum_Product_Scores(double[] dataX, double[] dataY)
        {
            double num = 0.0;
            for (int i = 0; i < this.m_numbers; i++)
            {
                num += dataX[i] * dataY[i];
            }
            return num;
        }

        private double Sum_Scores(double[] data)
        {
            double num = 0.0;
            for (int i = 0; i < this.m_numbers; i++)
            {
                num += data[i];
            }
            return num;
        }

        private double Sum_Square_Scores(double[] data)
        {
            double num = 0.0;
            for (int i = 0; i < this.m_numbers; i++)
            {
                num += Math.Pow(data[i], 2.0);
            }
            return num;
        }
    }
}

