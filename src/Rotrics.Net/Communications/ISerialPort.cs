using System;
using System.IO.Ports;

namespace Rotrics.Net.Communications
{
    public interface ISerialPort : IDisposable
    {
        string PortName { get; set; }

        int BaudRate { get; set; }

        Parity Parity { get; set; }

        StopBits StopBits { get; set; }

        int DataBits { get; set; }

        Handshake Handshake { get; set; }

        bool DtrEnable { get; set; }

        bool IsOpen { get; }

        void Open();

        string ReadLine();

        void Write(string data);
    }
}