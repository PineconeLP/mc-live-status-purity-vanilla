using System;
using System.Threading.Tasks;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using MCLiveStatus.Pinger.Schedulers;
using Moq;
using NUnit.Framework;

namespace MCLiveStatus.Pinger.Tests.Schedulers
{
    [TestFixture]
    public class TimerServerPingerSchedulerTests
    {
        private TimerServerPingerScheduler _scheduler;

        private Mock<IServerPinger> _mockPinger;

        [SetUp]
        public void Setup()
        {
            _mockPinger = new Mock<IServerPinger>();

            _scheduler = new TimerServerPingerScheduler(_mockPinger.Object);
        }

        [Test]
        public async Task Schedule_WithValidServerAddress_ExecutesOnPingCallback()
        {
            bool executed = false;
            ServerAddress validServerAddress = new ServerAddress(It.IsAny<string>(), 25565);
            Action<ServerPingResponse> onPing = (r) => executed = true;
            _mockPinger.Setup(s => s.Ping(validServerAddress)).ReturnsAsync(new ServerPingResponse());

            IServerPingerSchedulerHandler handler = await _scheduler.Schedule(validServerAddress, 0.1, onPing);
            await Task.Delay(150);

            Assert.IsTrue(executed);
            await handler.StopPingSchedule();
        }

        [Test]
        public async Task Schedule_WithInvalidServerAddress_ExecutesOnExceptionCallback()
        {
            bool executed = false;
            ServerAddress invalidServerAddress = new ServerAddress(It.IsAny<string>(), It.IsAny<int>());
            Action<Exception> onException = (e) => executed = true;
            _mockPinger.Setup(s => s.Ping(invalidServerAddress)).ThrowsAsync(new Exception());

            IServerPingerSchedulerHandler handler = await _scheduler.Schedule(invalidServerAddress, 0.1, null, onException);
            await Task.Delay(150);

            Assert.IsTrue(executed);
            await handler.StopPingSchedule();
        }

        [Test]
        public async Task Schedule_OnStopPingSchedule_StopsTimer()
        {
            bool executed = false;
            bool executedBeforeStop = false;
            bool executedAfterStop = false;
            ServerAddress invalidServerAddress = new ServerAddress(It.IsAny<string>(), It.IsAny<int>());
            Action<ServerPingResponse> onPing = (e) => executed = true;

            IServerPingerSchedulerHandler handler = await _scheduler.Schedule(invalidServerAddress, 0.1, onPing);
            await Task.Delay(150);
            executedBeforeStop = executed;
            executed = false; // Reset executed flag.
            await handler.StopPingSchedule();
            await Task.Delay(150);
            executedAfterStop = executed;

            Assert.IsTrue(executedBeforeStop);
            Assert.IsFalse(executedAfterStop);
        }
    }
}