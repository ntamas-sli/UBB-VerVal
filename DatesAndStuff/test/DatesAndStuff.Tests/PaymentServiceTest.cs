using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace DatesAndStuff.Tests
{
    internal class PaymentServiceTest
    {
        //[Test]
        //public void TestPaymentService_ManualMock()
        //{
        //    // Arrange
        //    Person sut = new Person("Test Pista",
        //     new EmploymentInformation(
        //         54,
        //         new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })),
        //     new TestPaymentService(),
        //     new LocalTaxData("4367558"),
        //     new FoodPreferenceParams()
        //     {
        //         CanEatChocolate = true,
        //         CanEatEgg = true,
        //         CanEatLactose = true,
        //         CanEatGluten = true
        //     }
        //    );
        //
        //    // Act
        //    bool result = sut.PerformSubsriptionPayment();
        //
        //    // Assert
        //    result.Should().BeTrue();
        //}

        [Test]
        public void TestPaymentService_ManualMock_InsuficentFunds()
        {
            // Arrange
            Person sut = new Person("Test Pista",
             new EmploymentInformation(
                 54,
                 new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })),
             new TestPaymentService(balance: Person.SubscriptionFee - 100),
             new LocalTaxData("4367558"),
             new FoodPreferenceParams()
             {
                 CanEatChocolate = true,
                 CanEatEgg = true,
                 CanEatLactose = true,
                 CanEatGluten = true
             }
            );

            // Act
            bool result = sut.PerformSubsriptionPayment();

            // Assert
            result.Should().BeFalse();
        }

        //[Test]
        //public void TestPaymentService_ManualMock_SuficentFunds()
        //{
        //    // Arrange
        //    Person sut = new Person("Test Pista",
        //     new EmploymentInformation(
        //         54,
        //         new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })),
        //     new TestPaymentService(balance: Person.SubscriptionFee + 100),
        //     new LocalTaxData("4367558"),
        //     new FoodPreferenceParams()
        //     {
        //         CanEatChocolate = true,
        //         CanEatEgg = true,
        //         CanEatLactose = true,
        //         CanEatGluten = true
        //     }
        //    );
        //
        //    // Act
        //    bool result = sut.PerformSubsriptionPayment();
        //
        //    // Assert
        //    result.Should().BeTrue();
        //}

        [Test]
        public void TestPaymentService_Mock()
        {
            // Arrange
            var paymentSequence = new MockSequence();
            var paymentService = new Mock<IPaymentService>();

            paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment());
            paymentService.InSequence(paymentSequence).Setup(m => m.GetBalance()).Returns(1000);
            paymentService.InSequence(paymentSequence).Setup(m => m.SpecifyAmount(Person.SubscriptionFee));
            paymentService.InSequence(paymentSequence).Setup(m => m.ConfirmPayment());

            var paymentServiceMock = paymentService.Object;

            Person sut = new Person("Test Pista",
             new EmploymentInformation(
                 54,
                 new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })),
                 paymentServiceMock
             ,
             new LocalTaxData("4367558"),
             new FoodPreferenceParams()
             {
                 CanEatChocolate = true,
                 CanEatEgg = true,
                 CanEatLactose = true,
                 CanEatGluten = true
             }
            );

            // Act
            bool result = sut.PerformSubsriptionPayment();

            // Assert
            result.Should().BeTrue();
            paymentService.Verify(m => m.StartPayment(), Times.Once);
            paymentService.Verify(m => m.GetBalance(), Times.Once);
            paymentService.Verify(m => m.SpecifyAmount(Person.SubscriptionFee), Times.Once);
            paymentService.Verify(m => m.ConfirmPayment(), Times.Once);
        }

        [Test]
        public void TestPaymentService_Mock_InsuficentFunds()
        {
            // Arrange
            var paymentSequence = new MockSequence();
            var paymentService = new Mock<IPaymentService>();

            paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment());
            paymentService.InSequence(paymentSequence).Setup(m => m.GetBalance()).Returns(499);
            paymentService.InSequence(paymentSequence).Setup(m => m.CancelPayment());

            var paymentServiceMock = paymentService.Object;

            Person sut = new Person("Test Pista",
             new EmploymentInformation(
                 54,
                 new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })),
                 paymentServiceMock
             ,
             new LocalTaxData("4367558"),
             new FoodPreferenceParams()
             {
                 CanEatChocolate = true,
                 CanEatEgg = true,
                 CanEatLactose = true,
                 CanEatGluten = true
             }
            );

            // Act
            bool result = sut.PerformSubsriptionPayment();

            // Assert
            result.Should().BeFalse();
            paymentService.Verify(m => m.StartPayment(), Times.Once);
            paymentService.Verify(m => m.GetBalance(), Times.Once);
            paymentService.Verify(m => m.CancelPayment(), Times.Once);
        }

        [Test]
        public void TestPaymentService_Mock_SuficentFunds()
        {
            // Arrange
            var paymentSequence = new MockSequence();
            var paymentService = new Mock<IPaymentService>();

            paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment());
            paymentService.InSequence(paymentSequence).Setup(m => m.GetBalance()).Returns(500);
            paymentService.InSequence(paymentSequence).Setup(m => m.SpecifyAmount(Person.SubscriptionFee));
            paymentService.InSequence(paymentSequence).Setup(m => m.ConfirmPayment());

            var paymentServiceMock = paymentService.Object;

            Person sut = new Person("Test Pista",
             new EmploymentInformation(
                 54,
                 new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int>() { 6201, 7210 })),
                 paymentServiceMock
             ,
             new LocalTaxData("4367558"),
             new FoodPreferenceParams()
             {
                 CanEatChocolate = true,
                 CanEatEgg = true,
                 CanEatLactose = true,
                 CanEatGluten = true
             }
            );

            // Act
            bool result = sut.PerformSubsriptionPayment();

            // Assert
            result.Should().BeTrue();
            paymentService.Verify(m => m.StartPayment(), Times.Once);
            paymentService.Verify(m => m.GetBalance(), Times.Once);
            paymentService.Verify(m => m.SpecifyAmount(Person.SubscriptionFee), Times.Once);
            paymentService.Verify(m => m.ConfirmPayment(), Times.Once);
        }

        //[Test]
        //[CustomPersonCreationAutodataAttribute]
        //public void TestPaymentService_MockWithAutodata(Person sut, Mock<IPaymentService> paymentService)
        //{
        //    // Arrange
        //
        //    // Act
        //    bool result = sut.PerformSubsriptionPayment();
        //
        //    // Assert
        //    result.Should().BeTrue();
        //    paymentService.Verify(m => m.StartPayment(), Times.Once);
        //    paymentService.Verify(m => m.GetBalance(), Times.Once);
        //    paymentService.Verify(m => m.SpecifyAmount(Person.SubscriptionFee), Times.Once);
        //    paymentService.Verify(m => m.ConfirmPayment(), Times.Once);
        //}
    }
}
