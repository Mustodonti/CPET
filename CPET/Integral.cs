using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CPET
{
    class integral
    {
        public double y0 = 0, result = 0;
        static double y00 = 0,result0=0;
        public double Trap(double y, double n)  //трапеций
        {
            result = (y + y0) * n * 0.5;
            y0 = y;
            return result;
        }
        static public double TrapList(List <double> y, double n, double C2=1)  //трапеций
        {
            y00 = 0;
            result0 = 0;
            for (int i=0;i<y.Count();i++)
            {
                result0 +=(y[i] + y00) * n * 0.5*C2;
                y00 = y[i];
            }
            return result0;
        }
    }
}
