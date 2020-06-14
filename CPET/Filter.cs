using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPET
{
    public class Filter
    {

        public List<double> LowFreg1(List<double> X,double F,double Td)
        {
            double k = (1 / (Math.PI * F * Td));
            List<double> Y = new List<double>() {0};
            double y0 = 0, x0;
            x0 = X[0];
            for (int i=1;i<X.Count();i++)
            {
                Y.Add(((y0 * (k - 1) + X[i] + x0) / (k + 1)));
                x0 = X[i];
                y0 = Y[i];
                Y[i] -= 5;
            }
            
            return Y;
        }

        
        static public void Meddiana(List<double> data,double Td, int size,out List<double> Y, out List<double> X)
        {
            int N = size;
            Y = new List<double>() { };
            X = new List<double>() { };
            List<double> Ybuf = new List<double>() { };
            List<double> Time = new List<double>() {};
            List<double> Xdata = new List<double>() { };
            for(int q=0;q< data.Count();q++)
            {
                Time.Add(Td*q);
            }
            List<double> Buffer = new List<double>() { };
            for (int i = 0; i < data.Count(); i+=N)
            {
                if (data.Count() - i -N>= 0)
                {
                    for (int a = 0; a < N; a++)
                    {
                        Buffer.Add(data[i + a]); 
                    }
                    Buffer.Sort();
                    Ybuf.Add(Buffer[Buffer.Count() / 2]);
                    Xdata.Add(Time[i + (int)Buffer.Count() / 2]);
                    Buffer.Clear();
                }
                else 
                {
                    for (int a = 0; a < data.Count-i; a++)
                    {
                        Buffer.Add(data[i + a]);
                    }
                    Buffer.Sort();
                    Ybuf.Add(Buffer[Buffer.Count() / 2]);
                    Xdata.Add(Time[i + (int)Buffer.Count() / 2]);
                    Buffer.Clear();
                }
                Y = Ybuf;
                X = Xdata;
            }

        }


        static public void MoveAverage(List <double> Y0,List<double> X0,out List<double> resultY,out List<double> resultX,int S)
        {
            resultY = new List<double> { };
            resultX = new List<double> { };
            int N = S;
            int n = 0;
            double temp=0;
            double[] M = new double[N];
            for (int i=0;i<Y0.Count();i++)
            {
                M[n] = Y0[i];
                n = (n + 1) % N;
                temp = 0;
                if (n==N-1)
                {
                    for (int j = 0; j < N; j++)
                    {
                        temp = temp + M[j];
                    }
                    resultY.Add(((double)temp / N));
                    resultX.Add(X0[i-N/2]);
                }
                
            }
        }

        static public List<double> RecursiveMoveAverage(List<double> Y0)
        {
            List<double> result = new List<double> { };
            int N = 20;
            int n = 0;
            double Y = 0;
            double[] M = new double[N];
            for (int i = 0; i < Y0.Count(); i++)
            {
                Y =Y+(Y0[i]-M[n]);
                M[n] = Y0[i];
                n = (n + 1) % N;
                result.Add((double)Y / N);
            }
            return result;
        }
    }
}
