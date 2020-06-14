using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using System.Collections.ObjectModel;

namespace CPET
{
    public partial class MainWindow : Window
    {
        string _path = @"C:\SPIRODATE";

        //Данные из \Integral\Integral\bin\Debug (data(1-3).txt)
        List<double> axis1YData;
        List<double> axis2YData;
        List<double> axis3YData;
        List<double> axis1XData;
        List<double> axis2XData;
        List<double> axis3XData;

        List<double> axis1EYData;
        List<double> axis1EXData;
        List<double> axis2EYData;
        List<double> axis2EXData;
        List<double> axis3EYData;
        List<double> axis3EXData;

        List<double> axis1AEYData;
        List<double> axis2AEYData;
        List<double> axis3AEYData;

        List<double> axis1MEYData = new List<double>() { };
        List<double> axis1MEXData = new List<double>() { };
        List<double> axis2MEYData = new List<double>() { };
        List<double> axis2MEXData = new List<double>() { };
        List<double> axis3MEYData = new List<double>() { };
        List<double> axis3MEXData = new List<double>() { };


        List<double> axis1MAEYData = new List<double>() { };
        List<double> axis1MAEXData = new List<double>() { };
        List<double> axis2MAEYData = new List<double>() { };
        List<double> axis2MAEXData = new List<double>() { };
        List<double> axis3MAEYData = new List<double>() { };
        List<double> axis3MAEXData = new List<double>() { };
        //Инициализация графиков
        //Графики на 1 вкладке
        GraphicsInit FlowTime;
        GraphicsInit O2Time;
        GraphicsInit CO2Time;

        //Графики на 2 вкладке
        GraphicsInit LoopFlowVolume;
        GraphicsInit VO2vsVCO2;
        GraphicsInit EQO2vsEQCO2;
        GraphicsInit VEvsW;
        GraphicsInit FETO2vsFETCO2;
        //Графики на 3 вкладке
        GraphicsInit Example_O2;
        GraphicsInit Example_CO2;
        GraphicsInit Example_Flow;
        //Графики на 4 вкладке
        GraphicsInit Test_LoopFlowVolume;
        GraphicsInit Test_VO2vsVCO2;
        GraphicsInit Test_EQO2vsEQCO2;
        GraphicsInit Test_VEvsW;
        GraphicsInit Test_FETO2vsFETCO2;

        //График на 1 вкладке для иллюстрации вычисления VD
        static public GraphicsInit ExampleCO2VD;
        RespiratoryCalc Calculations;




        public double SampleTime = 0.1, SampleTime05 = 0.05;// данные приходят с частотой 10Гц

        double ZeroLine = 0;// Константа,взял исходя из придуманного мной графика потока
        double _dataFlow, _dataO2, _dataCO2, _kFlow;//_dataFlow,_dataO2,_dataCO2 - текущие данные,взятые из файла,_kFlow,_kO2, _kCO2 - предыдущие данные,взятые из файла
        double _t0 = 0, _t1 = 0, _t2 = 0, _t;//_t0-начало вдоха; _t1-конец вдоха,начало выдоха; _t2-конец выдоха;_t-текущие значение
        double _bN = 0, Vcurrent = 6;

        double ZeroLine_test = 0;// Константа,взял исходя из придуманного мной графика потока
        double _dataFlow_test, _dataFlowA_test, _dataO2_test, _dataCO2_test, _dataO2A_test, _dataCO2A_test, _kFlow_test, _kFlowA_test, _kO2, _kCO2;//_dataFlow,_dataO2,_dataCO2 - текущие данные,взятые из файла,_kFlow,_kO2, _kCO2 - предыдущие данные,взятые из файла
        double _t0_test = 0, _t1_test = 0, _t2_test = 0, _t_test;//_t0-начало вдоха; _t1-конец вдоха,начало выдоха; _t2-конец выдоха;_t-текущие значение
        double _bN_test = 0, T_status = 1;
        string status = "End Expiration";
        public ObservableCollection<RespiratoryCalc_C> Calculations_M { get; set; } = new ObservableCollection<RespiratoryCalc_C>();
        public ObservableCollection<RespiratoryCalc_C> Calculations_MF { get; set; } = new ObservableCollection<RespiratoryCalc_C>();
        public ObservableCollection<RespiratoryCalc_C> Calculations_T { get; set; } = new ObservableCollection<RespiratoryCalc_C>();
        public ObservableCollection<RespiratoryCalc_C> Calculations_TF { get; set; } = new ObservableCollection<RespiratoryCalc_C>();
        public MainWindow()
        {

            InitializeComponent();
            Calculations = new RespiratoryCalc();
            DataContext = this;

            chart.BackColor = System.Drawing.Color.GhostWhite;
            //Инициализация Графиков
            {
                axis1YData = WorkWithData.DataFromTXT("data1.txt");
                axis1XData = new List<double>() { };
                double t = 0;//Какое-то начальное время исследования
                             //по оси X - это график времени с шагом SampleTime = 0,1
                foreach (double i in axis1YData)
                {
                    axis1XData.Add(t);
                    t += SampleTime;
                }
                FlowTime = new GraphicsInit(chart, "Default1", "Series1", 0);
                FlowTime.SetOfAxis(0, 12, -10, 10, 0.5, 1, "Время [С]", "V(t) [Л/С]");
                FlowTime.CommonLine("Series11", 0, 20, 0, 0);

                axis2YData = WorkWithData.DataFromTXT("data2.txt");
                axis2XData = new List<double>() { };
                double t1 = 0;
                foreach (double i in axis2YData)
                {
                    axis2XData.Add(t1);
                    t1 += SampleTime;
                }
                O2Time = new GraphicsInit(chart, "Default2", "Series2", 1);
                O2Time.SetOfAxis(0, 12, 13, 21, 0.5, 1, "Время [С]", "FEO2(t) / FIO2(t) [Vol.%]");


                axis3YData = WorkWithData.DataFromTXT("data3.txt");
                axis3XData = new List<double>() { };
                double t2 = 0;
                foreach (double i in axis3YData)
                {
                    axis3XData.Add(t2);
                    t2 += 0.1;
                }
                CO2Time = new GraphicsInit(chart, "Default3", "Series3", 2);
                CO2Time.SetOfAxis(0, 12, 0, 7, 0.5, 1, "Время [С]", "FECO2(t) / FICO2(t) [Vol.%]");

                ExampleCO2VD = new GraphicsInit(ExampleCO2_VD, "Default4", "1", "2", "3", 0);
                ExampleCO2VD.DashLine_S("4");
                ExampleCO2VD.SetOfAxis(-1, 4, 0, 7, 0.1, 1, "Время [С]", "FECO2(t)  [Vol.%]");

                axis1EYData = WorkWithData.DataFromTXT("data1E.txt");
                axis1EXData = new List<double>() { };
                double tE0 = 0;//Какое-то начальное время исследования
                               //по оси X - это график времени с шагом SampleTime = 0,1
                foreach (double i in axis1EYData)
                {
                    axis1EXData.Add(tE0);
                    tE0 += SampleTime05;
                }
                Example_Flow = new GraphicsInit(Test, "Default0", "Series0", "Series01", 0);
                Example_Flow.SetOfAxis(0, 130, -5, 3, 1, 0.5, "Время [С]", "V(t) [Л/С]");
                //Example_Flow.AddDataXY(axis1EXData, axis1EYData);
                //Example_Flow.AddSeries("Mean0", System.Drawing.Color.Purple);
                //Example_Flow.AddDataXY("Mean0", axis1EXData, Filter.MoveAverage(axis1EYData,8));
                //axis1AEYData = Filter.MoveAverage(axis1EYData, 10);
                Filter.Meddiana(axis1EYData, 0.05, 9, out axis1MEYData, out axis1MEXData);


                axis2EYData = WorkWithData.DataFromTXT("data2E.txt");
                axis2EXData = new List<double>() { };
                double tE1 = 0;//Какое-то начальное время исследования
                               //по оси X - это график времени с шагом SampleTime = 0,1
                foreach (double i in axis2EYData)
                {
                    axis2EXData.Add(tE1);
                    tE1 += SampleTime05;
                }
                Example_O2 = new GraphicsInit(Test, "Default1", "Series1", "Series12", 1);
                Example_O2.SetOfAxis(0, 130, 16.5, 22.5, 1, 0.5, "Время [С]", "FEO2(t) / FIO2(t) [Vol.%]");
                //Example_O2.AddDataXY(axis2EXData, axis2EYData);
                //Example_O2.AddSeries("Mean1", System.Drawing.Color.Purple);
                //Example_O2.AddDataXY("Mean1", axis2EXData, Filter.MoveAverage(axis2EYData,16));
                //axis2AEYData = Filter.MoveAverage(axis2EYData, 12);
                Filter.Meddiana(axis2EYData, 0.05, 3, out axis2MEYData, out axis2MEXData);
                Filter.MoveAverage(axis2MEYData, axis2MEXData, out axis2MAEYData, out axis2MAEXData, 3);

                axis3EYData = WorkWithData.DataFromTXT("data3E.txt");
                axis3EXData = new List<double>() { };
                double tE2 = 0;//Какое-то начальное время исследования
                               //по оси X - это график времени с шагом SampleTime = 0,1
                foreach (double i in axis3EYData)
                {
                    axis3EXData.Add(tE2);
                    tE2 += SampleTime05;
                }
                Example_CO2 = new GraphicsInit(Test, "Default2", "Series2", "Series23", 2);
                Example_CO2.SetOfAxis(0, 130, -1, 5.5, 1, 0.5, "Время [С]", "FECO2(t) / FICO2(t) [Vol.%]");
                //Example_CO2.AddDataXY(axis3EXData, axis3EYData);
                //Example_CO2.AddSeries("Mean2", System.Drawing.Color.Purple);
                //Example_CO2.AddDataXY("Mean2", axis3EXData, Filter.MoveAverage(axis3EYData,16));
                //axis3AEYData = Filter.MoveAverage(axis3EYData, 10);
                Filter.Meddiana(axis3EYData, 0.05, 3, out axis3MEYData, out axis3MEXData);
                Filter.MoveAverage(axis3MEYData, axis3MEXData, out axis3MAEYData, out axis3MAEXData, 3);



                LoopFlowVolume = new GraphicsInit(FlowVolumeLoop, "Default1", "LoopFlowVolume", 0);
                LoopFlowVolume.SetOfAxis(0, 7, -10, 10, 0.5, 1, "Обьем [Л]", "Поток    [Л/С]");
                LoopFlowVolume.CommonLine("Series1", 0, 7, 0, 0);

                VO2vsVCO2 = new GraphicsInit(VO2_VCO2, "Default1", "1", "2", 0);
                VO2vsVCO2.SetOfAxis(0, 5, 0, 5, 0.1, 0.2, "Time", "VO2  [mLiters/Minute]");
                VO2vsVCO2.AddAxisY2(0, 5, 0.2, "VCO2  [mLiters/Minute]");
                VO2vsVCO2.AddXY(2, 0);
                VO2vsVCO2.AddXY(2, 4);
                VO2vsVCO2.AddXY2(0, 0);
                VO2vsVCO2.AddXY2(2, 2);

                EQO2vsEQCO2 = new GraphicsInit(EQO2_EQCO2, "Default1", "1", "2", 0);
                EQO2vsEQCO2.SetOfAxis(0, 5, 0, 5, 0.1, 0.2, "Time", "EQO2");
                EQO2vsEQCO2.AddAxisY2(0, 5, 0.2, "EQCO2");
                EQO2vsEQCO2.AddXY(2, 0);
                EQO2vsEQCO2.AddXY(2, 4);
                EQO2vsEQCO2.AddXY2(0, 0);
                EQO2vsEQCO2.AddXY2(2, 2);

                VEvsW = new GraphicsInit(VE_W, "Default1", "1", "2", 0);
                VEvsW.SetOfAxis(0, 5, 0, 5, 0.1, 0.2, "Time", "VE   [Liters/Seconds]");
                VEvsW.AddAxisY2(0, 5, 0.2, "W [Watt]");
                VEvsW.AddXY(2, 0);
                VEvsW.AddXY(2, 4);
                VEvsW.AddXY2(0, 0);
                VEvsW.AddXY2(2, 2);

                FETO2vsFETCO2 = new GraphicsInit(FETO2_FETCO2, "Default1", "1", "2", 0);
                FETO2vsFETCO2.SetOfAxis(0, 5, 0, 5, 0.1, 0.2, "Time", "FETO2 [Vol.%]");
                FETO2vsFETCO2.AddAxisY2(0, 5, 0.2, "FETCO2 [Vol.%]");
                FETO2vsFETCO2.AddXY(2, 0);
                FETO2vsFETCO2.AddXY(2, 4);
                FETO2vsFETCO2.AddXY2(0, 0);
                FETO2vsFETCO2.AddXY2(2, 2);



                Test_LoopFlowVolume = new GraphicsInit(Test_FlowVolumeLoop, "Default1", "LoopFlowVolume", 0);
                Test_LoopFlowVolume.SetOfAxis(2, 9, -7, 7, 0.5, 1, "Обьем [Л]", "Поток  [Л/С]");
                Test_LoopFlowVolume.CommonLine("1", 0, 12, 0, 0);

                Test_VO2vsVCO2 = new GraphicsInit(Test_VO2_VCO2, "Default1", "1", "2", 0);
                Test_VO2vsVCO2.SetOfAxis(0, 130, 0, 5, 10, 0.5, "Время", "VO2  [Литров / Минута]");
                Test_VO2vsVCO2.AddAxisY2(0, 5, 0.5, "VCO2  [Литров / Минута]");


                Test_EQO2vsEQCO2 = new GraphicsInit(Test_EQO2_EQCO2, "Default1", "1", "2", 0);
                Test_EQO2vsEQCO2.SetOfAxis(0, 130, 0, 70, 10, 10, "Время", "EQO2");
                Test_EQO2vsEQCO2.AddAxisY2(0, 70, 10, "EQCO2");


                Test_VEvsW = new GraphicsInit(Test_VE_W, "Default1", "1", "2", 0);
                Test_VEvsW.SetOfAxis(0, 130, 0, 70, 10, 5, "Время", "VE   [Литров / Секунда]");
                //Test_VEvsW.AddAxisY2(0, 70, 5, "W [Watt]");


                Test_FETO2vsFETCO2 = new GraphicsInit(Test_FETO2_FETCO2, "Default1", "1", "2", 0);
                Test_FETO2vsFETCO2.SetOfAxis(0, 130, 0, 24, 5, 2, "Время", "FETO2 [Vol.%]");
                Test_FETO2vsFETCO2.AddAxisY2(0, 24, 2, "FETCO2 [Vol.%]");


                //Обозначаем соответствующие столбики на 1 панели
                FlowText.Text = "Flow (Vins,Vexp)";
                O2Text.Text = "O2";
                CO2Text.Text = "CO2";
                TestText.Text = "Status";
                VO2Text.Text = "VO2";
                VCO2Text.Text = "VCO2";
                CalculationsText.Text = "\nCalculations:";
                DerivateText.Text = "Derivate";

                
            }
            WorkWithData.CreateDirectoryToTargetPathWWD001(_path);
            using (StreamWriter sw = new StreamWriter(@"C:\SPIRODATE\Log.txt", false, System.Text.Encoding.Default))
            {
                sw.WriteLine("Flow VO2 VCO2 T BN");
                sw.Close();
            }
        }
        void V_Start(object sender, RoutedEventArgs e)
        {


            for (int i = 0; i <= Convert.ToInt32(axis1YData.Count) - 1; i++)
            {

                //Добавление текущего значения на графики
                FlowTime.AddXY(axis1XData[i], axis1YData[i]);
                O2Time.AddXY(axis2XData[i], axis2YData[i]);
                CO2Time.AddXY(axis3XData[i], axis3YData[i]);

                //Запись текущих значений в соответствующие переменные
                _dataFlow = axis1YData[i] - 10;
                _dataO2 = axis2YData[i];
                _dataCO2 = axis3YData[i];
                _t = axis1XData[i];
                Calculations.t = axis1XData[i];

                if (_dataFlow < ZeroLine)//Expiration flow
                {

                    Calculations.Expiration(_dataFlow, _dataO2, _dataCO2, SampleTime, ZeroLine);

                    //Добавление значение на график "Петля Обьем-Поток"

                    LoopFlowVolume.AddXY(Vcurrent += Calculations._Flowexp, -_dataFlow);
                    //Добавление на панель
                    {
                        status = "Expiration";
                        //Добавление на 1 панель значений
                        TestText.Text += $"\nСчет {i}: Expiration";
                        VO2Text.Text += $"\nСчет {i}: EXP: VO2exp = {Calculations._VO2exp} VO2ins = {Calculations._VO2ins} ";
                        VCO2Text.Text += $"\nСчет {i}: EXP: VCO2exp = {Calculations._VCO2exp} VCO2ins = {Calculations._VCO2ins} ";
                    }
                }
                else if (_dataFlow > ZeroLine)//Inspiration flow
                {
                    // MessageBox.Show($"INS : {Calculations._VT} - {Calculations.Vins}\n {-_dataFlow}\n {Calculations._Flowexp}");
                    Calculations.Inspiration(_dataFlow, _dataO2, _dataCO2, SampleTime, ZeroLine);
                    //Добавление значение на график "Петля Обьем-Поток"
                    LoopFlowVolume.AddXY(Vcurrent -= Calculations._Flowins, -_dataFlow);
                    //Добавление на панель
                    {
                        status = "Inspiration";
                        //Добавление на 1 панель значений
                        TestText.Text += $"\nСчет {i}: Inspiration";
                        VO2Text.Text += $"\nСчет {i}:  INS: VO2exp = {Calculations._VO2exp} VO2ins = {Calculations._VO2ins} ";
                        VCO2Text.Text += $"\nСчет {i}:  INS: VCO2exp = {Calculations._VCO2exp} VCO2ins = {Calculations._VCO2ins} ";
                    }
                }
                else if (_dataFlow == ZeroLine && _dataFlow > _kFlow)//End Expiration
                {

                    _t2 = axis1XData[i];//Установка времени конца выдоха
                    Calculations.EndExpiration(_dataFlow, _dataO2, _dataCO2, SampleTime, ZeroLine, _t0, _t2);

                    //Добавление значение на график "Петля Обьем-Поток"
                    LoopFlowVolume.AddXY(Vcurrent += Calculations._Flowexp, -_dataFlow);
                    //Добавление на панель
                    {
                        status = "End Expiration";
                        //Добавление на 1 панель значений
                        TestText.Text += $"\nСчет {i}: " +
                            "EEXP Flow>Flow1 " +
                            $"\nVO2exp={Calculations._VO2exp}       VO2ins = {Calculations._VO2ins}   ==> V02=VO2ins-VO2exp = {Calculations._VO2} " +
                            $"\nVCO2exp = {Calculations._VCO2exp}   VCO2ins = {Calculations._VCO2ins} ==> VC02 = VCO2exp - VCO2ins = {Calculations._VCO2}" +
                            $"\nVexp = {Calculations._VT} Vins = {Calculations.Vins}";
                        VO2Text.Text += $"\nСчет {i}:  EEXP: VO2exp = {Calculations._VO2exp} VO2ins = {Calculations._VO2ins} ";
                        VCO2Text.Text += $"\nСчет {i}:  EEXP: VCO2exp = {Calculations._VCO2exp} VCO2ins = {Calculations._VCO2ins} ";
                        CalculationsText.Text += $"\nСчет {i}:" +
                            $"\nRER      = {Calculations._RER}" +
                            $"\nBR       = {Calculations._BF}" +
                            $"\nVE       = {Calculations._VE}" +
                            $"\nVins = {Calculations.Vins}" +
                            $"\n FETO2 = {Calculations._FETO2}" +
                            $"\n  FETCO2 = {Calculations._FETCO2}";
                        Calculations_M.Add(new RespiratoryCalc_C() { _BN = Calculations._bN, _T2 = _t2, _VT = Calculations._VT, _VE = Calculations._VE, _VO2 = Calculations._VO2, _VCO2 = Calculations._VCO2, _FETO2 = Calculations._FETO2, _FETCO2 = Calculations._FETCO2,  _BF = Calculations._BF, _RER = Calculations._RER });

                        //Добавление на 2 панель значений на 1 вкладку
                        VEBOX.Text = $"VE = {Calculations._VE} [Л/Мин];";
                        VABOX.Text = $"VA = {Calculations._VA} [Л/Мин];";
                        VDBOX.Text = $"VD = {Calculations._VD} [Л];";
                        VO2BOX.Text = $"VO2 ={Calculations._VO2} [Л/Мин] ";
                        VCO2BOX.Text = $"VCO2 = {Calculations._VCO2} [Л/Мин]";
                        VO2_HTBOX.Text = $"VO2_HT ={Calculations._VO2_HT} [Л/Мин]";
                        VCO2_HTBOX.Text = $"VCO2_HT ={Calculations._VCO2_HT} [Л/Мин]";
                        VO2_ETBOX.Text = $"VO2_ET ={Calculations._VO2_ET} [Л/Мин]";
                        VCO2_ETBOX.Text = $"VCO2_ET ={Calculations._VCO2_ET} [Л/Мин]";
                        //VO2predBOX.Text = $"VO2pred ={Calculations._VO2pred} [Л/Мин]";
                        FETO2BOX.Text = $"FETO2 = {Calculations._FETO2} [Vol.%]";
                        FETCO2BOX.Text = $"FETCO2 = {Calculations._FETCO2} [Vol.%]";
                        PAO2BOX.Text = $"PAO2 = {Calculations._PAO2} [Мм.рт.ст.]";
                        PACO2BOX.Text = $"PACO2 = {Calculations._PACO2} [Мм.рт.ст.]";
                        //PaO2BOX.Text = $"PaO2 = {Calculations._PaO2} [mm Hg]";
                        PaCO2BOX.Text = $"PaCO2 = {Calculations._PaCO2} [Мм.рт.ст.]";
                        PECO2BOX.Text = $"PECO2 = {Calculations._PECO2} [Мм.рт.ст.]";
                        //Aa_DO2BOX.Text = $"Aa_DO2 = {Calculations._Aa_DO2} [mm Hg]";
                        //aET_DCO2BOX.Text = $"aET_DCO2 = {Calculations._aET_DCO2} [mm Hg]";
                        //DLO2BOX.Text = $"DLO2 = {Calculations._DLO2}";
                        RERBOX.Text = $"RER = {Calculations._RER}";
                        EQO2BOX.Text = $"EQO2 = {Calculations._EQO2}";
                        EQCO2BOX.Text = $"EQCO2 = {Calculations._EQCO2}";
                        METSBOX.Text = $"METS = {Calculations._METS}";
                        BRBOX.Text = $"BR  = {Calculations._BF} [Дых./Мин]";
                        TimeBOX.Text = $"Time = {_t} [С]";
                        FlowDATAText.Text += $"Vins\n";
                        Calculations.FlowinsDATA.ForEach(s => FlowDATAText.Text += $"VinsDATA = {s}\n");
                        FlowDATAText.Text += $"Vexp\n";
                        Calculations.FlowexpDATA.ForEach(s => FlowDATAText.Text += $"VexpDATA = {s}\n");
                        O2DATAText.Text += $"FIO2\n";
                        Calculations.FIO2DATA.ForEach(s => O2DATAText.Text += $"FIO2DATA = {s}\n");
                        O2DATAText.Text += $"FEO2\n";
                        Calculations.FEO2DATA.ForEach(s => O2DATAText.Text += $"FEO2DATA = {s}\n");
                        CO2DATAText.Text += $"FICO2\n";
                        Calculations.FICO2DATA.ForEach(s => CO2DATAText.Text += $"FICO2DATA = {s}\n");
                        CO2DATAText.Text += $"FECO2\n";
                        Calculations.FECO2DATA.ForEach(s => CO2DATAText.Text += $"FECO2DATA = {s}\n");

                        //Добавление на 2 панель значений на 2 вкладку
                        FEV05BOX.Text = $"FEV05 = {loopVolumeFlow.FEV05} [Л];";
                        FEV1BOX.Text = $"FEV1 = {loopVolumeFlow.FEV1} [Л];";
                        FEV3BOX.Text = $"FEV3 = {loopVolumeFlow.FEV3} [Л];";
                        PEFBOX.Text = $"PEF = {loopVolumeFlow.PEF} [Л/C];";
                        PIFBOX.Text = $"PIF = {loopVolumeFlow.PIF} [Л/C];";
                        FEF25BOX.Text = $"FEF25 = {loopVolumeFlow.FEF25} [Л/C];";
                        FEF50BOX.Text = $"FEF50 = {loopVolumeFlow.FEF50} [Л/C];";
                        FEF75BOX.Text = $"FEF75 = {loopVolumeFlow.FEF75} [Л/C];";
                        FIF25BOX.Text = $"FIF25 = {loopVolumeFlow.FIF25} [Л/C];";
                        FIF50BOX.Text = $"FIF50 = {loopVolumeFlow.FIF50} [Л/C];";
                        FIF75BOX.Text = $"FIF75 = {loopVolumeFlow.FIF75} [Л/C];";
                        MEF25_75BOX.Text = $"MEF25_75 = {loopVolumeFlow.MEF25_75} [Л/C];";
                        VTBOX.Text = $"VT = {loopVolumeFlow.VT} [Л];";

                    }
                    //MessageBox.Show("");
                    //
                    //MessageBox.Show("");
                    //Обнуление буферных значений
                    _t1_test++;
                    if (Calculations._bN == _t1_test)
                    {
                        MessageBox.Show($"Calculations._bN");
                        break;
                    }
                    LoopFlowVolume.Clear();
                    LoopFlowVolume.AddXY(Vcurrent, 0);
                    Calculations.ClearBuffer(Calculations.status);
                    _t0 = _t2;
                }
                else if (_dataFlow == ZeroLine && _dataFlow < _kFlow)//End inspiration
                {

                    Calculations.EndInspiration(_dataFlow, _dataO2, _dataCO2, SampleTime, ZeroLine);
                    //Добавление значение на график "Петля Обьем-Поток"
                    LoopFlowVolume.AddXY(Vcurrent -= Calculations._Flowins, -_dataFlow);
                    //MessageBox.Show($"EINS : {Calculations._VT} - {Calculations.Vins}\n {-_dataFlow}\n {Calculations._Flowexp}");
                    //Добавление на панель
                    {
                        status = "End Inspiration";
                        //Добавление на 1 панель значений
                        TestText.Text += $"\nСчет {i}: End Inspiration Flow < Flow1" + $"Vexp = {Calculations._VT} Vins = {Calculations.Vins}";
                        VO2Text.Text += $"\nСчет {i}:  EINS: VO2exp = {Calculations._VO2exp} VO2ins = {Calculations._VO2ins} ";
                        VCO2Text.Text += $"\nСчет {i}: EINS: VCO2exp = {Calculations._VCO2exp} VCO2ins = {Calculations._VCO2ins} ";
                    }




                }
                Calculations_MF.Add(new RespiratoryCalc_C() { _T = _t, _Status = status, _DataFlow = _dataFlow - ZeroLine, _kDataFlow = _kFlow - ZeroLine, _Flowins = Calculations._Flowins, _Flowexp = Calculations._Flowexp, _O2ins = Calculations._O2, _O2exp = Calculations._O2, _VO2ins = Calculations._VO2ins, _VO2exp = Calculations._VO2exp, _CO2ins = Calculations._CO2, _CO2exp = Calculations._CO2, _VCO2ins = Calculations._VCO2ins, _VCO2exp = Calculations._VCO2exp });
                _kFlow = _dataFlow;
                Calculations._kO2 = _dataO2;
                Calculations._kCO2 = _dataCO2;

                {
                    //Добавление на 1 панель значений
                    DerivateText.Text += $"\nСчет {i}:  dO2 = {Calculations._dO2} dCO2 = {Calculations._dCO2} ";
                    CO2Text.Text += $"\nСчет {i}:  CO2exp = {Calculations._CO2} СO2ins = {Calculations._CO2} ";
                    O2Text.Text += $"\nСчет {i}:  O2exp = {Calculations._O2} O2ins = {Calculations._O2} ";
                    FlowText.Text += $"\nСчет {i}:  FlowIns = {Calculations._Flowins} FlowExp = {Calculations._Flowexp} ";
                    VexpVinsText.Text += $"\nСчет {i}: Vexp = {Calculations.Vexp} Vins = {Calculations.Vins} VT={Calculations._VT}";
                }
                // Запись данных в файл
                using (StreamWriter sw = new StreamWriter(@"C:\SPIRODATE\Log.txt", true, System.Text.Encoding.Default))
                {
                    sw.WriteLine($"{_dataFlow}-{_dataO2}-{_dataCO2}-{_t}-{Calculations._bN}");
                    sw.Close();
                }
            }

        }

        void V_Start_Test(object sender, RoutedEventArgs e)
        {
            RespiratoryCalc Calculations_Test = new RespiratoryCalc();
            Example_Flow.Clear();
            Example_O2.Clear();
            Example_CO2.Clear();
            
            int TE = 0;
            int TI = 0;
            double texp = 0, tins = 0;

            //for (int o = 0; o < axis1AEYData.Count(); o++)
            //{
            //    _dataFlowA_test = axis1AEYData[o];
            //    _dataO2A_test = axis2AEYData[o];
            //    _dataCO2A_test = axis3AEYData[o];
            //    Example_Flow.AddXY2(axis1EXData[o], _dataFlowA_test);
            //    Example_O2.AddXY2(axis2EXData[o], _dataO2A_test);
            //    Example_CO2.AddXY2(axis3EXData[o], _dataCO2A_test);

            //}
            //for (int o = 0; o < axis1EYData.Count(); o++)
            //{
            //    _dataFlow_test = axis1EYData[o];
            //    _dataO2_test = axis2EYData[o];
            //    _dataCO2_test = axis3EYData[o];
            //    Example_Flow.AddXY(axis1EXData[o], _dataFlow_test);
            //    Example_O2.AddXY(axis2EXData[o], _dataO2_test);
            //    Example_CO2.AddXY(axis3EXData[o], _dataCO2_test);

            //}

            for (int i = 0; i < Convert.ToInt32(axis2EYData.Count()); i++)
            {


                //Запись текущих значений в соответствующие переменные
                _dataFlow_test = axis1EYData[i];
                _dataO2_test = axis2EYData[i];
                _dataCO2_test = axis3EYData[i];
                if (_dataCO2_test < 0)
                {
                    _dataCO2_test = 0;
                }


                if (_dataCO2A_test < 0)
                {
                    _dataCO2A_test = 0;
                }

                //Добавление текущего значения на графики
                // Example_Flow.AddXY2(axis1MEXData[i], _dataFlow_test);
                //Example_O2.AddXY2(axis2MAEXData[i], _dataO2_test);
                // Example_CO2.AddXY2(axis3MAEXData[i], _dataCO2_test);
                Example_Flow.AddXY(axis1EXData[i], _dataFlow_test);
                Example_O2.AddXY(axis2EXData[i], _dataO2_test);
                Example_CO2.AddXY(axis3EXData[i], _dataCO2_test);


                _t_test = axis1EXData[i];
                Calculations_Test.t = axis1EXData[i];
                Calculations_Test.CO2DATA.Add(_dataCO2_test);
                Calculations_Test.Time.Add(axis1EXData[i]);


                if (_dataFlow_test >= ZeroLine_test && _kFlow_test <= ZeroLine_test && Calculations_Test.FECO2DATA.Count() >= 10)//End Expiration 
                {
                    _t2_test = axis1EXData[i];//Установка времени конца выдоха

                    //Основные вычисления
                    Calculations_Test.EndExpiration(_kFlow_test, _kO2, _kCO2, SampleTime05, ZeroLine_test, _t0_test, _t2_test);

                    //Добавление значение на график "Петля Обьем-Поток"
                    Test_LoopFlowVolume.AddXY(Vcurrent += Calculations_Test._Flowexp, -_dataFlowA_test);

                    //MessageBox.Show($"{_t_test}\n{Calculations_Test._FETCO2} \n  {Calculations_Test._FETO2}");

                    //Добавление значение в таблицу с результатами и в общую
                    Calculations_TF.Add(new RespiratoryCalc_C() { _T = _t_test, _Status = Calculations_Test.status, _DataFlow = Math.Abs(_dataFlow_test), _kDataFlow = Math.Abs(_kFlow_test), _Flowins = Calculations_Test._Flowins, _Flowexp = Calculations_Test._Flowexp, _O2ins = _dataCO2_test, _O2exp = Calculations_Test._O2, _VO2ins = Calculations_Test._VO2ins, _VO2exp = Calculations_Test._VO2exp, _CO2ins = Calculations_Test._CO2, _CO2exp = Calculations_Test._CO2A, _VCO2ins = Calculations_Test._VCO2ins, _VCO2exp = Calculations_Test._VCO2exp, _dCO2 = Calculations_Test._dCO2, _FETCO2 = Calculations_Test._FETCO2, _dO2 = Calculations_Test._dO2, _FETO2 = Calculations_Test._FETO2});

                    Calculations_T.Add(new RespiratoryCalc_C() { _BN = Calculations_Test._bN, _T2 = _t2_test, _VT = Calculations_Test._VT, _VE = Calculations_Test._VE, _VO2 = Calculations_Test._VO2, _VO2_s = Calculations_Test._VO2_s, _VO2_HT = Calculations_Test._VO2_HT, _VO2_ET = Calculations_Test._VO2_ET, _VCO2 = Calculations_Test._VCO2, _VCO2_s = Calculations_Test._VCO2_s, _VCO2_HT = Calculations_Test._VCO2_HT, _VCO2_ET = Calculations_Test._VCO2_ET, _FETO2 = Calculations_Test._FETO2, _FETCO2 = Calculations_Test._FETCO2, _BF = Calculations_Test._BF, _RER = Calculations_Test._RER });

                    //Добавление значений на 4 панельный график Вассерману
                    Test_VO2vsVCO2.AddXY(_t_test, Calculations_Test._VO2_ET);
                    Test_VO2vsVCO2.AddXY2(_t_test, Calculations_Test._VCO2_ET);

                    Test_EQO2vsEQCO2.AddXY(_t_test, Calculations_Test._EQO2);
                    Test_EQO2vsEQCO2.AddXY2(_t_test, Calculations_Test._EQCO2);

                    Test_VEvsW.AddXY(_t_test, Calculations_Test._VE);

                    Test_FETO2vsFETCO2.AddXY(_t_test, Calculations_Test._FETO2);
                    Test_FETO2vsFETCO2.AddXY2(_t_test, Calculations_Test._FETCO2);

                    //Добавление на панель
                    {

                        //Добавление на 2 панель значений на 1 вкладку
                        Test_VEBOX.Text = $"VE = {String.Format("{0:0.00}",Calculations_Test._VE)} [Л/Мин];";
                        Test_VABOX.Text = $"VA = {String.Format("{0:0.00}", Calculations_Test._VA)} [Л/Мин];";
                        Test_VDBOX.Text = $"VD = {String.Format("{0:0.00}", Calculations_Test._VD)} [Л];";
                        Test_VO2BOX.Text = $"VO2 ={String.Format("{0:0.00}", Calculations_Test._VO2)} [Л/Мин] ";
                        Test_VCO2BOX.Text = $"VCO2 = {String.Format("{0:0.00}", Calculations_Test._VCO2)} [Л/Мин]";
                        Test_VO2_HTBOX.Text = $"VO2_HT ={String.Format("{0:0.00}", Calculations_Test._VO2_HT)} [Л/Мин]";
                        Test_VCO2_HTBOX.Text = $"VCO2_HT ={String.Format("{0:0.00}", Calculations_Test._VCO2_HT)} [Л/Мин]";
                        Test_VO2_ETBOX.Text = $"VO2_ET ={String.Format("{0:0.00}", Calculations_Test._VO2_ET)} [Л/Мин]";
                        Test_VCO2_ETBOX.Text = $"VCO2_ET ={String.Format("{0:0.00}", Calculations_Test._VCO2_ET)} [Л/Мин]";
                        //Test_VO2predBOX.Text = $"VO2pred ={Calculations_Test._VO2pred} [Л/Мин]";
                        Test_FETO2BOX.Text = $"FETO2 = {String.Format("{0:0.00}", Calculations_Test._FETO2)} [Vol.%]";
                        Test_FETCO2BOX.Text = $"FETCO2 = {String.Format("{0:0.00}", Calculations_Test._FETCO2)} [Vol.%]";
                        Test_PAO2BOX.Text = $"PAO2 = {String.Format("{0:0.00}", Calculations_Test._PAO2)} [Мм.рт.ст.]";
                        Test_PACO2BOX.Text = $"PACO2 = {String.Format("{0:0.00}", Calculations_Test._PACO2)} [Мм.рт.ст.]";
                        //Test_PaO2BOX.Text = $"PaO2 = {Calculations_Test._PaO2} [mm Hg]";
                        Test_PaCO2BOX.Text = $"PaCO2 = {String.Format("{0:0.00}", Calculations_Test._PaCO2)} [Мм.рт.ст.]";
                        Test_PECO2BOX.Text = $"PECO2 = {String.Format("{0:0.00}", Calculations_Test._PECO2)} [Мм.рт.ст.]";
                        //Test_Aa_DO2BOX.Text = $"Aa_DO2 = {Calculations_Test._Aa_DO2} [mm Hg]";
                        //Test_aET_DCO2BOX.Text = $"aET_DCO2 = {Calculations_Test._aET_DCO2} [mm Hg]";
                        //Test_DLO2BOX.Text = $"DLO2 = {Calculations_Test._DLO2}";
                        Test_RERBOX.Text = $"RER = {String.Format("{0:0.00}", Calculations_Test._RER)}";
                        Test_EQO2BOX.Text = $"EQO2 = {String.Format("{0:0.00}", Calculations_Test._EQO2)}";
                        Test_EQCO2BOX.Text = $"EQCO2 = {String.Format("{0:0.00}", Calculations_Test._EQCO2)}";
                        Test_METSBOX.Text = $"METS = {String.Format("{0:0.00}", Calculations_Test._METS)}";
                        Test_BRBOX.Text = $"BR  = {String.Format("{0:0.00}", Calculations_Test._BF)} [Дых./Мин]";
                        Test_TimeBOX.Text = $"Time = {_t_test} [C]";

                        //Добавление на 2 панель значений на 2 вкладку
                        Test_FEV05BOX.Text = $"FEV05 = {String.Format("{0:0.00}", loopVolumeFlow.FEV05)} [Л];";
                        Test_FEV1BOX.Text = $"FEV1 = {String.Format("{0:0.00}", loopVolumeFlow.FEV1)} [Л];";
                        Test_FEV3BOX.Text = $"FEV3 = {String.Format("{0:0.00}", loopVolumeFlow.FEV3)} [Л];";
                        Test_PEFBOX.Text = $"PEF = {String.Format("{0:0.00}", loopVolumeFlow.PEF)} [Л/C];";
                        Test_PIFBOX.Text = $"PIF = {String.Format("{0:0.00}", loopVolumeFlow.PIF)} [Л/C];";
                        Test_FEF25BOX.Text = $"FEF25 = {String.Format("{0:0.00}", loopVolumeFlow.FEF25)} [Л/C];";
                        Test_FEF50BOX.Text = $"FEF50 = {String.Format("{0:0.00}", loopVolumeFlow.FEF50)} [Л/C];";
                        Test_FEF75BOX.Text = $"FEF75 = {String.Format("{0:0.00}", loopVolumeFlow.FEF75)} [Л/C];";
                        Test_FIF25BOX.Text = $"FIF25 = {String.Format("{0:0.00}", loopVolumeFlow.FIF25)} [Л/C];";
                        Test_FIF50BOX.Text = $"FIF50 = {String.Format("{0:0.00}", loopVolumeFlow.FIF50)} [Л/C];";
                        Test_FIF75BOX.Text = $"FIF75 = {String.Format("{0:0.00}", loopVolumeFlow.FIF75)} [Л/C];";
                        Test_MEF25_75BOX.Text = $"MEF25_75 = {String.Format("{0:0.00}", loopVolumeFlow.MEF25_75)} [Л/C];";
                        Test_VTBOX.Text = $"VT = {String.Format("{0:0.00}", loopVolumeFlow.VT)} [Л];";
                    }

                    ////Обнуление буферных значений
                    //Example_Flow.SetOfAxis(_t0_test, _t2_test, -Calculations_Test.FlowexpDATA.Max() - 0.5, Calculations_Test.FlowinsDATA.Max() + 0.5, 0.05, Calculations_Test.FlowinsDATA.Max() / 5);
                    //Example_O2.SetOfAxis(_t0_test, _t2_test, 15, 23, 0.05, 0.5);
                    //Example_CO2.SetOfAxis(_t0_test, _t2_test, 0, 7, 0.05, 0.5);

                    ////MessageBox.Show($"{Calculations_Test._bN } {T_status} {_t1_test}");

                    if (Calculations_Test._bN == T_status+21)
                    {
                        T_status++;
                        MessageBox.Show($"{Calculations_Test._bN } {_t0_test} {tins} {_t1_test} {texp} {_t2_test} ");
                        break;
                    }
                    Test_LoopFlowVolume.Clear();
                    Calculations_Test.ClearBuffer(Calculations_Test.status);
                    _t0_test = _t2_test;
                    TE = 0;
                    TI = 0;
                }
                else if (_dataFlow_test <= ZeroLine_test && _kFlow_test >= ZeroLine_test && Calculations_Test.FICO2DATAbuf.Count() >= 10)//End inspiration 
                {
                    _t1_test = _t_test;

                    //Основные вычисления
                    Calculations_Test.EndInspiration(_kFlow_test, _kO2, _kCO2, SampleTime05, ZeroLine_test, tins);

                    //Добавление значение на график "Петля Обьем-Поток"
                    Test_LoopFlowVolume.AddXY(Vcurrent -= Calculations_Test._Flowins, -_dataFlowA_test, Calculations_Test._Flowins);

                    //Добавление значение в таблицу
                    Calculations_TF.Add(new RespiratoryCalc_C() { _T = _t_test, _Status = Calculations_Test.status, _DataFlow = Math.Abs(_dataFlow_test), _kDataFlow = Math.Abs(_kFlow_test), _Flowins = Calculations_Test._Flowins, _Flowexp = Calculations_Test._Flowexp, _O2ins = _dataCO2_test, _O2exp = Calculations_Test._O2, _VO2ins = Calculations_Test._VO2ins, _VO2exp = Calculations_Test._VO2exp, _CO2ins = Calculations_Test._CO2, _CO2exp = Calculations_Test._CO2, _VCO2ins = Calculations_Test._VCO2ins, _VCO2exp = Calculations_Test._VCO2exp, _dCO2 = Calculations_Test._dCO2, _FETCO2 = Calculations_Test._FETCO2, _dO2 = Calculations_Test._dO2, _FETO2 = Calculations_Test._FETO2});

                }
                else if (_dataFlow_test >= 0.3)//Inspiration flow
                {
                    if (TI == 0)
                    {
                        tins = _t_test;
                        TI++;
                    }
                    //Основные вычисления
                    Calculations_Test.Inspiration(_kFlow_test, _kO2, _kCO2, SampleTime05, ZeroLine_test, tins);

                    //Добавление значение на график "Петля Обьем-Поток"
                    Test_LoopFlowVolume.AddXY(Vcurrent -= Calculations_Test._Flowins, -_kFlow_test, Calculations_Test._Flowins);

                    //Добавление значение в таблицу
                    Calculations_TF.Add(new RespiratoryCalc_C() { _T = _t_test, _Status = Calculations_Test.status, _DataFlow = Math.Abs(_dataFlow_test), _kDataFlow = Math.Abs(_kFlow_test), _Flowins = Calculations_Test._Flowins, _Flowexp = Calculations_Test._Flowexp, _O2ins = _dataCO2_test, _O2exp = Calculations_Test._O2, _VO2ins = Calculations_Test._VO2ins, _VO2exp = Calculations_Test._VO2exp, _CO2ins = Calculations_Test._CO2, _CO2exp = Calculations_Test._CO2A, _VCO2ins = Calculations_Test._VCO2ins, _VCO2exp = Calculations_Test._VCO2exp, _dCO2 = Calculations_Test._dCO2, _FETCO2 = Calculations_Test._FETCO2, _dO2 = Calculations_Test._dO2, _FETO2 = Calculations_Test._FETO2});
                }
                else if (_dataFlow_test <= -0.3)//Expiration 
                {
                    if (TE == 0)
                    {
                        texp = _t_test;
                        TE++;
                    }
                    //Основные вычисления
                    Calculations_Test.Expiration(_kFlow_test, _kO2, _kCO2, SampleTime05, ZeroLine_test, texp);

                    //Добавление значение на график "Петля Обьем-Поток"
                    Test_LoopFlowVolume.AddXY(Vcurrent += Calculations_Test._Flowexp, -_kFlow_test, Calculations_Test._Flowins);

                    //Добавление на панель
                    Calculations_TF.Add(new RespiratoryCalc_C() { _T = _t_test, _Status = Calculations_Test.status, _DataFlow = Math.Abs(_dataFlow_test), _kDataFlow = Math.Abs(_kFlow_test), _Flowins = Calculations_Test._Flowins, _Flowexp = Calculations_Test._Flowexp, _O2ins = _dataCO2_test, _O2exp = Calculations_Test._O2, _VO2ins = Calculations_Test._VO2ins, _VO2exp = Calculations_Test._VO2exp, _CO2ins = Calculations_Test._CO2, _CO2exp = Calculations_Test._CO2A, _VCO2ins = Calculations_Test._VCO2ins, _VCO2exp = Calculations_Test._VCO2exp, _dCO2 = Calculations_Test._dCO2, _FETCO2 = Calculations_Test._FETCO2, _dO2 = Calculations_Test._dO2, _FETO2 = Calculations_Test._FETO2});
                }


                _kFlow_test = _dataFlow_test;
                _kFlowA_test = _dataFlowA_test;
                _kO2 = _dataO2_test;
                _kCO2 = _dataCO2_test;

                // Запись данных в файл
                using (StreamWriter sw = new StreamWriter(@"C:\SPIRODATE\Log.txt", true, System.Text.Encoding.Default))
                {
                    sw.WriteLine($"{_dataFlow_test}-{_dataO2_test}-{_dataCO2_test}-{_t_test}-{Calculations_Test._bN}");
                    sw.Close();
                }

            }

        }
    }

}
