using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DatesAndStuff.Web.Tests
{
    [TestFixture]
    public class PersonPageTests
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private const string BaseURL = "http://localhost:5091";
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
        [TestCase(-9)]
        public void Person_SalaryIncrease_ShouldIncrease(double salaryIncreasePercentage)
        {
            // Arrange
            String path = BaseURL + "/person";
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

            System.Threading.Thread.Sleep(1000);
            // Megkeressük az input mezőt, töröljük és -15-öt írunk be
            var input = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='SalaryIncreasePercentageInput']")));
            input.Clear();
            input.SendKeys("-15");

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
    }
}