using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RJCP.IO.Ports;

namespace TestSerialPortStream
{
    class SerialPortStreamTest
    {
        public static void Test()
        {
            var sps = new SerialPortStream("COM1", 115200, 8, Parity.None, StopBits.One);
            try
            {
                sps.Open();
                if (sps.IsOpen)
                {
                    sps.DataReceived += OnDataReceived;
                    Console.WriteLine($"Connected to {sps} successfully.");

                    while (true)
                    {
                        Thread.Sleep(500);

                        Console.WriteLine($"Input something to chat, or input \"q\" to exit: ");
                        var input = Console.ReadLine();
                        if (input.ToLower() == "q")
                            break;

                        input = $"[From {sps.PortName}]: {input}\n";
                        if (sps.CanWrite)
                        {
                            sps.WriteAndFlush(input);
                        }
                    }
                    sps.DataReceived -= OnDataReceived;
                    sps.WriteAndFlush($"[From {sps.PortName}]: Bye");
                    Console.WriteLine("Chatting is quit, existing...");
                }
                else
                    Console.WriteLine($"Failed to connect {sps}.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                sps.Dispose();
            }
        }
        private static void OnDataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            if (args != null && sender is SerialPortStream sps)
            {
                if (sps.CanRead)
                {
                    var all = sps.ReadExisting();
                    var print = $"[From {sps.PortName}]: {all}";
                    Console.WriteLine(print);
                }
            }
        }
        private static void OnErrorReceived(object sender, SerialErrorReceivedEventArgs args)
        {
            if (args != null && sender is SerialPortStream sps)
            {
                Console.WriteLine($"Error occured: {args.EventType}");
            }
        }
    }
}
