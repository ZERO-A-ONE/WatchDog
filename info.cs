using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenHardwareMonitor.Hardware;

namespace WindowsFormsApp1
{
    class info
    {
        public string SysInfo;
        UpdateVisitor updateVisitor;
        public Computer computer;
        public struct Source
        {
            public int num;
            public bool CPU;
            public int CPUindex;
            public int CPUNUM;
            public bool GPU;
            public int GPUindex;
            public int GPUNUM;
        }
        public Source source;
        public struct CPUinfo
        {
            public bool Tem;
            public int TemSensorNum;
            public int TemSensorindex;
            public bool Load;
            public int LoadSensorNum;
            public int LoadSensorindex;
            public bool Clock;
            public int ClockSensorNum;
            public int ClockSensorindex;
        }
        public CPUinfo cpuinfo;
        public info()
        {
            
        }
        private void Sinit()//硬件初始化
        {
            source = new Source();
            source.CPU = false;
            source.GPU = false;
            cpuinfo = new CPUinfo();
            cpuinfo.Tem = false;
            cpuinfo.Load = false;
            cpuinfo.Clock = false;
            updateVisitor = new UpdateVisitor();
            computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.GPUEnabled = true;
            computer.HDDEnabled = true;
            computer.RAMEnabled = true;
            computer.Accept(updateVisitor);
            source.num = computer.Hardware.Length;
            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                if (computer.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    Console.WriteLine("CPU YES");
                    source.CPU = true;
                    source.CPUindex = i;
                }
                if (computer.Hardware[i].HardwareType == HardwareType.GpuAti)
                {
                    Console.WriteLine("GpuAti YES");
                    source.GPU = true;
                    source.GPUindex = i;
                }
                if (computer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    Console.WriteLine("GpuNvidia YES");
                    source.GPU = true;
                    source.GPUindex = i;
                }
            }
            //CPUINFO初始化
            int num = 0;
            //TEM初始化
            for (int i = 0; i < computer.Hardware[source.CPUindex].Sensors.Length; i++)
            {
                if (computer.Hardware[source.CPUindex].Sensors[i].SensorType == SensorType.Temperature)
                {
                    cpuinfo.Tem = true;
                    cpuinfo.TemSensorindex = i;
                    break;
                }
            }
            for (int i = 0; i < computer.Hardware[source.CPUindex].Sensors.Length; i++)
            {               
                if (computer.Hardware[source.CPUindex].Sensors[i].SensorType == SensorType.Temperature)
                {
                    num++;                  
                }
            }
            cpuinfo.TemSensorNum = num;
            num = 0;
            //Clock初始化
            for (int i = 0; i < computer.Hardware[source.CPUindex].Sensors.Length; i++)
            {
                if (computer.Hardware[source.CPUindex].Sensors[i].SensorType == SensorType.Clock)
                {
                    cpuinfo.Clock = true;
                    cpuinfo.ClockSensorindex = i;
                    break;
                }
            }
            for (int i = 0; i < computer.Hardware[source.CPUindex].Sensors.Length; i++)
            {
                if (computer.Hardware[source.CPUindex].Sensors[i].SensorType == SensorType.Clock)
                {
                    num++;
                }
            }
            cpuinfo.ClockSensorNum = num;
            num = 0;
            //Load初始化
            for (int i = 0; i < computer.Hardware[source.CPUindex].Sensors.Length; i++)
            {
                if (computer.Hardware[source.CPUindex].Sensors[i].SensorType == SensorType.Load)
                {
                    cpuinfo.Load = true;
                    cpuinfo.LoadSensorindex = i;
                    break;
                }
            }
            for (int i = 0; i < computer.Hardware[source.CPUindex].Sensors.Length; i++)
            {
                if (computer.Hardware[source.CPUindex].Sensors[i].SensorType == SensorType.Load)
                {
                    num++;
                }
            }
            cpuinfo.LoadSensorNum = num;
            num = 0;
            computer.Close();
        }
        public void InfoShow(SetINFO setinfo)
        {
            updateVisitor = new UpdateVisitor();
            computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.GPUEnabled = true;
            computer.HDDEnabled = true;
            computer.RAMEnabled = true;
            computer.Accept(updateVisitor);
            if (setinfo.ShowINFO == 0)
            {
                string tempsstr = "";
                string cputemp = "";
                string cpuload = "";
                string cpuclock = "";
                if (source.CPU && cpuinfo.Load)
                {
                    cpuload += "CPU使用率 " + computer.Hardware[source.CPUindex].Sensors[cpuinfo.LoadSensorNum - 1].Value.ToString() + "%\n";
                }
                tempsstr += cpuload;
                if (source.CPU && cpuinfo.Tem)
                {
                    cputemp += "CPU_Temp:" + computer.Hardware[source.CPUindex].Sensors[cpuinfo.TemSensorNum - 1].Value.ToString() + "\n";
                }
                tempsstr += cputemp;
                if (source.CPU && cpuinfo.Clock)
                {
                    double clock = 0.0;
                    for (int i = 0; i < cpuinfo.ClockSensorNum; i++)
                    {
                        clock += (double)computer.Hardware[source.CPUindex].Sensors[cpuinfo.ClockSensorindex + i].Value;
                    }
                    clock = clock / cpuinfo.ClockSensorNum;
                    cpuclock += "CPU"+ "_Clock:" + clock.ToString() + "MHz\n";
                }
                tempsstr += cpuclock;
                SysInfo = tempsstr;
            }
            else
            {
                string tempsstr = "";
                string cputemp = "";
                string cpuload = "";
                string cpuclock = "";
                if (source.CPU && cpuinfo.Load)
                {
                    for(int i = 0; i < cpuinfo.LoadSensorNum; i++)
                    {
                        if(i < cpuinfo.LoadSensorNum - 1)
                        {
                            cpuload += "CPU" + "#" + i.ToString() + "_Load:" + computer.Hardware[source.CPUindex].Sensors[cpuinfo.LoadSensorindex + i].Value.ToString() + "%\n";
                        }
                        else
                        {
                            cpuload += "CPU" + "Total"+ "_Load:" + computer.Hardware[source.CPUindex].Sensors[cpuinfo.LoadSensorindex + i].Value.ToString() + "%\n";
                        }
                    }                       
                }
                tempsstr += cpuload;
                /*
                if (source.CPU && cpuinfo.Tem)
                {
                    for (int i = 0; i < cpuinfo.TemSensorNum; i++)
                    {
                        if (i < cpuinfo.TemSensorNum - 1)
                        {
                            cputemp += "CPU" + "#" + i.ToString() + "_Temp:" + computer.Hardware[source.CPUindex].Sensors[cpuinfo.TemSensorindex + i].Value.ToString() + "\n";
                        }
                        else
                        {
                            cputemp += "CPU" + "Total"+ "_Temp:" + computer.Hardware[source.CPUindex].Sensors[cpuinfo.TemSensorindex + i].Value.ToString() + "\n";
                        }
                    }
                }
                tempsstr += cputemp;
                if (source.CPU && cpuinfo.Clock)
                {
                    for (int i = 0; i < cpuinfo.ClockSensorNum; i++)
                    {
                        if (i < cpuinfo.ClockSensorNum - 1)
                        {
                            cpuclock += "CPU" + "#" + i.ToString() + "_Clock:" + computer.Hardware[source.CPUindex].Sensors[cpuinfo.ClockSensorindex + i].Value.ToString() + "MHz\n";
                        }
                        else
                        {
                            cpuclock += "CPU" + "Total"+ "_Clock:" + computer.Hardware[source.CPUindex].Sensors[cpuinfo.ClockSensorindex + i].Value.ToString() + "MHz\n";
                        }
                    }
                }
                tempsstr += cpuclock;*/
                SysInfo = tempsstr;
            }
        }
        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }
        public void Main(SetINFO setinfo)
        {
            Sinit();
            while (true)
            {              
                Thread.Sleep(50);
                /*
                Console.WriteLine(setinfo.ShowINFO);
                Console.WriteLine(setinfo.satus);
                Console.WriteLine(source.num);
                Console.WriteLine(SysInfo);
                */
                InfoShow(setinfo);              
                if (setinfo.satus == 0)
                {
                    computer.Close();
                    break;
                }
            }           
        }
    }
}
