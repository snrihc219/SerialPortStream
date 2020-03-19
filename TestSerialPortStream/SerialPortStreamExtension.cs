using System;
using System.Collections.Generic;
using System.Text;
using RJCP.IO.Ports;

namespace TestSerialPortStream
{
    public static class SerialPortStreamExtension
    {
        public static void WriteAndFlush(this SerialPortStream serialPort, string text)
        {
            serialPort.Write(text);
            serialPort.Flush();
        }
    }
}
