using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleMonad;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class TryTest
    {
        public static Try<int> Div(int numerator, int denominator)
        {
            Func<int> f = () => numerator / denominator;
            return f.Try();
        }

        [TestMethod]
        public void CheckTrySelectMany()
        {
            var result0 =
                from x in Div(12, 2)
                from y in Div(x, 2)
                select y;

            Assert.IsTrue(result0.Try().IsValue);
            Assert.AreEqual(result0.Try().Value, 12 / 2 / 2);
        }

        [TestMethod]
        public void CheckTryWithException()
        {
            var result0 =
                from x in Div(12, 0)
                from y in Div(x, 2)
                select y;

            Assert.IsTrue(result0.Try().IsException);
        }

        [TestMethod]
        public void CheckTryWithWhere()
        {
            var result0 =
                from x in Div(12, 2)
                from y in Div(x, 2)
                where y > 2
                select y;

            Assert.IsTrue(result0.Try().IsValue);
            Assert.AreEqual(result0.Try().Value, 12 / 2 / 2);
        }

        [TestMethod]
        public void CheckTryWithNone()
        {
            var result0 =
                from x in Div(12, 2)
                from y in Div(x, 2)
                where y < 2
                select y;

            Assert.IsFalse(result0.Try().IsValue);
            Assert.IsTrue(result0.Try().IsException);
        }

        [TestMethod]
        public void CherckFullTry()
        {
            Try<int> result0 = 
                (from x in Div(12, 2) //§ 6
                from y in Div(x, 2) //§ 3
                from z in Div(x, y) //§ 2
                where y > 2
                select z + y).Try();

            Assert.IsTrue(result0.IsValue);
            Assert.AreEqual(result0.Value, 5);
        }

    }
}
