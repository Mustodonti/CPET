using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CPET
{
    public class RespiratoryCalc
    {
        public double _VT { get; private set; }
        public double _VI { get; private set; }//Inspiration Volume VI=integral(Vins) - вдохнутый обьем [liters]
        public double _VE { get; private set; }//Minute ventilation VE = VT*BR [liters/minute]
        public double _VA { get; private set; }//Alveoleles Minute ventilation VE = VT*BR [liters/minute]
        public double _VD { get; private set; }//Death volume 
        public double _VDm { get; private set; }//Death volume of mouth


        public double _VO2 { get; private set; }//O2 consumption rate [liters/min]
        public double _VO2pred { get; private set; }//O2 consumption rate predictable [liters/min]
        public double _VCO2 { get; private set; }//CO2 production rate[liters/min]
        public double _VCO2_s { get; private set; }
        public double _VO2_s { get; private set; }
        public double _kH { get; private set; }
        public double _VO2_HT { get; private set; }//O2 consumption rate [liters/min]
        public double _VCO2_HT { get; private set; }//CO2 production rate[liters/min]
        public double _kE { get; private set; }
        public double _VO2_ET { get; private set; }//O2 consumption rate [liters/min]
        public double _VCO2_ET { get; private set; }//CO2 production rate[liters/min]


       
        public double _PetCO2 { get; private set; }//End-tidal PCO2 (PetCO2) = PCO2 (highest expired value) [mmHg]
        public double _PECO2 { get; private set; }//  [mmHg]
        public double _PaCO2 { get; private set; }//partial of pressure CO2 in arterial blood  [mmHg]
        public double _PaO2 { get; private set; }//partial of pressure O2 in arterial blood [mmHg]
        public double FAO2 { get; private set; }
        public double FACO2 { get; private set; }
        public double _PACO2 { get; private set; }//partial of pressure CO2 in alveoles  [mmHg]
        public double _PAO2 { get; private set; }//partial of pressure O2 in alveoles [mmHg]
        public double _FETCO2 { get; private set; }//FETCO2 - значение концентраций в конце выдоха
        public double _FETO2 { get; private set; }//FETO2 - значение концентраций в конце выдоха
        public double _Aa_DO2 { get; private set; }//[mmHg]
        public double _aET_DCO2 { get; private set; }//[mmHg]
        public double _DLO2 { get; private set; }//
       
       
        public double _EQO2 { get; private set; }//Minute ventilation VE = VT*BR [liters/minute]
        public double _EQCO2 { get; private set; }//Respiratory gas exchange ratio RER=VCO2/VO2 [None]
        public double _METS { get; private set; }//METS
        public double _RER { get; private set; }
        public double _BF { get; private set; }
        public double _BW { get; private set; }//Body weight [kg]
        public double _W { get; private set; }//Width [Watts]

        
        


        

        public List<double> FlowinsDATA = new List<double> {};
        public List<double> FlowexpDATA = new List<double> {};
        public List<double> VexpDATA = new List<double> {};
        public List<double> FIO2DATA = new List<double> {};
        public List<double> FEO2DATA = new List<double> {};
        public List<double> FICO2DATA = new List<double> {};
        public List<double> FICO2DATAbuf = new List<double> {};
        public List<double> FECO2DATA = new List<double> {};
        public List<double> LMSCO2DATA_Y = new List<double> {};
        public List<double> LMSCO2DATA_Y1= new List<double> {};
        public List<double> ExampleCO2DATA_Y = new List<double> {};
        public List<double> LMSCO2DATA_X = new List<double> {};
        public List<double> LMSCO2DATA_X1 = new List<double> {};
        public List<double> ExampleCO2DATA_X = new List<double> {};
        public List<double> LSM_DATAX = new List<double> {};
        public List<double> LSM_DATAY = new List<double> {};
        public List<double> LSM_DATAX1 = new List<double> {};
        public List<double> LSM_DATAY1= new List<double> {};
        public List<double> Tins = new List<double> {};
        public List<double> Texp = new List<double> {};
        public List<double> dCO2ins = new List<double> {};
        public List<double> dCO2exp = new List<double> {};
        public List<double> CO2DATA = new List<double> {};
        public List<double> Time= new List<double> {};
        //VO2ins,VCO2ins,Vins- посчитанные обьемы по мгновенным значениям на вдохе;_VO2exp,VCO2exp,Vexp- посчитанные обьемы по мгновенным значениям на выдохе
        public double _VO2ins { get; private set; }
        public double _VCO2ins { get; private set; }
        public double _VO2exp { get; private set; }
        public double _VCO2exp { get; private set; }
        public double _Flowexp { get; private set; }
        public double _Flowins { get; private set; }
        public double Vins { get; private set; }
        public double Vexp { get; private set; }
        



        public double _dO2, _dCO2, _kdO2 = 0, _kdCO2 = 0;//_dO2,_dCO2 - текущие дифференциалы по O2,CO2;_kdO2,_kdCO2- предыдущие дифференциалы по O2,CO2
        public double _kO2, _kCO2, _CO2, _O2, _CO2A, _O2A, t;
        public double _bN = 0;
        public double _tins = 0, _texp = 0;
        public double A, B = 0;
        public double A1, B1 = 0;
        public double _status = 0, tVD = 0;
        int index;
        public string status;

        integral IntegralV = new integral();

        Derivate CO2 = new Derivate();

       



        public void Inspiration(double dataFlow, double dataO2, double dataCO2, double SampleTime, double ZeroLine,double tins=0)
        {
            status = "ins";

            _O2 = dataO2;
            _CO2 = dataCO2;


            //// усредненное значение за время между приходом данных
            //FIO2 = (_O2 + _kO2) * 0.5;
            //FICO2 = (_CO2 + _kCO2) * 0.5;

            _Flowins = IntegralV.Trap(dataFlow - ZeroLine, SampleTime)* C1_fromATPtoSTPD();// мгновенный обьем, который был вдохнут в течении одного такта 
            Vins += _Flowins;//Сумма мгновенных вдохнутых обьемов
            _VO2ins += _Flowins * _O2 * 0.01;
            _VCO2ins += _Flowins * _CO2 * 0.01;

            Tins.Add(t);
            FlowinsDATA.Add(dataFlow - ZeroLine);
            FIO2DATA.Add(_O2);
            FICO2DATA.Add(_CO2);
        }

        public void EndInspiration(double dataFlow, double dataO2, double dataCO2,  double SampleTime, double ZeroLine, double tins=0)
        {
            status = "eins";
            Tins.Add(t);
            if (dataFlow - ZeroLine < 0)
            {
                dataFlow = 0;
            }
            
            _O2 = dataO2;
            _CO2 = dataCO2;
            _tins = tins;

            //FIO2 = (_O2 + _kO2) * 0.5;
            //FICO2 = (_CO2 + _kCO2) * 0.5;

            _Flowins = IntegralV.Trap(dataFlow - ZeroLine, SampleTime);
            Vins += _Flowins* C1_fromATPtoSTPD();
            _VO2ins += _Flowins * _O2 * 0.01;
            _VCO2ins += _Flowins * _CO2 * 0.01;

            Tins.Add(t);
            FlowinsDATA.Add(dataFlow - ZeroLine);
            FIO2DATA.Add(_O2);
            FICO2DATAbuf.Add(_CO2);

            _VI = VI(FlowinsDATA, SampleTime);
            ClearBuffer(status);
        }
        public void Expiration(double dataFlow, double dataO2, double dataCO2,double SampleTime, double ZeroLine,double texp=0)
        {
            if (ZeroLine - dataFlow < 0)
            {
                dataFlow = 0;
            }
            status = "exp";

           
            _O2 = dataO2;
            _CO2 = dataCO2;
            _texp = texp;

            //FEO2 = (_O2 + _kO2) * 0.5;
            //FECO2 = (_CO2 + _kCO2) * 0.5;

            _Flowexp = IntegralV.Trap(ZeroLine - dataFlow, SampleTime);
            Vexp += _Flowexp* C2_fromBTPStoSTPD();
            VexpDATA.Add(Vexp);
            _VO2exp += _Flowexp * _O2 * 0.01;
            _VCO2exp += _Flowexp * _CO2 * 0.01;

            Texp.Add(t);
            FlowexpDATA.Add(ZeroLine - dataFlow);
            FEO2DATA.Add(_O2);
            FECO2DATA.Add(_CO2);

            VDdetect(_CO2, SampleTime, texp);
        }
        public void EndExpiration(double dataFlow, double dataO2, double dataCO2, double SampleTime, double ZeroLine, double t0, double t2, double texp=0)
        {
            status = "eexp";
            if (ZeroLine - dataFlow < 0)
            {
                dataFlow = 0;
            }
            
            _O2 = dataO2;
            _CO2 = dataCO2;

            //FEO2 = (_O2 + _kO2) * 0.5;
            //FECO2 = (_CO2 + _kCO2) * 0.5;

            _Flowexp = IntegralV.Trap(ZeroLine - dataFlow, SampleTime);
            Vexp += _Flowexp* C2_fromBTPStoSTPD();
            VexpDATA.Add(Vexp);
            _VO2exp += _Flowexp * _O2 * 0.01;
            _VCO2exp += _Flowexp * _CO2 * 0.01;

            Texp.Add(t);
            FlowexpDATA.Add(ZeroLine - dataFlow);
            FEO2DATA.Add(_O2);
            FECO2DATA.Add(_CO2);

            FETO2andFETCO2_test(FECO2DATA, FEO2DATA, 5);//определение конечных значений по 5 последним значениям
            _VT = VT(FlowexpDATA, SampleTime);
            _BF = BF(t0, t2);
            _VE = VE();
            //Standart();
            Standarts(FlowinsDATA, FlowexpDATA, FIO2DATA, FICO2DATA, FEO2DATA, FECO2DATA,_BF,SampleTime);
            Haldane_transformation();
            Eschenbacher_transformation();
            _RER = RER(_VO2_ET,_VCO2_ET);
            _EQO2 = EQO2(_VE,_VO2_ET,_BF);
            _EQCO2 = EQCO2();
            _VO2pred = VO2pred();
            loopVolumeFlow.Calculations(FlowexpDATA, FlowinsDATA, SampleTime);
            //VD_Graphic(SampleTime);
            VDdetect(_CO2, SampleTime, texp);
            VD_Calcilations(FECO2DATA, FICO2DATA, Texp,Tins, dCO2exp, SampleTime);
            _VA = VA(2);
            _PaCO2 = PaCO2(0, _VCO2_ET, _VA);
            _PAO2 = PAO2(FIO2DATA, _PaCO2, _RER);
            _PACO2 = PACO2(_FETCO2);
            _PECO2 = PECO2(_VCO2_ET, _VE);
            _METS = METS(_VO2_ET, 76);


        }

        public void Standart()
        {
            _VCO2 = (_VCO2exp - _VCO2ins) * _BF;
            _VO2 = (_VO2ins - _VO2exp) * _BF;
        }
        public void Standarts(List <double> FlowinsData, List<double> FlowexpData, List<double> FIO2DATA, List<double> FICO2DATA, List<double> FEO2DATA, List<double> FECO2DATA,double BF, double SampleTime)
        {
            double VO2ins_s = 0, VO2exp_s = 0;
            IntegralV.y0 = 0;
            FlowinsDATA.ForEach(i => VO2ins_s += (IntegralV.Trap(i, SampleTime))* C1_fromATPtoSTPD()*FIO2DATA[FlowinsDATA.IndexOf(i)] * 0.01);
            IntegralV.y0 = 0; //Нужно ли снова ставить 0?
            FlowexpDATA.ForEach(i => VO2exp_s += (IntegralV.Trap(i, SampleTime)) * C2_fromBTPStoSTPD()*FEO2DATA[FlowexpDATA.IndexOf(i)] * 0.01);
            _VO2_s = (VO2ins_s - VO2exp_s) * BF;

            double VCO2exp_s = 0;
            IntegralV.y0 = 0;
            //FlowinsDATA.ForEach(i => VCO2ins_s += (IntegralV.Trap(i, SampleTime)) * C1_fromATPtoSTPD() * FICO2DATA[FlowinsDATA.IndexOf(i)] * 0.01); нужно ли нам это?
            //IntegralV.y0 = 0; Нужно ли снова ставить 0?
            FlowexpDATA.ForEach(i => VCO2exp_s += (IntegralV.Trap(i, SampleTime)) * C2_fromBTPStoSTPD() * FECO2DATA[FlowexpDATA.IndexOf(i)] * 0.01);
            _VCO2_s = (VCO2exp_s) * BF;
        }
        public void WASSERMAN1973(double Vbv = 0, double FIO2 = 0, double FETCO2 = 0, double FEH2O = 0, double FRH2O = 0)
        {
            _VCO2 = _VCO2exp - Vbv * FETCO2;
            _VO2 = ((_VO2ins - _VO2exp) - Vbv * (FIO2 - _FETO2) - FIO2 * (_VCO2 + _VE * (FEH2O - FRH2O))) / (1 - FIO2);
        }
        public void Haldane_transformation()
        {
            if (FIO2DATA.Count() != 0 )
            {
                _kH = (1 - FEO2DATA.Average() * 0.01 - FECO2DATA.Average() * 0.01) / (1 - FIO2DATA.Average() * 0.01);
                _VCO2_HT = _VE * FECO2DATA.Average() * 0.01;
                _VO2_HT = _VE * (_kH * FIO2DATA.Average() * 0.01 - FEO2DATA.Average() * 0.01);
            }


        }
        public void Eschenbacher_transformation()
        {
            if (FIO2DATA.Count() != 0 )
            {
                _kE = (1 - FECO2DATA.Average() * 0.01 ) / (1 - FIO2DATA.Average() * 0.01 + FEO2DATA.Average() * 0.01);
                _VCO2_ET = _VE * FECO2DATA.Average() * 0.01;
                _VO2_ET = _VE * _kE * (FIO2DATA.Average() * 0.01 - FEO2DATA.Average() * 0.01);
            }

        }
        public void FRC()
        {

        }
        public double VO2pred(int number = 0)
        {
            switch (number)
            {
                case 0:
                    return 5.8 * _BW + 151 + 10.3 * _W;
                case 1:
                    return 2 * _W + 3.5 * _BW;
                default:
                    return 909;
            }
        }

        public double RER(double VO2, double VCO2)
        {
            return VCO2 / VO2;
        }

        public double EQO2(double VE, double VO2, double BF,double VDm = 0)
        {
            return (VE - VDm * BF) / VO2;
        }
        public double EQCO2(double VDm = 0, int number = 0)
        {
            switch (number)
            {
                case 0:
                    return (_VE - VDm * _BF) / _VCO2_ET;
                case 1:
                    return 1 / (_PaCO2 * (1 - _VD / _VT));
                default:
                    return 910;
            }
        }
        public double VT(List<double> _Flowexp, double SampleTime)
        {
            return integral.TrapList(_Flowexp, SampleTime, C2_fromBTPStoSTPD());
        }
        public double VI(List<double> _Flowins, double SampleTime)
        {
            return integral.TrapList(_Flowins, SampleTime);
        }
        public double VE(int number = 0)
        {
            switch (number)
            {
                case 0:
                    return _VT * _BF;
                case 1:
                    return (_VCO2 * 863) / (_PaCO2 * (1 - _VD / _VT));
                default:
                    return 909;
            }

        }
        public double VA(int number = 0)
        {
            switch (number)
            {
                case 0:
                    return _VO2 / (FIO2DATA.Average() - FAO2);
                case 1:
                    return _VE - _BF * (_VD + _VDm);
                case 2:
                    return _VE - _BF * _VD;
                default:
                    return 909;
            }

        }
        public double VD(double VT, double PaCO2, double PECO2, double VDm)
        {
            //LineRegress.LSM()
            return (VT * (PaCO2 - PECO2) / PaCO2) - VDm;
        }
        public double BF(double t0, double t2)
        {
            if ((t2 - t0) != 0)
            {
                _bN += 1;
            }
            return 60 / (t2 - t0);
        }


        public double PaCO2(int number = 0, double VCO2 = 0, double VA = 0, double PETCO2 = 0, double VT = 0)
        {
            switch (number)
            {
                case 0:
                    return VCO2 * 863 / VA;
                case 1:
                    return 5.5 + (0.9 * PETCO2) - (0.0021 * VT);
                default:
                    return 909;
            }
        }
        public double PACO2(double FETCO2)
        {
            return FETCO2*7.13;
        }
        public double PAO2(List<double> FIO2, double PaCO2, double RER)
        {
            return FIO2.Average() * 0.01 * 713 - PaCO2 / RER;
        }
        public double PECO2(double VCO2, double VE)
        {
            return VCO2*713/VE;
        }
        public double Aa_DO2(double PACO2, double PaCO2)
        {
            return PACO2 - PaCO2;
        }
        public double aET_DCO2(double PaCO2, double PETCO2)
        {
            return PaCO2 - PETCO2;
        }
        public double DLO2(double VO2, double AaDO2)
        {
            return VO2 / AaDO2;
        }

        public double METS(double VO2, double BW)
        {
            return VO2*1000 / (3.5 * BW);
        }


        public double C1_fromATPtoSTPD(double Ta = 37)
        {
            return (273 * (760 - 47)) / ((273 + Ta) * 760);//0.826
        }
        public double C2_fromBTPStoSTPD(double RH=100, double PH2O=47, double Ta=37)
        {
            return (273 * (760 - RH * PH2O / 100)) / ((273 + Ta) * 760);//0.826
        }
        public double C3_fromATPStoSTPD(double PH2O, double Ta)
        {
            return (273 * (760 - PH2O)) / ((273 + Ta) * 760);
        }


        public void ClearBuffer(string status)
        {
            switch (status)
            {
                case "eexp":
                    //_Flowexp = 0; нельзя обнулять так как статус для построения петли поток обьем
                    // _Flowins = 0;
                    CO2DATA.Clear();
                    Time.Clear();
                    _VCO2 = 0;
                    _VO2 = 0;
                    //_VO2exp = 0;
                    //_VO2ins = 0;
                    _VO2exp = 0;
                    _VCO2exp = 0;
                    _VO2ins = 0;
                    _VCO2ins = 0;
                    Vins = 0;
                    Texp.Clear();
                    Tins.Clear();
                    dCO2exp.Clear();
                    dCO2ins.Clear();
                    FlowinsDATA.Clear();
                    FlowexpDATA.Clear();
                    //FlowexpDATA.Add(0);
                    //FlowinsDATA.Add(0);
                    FIO2DATA.Clear();

                    FEO2DATA.Clear();

                    //Данные нужные для VD_Graphic 
                    LMSCO2DATA_Y.Clear();
                    ExampleCO2DATA_Y.Clear();
                    LMSCO2DATA_X.Clear();
                    ExampleCO2DATA_X.Clear();
                    _status = 0;
                    VexpDATA.Clear();
                    FECO2DATA.Clear();
                    Vexp = 0;
                    LSM_DATAX.Clear();
                    LSM_DATAY.Clear();
                    break; 
                case "eins":
                    FICO2DATA = FICO2DATAbuf;
                    FICO2DATAbuf.Clear();
                    break;
                case "exp":
                    
                    break;
                case "ins":
                    
                    break;
                default:
                    MessageBox.Show("Error in ClearBuffer");
                    break;
            }

        }

        public void FETO2andFETCO2_test(List <double> FECO2DATA, List<double> FEO2DATA,int N)
        {
            List<double> FECO2list = new List<double>() {};
            List<double> FEO2list = new List<double>() {};
            for (int i = FECO2DATA.Count()-N-1;i<FECO2DATA.Count()-1;i++)
            {
                FECO2list.Add(FECO2DATA[i]);
            }
            for (int i = FEO2DATA.Count() - N-1; i < FEO2DATA.Count()-1; i++)
            {
                FEO2list.Add(FEO2DATA[i]);
            }
            _FETCO2 = FECO2list.Average();
            _FETO2 = FEO2list.Average();
        }

        public void VDdetect( double dataCO2 ,double SampleTime,double texp)
        {
            ////Дифференциал O2
            //_dO2 = O2.CommonDifference(dataO2, SampleTime);
            //Дифференциал CO2
            _dCO2 = CO2.CommonDifference(dataCO2, SampleTime);
            if (status == "exp" || status == "eexp")
            {
                dCO2exp.Add(_dCO2);
                //Выборка для линейной регрессии
                if (((double)_dCO2 - (double)_kdCO2) < 0)
                {
                    LMSCO2DATA_Y.Add(dataCO2);
                    LMSCO2DATA_X.Add(t - texp);
                }
                //Выборка для линейной регрессии выборка от FITCO2 до FETCO2 - как бы вырезаем часть графика FECO2
                if (_status == 1)
                {
                    ExampleCO2DATA_Y.Add(dataCO2);
                    ExampleCO2DATA_X.Add(t - texp);
                }
            }
            _kdCO2 = _dCO2;
        }

        public void VD_Calcilations(List<double> FECO2DATAY, List<double> FICO2DATA, List<double> Texp, List<double> Tins, List<double> dCO2exp, double SampleTime)
        {
            double _average_dCO2exp = (dCO2exp.Average()+ dCO2exp.Max())/2;
            List<double> Texp_p = new List<double>() { };
            List<double> FECO2DATA = new List<double>() { };
            FECO2DATA.Add(CO2DATA[Time.IndexOf(Texp[0])-2]);
            FECO2DATA.Add(CO2DATA[Time.IndexOf(Texp[0]) - 1]);
            Texp_p.Add(0);
            Texp_p.Add(Time[Time.IndexOf(Texp[0])-1]- Time[Time.IndexOf(Texp[0]) - 2]);
            int trigger = 0;
            LMSCO2DATA_X.Clear();
            LMSCO2DATA_Y.Clear();
            LMSCO2DATA_X1.Clear();
            LMSCO2DATA_Y1.Clear();

            LSM_DATAX.Clear();
            LSM_DATAY.Clear();
            for (int i = 0; i < FECO2DATAY.Count(); i++)
            {
                FECO2DATA.Add(FECO2DATAY[i]);
                if (trigger == 0)
                {
                    if( FECO2DATAY[i+1]!=0)
                    {
                        LMSCO2DATA_Y1.Add(FECO2DATAY[i]);
                        LMSCO2DATA_X1.Add(Texp[i] - Time[Time.IndexOf(Texp[0]) - 2]);
                    }
                }
                    
                if (dCO2exp[i] > _average_dCO2exp && trigger==0)
                {
                    trigger = 1;
                }
                
                if (dCO2exp[i] < _average_dCO2exp && trigger == 1)
                {
                    LMSCO2DATA_Y.Add(FECO2DATAY[i-1]);
                    LMSCO2DATA_X.Add(Texp[i-1] - Time[Time.IndexOf(Texp[0]) - 2]);
                }
                Texp_p.Add(Texp[i] - Time[Time.IndexOf(Texp[0]) - 2]);
            }
            MainWindow.ExampleCO2VD.Clear3();
            MainWindow.ExampleCO2VD.AddDataXY(Texp_p, FECO2DATA);
            MainWindow.ExampleCO2VD.AddDataXY2(LMSCO2DATA_X, LMSCO2DATA_Y);
            MainWindow.ExampleCO2VD.ChangeDot2();
            if (LMSCO2DATA_Y.Count() != 0)
            {
                LineRegress.LSM(LMSCO2DATA_X, LMSCO2DATA_Y, out A, out B);
                LineRegress.LSM(LMSCO2DATA_X1, LMSCO2DATA_Y1, out A1, out B1);
                //MessageBox.Show($"A={A},B={B}");
                LineRegress.LSM_DATAXY(A, B, 0, 4, SampleTime, out LSM_DATAX, out LSM_DATAY);
                MainWindow.ExampleCO2VD.AddDataXY3(LSM_DATAX, LSM_DATAY);
                index = FindSquare(Texp_p, FECO2DATA, SampleTime, LMSCO2DATA_Y[0]);
                MainWindow.ExampleCO2VD.AddPointDashLine_S("4", Texp_p[index], Texp_p[index], 0, 7);

                //Console.WriteLine($"ExampleCO2DATA_Y = {ExampleCO2DATA_Y[index]}\nCO2={FECO2DATA.Count()}\nV={VexpDATA.Count()}");
                //LMSCO2DATA_X.ForEach(i => Console.WriteLine($"Texp[0] = {Texp[0]}   : {i}"));
                //index = FECO2DATA.IndexOf(ExampleCO2DATA_Y[index]);
                //Console.WriteLine($"index = {index}\nFECO2DATA[index]={FECO2DATA[index]}\n");//VexpDATA[index]={VexpDATA[index - 1]}
                if (index - 1 < 0)
                {
                    _VD = VexpDATA[0];
                }
                else
                {
                    _VD = VexpDATA[index];
                }
            }
        }
        public int FindSquare(List<double> X, List<double> Y, double SampleTime, double Y0)
        {
            List<double> Square = new List<double> { };
            int stop_indexY = Y.IndexOf(Y0);
            for (int i = 0; i < Y.Count(); i++)
            {
                integral LeftArea = new integral();
                integral RightArea = new integral();
                double SquareLeftArea = 0;
                double SquareRightArea = 0;

                LeftArea.y0 = Y[0];
                for (int L = 1; L <= i; L++)
                {
                    SquareLeftArea += LeftArea.Trap(Y[L], SampleTime);
                }
                RightArea.y0 = (A * X[i] + B) - Y[i];
                for (int R = i + 1; R < stop_indexY; R++)
                {
                    SquareRightArea += RightArea.Trap((A * X[R] + B) - Y[R], SampleTime);
                }
                Square.Add(Math.Abs(SquareRightArea- SquareLeftArea));
            }
            return Square.IndexOf(Square.Min());
        }    
    }

    public class RespiratoryCalc_C : INotifyPropertyChanged
    {
        public double _bN { get; set; }
        public double _BN
        {
            get { return _bN; }
            set
            {
                _bN = value;
                OnPropertyChanged("_BN");
            }
        }
        public double _t { get; set; }
        public double _T
        {
            get { return _t; }
            set
            {
                _t = value;
                OnPropertyChanged("_T");
            }
        }
        public double _t1 { get; set; }
        public double _T1
        {
            get { return _t1; }
            set
            {
                _t1 = value;
                OnPropertyChanged("_T1");
            }
        }
        public double _t2 { get; set; }
        public double _T2
        {
            get { return _t2; }
            set
            {
                if (_bN != 0)
                {
                    _t2 = value;
                    OnPropertyChanged("_T2");
                }
            }
        }
        public double _vT { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _VT
        {
            get { return _vT; }
            set
            {
                if (_bN != 0)
                {
                    _vT = value;
                    OnPropertyChanged("_VT");
                }

            }
        }
        public double _vE { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _VE
        {
            get { return _vE; }
            set
            {
                if (_bN != 0)
                {
                    _vE = value;
                    OnPropertyChanged("_VE");
                }
            }
        }
        public double _vO2 { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _VO2
        {
            get { return _vO2; }
            set
            {
                if (_bN != 0)
                {
                    _vO2 = value;
                    OnPropertyChanged("_VO2");
                }
            }
        }
        public double _vO2_s { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _VO2_s
        {
            get { return _vO2_s; }
            set
            {
                if (_bN != 0)
                {
                    _vO2_s = value;
                    OnPropertyChanged("_VO2_s");
                }
            }
        }
        public double _vO2_HT { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _VO2_HT
        {
            get { return _vO2_HT; }
            set
            {
                if (_bN != 0)
                {
                    _vO2_HT = value;
                    OnPropertyChanged("_VO2_HT");
                }
            }
        }
        public double _vO2_ET { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _VO2_ET
        {
            get { return _vO2_ET; }
            set
            {
                if (_bN != 0)
                {
                    _vO2_ET = value;
                    OnPropertyChanged("_VO2_ET ");
                }
            }
        }
        public double _vCO2 { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _VCO2
        {
            get { return _vCO2; }
            set
            {
                if (_bN != 0)
                {
                    _vCO2 = value;
                    OnPropertyChanged("_VCO2");
                }
            }
        }
        public double _vCO2_s { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _VCO2_s
        {
            get { return _vCO2_s; }
            set
            {
                if (_bN != 0)
                {
                    _vCO2_s = value;
                    OnPropertyChanged("_VCO2_s");
                }
            }
        }
        public double _vCO2_HT { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _VCO2_HT
        {
            get { return _vCO2_HT; }
            set
            {
                if (_bN != 0)
                {
                    _vCO2_HT = value;
                    OnPropertyChanged("_VCO2_HT");
                }
            }
        }
        public double _vCO2_ET { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _VCO2_ET
        {
            get { return _vCO2_ET; }
            set
            {
                if (_bN != 0)
                {
                    _vCO2_ET = value;
                    OnPropertyChanged("_VCO2_ET ");
                }
            }
        }
        public double _fETO2 { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _FETO2
        {
            get { return _fETO2; }
            set
            {
               
                    _fETO2 = value;
                    OnPropertyChanged("_FETO2");
               
            }
        }
        public double _fITO2 { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _FITO2
        {
            get { return _fITO2; }
            set
            {
                
                    _fITO2 = value;
                    OnPropertyChanged("_FITO2");
                
            }
        }
        public double _fETCO2 { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _FETCO2
        {
            get { return _fETCO2; }
            set
            {
               
                    _fETCO2 = value;
                    OnPropertyChanged("_FETCO2");
                
            }
        }
        public double _fITCO2 { get; set; } //Tidal Volume VT=integral(Vexp) - выдохнутый обьем [liters]
        public double _FITCO2
        {
            get { return _fITCO2; }
            set
            {
               
                    _fITCO2 = value;
                    OnPropertyChanged("_FITCO2");
                
            }
        }
        public double _bF { get; set; }
        public double _BF
        {
            get { return _bF; }
            set
            {
                if (_bN != 0)
                {
                    _bF = value;
                    OnPropertyChanged("_BF");
                }
            }
        }//Breath frequency BR=60/(t2-t0) [minute^-1]
        public double _rER { get; set; }//Respiratory gas exchange ratio RER=VCO2/VO2 [None]
        public double _RER
        {
            get { return _rER; }
            set
            {
                if (_bN != 0)
                {
                    _rER = value;
                    OnPropertyChanged("_RER");
                }
            }
        }

        public double _dataFlow { get; set; }//Respiratory gas exchange ratio RER=VCO2/VO2 [None]
        public double _DataFlow
        {
            get { return _dataFlow; }
            set
            {
                _dataFlow = value;
                OnPropertyChanged("_DataFlow");
            }
        }
        public double _kdataFlow { get; set; }//Respiratory gas exchange ratio RER=VCO2/VO2 [None]
        public double _kDataFlow
        {
            get { return _kdataFlow; }
            set
            {
                _kdataFlow = value;
                OnPropertyChanged("_kDataFlow");
            }
        }
        public double _dataO2 { get; set; }//Respiratory gas exchange ratio RER=VCO2/VO2 [None]
        public double _DataO2
        {
            get { return _dataO2; }
            set
            {
                _dataO2 = value;
                OnPropertyChanged("_DataO2");
            }
        }
        public double _dataCO2 { get; set; }//Respiratory gas exchange ratio RER=VCO2/VO2 [None]
        public double _DataCO2
        {
            get { return _dataCO2; }
            set
            {
                _dataCO2 = value;
                OnPropertyChanged("_DataCO2");
            }
        }
        public double _flowins { get; set; }//Respiratory gas exchange ratio RER=VCO2/VO2 [None]
        public double _Flowins
        {
            get { return _flowins; }
            set
            {
                _flowins = value;
                OnPropertyChanged("_Flowins");
            }
        }
        public double _flowexp { get; set; }
        public double _Flowexp
        {
            get { return _flowexp; }
            set
            {
                _flowexp = value;
                OnPropertyChanged("_Flowexp");
            }
        }
        public double _kflowins { get; set; }
        public double _kFlowins
        {
            get { return _kflowins; }
            set
            {
                _kflowins = value;
                OnPropertyChanged("_kFlowins");
            }
        }
        public double _kflowexp { get; set; }
        public double _kFlowexp
        {
            get { return _kflowexp; }
            set
            {
                _kflowexp = value;
                OnPropertyChanged("_kFlowexp");
            }
        }
        public double _o2ins { get; set; }
        public double _O2ins
        {
            get { return _o2ins; }
            set
            {
                _o2ins = value;
                OnPropertyChanged("_O2ins");
            }
        }
        public double _o2exp { get; set; }
        public double _O2exp
        {
            get { return _o2exp; }
            set
            {
                _o2exp = value;
                OnPropertyChanged("_O2exp");
            }
        }
        public double _vo2exp { get; set; }
        public double _VO2exp
        {
            get { return _vo2exp; }
            set
            {
                _vo2exp = value;
                OnPropertyChanged("_VO2exp");
            }
        }
        public double _vo2ins { get; set; }
        public double _VO2ins
        {
            get { return _vo2ins; }
            set
            {
                _vo2ins = value;
                OnPropertyChanged("_VO2ins");
            }
        }

       

        public double _co2ins { get; set; }
        public double _CO2ins
        {
            get { return _co2ins; ; }
            set
            {
                _co2ins = value;
                OnPropertyChanged("_CO2ins");
            }
        }
        public double _co2exp { get; set; }
        public double _CO2exp
        {
            get { return _co2exp; }
            set
            {
                _co2exp = value;
                OnPropertyChanged("_CO2exp");
            }
        }
        public double _vco2ins { get; set; }
        public double _VCO2ins
        {
            get { return _vco2ins; }
            set
            {
                _vco2ins = value;
                OnPropertyChanged("_VCO2ins");
            }
        }
        public double _vco2exp { get; set; }
        public double _VCO2exp
        {
            get { return _vco2exp; }
            set
            {
                _vco2exp = value;
                OnPropertyChanged("_VCO2exp");
            }
        }
        public string _status { get; set; }
        public string _Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("_Status");
            }
        }
        public double _dco2 { get; set; }
        public double _dCO2
        {
            get { return _dco2; }
            set
            {
                _dco2 = value;
                OnPropertyChanged("_dCO2");
            }
        }
        public double _do2 { get; set; }
        public double _dO2
        {
            get { return _do2; }
            set
            {
                _do2 = value;
                OnPropertyChanged("_dO2");
            }
        }
        public double _kdco2 { get; set; }
        public double _kdCO2
        {
            get { return _kdco2; }
            set
            {
                _kdco2 = value;
                OnPropertyChanged("_kdCO2");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string name = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
