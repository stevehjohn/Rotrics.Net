namespace Rotrics.Net.Communications
{
    public class SerialPortEnumeratorFactory : ISerialPortEnumeratorFactory
    {
        public ISerialPortEnumerator GetSerialPortEnumerator()
        {
            return new SerialPortEnumerator();
        }
    }
}