using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace RaffleNamespace
{
    [TestFixture]
    public class RaffleTests
    {
        [Test]
        public void TestGenerateRandomTicket()
        {
            List<int> winningNumbers = Raffle.GenerateRandomTicket();

            Assert.IsNotNull(winningNumbers);
            Assert.AreEqual(5, winningNumbers.Count);

            foreach (int number in winningNumbers)
            {
                Assert.IsTrue(number >= 1 && number <= 15, $"Number {number} is not within the expected range [1-15]");
            }
        }

        [Test]
        public void TestCountMatchingNumbers()
        {
            IEnumerable<int> number = new List<int> { 4, 7, 12, 8, 10 };

            IEnumerable<int> winningNumbers = new List<int> { 5, 8, 13, 4, 9 };

            int count = winningNumbers.Intersect(number).Count();

            Assert.AreEqual(2, count);
        }

        [Test]
        public void TestGetRewardPercentage()
		{
            int groupNumber = 3;

            double rewards = Raffle.GetRewardPercentage(groupNumber);

            Assert.AreEqual(15, rewards);

		}
    }
}