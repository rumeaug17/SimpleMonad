using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleMonad;
using System.Globalization;

namespace SimpleMonadLibrayUnitTests
{
    [TestClass]
    public class ReaderEnvTest
    {
        [TestMethod]
        public void ReaderTest()
        {
            bool isExecuted1 = false;
            bool isExecuted2 = false;

            var f1 = new Reader<int, int>(x => { isExecuted1 = true; return x + 1; });
            var f2 = new Reader<int, string> (x =>
            { isExecuted2 = true; return x.ToString(CultureInfo.InvariantCulture); });

            var query1 = from x in f1
                         from y in f2
                         select (x, y);

            Assert.IsFalse(isExecuted1); // Laziness.
            Assert.IsFalse(isExecuted2); // Laziness.
            var result = query1.Run(1);

            Assert.AreEqual((2, "1"), result); // Execution.
            Assert.IsTrue(isExecuted1);
            Assert.IsTrue(isExecuted2);

            Assert.AreEqual((2, "1"), query1.Run(1)); // Execution.
            Assert.IsTrue(isExecuted1);
            Assert.IsTrue(isExecuted2);

        }
    }
}
