using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;
using System;

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

        [Test]
        public void JerrysPizza_ShouldOrderPizza()
        {
            var random = new Random();

            App.StartRecordingScreen();

            var mainMenu = App.FindElement(MobileBy.XPath("(//android.widget.ImageView[@resource-id=\"ro.jerryspizza.android:id/image\"])[3]"));
            mainMenu.Click();

            Thread.Sleep(random.Next(500, 2000));

            var pizzaItem = App.FindElement(MobileBy.XPath("//android.widget.TextView[@resource-id=\"ro.jerryspizza.android:id/name\" and @text=\"Pizza Texas\"]"));
            pizzaItem.Click();

            Thread.Sleep(random.Next(500, 2000));

            var crustOption = App.FindElement(MobileBy.XPath("//android.widget.TextView[@resource-id=\"ro.jerryspizza.android:id/name\" and @text=\"Normal crust\"]"));
            crustOption.Click();

            Thread.Sleep(random.Next(500, 2000));

            var sosOption = App.FindElement(MobileBy.XPath("(//androidx.cardview.widget.CardView[@resource-id=\"ro.jerryspizza.android:id/cardView\"])[7]/android.view.ViewGroup/android.view.ViewGroup"));
            sosOption.Click();

            Thread.Sleep(random.Next(500, 2000));
            
            var addToCart = App.FindElement(MobileBy.XPath("//android.widget.Button[@resource-id=\"ro.jerryspizza.android:id/addToCart\"]"));
            addToCart.Click();

            Thread.Sleep(random.Next(500, 2000));

            var cart = App.FindElement(MobileBy.XPath("//android.widget.FrameLayout[@resource-id=\"ro.jerryspizza.android:id/cart\"]/android.widget.ImageView"));
            cart.Click();

            Thread.Sleep(random.Next(500, 2000));

            var continueBtn = App.FindElement(MobileBy.XPath("//android.widget.Button[@resource-id=\"ro.jerryspizza.android:id/openCheckout\"]"));
            continueBtn.Click();

            Thread.Sleep(random.Next(500, 2000));

            var sendOrder = App.FindElement(MobileBy.XPath("//android.widget.Button[@resource-id=\"ro.jerryspizza.android:id/sendOrder\"]"));
            sendOrder.Click();

            Thread.Sleep(random.Next(500, 1000));

            string base64Video = App.StopRecordingScreen();

            File.WriteAllBytes("JerrysPizzaOrder.mp4", Convert.FromBase64String(base64Video));

            var errorBox = App.FindElement(MobileBy.XPath("//android.widget.TextView[@resource-id=\"ro.jerryspizza.android:id/snackbar_text\"]"));
            errorBox.Text.Should().Be("Please select a payment method");
        }
    }
}
