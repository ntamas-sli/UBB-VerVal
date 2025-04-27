using FluentAssertions;

namespace DatesAndStuff.Tests
{
    public sealed class SimulationTimeTests
    {
        [OneTimeSetUp]
        public void OneTimeSetupStuff()
        {
            // 
        }

        [SetUp]
        public void Setup()
        {
            // minden teszt felteheti, hogz elotte lefutott ez
        }

        [TearDown]
        public void TearDown()
        {
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }

        [Test]
        // Default time is not current time.
        public void SimulationTime_WhenCreated_DefaultIsNotCurrentTime()
        {
            // Arrange
            DateTime currentTime = DateTime.Now;
            SimulationTime sut = new SimulationTime(currentTime);

            // Act
            var result = sut.ToAbsoluteDateTime();

            // Assert
            Assert.AreNotEqual(currentTime, result);
        }

        // equal
        // not equal
        // <
        // >
        // <= different
        // >= different 
        // <= same
        // >= same
        // max
        // min
        public class CompareSimulationTimeTests
        {
            [Test]
            public void OperatorGreaterThan_ShouldWorkCorrectly()
            {
                // Arrange
                DateTime time1 = new DateTime(2021, 8, 21, 5, 4, 49);
                DateTime time2 = new DateTime(2020, 8, 21, 5, 4, 49);

                SimulationTime sut1 = new SimulationTime(time1);
                SimulationTime sut2 = new SimulationTime(time2);

                // Act
                var result = sut1 > sut2;

                // Assert
                result.Should().BeTrue();
            }

            [Test]
            public void OperatorLessThan_ShouldWorkCorrectly()
            {
                // Arrange
                DateTime time1 = new DateTime(2021, 8, 21, 5, 4, 49);
                DateTime time2 = new DateTime(2020, 8, 21, 5, 4, 49);

                SimulationTime sut1 = new SimulationTime(time1);
                SimulationTime sut2 = new SimulationTime(time2);

                // Act
                var result = sut1 < sut2;

                // Assert
                result.Should().BeFalse();
            }

            [Test]
            public void OperatorEquals_ShouldWorkCorrectly()
            {
                // Arrange
                DateTime time1 = new DateTime(2020, 8, 21, 5, 4, 49);
                DateTime time2 = new DateTime(2020, 8, 21, 5, 4, 49);

                SimulationTime sut1 = new SimulationTime(time1);
                SimulationTime sut2 = new SimulationTime(time2);

                // Act
                var result = sut1.TotalMilliseconds == sut2.TotalMilliseconds;

                // Assert
                result.Should().BeTrue();
            }

            [Test]
            public void OperatorNotEquals_ShouldWorkCorrectly()
            {
                // Arrange
                DateTime time1 = new DateTime(2021, 8, 21, 5, 4, 49);
                DateTime time2 = new DateTime(2020, 8, 21, 5, 4, 49);

                SimulationTime sut1 = new SimulationTime(time1);
                SimulationTime sut2 = new SimulationTime(time2);

                // Act
                var result = sut1 != sut2;

                // Assert
                result.Should().BeTrue();
            }

            [Test]
            public void OperatorGreaterThanOrEqual_ShouldWorkCorrectly()
            {
                // Arrange
                DateTime time1 = new DateTime(2021, 8, 21, 5, 4, 49);
                DateTime time2 = new DateTime(2020, 8, 21, 5, 4, 49);

                SimulationTime sut1 = new SimulationTime(time1);
                SimulationTime sut2 = new SimulationTime(time2);

                // Act
                var result = sut1 >= sut2;

                // Assert
                result.Should().BeTrue();
            }

            [Test]
            public void OperatorLessThanOrEqual_ShouldWorkCorrectly()
            {
                // Arrange
                DateTime time1 = new DateTime(2021, 8, 21, 5, 4, 49);
                DateTime time2 = new DateTime(2020, 8, 21, 5, 4, 49);

                SimulationTime sut1 = new SimulationTime(time1);
                SimulationTime sut2 = new SimulationTime(time2);

                // Act
                var result = sut1 <= sut2;

                // Assert
                result.Should().BeFalse();
            }
        }

        private class TimeSpanArithmeticTests
        {

            [Test]
            // TimeSpanArithmetic
            // add
            // substract
            // Given_When_Then
            public void Addition_SimulationTimeIsShifted()
            {
                // UserSignedIn_OrderSent_OrderIsRegistered
                // DBB, specflow, cucumber, gherkin

                // Arrange
                DateTime baseDate = new DateTime(2010, 8, 23, 9, 4, 49);
                SimulationTime sut = new SimulationTime(baseDate);

                var ts = TimeSpan.FromMilliseconds(4544313);

                // Act
                var result = sut + ts;

                // Assert
                var expectedDateTime = baseDate + ts;
                Assert.AreEqual(expectedDateTime, result.ToAbsoluteDateTime());
            }

            [Test]
            //Method_Should_Then
            public void Subtracttion_SimulationTimeShifted()
            {
                // code kozelibb
                // RegisterOrder_SignedInUserSendsOrder_OrderIsRegistered
                // Arrange
                DateTime baseDate = new DateTime(2010, 8, 23, 9, 4, 49);
                SimulationTime sut = new SimulationTime(baseDate);

                var ts = TimeSpan.FromMilliseconds(4544313);

                // Act
                var result = sut - ts;

                // Assert
                var expectedDateTime = baseDate - ts;
                result.ToAbsoluteDateTime().Should().Be(expectedDateTime);
                //Assert.AreEqual(expectedDateTime, result.ToAbsoluteDateTime());
            }
        }


        [Test]
        // simulation difference timespane and datetimetimespan is the same
        public void SimulationTime_SubtractingSimulationTime_SimulationTimeIsReduced()
        {
            // Arrange
            DateTime baseDate1 = new DateTime(2022, 1, 1, 10, 0, 0);
            DateTime baseDate2 = new DateTime(2022, 1, 1, 9, 30, 0);
            SimulationTime sut1 = new SimulationTime(baseDate1);
            SimulationTime sut2 = new SimulationTime(baseDate2);
            var expected = TimeSpan.FromMinutes(30);

            // Act
            var result = sut1 - sut2;

            // Assert
            //Assert.AreEqual(expected, result);
            expected.Should().Be(result);
        }

        [Test]
        // millisecond representation works
        public void MillisecondRepresentation_RepresentsSimulationTimeInMilliseconds()
        {
            // Arrange
            var initialTime = SimulationTime.MinValue.AddMilliseconds(10);

            // Act
            var result = initialTime.TotalMilliseconds;

            // Assert
            result.Should().Be(10);
        }

        [Test]
        // next millisec calculation works
        public void SimulationTime_NextMillisecondIsRequested_MillisecondIsIncreasedByOne()
        {
            // Arrange
            var t1 = SimulationTime.MinValue.AddMilliseconds(10);

            // Act
            var nextMillisecond = t1.NextMillisec;

            // Assert
            nextMillisecond.TotalMilliseconds.Should().Be(t1.TotalMilliseconds + 1);
            //Assert.AreEqual(t1.TotalMilliseconds + 1, nextMillisecond.TotalMilliseconds);
        }

        public class CreateSimulationTimeFromDateTime
        {
            [Test]
            // creat a SimulationTime from a DateTime, add the same milliseconds to both and check if they are still equal
            public void DateTime_CreatingSimulationTimeFromDateTime_MillisecondIsAddedToBothAndCheckedIfEqual()
            {
                // Arrange
                DateTime baseDate = new DateTime(2022, 5, 15, 8, 30, 45);
                SimulationTime simulationTime = new SimulationTime(baseDate);

                var ts = 500;

                // Act
                DateTime newDateTime = baseDate.AddMilliseconds(ts);
                SimulationTime newSimulationTime = simulationTime.AddMilliseconds(ts);

                // Assert
                newDateTime.Should().Be(newSimulationTime.ToAbsoluteDateTime());
                //Assert.AreEqual(newDateTime, newSimulationTime.ToAbsoluteDateTime());
            }

            [Test]
            // the same as before just with seconds
            public void DateTime_CreatingSimulationTimeFromDateTime_SecondIsAddedToBothAndCheckedIfEqual()
            {
                // Arrange
                DateTime baseDate = new DateTime(2022, 5, 15, 8, 30, 45);
                SimulationTime simulationTime = new SimulationTime(baseDate);

                var ts = 2;

                // Act
                DateTime newDateTime = baseDate.AddSeconds(ts);
                SimulationTime newSimulationTime = simulationTime.AddSeconds(ts);

                // Assert
                newDateTime.Should().Be(newSimulationTime.ToAbsoluteDateTime());
                //Assert.AreEqual(newDateTime, newSimulationTime.ToAbsoluteDateTime());
            }

            [Test]
            // same as before just with timespan
            public void DateTimeToSimulationTime_ShouldConvertDateTimeToSimulationTime_ThenCompareThemByTimeSpan()
            {
                // Arrange
                DateTime baseDate = new DateTime(2022, 5, 15, 8, 30, 45);
                SimulationTime simulationTime = new SimulationTime(baseDate);

                var ts = TimeSpan.FromMilliseconds(500);

                // Act
                DateTime newDateTime = baseDate.Add(ts);
                SimulationTime newSimulationTime = simulationTime + ts;

                // Assert
                newDateTime.Should().Be(newSimulationTime.ToAbsoluteDateTime());
                //Assert.AreEqual(newDateTime, newSimulationTime.ToAbsoluteDateTime());
            }

        }

        [Test]
        // check string representation given by ToString
        public void SimulationTime_ToStringIsUsed_ThenCheckRepresentaionGivenAsAResult()
        {
            // Arrange
            DateTime baseDate = new DateTime(2022, 5, 15, 8, 30, 45);
            SimulationTime simulationTime = new SimulationTime(baseDate);
            string expectedString = baseDate.ToString("yyyy-MM-ddTHH:mm:ss");

            // Act
            string result = simulationTime.ToString();

            // Assert
            result.Should().Be(expectedString);
            //Assert.AreEqual(expectedString, result);
        }
    }
}