namespace Rotrics.Net.Communications
{
    public interface ISerialPortEnumeratorFactory
    {
        ISerialPortEnumerator GetSerialPortEnumerator();
    }
}