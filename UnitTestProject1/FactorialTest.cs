using SimpleMonad;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Memo = System.Collections.Generic.Dictionary<ulong, ulong>;
using static SimpleMonad.OptionExt; // for None and Some

namespace UnitTestProject1
{
    internal static class DictionaryExt
    {
        public static Option<ulong> GetValueOrNone(this Memo memo, ulong key)
            => memo.ContainsKey(key) ? Some(memo[key]) : None<ulong>();      
    }

    [TestClass]
    public class FactorialTest
    {
        private static Memo DictUpdate(Memo m, ulong key, ulong value)
        {
            Console.WriteLine($"adding key {key}, for value {value}");
            m.Add(key, value);
            return m;
        }

        public static ulong Fact(ulong number)
            => FactImpl(number).Eval(new Memo());

        private static State<ulong, Memo> FactImpl(ulong number)
        {
            if (number <= 1)
            {
                return State<ulong, Memo>.Init(1);
            }

            var memoed = State<Option<ulong>, Memo>.GetS(memo => memo.GetValueOrNone(number));
            var result = memoed.Fmap(res =>
               res.Match(
                   someFunc: v => State<ulong, Memo>.Init(v),
                   noneFunc: () =>
                       from next in FactImpl(number - 1)
                       let r = number * next
                       from _ in State<ulong, Memo>.Update(m => DictUpdate(m, number, r))
                       select r)
           );
            return result;
        }

        [TestMethod]
        public void CheckFactInitValues()
        {
            Assert.IsTrue(1 == Fact(0));
            Assert.IsTrue(1 == Fact(1));
        }

        [TestMethod]
        public void CheckFact15()
        {
            Assert.AreEqual((ulong)1307674368000, Fact(15));
        }

        [TestMethod]
        public void CheckBigFact()
        {
            Assert.AreEqual(expected: (ulong)2_432_902_008_176_640_000, actual: Fact(20));
        }

        [TestMethod]
        public void CheckSumOfFirst10Fact()
        {
            // for a list gen we need a scanleft function on list or enumerable
            var res = from f1 in FactImpl(10)
                      from f2 in FactImpl(9)
                      from f3 in FactImpl(8)
                      from f4 in FactImpl(7)
                      from f5 in FactImpl(6)
                      from f6 in FactImpl(5)
                      from f7 in FactImpl(4)
                      from f8 in FactImpl(3)
                      from f9 in FactImpl(2)
                      from f10 in FactImpl(1)
                      select f1 + f2 + f3 + f4 + f5 + f6 + f7 + f8 + f9 + f10;

            var result = res.Eval(new Memo());
            Assert.AreEqual((ulong)4037913, result);
        }
    }
}
