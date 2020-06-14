using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integral
{
    class RespiratoryCalc
    {
        /*
         * t0-begining of Breath [secondes]
         * t1-end of inspiration,begining of expiration [secondes]
         * t2-ending of breath [secondes]
         */
        public double _BF { get; private set; }//Breath frequency BR=60/(t2-t0) [minute^-1]
        public double _difPetO2 { get; private set; }//End-tidal PO2 difference (difPetO2) = PO2 (lowest expired value) - PO2 (inspired) [mmHg][vol.%] ?
        public double _PetCO2 { get; private set; }//End-tidal PCO2 (PetCO2) = PCO2 (highest expired value) [mmHg]
        public double _PECO2 { get; private set; }//  [mmHg]
        public double _PaCO2 { get; private set; }//partial of pressure CO2 in arterial blood  [mmHg]
        public double _PaO2 { get; private set; }//partial of pressure O2 in arterial blood [mmHg]
        public double _PACO2 { get; private set; }//partial of pressure CO2 in alveoles  [mmHg]
        public double _PAO2 { get; private set; }//partial of pressure O2 in alveoles [mmHg]
        public double _Aa_DO2 { get; private set; }//[mmHg]
        public double _aET_DCO2 { get; private set; }//[mmHg]
        public double _DLO2 { get; private set; }//
        public double _VO2 { get; private set; }//O2 consumption rate [liters/min]
        public double _VO2pred { get; private set; }//O2 consumption rate predictable [liters/min]
        public double _VCO2 { get; private set; }//CO2 production rate[liters/min]
        public double _VT { get; private set; }//Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _VE { get; private set; }//Minute ventilation VE = VT*BR [liters/minute]
        public double _VA { get; private set; }//Alveoleles Minute ventilation VE = VT*BR [liters/minute]
        public double _RER { get; private set; }//Respiratory gas exchange ratio RER=VCO2/VO2 [None]
        public double _EQO2 { get; private set; }//Minute ventilation VE = VT*BR [liters/minute]
        public double _EQCO2 { get; private set; }//Respiratory gas exchange ratio RER=VCO2/VO2 [None]
        public double _METS { get; private set; }//METS
        public double _BW { get; private set; }//Body weight [kg]
        public double _W { get; private set; }//Width [Watts]
        public double _VD { get; private set; }//Death volume 
        public double _VDm { get; private set; }//Death volume of mouth

        //_instantO2ins,  _instantCO2ins, insVins - мгновенные значения для периода вдоха; _instantO2exp, _instantCO2exp, _instantFlowExp - мгновенные значения для периода выдоха
        public double FIO2 { get; private set; }
        public double FICO2 { get; private set; }
        public double FAO2 { get; private set; }
        public double FACO2 { get; private set; }
        public double insVins { get; private set; }
        public double FEO2 { get; private set; }
        public double FECO2 { get; private set; }

        public double insVexp { get; private set; }
        public List<double> insVinsDATA = new List<double> {0};
        public List<double> insVexpDATA = new List<double> {0};
        public List<double> FIO2DATA = new List<double> { 0 };
        public List<double> FEO2DATA = new List<double> { 0 };
        public List<double> FICO2DATA = new List<double> { 0 };
        public List<double> FECO2DATA = new List<double> { 0 };

        //VO2ins,VCO2ins,Vins- посчитанные обьемы по мгновенным значениям на вдохе;VO2exp,VCO2exp,Vexp- посчитанные обьемы по мгновенным значениям на выдохе
        public  double VO2ins { get; private set; }
        public double VCO2ins { get; private set; }
        public  double Vins { get; private set; }
        public  double VO2exp { get; private set; }
        public double VCO2exp { get; private set; }
        public double PROBAVE { get; private set; }
        public double PROBAVI { get; private set; }


        public double _dO2, _dCO2, _kdO2 = 0, _kdCO2 = 0;//_dO2,_dCO2 - текущие дифференциалы по O2,CO2;_kdO2,_kdCO2- предыдущие дифференциалы по O2,CO2
        public double FETO2 = 0, FITO2 = 0, FETCO2 = 0, FITCO2 = 0;//FITO2,FITCO2 - значение концентраций в конце вдоха; FETO2,FETCO2 - значение концентраций в конце выдоха
        public double _kO2, _kCO2;

        integral IntegralV = new integral();

        Derivate O2 = new Derivate();
        Derivate CO2 = new Derivate();

        public double _kH { get; private set; }
        public double _VO2_HT { get; private set; }//O2 consumption rate [liters/min]
        public double _VCO2_HT { get; private set; }//CO2 production rate[liters/min]

        public double _kE { get; private set; }
        public double _VO2_ET { get; private set; }//O2 consumption rate [liters/min]
        public double _VCO2_ET { get; private set; }//CO2 production rate[liters/min]
        public void Inspiration (double dataFlow, double dataO2, double dataCO2, double SampleTime, double ZeroLine)
        {
            insVinsDATA.Add(dataFlow - ZeroLine);
            FIO2DATA.Add(dataO2);
            FICO2DATA.Add(dataCO2);

            FETO2andFETCO2(dataO2, dataCO2, SampleTime);
            FIO2 = (dataO2+_kO2)*0.5;
            FICO2 = (dataCO2 +_kCO2)* 0.5;
            insVins = IntegralV.Trap(dataFlow - ZeroLine, SampleTime);
            Vins += insVins;
            VO2ins += insVins * FIO2 * 0.01;
            VCO2ins += insVins * FICO2 * 0.01;
        }

        public void EndInspiration(double dataFlow, double dataO2, double dataCO2, double SampleTime, double ZeroLine)
        {
            insVinsDATA.Add(dataFlow - ZeroLine);
            FIO2DATA.Add(dataO2);
            FICO2DATA.Add(dataCO2);

            FETO2andFETCO2(dataO2, dataCO2, SampleTime);

            FIO2 = (dataO2 + _kO2) * 0.5;
            FICO2 = (dataCO2 + _kCO2) * 0.5;
            insVins = IntegralV.Trap(dataFlow-ZeroLine, SampleTime);
            Vins += insVins;
            VO2ins += insVins * FIO2 * 0.01;
            VCO2ins += insVins * FICO2 * 0.01;
        }
        public void Expiration(double dataFlow, double dataO2, double dataCO2, double SampleTime,double ZeroLine)
        {
            insVexpDATA.Add(ZeroLine - dataFlow);
            FEO2DATA.Add(dataO2);
            FECO2DATA.Add(dataCO2);

            FETO2andFETCO2(dataO2, dataCO2, SampleTime);

            FEO2 = (dataO2 + _kO2) * 0.5;
            FECO2 = (dataCO2 + _kCO2) * 0.5;
            insVexp = IntegralV.Trap(ZeroLine-dataFlow, SampleTime);
            VO2exp += insVexp * FEO2 * 0.01;
            VCO2exp += insVexp * FECO2 * 0.01;
            _VT = VT(insVexp);
        }
        public void EndExpiration(double dataFlow, double dataO2, double dataCO2, double SampleTime, double ZeroLine,double t0, double t2)
        {
            insVexpDATA.Add(ZeroLine - dataFlow);
            FEO2DATA.Add(dataO2);
            FECO2DATA.Add(dataCO2);

            FETO2andFETCO2(dataO2, dataCO2, SampleTime);
            FEO2 = (dataO2 + _kO2) * 0.5;
            FECO2 = (dataCO2 + _kCO2) * 0.5;
            insVexp = IntegralV.Trap(ZeroLine - dataFlow, SampleTime);
            VO2exp += insVexp * FEO2*0.01;
            VCO2exp += insVexp * FECO2 * 0.01;
            _VT = VT(insVexp);
            _BF =BF(t0,t2);
            _VE =VE();
            Standart();
            Haldane_transformation();
            Eschenbacher_transformation();
            _RER = RER();
            _EQO2 =EQO2();
            _EQCO2=EQCO2();
            _VO2pred = VO2pred();
            PROBAVE = integral.TrapList(insVexpDATA, SampleTime);
            PROBAVI = integral.TrapList(insVinsDATA, SampleTime);
        }
        
        public void Standart()
        {
            _VCO2 = (VCO2exp - VCO2ins)*_BF;
            _VO2 = (VO2ins - VO2exp)* _BF;
        }
        public void WASSERMAN1973(double Vbv = 0, double FIO2 = 0, double FETCO2 = 0, double FEH2O = 0, double FRH2O = 0)
        {
            _VCO2 = VCO2exp - Vbv * FETCO2;
            _VO2 = ((VO2ins - VO2exp) - Vbv * (FIO2 - FETO2) - FIO2 * (_VCO2 + _VE * (FEH2O - FRH2O))) / (1 - FIO2);
        }
        public void Haldane_transformation ()
        {

            _kH = (1 - FEO2DATA.Average()*0.01 - FECO2DATA.Average() * 0.01) / (1 - FIO2DATA.Average() * 0.01 - FICO2DATA.Average() * 0.01);
            _VCO2_HT = _VE * FECO2DATA.Average() * 0.01;
            _VO2_HT = _VE * (_kH *  FIO2DATA.Average() * 0.01 - FEO2DATA.Average() * 0.01);

        }
        public void Eschenbacher_transformation()
        {
            _kE = (1 - FECO2DATA.Average() * 0.01 + FICO2DATA.Average() * 0.01) /(1- FIO2DATA.Average() * 0.01 + FEO2DATA.Average() * 0.01);
            _VCO2_ET = _VE * FECO2DATA.Average() * 0.01;
            _VO2_ET = _VE*_kE*(FIO2DATA.Average() * 0.01 - FEO2DATA.Average() * 0.01);
        }
        public void FRC()
        {
            
        }
        public double VO2pred(int number=0)
        {
            switch (number)
            {
                case 0:
                   return 5.8 * _BW + 151 + 10.3 * _W;
                case 1:
                   return 2*_W+3.5*_BW;
                default:
                    return 909;
            }
        }
        
         public double RER()
        {
            return _VCO2 / _VO2;
        }

        public double EQO2(double VDm = 0)
        {
            return (_VE-VDm*_BF) / _VO2;
        }
        public double EQCO2(double VDm=0,int number=0)
        {
            switch (number)
            {
                case 0:
                    return (_VE - VDm * _BF) / _VCO2;
                case 1:
                    return 1/(_PaCO2*(1-_VD/_VT));
                default:
                    return 910;
            }  
        }

        public double VT (double insVexp)
        {
            return _VT+=insVexp;
        }
        public double VE (int number=0)
        {
            switch (number)
            {
                case 0:
                    return _VT * _BF;
                case 1:
                    return (_VCO2*863)/(_PaCO2*(1-_VD/_VT));
                default:
                    return 909;
            }
            
        }
        public double VA(int number = 0)
        {
            switch (number)
            {
                case 0:
                    return _VO2/(FIO2-FAO2);
                case 1:
                    return _VE-_BF*(_VD+_VDm);
                default:
                    return 909;
            }

        }
        public double VD(double VT, double PaCO2, double PECO2, double VDm)
        {
            return (VT * (PaCO2 - PECO2) / PaCO2) - VDm;
        }
        public double BF (double t0,double t2)
        {
            return 60/(t2 - t0);
        }


        public double PaCO2(int number=0,double VCO2=0, double VA=0,double PETCO2=0,double VT=0)
        { 
            switch (number)
            {
                case 0:
                    return VCO2 * 863 / VA;
                case 1:
                    return 5.5+(0.9*PETCO2)-(0.0021*VT);
                default:
                    return 909;
            }
        }
        public double PACO2(double FETCO2)
        {
            return FETCO2;
        }
        public double PAO2(List<double> FIO2, double PaCO2,double RER)
        {
            return FIO2.Average() * 0.01*713-PaCO2/RER;
        }
        public double PECO2(double t0, double t2)
        {
            return 60 ;
        }
        public double Aa_DO2(double PACO2, double PaCO2)
        {
            return PACO2 - PaCO2;
        }
        public double aET_DCO2(double PaCO2, double PETCO2)
        {
            return PaCO2-PETCO2;
        }
        public double DLO2(double VO2, double AaDO2)
        {
            return VO2 / AaDO2;
        }


        public double METS(double VO2, double BW)
        {
            return VO2/(3.5*BW);
        }


        public double C1_fromATPtoSTPD(double Ta=37)
        {
            return (273*(760-47))/((273+Ta)*760);
        }
        public double C2_fromBTPStoSTPD(double RH, double PH2O,double Ta)
        {
            return (273 * (760 - RH*PH2O/100)) / ((273 + Ta) * 760);
        }
        public double C3_fromATPStoSTPD(double PH2O, double Ta)
        {
            return (273 * (760 - PH2O )) / ((273 + Ta) * 760);
        }


        public void ClearBuffer()
        {
            VO2exp = 0;
            VCO2exp = 0;
            VO2ins = 0;
            VCO2ins = 0;
            Vins = 0;
            _VT = 0;
            insVinsDATA.Clear();
            insVexpDATA.Clear();
            insVexpDATA.Add(0);
            insVinsDATA.Add(0);
            FIO2DATA.Clear();
            FICO2DATA.Clear();
            FEO2DATA.Clear();
            FECO2DATA.Clear();
        }
        public void FETO2andFETCO2(double dataO2, double dataCO2,double SampleTime)
        {
            //Дифференциал O2
            _dO2 = O2.CommonDifference(dataO2, SampleTime);
            //Дифференциал CO2
            _dCO2 = CO2.CommonDifference(dataCO2, SampleTime);

            //выделение Максимумов и минимумов О2 по изменению знака производной
            if (Math.Sign(_dO2) == 1 && Math.Sign(_kdO2) == -1)
            {
                FETO2 = _kO2;
            }
            else if (Math.Sign(_dO2) == -1 && Math.Sign(_kdO2) == 1)
            {
                FITO2 = _kO2;
            }
            //Выделение максимов и минимумов СО2 по изменению знака производной
            if (Math.Sign(_dCO2) == 1 && Math.Sign(_kdCO2) == -1)
            {
                FITCO2 = _kCO2;
            }
            else if (Math.Sign(_dCO2) == -1 && Math.Sign(_kdCO2) == 1)
            {
                FETCO2 = _kCO2;
            }
            _kdCO2 = _dCO2;
            _kdO2 = _dO2;
        }
    } 
}
            