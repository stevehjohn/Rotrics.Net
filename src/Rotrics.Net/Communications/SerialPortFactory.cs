namespace Rotrics.Net.Communications
{
    public class SerialPortFactory : ISerialPortFactory
    {
        public ISerialPort GetSerialPort()
        {
            return new SerialPort();
        }
    }
}