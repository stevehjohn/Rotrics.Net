using System.Linq;

namespace Rotrics.Net.Communications
{
    public class SerialPortEnumerator : ISerialPortEnumerator
    {
        public string[] GetPortNames()
        {
            return System.IO.Ports.SerialPort.GetPortNames().Distinct().ToArray();
        }
    }
}