using FluentAssertions;

namespace DatesAndStuff.Tests;

public class PersonTests
{
    Person sut;

    [SetUp]
    public void Setup()
    {
        this.sut = new Person("Test Pista", 54);
    }

    public class GotMerriedTests : PersonTests
    {
        [Test]
        public void First_NameShouldChange()
        {
            // Arrange
            string newName = "Test-Eleso Pista";
            double salaryBeforeMarriage = sut.Salary;
            var beforeChanges = Person.Clone(sut);

            // Act
            sut.GotMarried(newName);

            // Assert
            Assert.That(sut.Name, Is.EqualTo(newName)); // act = exp

            sut.Name.Should().Be(newName);
            sut.Should().BeEquivalentTo(beforeChanges, o => o.Excluding(p => p.Name));

            //sut.Salary.Should().Be(salaryBeforeMarriage);

            //Assert.AreEqual(newName, sut.Name); // = (exp, act)
            //Assert.AreEqual(salaryBeforeMarriage, sut.Salary);
        }

        [Test]
        public void Second_ShouldFail()
        {
            // Arrange
            string newName = "Test-Eleso-Felallo Pista";
            sut.GotMarried("");

            // Act
            var task = Task.Run(() => sut.GotMarried(""));
            try { task.Wait(); } catch { }

            // Assert
            Assert.IsTrue(task.IsFaulted);
        }
    }

    public class IncreaseSalaryTests : PersonTests
    {
        [Test]
        public void PositiveIncrease_ShouldIncrease()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void ZeroPercentIncrease_ShouldNotChange()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void NegativeIncrease_ShouldDecrease()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void SmallerThanMinusTenPerc_ShouldFail()
        {
            throw new NotImplementedException();
        }
    }
}