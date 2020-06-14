using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPET
{
    
    public class loopVolumeFlow
    {
        public string MM { get; set; }
        public static double VT { get; private set; }//Tidal volume 
        public static double FVC { get; private set; }//Forced vital capacity
        public static double FEV05 { get; private set; }//Forced expiratory volume in 0,5 sec
        public static double FEV1 { get; private set; }//Forced expiratory volume in 1 sec
        public static double FEV3 { get; private set; }//Forced expiratory volume in 1 sec
        public static double PEF  { get; private set; }//Peak expiratory flow
        public static double PIF { get; private set; }//Peak inspiratory flow
        public static double FEF25 { get; private set; }//Forced expiratory flow during the 25% of FVC
        public static double FEF50 { get; private set; }//Forced expiratory flow during the 50% of FVC
        public static double FEF75 { get; private set; }//Forced expiratory flow during the 75% of FVC
        public static double FIF25 { get; private set; }//Forced inspiratory flow during the 25% of FVC
        public static double FIF50 { get; private set; }//Forced inspiratoryflow during the 50% of FVC
        public static double FIF75 { get; private set; }//Forced inspiratory flow during the 75% of FVC
        public static double MEF25_75 { get; private set; }//Mean of expiratory flow during the 25%-75% of FVC

        public static double VI { get; private set; }//Inspiration Volume VI=integral(Vins) - вдохнутый обьем [liters]
        public static double Y0 { get; private set; }//
        public static double currentvolumeexp { get; private set; }//
        public static double currentvolumeins { get; private set; }//
        public static double currenttime { get; private set; }//
        static double statusFEV = 0, statusFEF = 0, statusFIF = 0;
        static double buffer_PEF=0, buffer_PIF=0;
        static List<double> buffer_MEF25_75 = new List<double> {};
        public loopVolumeFlow(double dVI,double YO)
        {
            VI = dVI;
            Y0 = YO;
        }
        public static void Calculations(List<double> insVexp, List<double> insVins,double SampleTime)
        {
            VT = integral.TrapList(insVexp, SampleTime);//выдохнутый обьем
            VI = integral.TrapList(insVins, SampleTime);//вдутый обьем
            Y0 = insVexp[0];
            currentvolumeexp = 0;
            currentvolumeins = 0;
            currenttime = 0;
            statusFEV = 0;
            statusFEF = 0;
            statusFIF = 25;
            buffer_PEF = 0;
            buffer_PIF = 0;
            FEV3 = 0;
            FIF75 = 0;
            for (int i = 1; i < insVexp.Count(); i++)
            {
                currenttime += (SampleTime);
                currentvolumeexp += (insVexp[i] + Y0) * SampleTime * 0.5;
                Y0 = insVexp[i];
                DefinitionFEV(currenttime, currentvolumeexp);
                DefinitionPEF(insVexp[i]);
                DefinitionFEF(insVexp[i], currentvolumeexp,VT);
            }
            PEF=buffer_PEF;
            for (int i = insVins.Count()-1; i >= 0; i--)
            {
                currentvolumeins += (insVins[i] + Y0) * SampleTime * 0.5;
                Y0 = insVins[i];
                DefinitionPIF(insVins[i]);
                DefinitionFIF(insVins[i], currentvolumeins, VT);
            }
            PIF = buffer_PIF;
        }
        static void DefinitionFEV (double currenttime,double currentvolume)
        {
            if (currenttime>=0.5 && statusFEV==0)
            {
                FEV05 = currentvolume;
                statusFEV = 1;
            }
            else if (currenttime>=1 && statusFEV == 1)
            {
                FEV1 =currentvolume;
                statusFEV = 3;
            }
            else if (currenttime>=3 && statusFEV == 3)
            {
                FEV3 =currentvolume;
                statusFEV = 4;
            }
        }
        static void DefinitionPEF(double currentflow)
        {
            if (buffer_PEF<currentflow)
            {
                buffer_PEF = currentflow;
            }
        }
        static void DefinitionPIF(double currentflow)
        {
            if (buffer_PIF < currentflow)
            {
                buffer_PIF = currentflow;
            }
        }
        static void DefinitionFEF(double currentflow,double currentvolume,double totalvolume)
        {
            if (currentvolume >= 0.25*totalvolume && statusFEF == 0)
            {
                FEF25 = currentflow;
                statusFEF = 1;
            }
            else if (currentvolume >= 0.5 * totalvolume && statusFEF == 1)
            {
                FEF50 = currentflow;
                statusFEF = 3;
            }
            else if (currentvolume >= 0.75 * totalvolume && statusFEF == 3)
            {
                DefinitionMEF25_75(currentflow, statusFEF);
                FEF75 = currentflow;
                statusFEF = 4;
            }
            DefinitionMEF25_75(currentflow,statusFEF);
        }
        static void DefinitionFIF(double currentflow, double currentvolume, double totalvolume)
        {
            if (currentvolume >= 0.25 * totalvolume &&  statusFIF == 25)
            {
                FIF25 = currentflow;
                statusFIF = 50;
            }
            else if (currentvolume >= 0.5 * totalvolume &&  statusFIF == 50)
            {
                FIF50 = currentflow;
                statusFIF = 75;
            }
            else if (currentvolume >= 0.75 * totalvolume &&  statusFIF == 75)
            {
                FIF75 = currentflow;
                statusFIF = 1;
            } 
        }
        static void DefinitionMEF25_75(double currentflow,double status)
        {
            if (status==1||status==3)
            {
                buffer_MEF25_75.Add(currentflow);
            }
            else if (status==4)
            {
                MEF25_75 = buffer_MEF25_75.Average();
            }
            else if (status==0)
            {
                buffer_MEF25_75.Clear();
            }
        }
    }
}
