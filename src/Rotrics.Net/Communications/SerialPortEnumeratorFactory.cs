using System.Diagnostics.CodeAnalysis;

namespace Rotrics.Net.Communications
{
    [ExcludeFromCodeCoverage]
    public class SerialPortEnumeratorFactory : ISerialPortEnumeratorFactory
    {
        public ISerialPortEnumerator GetSerialPortEnumerator()
        {
            return new SerialPortEnumerator();
        }
    }
}