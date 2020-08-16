using Rotrics.Net.Communications;
using Rotrics.Net.Exceptions;
using System;
using System.IO;
using System.IO.Ports;

namespace Rotrics.Net
{
    public class Controller : IDisposable
    {
        private readonly ISerialPortEnumeratorFactory _portEnumeratorFactory;
        private readonly ISerialPortFactory _portFactory;

        private ISerialPort _port;
        private bool _hasMovedHome;

        public Controller(ISerialPortEnumeratorFactory portEnumeratorFactory, ISerialPortFactory portFactory)
        {
            _portEnumeratorFactory = portEnumeratorFactory;
            _portFactory = portFactory;
        }

        public void Connect()
        {
            var portEnumerator = _portEnumeratorFactory.GetSerialPortEnumerator();

            var ports = portEnumerator.GetPortNames();

            foreach (var port in ports)
            {
                _port = _portFactory.GetSerialPort();

                _port.PortName = port;
                _port.BaudRate = 115200;
                _port.Parity = Parity.None;
                _port.StopBits = StopBits.One;
                _port.DataBits = 8;
                _port.Handshake = Handshake.XOnXOff;
                _port.DtrEnable = true;

                try
                {
                    _port.Open();
                }
                catch (FileNotFoundException)
                {
                    throw new RotricsConnectionException($"Unable to open port {port}.");
                }

                // TODO: Is this really the best way to verify a connection?
                var response = _port.ReadLine();

                if (response == "wait")
                {
                    _hasMovedHome = false;

                    return;
                }

                _port.Dispose();
            }

            throw new RotricsConnectionException("Unable to locate Rotics arm on any serial port.");
        }

        public void MoveToHome()
        {
            _hasMovedHome = true;

            Write("M1112");
        }

        public void StartLaser(byte power)
        {
            Write($"M3 S{power}");
        }

        public void StopLaser()
        {
            Write("M5");
        }

        public void MoveAbsolute(int x, int y, int z)
        {
            Write("G90");

            Write($"G0 X{x} Y{y} Z{z}");
        }

        public void SendRaw(string command)
        {
            Write(command);
        }

        public string ReadRaw()
        {
            // TODO: Add reasonable timeout?
            return _port.ReadLine();
        }

        public void Dispose()
        {
            _port?.Dispose();
        }

        private void Write(string command)
        {
            if (_port == null || !_port.IsOpen)
            {
                throw new RotricsConnectionException("Not connected to arm.");
            }

            if (! _hasMovedHome)
            {
                throw new RotricsCommandException("Please issue MoveToHome command before any others.");
            }

            _port.Write($"{command}\r\n");
        }
    }
}