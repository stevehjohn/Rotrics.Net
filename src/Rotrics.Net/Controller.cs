using System;
using System.IO;
using System.IO.Ports;
using System.Linq;

namespace Rotrics.Net
{
    public class Controller : IDisposable
    {
        private SerialPort _port;

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
                    return;
                }

                _port.Dispose();
            }

            throw new RotricsConnectionException("Unable to locate Rotics arm on any serial port.");
        }

        public void Dispose()
        {
            _port?.Dispose();
        }
    }
}