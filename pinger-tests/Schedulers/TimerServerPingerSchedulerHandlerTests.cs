using System;
using System.Threading.Tasks;
using System.Timers;
using MCLiveStatus.Pinger.Models;
using MCLiveStatus.Pinger.Pingers;
using MCLiveStatus.Pinger.Schedulers;
using Moq;
using NUnit.Framework;

namespace MCLiveStatus.Pinger.Tests.Schedulers
{
    [TestFixture]
    public class TimerServerPingerSchedulerHandlerTests
    {
        private TimerServerPingerSchedulerHandler _handler;

        private Timer _timer;

        [SetUp]
        public void Setup()
        {
            _timer = new Timer();
            _timer.Start();

            _handler = new TimerServerPingerSchedulerHandler(_timer);
        }

        [TearDown]
        public void TearDown()
        {
            _timer.Dispose();
        }

        [Test]
        public async Task UpdatePingScheduleInterval__WithIntervalGreaterThanZero_SetsTimerInterval()
        {
            double expectedInterval = 10000;
            double intervalSeconds = expectedInterval / 1000;

            await _handler.UpdatePingScheduleInterval(intervalSeconds);
            double actualInterval = _timer.Interval;

            Assert.AreEqual(expectedInterval, actualInterval);
        }

        [Test]
        public void UpdatePingScheduleInterval__WithIntervalZero_ThrowsArgumentException()
        {
            double intervalSeconds = 0;
            Assert.ThrowsAsync<ArgumentException>(() => _handler.UpdatePingScheduleInterval(intervalSeconds));
        }

        [Test]
        public void UpdatePingScheduleInterval__WithIntervalLessThanZero_ThrowsArgumentException()
        {
            double intervalSeconds = -1;
            Assert.ThrowsAsync<ArgumentException>(() => _handler.UpdatePingScheduleInterval(intervalSeconds));
        }

        [Test]
        public async Task StopPingSchedule_DisposesTimer()
        {
            bool disposed = false;
            _timer.Disposed += (s, e) => disposed = true;

            await _handler.StopPingSchedule();

            Assert.IsTrue(disposed);
        }

        [Test]
        public async Task StopPingSchedule_SetsIsStoppedToTrue()
        {
            await _handler.StopPingSchedule();

            Assert.IsTrue(_handler.IsStopped);
        }
    }
}