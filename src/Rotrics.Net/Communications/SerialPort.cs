using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;

namespace Rotrics.Net.Communications
{
    [ExcludeFromCodeCoverage]
    public class SerialPort : ISerialPort
    {
        private readonly System.IO.Ports.SerialPort _port;

        public SerialPort()
        {
            _port = new System.IO.Ports.SerialPort();
        }

        public string PortName
        {
            get => _port.PortName;
            set => _port.PortName = value;
        }

        public int BaudRate
        {
            get => _port.BaudRate;
            set => _port.BaudRate = value;
        }

        public Parity Parity
        {
            get => _port.Parity;
            set => _port.Parity = value;
        }
        
        public StopBits StopBits
        {
            get => _port.StopBits;
            set => _port.StopBits = value;
        }
        public int DataBits
        {
            get => _port.DataBits;
            set => _port.DataBits = value;
        }
        public Handshake Handshake
        {
            get => _port.Handshake;
            set => _port.Handshake = value;
        }
        
        public bool DtrEnable
        {
            get => _port.DtrEnable;
            set => _port.DtrEnable = value;
        }

        public bool IsOpen => _port.IsOpen;

        public void Open()
        {
            _port.Open();
        }

        public string ReadLine()
        {
            return _port.ReadLine();
        }

        public void Write(string data)
        {
            _port.Write(data);
        }

        public void Dispose()
        {
            _port?.Dispose();
        }
    }
}