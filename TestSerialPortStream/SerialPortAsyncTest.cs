using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace TestSerialPortStream
{
    public class SerialAsyncPortTest
    {
        public static void Test()
        {
            var sp = new SerialPort("COM1", 115200, Parity.None, 8, StopBits.One);
            try
            {
                sp.Open();
                if (sp.IsOpen)
                {
                    Stream s = sp.BaseStream;
                    var sw = new StreamWriter(s);

                    sp.DataReceived += OnDataReceived;
                    Console.WriteLine($"Connected to {sp} successfully.");

                    while (true)
                    {
                        Thread.Sleep(10);

                        Console.WriteLine($"Input something to chat, or input \"q\" to exit: ");
                        var input = Console.ReadLine();
                        if (input.ToLower() == "q")
                            break;

                        input = $"[From {sp.PortName}]: {input}\n";
                        sp.Write(input);
                    }
                    sp.DataReceived -= OnDataReceived;
                    sp.Write($"[From {sp.PortName}]: Bye");
                    Console.WriteLine("Chatting is quit, existing...");
                }
                else
                    Console.WriteLine($"Failed to connect {sp}.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                sp.Dispose();
            }
        }
        private static void OnDataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            if (args != null && sender is SerialPort sp)
            {
                var all = sp.ReadExisting();
                var print = $"[From {sp.PortName}]: {all}";
                Console.WriteLine(print);
            }
        }
        private static void OnErrorReceived(object sender, SerialErrorReceivedEventArgs args)
        {
            if (args != null && sender is SerialPort sps)
            {
                Console.WriteLine($"Error occured: {args.EventType}");
            }
        }
    }
}
