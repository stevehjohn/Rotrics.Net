using System.Diagnostics.CodeAnalysis;

namespace Rotrics.Net.Communications
{
    [ExcludeFromCodeCoverage]
    public class SerialPortFactory : ISerialPortFactory
    {
        public ISerialPort GetSerialPort()
        {
            return new SerialPort();
        }
    }
}