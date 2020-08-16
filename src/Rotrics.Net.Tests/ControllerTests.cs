using Moq;
using NUnit.Framework;
using Rotrics.Net.Communications;

namespace Rotrics.Net.Tests
{
    [TestFixture]
    public class ControllerTests
    {
        private Mock<ISerialPort> _port;

        private Controller _controller;

        [SetUp]
        public void SetUp()
        {
            _port = new Mock<ISerialPort>();

            var portFactory = new Mock<ISerialPortFactory>();

            portFactory.Setup(f => f.GetSerialPort())
                       .Returns(_port.Object);

            _controller = new Controller(portFactory.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [TestCase(0)]
        [TestCase(128)]
        [TestCase(255)]
        public void Passes_correct_value_to_laser_start(byte power)
        {
            _port.Setup(p => p.ReadLine())
                 .Returns("wait");

            _controller.Connect();

            _controller.MoveToHome();

            _controller.StartLaser(power);

            _port.Verify(p => p.Write($"M3 S{power}\r\n"));
        }
    }
}