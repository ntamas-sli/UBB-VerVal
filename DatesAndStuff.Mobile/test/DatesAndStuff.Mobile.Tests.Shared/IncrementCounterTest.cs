using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;

namespace DatesAndStuff.Mobile.Tests
{
    internal class IncrementCounterTest:BaseTest
    {
        [Test]
        public void ClickCounterTest()
        {
            // Arrange

            // navigate to the counter page
            var drawer = App.FindElement(MobileBy.XPath("//android.widget.ImageButton[@content-desc=\"Open navigation drawer\"]"));
            drawer.Click();
            var counterMenu = App.FindElement(MobileBy.XPath("//android.widget.TextView[@text=\"Counter\"]"));
            counterMenu.Click();

            // check the current count
            var currentCountTextView = FindUIElement("CounterNumberLabel");
            int originalCount = 0;
            string currentCountValue = currentCountTextView.Text.Substring(currentCountTextView.Text.IndexOf(':') + 1);
            int.TryParse(currentCountValue, out originalCount);

            var buttonToClick = FindUIElement("CounterIncreaseBtn");

            // Act
            buttonToClick.Click();
            //Task.Delay(500).Wait(); // Wait for the click to register and show up on the screenshot

            // Assert
            currentCountValue = currentCountTextView.Text.Substring(currentCountTextView.Text.IndexOf(':') + 1);
            int updatedCount = 0;
            int.TryParse(currentCountValue, out updatedCount);

            // Assert
            updatedCount.Should().Be(originalCount + 1);
        }

        [TestCase(10)]
        [TestCase(0)]
        [TestCase(-5)]
        [TestCase(-9.9)]
        public void Person_SalaryIncrease_ShouldIncrease(double salaryIncreasePercentage)
        {
            // Arrange

            // navigate to the person page
            var drawer = App.FindElement(MobileBy.XPath("//android.widget.ImageButton[@content-desc=\"Open navigation drawer\"]"));
            drawer.Click();
            var personMenu = App.FindElement(MobileBy.XPath("//android.widget.TextView[@text=\"Person\"]"));
            personMenu.Click();

            var salaryDisplay = FindUIElement("SalaryDisplay");
            var displayedSalary = salaryDisplay.Text;
            var initialSalary = double.Parse(displayedSalary);

            var input = FindUIElement("PrecentInput");
            input.Clear();
            input.SendKeys(salaryIncreasePercentage.ToString());

            // Act
            var submitButton = App.FindElement(MobileBy.XPath("//android.widget.Button[@text=\"Submit\"]"));
            submitButton.Click();


            // Assert
            var salaryLabel = FindUIElement("SalaryDisplay");
            var salaryAfterSubmission = double.Parse(salaryLabel.Text);
            var expectedSalary = initialSalary * (100 + salaryIncreasePercentage) / 100;
            salaryAfterSubmission.Should().BeApproximately(expectedSalary, 0.001);
        }

        [Test]
        public void Person_SalaryIncrease_ShouldShowErrors_WhenBelowMinimum()
        {
            // Arrange

            
            // navigate to the person page
            var drawer = App.FindElement(MobileBy.XPath("//android.widget.ImageButton[@content-desc=\"Open navigation drawer\"]"));
            drawer.Click();
            var personMenu = App.FindElement(MobileBy.XPath("//android.widget.TextView[@text=\"Person\"]"));
            personMenu.Click();

            var input = FindUIElement("PrecentInput");
            input.Clear();
            input.SendKeys("-10");

            // Act
        
            // Assert
            var inputError = FindUIElement("InputError");
            inputError.Text.Should().NotBeNullOrEmpty();
        }
    }
}
