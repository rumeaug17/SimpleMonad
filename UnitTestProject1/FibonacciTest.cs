using Microsoft.VisualStudio.TestTools.UnitTesting;
using Memo = System.Collections.Generic.Dictionary<ulong, ulong>;
using static SimpleMonad.OptionExt; // for None and Some

namespace SimpleMonad
{
    internal static class DictionaryExt
    {
        public static Option<ulong> GetValueOrNone(this Memo memo, ulong key)
            => memo.ContainsKey(key) ? Some(memo[key]) : None<ulong>();
    }

    [TestClass]
    public class FibonacciTest
    {
        public static ulong Fibo(ulong number)
            => FiboImpl(number).Eval(new Memo());

        private static Memo DictUpdate(Memo m, ulong key, ulong value)
        {
            m.Add(key, value);
            return m;
        }

        private static State<ulong, Memo> FiboImpl(ulong number)
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
                    from a in FiboImpl(number - 1)
                    from b in FiboImpl(number - 2)
                    let x = a + b
                    from _ in State<ulong, Memo>.Update(m => DictUpdate(m, number, x))
                    select x)      
            );
            return result;
        }

        [TestMethod]
        public void CheckInitValuesForFibonacciSuite()
        {
            Assert.AreEqual(1ul, Fibo(0));
            Assert.AreEqual(1ul, Fibo(1));
        }

        [TestMethod]
        public void CheckFirstValuesForFibonacciSuite()
        {
            Assert.AreEqual(2ul, Fibo(2));
            Assert.AreEqual(3ul, Fibo(3));
            Assert.AreEqual(5ul, Fibo(4));
            Assert.AreEqual(8ul, Fibo(5));
            Assert.AreEqual(13ul, Fibo(6));
            Assert.AreEqual(21ul, Fibo(7));

        }

        [TestMethod]
        public void Check10thValueForFibonacciSuite()
        {
            Assert.AreEqual(89ul, Fibo(10));
        }

        [TestMethod]
        public void CheckBigValueForFibonacciSuite()
        {
            Assert.AreEqual(7_540_113_804_746_346_429UL, Fibo(91));                           
        }

        [TestMethod]
        public void CheckAdditionWithFibonacciSuite()
        {
            var res = from f1 in FiboImpl(5)
                      from f2 in FiboImpl(f1)
                      from f3 in FiboImpl(15)
                      select f1 + f2 + f3;
            var result = res.Eval(new Memo());

            Assert.AreEqual(1029ul, result);
        }
    }
}
