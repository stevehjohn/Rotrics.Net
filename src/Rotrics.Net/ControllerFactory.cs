using System.Diagnostics.CodeAnalysis;
using Rotrics.Net.Communications;

namespace Rotrics.Net
{
    public static class ControllerFactory
    {
        [ExcludeFromCodeCoverage]
        public static Controller GetController()
        {
            return new Controller(new SerialPortEnumeratorFactory(), new SerialPortFactory());
        }
    }
}