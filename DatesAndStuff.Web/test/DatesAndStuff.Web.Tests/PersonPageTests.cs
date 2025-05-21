using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using FluentAssertions;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DatesAndStuff.Web.Tests
{
    [TestFixture]
    public class PersonPageTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private const string BaseURL = "http://localhost:5091";
        private const string WizzURL = "https://www.wizzair.com/en-gb";
        private bool acceptNextAlert = true;

        private Process? _blazorProcess;

        [OneTimeSetUp]
        public void StartBlazorServer()
        {
            var webProjectPath = Path.GetFullPath(Path.Combine(
                Assembly.GetExecutingAssembly().Location,
                "../../../../../../src/DatesAndStuff.Web/DatesAndStuff.Web.csproj"
                ));

            var webProjFolderPath = Path.GetDirectoryName(webProjectPath);

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                //Arguments = $"run --project \"{webProjectPath}\"",
                Arguments = "dotnet run --no-build",
                WorkingDirectory = webProjFolderPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            _blazorProcess = Process.Start(startInfo);

            // Wait for the app to become available
            var client = new HttpClient();
            var timeout = TimeSpan.FromSeconds(30);
            var start = DateTime.Now;

            while (DateTime.Now - start < timeout)
            {
                try
                {
                    var result = client.GetAsync(BaseURL).Result;
                    if (result.IsSuccessStatusCode)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        [OneTimeTearDown]
        public void StopBlazorServer()
        {
            if (_blazorProcess != null && !_blazorProcess.HasExited)
            {
                _blazorProcess.Kill(true);
                _blazorProcess.Dispose();
            }
        }

        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            verificationErrors = new StringBuilder();
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
                driver.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [TestCase(10)]
        [TestCase(0)]
        [TestCase(-5)]
        [TestCase(-9.9)]
        public void Person_SalaryIncrease_ShouldIncrease(double salaryIncreasePercentage)
        {
            // Arrange
            string path = BaseURL + "/person";
            driver.Navigate().GoToUrl(path);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            System.Threading.Thread.Sleep(1000);

            string salaryText = string.Empty;
            int attempts = 0;

            while (attempts < 3)
            {
                try
                {
                    var displayedSalary = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='DisplayedSalary']")));
                    salaryText = displayedSalary.Text;
                    break;
                }
                catch (StaleElementReferenceException)
                {
                    attempts++;
                }
            }

            var initialSalary = double.Parse(salaryText);


            var input = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='SalaryIncreasePercentageInput']")));
            input.Clear();
            input.SendKeys(salaryIncreasePercentage.ToString());

            // Act
            var submitButton = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='SalaryIncreaseSubmitButton']")));
            submitButton.Click();


            // Assert
            var salaryLabel = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='DisplayedSalary']")));
            var salaryAfterSubmission = double.Parse(salaryLabel.Text);
            var expectedSalary = initialSalary * (100 + salaryIncreasePercentage) / 100;
            salaryAfterSubmission.Should().BeApproximately(expectedSalary, 0.001);
        }

        [Test]
        public void Person_SalaryIncrease_ShouldShowErrors_WhenBelowMinimum()
        {
            // Arrange
            string path = BaseURL + "/person";
            driver.Navigate().GoToUrl(path);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            System.Threading.Thread.Sleep(2000);
            var input = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='SalaryIncreasePercentageInput']")));
            input.Clear();
            input.SendKeys("-10");

            // Act
            var submitButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@data-test='SalaryIncreaseSubmitButton']")));
            submitButton.Click();

            // Assert
            var topError = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@data-test='SalaryIncreaseTopError']")));
            topError.Text.Should().NotBeNullOrEmpty();

            var inputError = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@data-test='SalaryIncreaseBottomError']")));
            inputError.Text.Should().NotBeNullOrEmpty();
        }




        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }

        [Test]
        public void Person_CheckFlight_ShouldbeTrue()
        {
            var options = new ChromeOptions();

            Random rand = new Random();
            int delay = rand.Next(1000, 2500);

            options.AddArgument("--lang=en-US");

            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/114.0.5735.110 Safari/537.36");

            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);

            options.AddArgument("--window-size=1920,1080");

            driver = new ChromeDriver(options);

            driver.Navigate().GoToUrl(WizzURL);

            ((IJavaScriptExecutor)driver).ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => undefined})");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            System.Threading.Thread.Sleep(8000);
            var input = wait.Until(ExpectedConditions.ElementExists(By.XPath("/ html / body / div[7] / div[2] / div / div[2] / div[1] / div / div[2] / div / div[1] / button")));
            input.Click();

            delay = rand.Next(1000, 2500);
            Thread.Sleep(delay);
            input = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='search-departure-station']")));
            input.Clear();
            foreach (char c in "TGM")
            {
                input.SendKeys(c.ToString());
                delay = rand.Next(100, 250);
                Thread.Sleep(delay);
            }
            delay = rand.Next(1000, 2500);
            Thread.Sleep(delay);
            input.SendKeys(Keys.Enter);

            delay = rand.Next(1000, 2500);
            Thread.Sleep(delay);
            input = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='search-arrival-station']")));
            input.Clear();
            foreach (char c in "BUD")
            {
                input.SendKeys(c.ToString());
                delay = rand.Next(100, 250);
                Thread.Sleep(delay);
            }
            delay = rand.Next(1000, 2500);
            Thread.Sleep(delay);
            input.SendKeys(Keys.Enter);
            delay = rand.Next(1000, 2500);
            Thread.Sleep(delay);
            input = wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[1]/div/main/div/div/div[1]/div[1]/div[1]/div[2]/div/div[2]/div/div[1]/form/div/fieldset[2]/div/div[1]/div[2]/div/input")));
            input.Click();
            delay = rand.Next(1000, 2500);
            Thread.Sleep(delay);

            DateTime today = DateTime.Today;

            bool startingDayFound = false;
            bool endDayFound = false;
            DateTime startDate = today;
            DateTime endDate = today;
            string dateXPath = "";

            for (DateTime t = today.AddDays(1); t <= today.AddDays(7); t = t.AddDays(1))
            {
                delay = rand.Next(1000, 2500);
                Thread.Sleep(delay);
                string day = t.Day.ToString();
                string month = t.ToString("MMMM");
                string year = t.Year.ToString();
                string dayName = t.DayOfWeek.ToString();

                string xPathstring = "//*[@aria-label='" + dayName + " " + day + " " + month + " " + year + "']";

                var button = wait.Until(ExpectedConditions.ElementExists(By.XPath(xPathstring)));

                if (!startingDayFound)
                {
                    string classAttr = button.GetAttribute("class");

                    if (classAttr != null && classAttr.Contains("vc-custom-start"))
                    {
                        startingDayFound = true;
                        startDate = t;
                    }
                }
                else
                {
                    string ariaLabelAttr = button.GetAttribute("aria-disabled");
                    if (ariaLabelAttr != null && ariaLabelAttr == "false")
                    {
                        endDayFound = true;
                        endDate = t;
                        dateXPath = xPathstring;
                    }
                }
            }
            delay = rand.Next(1000, 2500);
            Thread.Sleep(delay);
            input = wait.Until(ExpectedConditions.ElementExists(By.XPath(dateXPath)));
            //*[@id="popper-panel-12"]
            input.Click();
            delay = rand.Next(1000, 2500);
            Thread.Sleep(delay);
            input = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='flight-search-submit']")));
            input.Click();
            //*[@id="popper-panel-14"]/div/div[2]/div[1]/div/div[2]/div[24]/span
            Console.Write(input.ToString());

            (startingDayFound && endDayFound).Should().BeTrue();


            driver.Quit();
            driver.Dispose();
        }
    }
}