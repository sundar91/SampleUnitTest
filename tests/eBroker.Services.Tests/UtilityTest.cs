using System;
using System.Globalization;
using Xunit;

namespace eBroker.Services.Tests
{

    public class UtilityTest
    {
        //[Fact]
        //public void IsValidTime()
        //{
        //    // Arrange
        //    var mock = new Mock<IUtilityWrapper>();
        //    mock.Setup(m => m.IsValidDuration(It.IsAny<DateTime>()));

        //    // Act


        //    // Assert

        //}

        [Theory]
        [InlineData("01/10/2021 12:34 PM")]
        [InlineData("20/08/2021 02:00 PM")]
        [InlineData("01/01/2021 01:34 PM")]
        [InlineData("30/10/2021 10:00 AM")]
        public void Is9AM_3PMValidDurationReturnsTrue(String date)
        {
            var testDate = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            var result = Utility.IsValidDuration(testDate);

            Assert.True(result);
        }

        [Theory]
        [InlineData("01/10/2021 08:34 PM")]
        [InlineData("20/08/2021 02:00 AM")]
        [InlineData("01/01/2021 07:34 AM")]
        [InlineData("30/10/2021 06:00 PM")]
        public void Is9AM_3PM_NotValidDurationReturnsFalse(String date)
        {
            var testDate = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            var result = Utility.IsValidDuration(testDate);

            Assert.False(result);
        }

        [Theory]
        [InlineData("18/12/2021 08:34 PM")]
        [InlineData("19/12/2021 02:00 AM")]
        [InlineData("11/12/2021 07:34 AM")]
        [InlineData("12/12/2021 06:00 PM")]
        public void IsMonday_Friday_ValidWeekReturnsFalse(String date)
        {
            var testDate = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            var result = Utility.IsValidDayOfWeek(testDate);

            Assert.False(result);
        }

        [Theory]
        [InlineData("17/12/2021 02:34 PM")]
        [InlineData("15/12/2021 11:00 AM")]
        [InlineData("10/12/2021 10:34 AM")]
        [InlineData("08/12/2021 01:00 PM")]
        public void IsMonday_Friday_ValidWeekReturnsTrue(String date)
        {
            var testDate = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            var result = Utility.IsValidDayOfWeek(testDate);

            Assert.True(result);
        }

    }
}
