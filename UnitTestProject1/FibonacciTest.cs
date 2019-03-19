using Microsoft.VisualStudio.TestTools.UnitTesting;
using Memo = System.Collections.Generic.Dictionary<long, long>;
using static SimpleMonad.OptionExt; // for None and Some

namespace SimpleMonad
{
    [TestClass]
    public class FibonacciTest
    {
        public static long Fibo(long number)
            => FiboImpl(number).Eval(new Memo());

        private static Memo DictUpdate(Memo m, long key, long value)
        {
            m.Add(key, value);
            return m;
        }

        private static State<long, Memo> FiboImpl(long number)
        {
            if (number <= 1)
            {
                return State<long, Memo>.Init(1);
            }

            var memoed = State<Option<long>, Memo>.GetS(memo => memo.ContainsKey(number) ? Some(memo[number]) : None<long>());
            var result = memoed.Fmap(res =>
                res.Match(
                    someFunc: v => State<long, Memo>.Init(v),
                    noneFunc: () =>
                    from a in FiboImpl(number - 1)
                    from b in FiboImpl(number - 2)
                    let x = a + b
                    from _ in State<long, Memo>.Update(m => DictUpdate(m, number, x))
                    select x)      
            );
            return result;
        }

        [TestMethod]
        public void CheckInitValuesForFibonacciSuite()
        {
            Assert.AreEqual(1, Fibo(0));
            Assert.AreEqual(1, Fibo(1));
        }

        [TestMethod]
        public void CheckFirstValuesForFibonacciSuite()
        {
            Assert.AreEqual(2, Fibo(2));
            Assert.AreEqual(3, Fibo(3));
            Assert.AreEqual(5, Fibo(4));
            Assert.AreEqual(8, Fibo(5));
            Assert.AreEqual(13, Fibo(6));
            Assert.AreEqual(21, Fibo(7));

        }

        [TestMethod]
        public void Check10thValueForFibonacciSuite()
        {
            Assert.AreEqual(89, Fibo(10));
        }

        [TestMethod]
        public void CheckAdditionWithFibonacciSuite()
        {
            var res = from f1 in FiboImpl(5)
                      from f2 in FiboImpl(f1)
                      from f3 in FiboImpl(15)
                      select f1 + f2 + f3;
            var result = res.Eval(new Memo());

            Assert.AreEqual(1029, result);
        }
    }
}
