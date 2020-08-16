using System;
using System.IO;
using System.IO.Ports;
using System.Linq;
using Rotrics.Net.Exceptions;

namespace Rotrics.Net
{
    public class Controller : IDisposable
    {
        private SerialPort _port;
        private bool _hasMovedHome;

        public void Connect()
        {
            var ports = SerialPort.GetPortNames().Distinct();

            foreach (var port in ports)
            {
                _port = new SerialPort(port)
                        {
                            BaudRate = 115200,
                            Parity = Parity.None,
                            StopBits = StopBits.One,
                            DataBits = 8,
                            Handshake = Handshake.XOnXOff,
                            DtrEnable = true
                        };

                try
                {
                    _port.Open();
                }
                catch (FileNotFoundException)
                {
                    throw new RotricsConnectionException($"Unable to open port {port}.");
                }

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

            if (!_hasMovedHome)
            {
                throw new RotricsCommandException("Please issue MoveToHome command before any others.");
            }

            _port.Write($"{command}\r\n");
        }
    }
}