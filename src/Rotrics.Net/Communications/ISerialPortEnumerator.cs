namespace Rotrics.Net.Communications
{
    public interface ISerialPortEnumerator
    {
        string[] GetPortNames();
    }
}