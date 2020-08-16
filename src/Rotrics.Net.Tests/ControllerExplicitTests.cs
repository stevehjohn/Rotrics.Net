using NUnit.Framework;
using Rotrics.Net.Communications;
using Rotrics.Net.Exceptions;

namespace Rotrics.Net.Tests
{
    [TestFixture]
    public class ControllerExplicitTests
    {
        private Controller _controller;

        [SetUp]
        public void SetUp()
        {
            _controller = new Controller(new SerialPortEnumeratorFactory(),  new SerialPortFactory());
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        [Explicit]
        public void Can_connect_to_arm()
        {
            Assert.DoesNotThrow(() => _controller.Connect());
        }

        [Test]
        [Explicit]
        public void Throws_exception_when_unable_to_connect()
        {
            Assert.Throws<RotricsConnectionException>(() => _controller.Connect());
        }

        [Test]
        [Explicit]
        public void Move_to_boundaries()
        {
            _controller.Connect();

            _controller.MoveToHome();

            _controller.MoveAbsolute(-250, 300, 0);
            _controller.MoveAbsolute(250, 300, 0);

            _controller.MoveAbsolute(0, 180, 0);
            _controller.MoveAbsolute(0, 390, 0);

            _controller.MoveAbsolute(0, 300, -110);
            _controller.MoveAbsolute(0, 300, 160);

            _controller.MoveToHome();
        }
    }
}