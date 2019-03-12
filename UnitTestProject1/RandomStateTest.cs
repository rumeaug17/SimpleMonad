using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SimpleMonad
{
    [TestClass]
    public class RandomStateTest
    {
        private static Tuple<short, int> NextShort(int state)
        {
            const int multiplier = 214013;
            const int increment = 2531011;
            const int modulus = int.MaxValue;
            var newState = multiplier * state + increment;
            var rand = (short)((newState & modulus) >> 16);

            return Tuple.Create(rand, newState);
        }

        public static State<short, int> GetRandom() => new State<short, int>
        {
            Run = state => NextShort(state)
        };

        [TestMethod]
        public void TestRandomGen()
        {
            var result = from a in GetRandom()
                         from b in GetRandom()
                         from c in GetRandom()
                         select (short) (a + b + c);

            var seed = 0;
            Assert.AreEqual(28995, result.Eval(seed));
        }

    }
}
