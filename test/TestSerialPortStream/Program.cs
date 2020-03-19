using System;
using RJCP.IO.Ports;
using System.Threading;

namespace TestSerialPortStream
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Test();
        }
        static void Test()
        {
            SerialPortStream serialPortStream = new SerialPortStream("COM1", 115200, 8, Parity.None, StopBits.One);
            serialPortStream.DataReceived += new EventHandler<SerialDataReceivedEventArgs>(DataReceived);
            serialPortStream.Open();
            if(serialPortStream.IsOpen)
            {
                serialPortStream.WriteLine("This is a test");
                serialPortStream.Flush();
            }

            while(true)
            {
                Thread.Sleep(1000);
                if(serialPortStream.CanRead)
                {
                    var all = serialPortStream.ReadExisting();
                    if(!string.IsNullOrEmpty(all))
                        Console.WriteLine(all);
                }
            }
        }
        static void DataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            if(args != null)
            {
                if(sender is SerialPortStream sps)
                {
                    var all = sps.ReadExisting();
                    Console.WriteLine(all);
                }
            }
        }
    }
}
