using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatesAndStuff.Tests
{
    internal class TestPaymentService : IPaymentService
    {
        uint startCallCount = 0;
        uint specifyCallCount = 0;
        uint confirmCallCount = 0;
        uint getBalanceCallCount = 0;
        double balance;

        public TestPaymentService() { 
            this.balance = 1000;
        }
        public TestPaymentService(double balance)
        {
            this.balance = balance;
        }

        public void StartPayment()
        {
            if (startCallCount != 0 || specifyCallCount > 0 || confirmCallCount > 0 || getBalanceCallCount > 0)
                throw new Exception();

            startCallCount++;
        }

        public void SpecifyAmount(double amount)
        {
            if (amount < balance) 
                throw new Exception();
            if (startCallCount != 1 || specifyCallCount > 0 || confirmCallCount > 0 || getBalanceCallCount != 1)
                throw new Exception();

            specifyCallCount++;
        }

        public void ConfirmPayment()
        {
            if (startCallCount != 1 || specifyCallCount != 1 || confirmCallCount > 0 || getBalanceCallCount != 1)
                throw new Exception();

            confirmCallCount++;
        }

        public bool SuccessFul()
        {
            return startCallCount == 1 && specifyCallCount == 1 && confirmCallCount == 1 && getBalanceCallCount == 1;
        }

        public double GetBalance()
        {
            getBalanceCallCount++;
            return balance;
        }

        public bool CancelPayment()
        {
            return startCallCount != 1 || specifyCallCount != 1 || confirmCallCount != 0 || getBalanceCallCount != 1;
        }
    }
}
