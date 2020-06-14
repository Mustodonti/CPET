using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPET
{
    class Derivate
    {
        double d0,d1,d21;
        double y0 = 0;
        double y00=0, y01=0;
        double y000=0,d20;
        public double CommonDifference(double y,double T)
        {
            d0 = (double)(y - y0) / T;
            y0 = y;
            return d0;
        }

        public double NONRecursive(double y02, double T)
        {
            // d1=()/T
            d1 = (2 * y02 - 4 * y01 + y00) / (2 * T);
            y00 = y01;
            y01 = y02;
            return d1;
        }
        public double Recursive(double y001, double T)
        {
            d21 = (((y001 - y000) * 8) / (7 * T)) - d20 / 7;
            y000 = y001;
            d20 = d21;
            return d21;
        }
    }
}
