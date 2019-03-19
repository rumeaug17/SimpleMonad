using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleMonad;
using System;

namespace SimpleMonadLibrayUnitTests
{
    internal static class Calculator
    {
        public static Writer<int, string> sum(int a, int b) =>
            new Writer<int, string>(a + b, $"sum({a}, {b})");

        public static Writer<int, string> mul(int a, int b) =>
            new Writer<int, string>(a * b, $"mul({a}, {b})");

        public static Writer<int, string> diff(int a, int b) =>
            new Writer<int, string>(a - b, $"diff({a}, {b})");

        public static Writer<int, string> div(int a, int b) =>
            new Writer<int, string>(a / b, $"div({a}, {b})");

        public static Writer<int, string> squared(int a) =>
            new Writer<int, string>(a * a, $"squared({a})");

    }

    [TestClass]
    public class CalculatorTest
    {
        [TestMethod]
        public void CheckCalculator()
        {
            var final = Calculator.sum(5, 5)
            .Fmap(n => Calculator.sum(n, 3))
            .Fmap(n => Calculator.sum(n, 10))
            .Fmap(n => Calculator.squared(n))
            .Fmap(n => Calculator.diff(n, 7));

           foreach(string s in final.Log)
            {
                Console.WriteLine(s);
            }

            Assert.AreEqual(522, final.Value);
        }

        [TestMethod]
        public void CheckCalculatorLinq()
        {
            var final = from n in Calculator.sum(5, 5)
                        from n2 in Calculator.sum(n, 3)
                        from n3 in Calculator.sum(n2, 10)
                        from n4 in Calculator.squared(n3)
                        from n5 in Calculator.diff(n4, 7)
                        select n5;

            foreach (string s in final.Log)
            {
                Console.WriteLine(s);
            }

            Assert.AreEqual(522, final.Value);
        }

    }
}
