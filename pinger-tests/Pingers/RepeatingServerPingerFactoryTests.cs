using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using Moq;
using NUnit.Framework;

namespace MCLiveStatus.Pinger.Tests.Pingers
{
    [TestFixture]
    public class RepeatingServerPingerFactoryTests
    {
        private RepeatingServerPingerFactory _factory;

        private ServerAddress _serverAddress = new ServerAddress(It.IsAny<string>(), 25565);

        [SetUp]
        public void SetUp()
        {
            _factory = new RepeatingServerPingerFactory();
        }

        [Test]
        public void CreateRepeatingServerPinger_ReturnsInstance()
        {
            RepeatingServerPinger pinger = _factory.CreateRepeatingServerPinger(_serverAddress);

            Assert.IsNotNull(pinger);
        }
    }
}