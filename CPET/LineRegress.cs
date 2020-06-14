using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPET
{
    class LineRegress
    {
        static public void LSM(List <double> dataX, List<double> dataY, out double a, out double b)
        {
            double SumOfPowX = 0, SumOfX = 0, n = dataY.Count, SumOfXY = 0, SumOfY = 0;
            for (int i = 0; i < dataY.Count; i++)
            {
                SumOfPowX += Math.Pow(dataX[i], 2);
                SumOfX += dataX[i];
                SumOfXY += dataX[i] * dataY[i];
                SumOfY += dataY[i];
            }
            double det = SumOfPowX * n - Math.Pow(SumOfX, 2);
            //if (Math.Abs(det) < 1e-17)
            //{
            //    throw new ArgumentException("Invalid data!");
            //  
            //}
            a = (SumOfXY * n - SumOfX * SumOfY) / det;
            b = (SumOfY - a * SumOfX) / n;
        }
        static public void LSM_DATAXY(double A,double B,double X0,double Xk,double intervalX,out List<double> LSM_DATAX, out List<double> LSM_DATAY)
        {
            List<double> LSM_DATAX0 = new List<double> { };
            List<double> LSM_DATAY0 = new List<double> { };
            for (double i=X0;i<=Xk;i+=intervalX)
            {
                LSM_DATAX0.Add(i);
                LSM_DATAY0.Add(A * i + B);
            }
            LSM_DATAX = LSM_DATAX0;
            LSM_DATAY = LSM_DATAY0;
        }
    }
}
