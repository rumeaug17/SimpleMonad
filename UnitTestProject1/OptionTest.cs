using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleMonad;

namespace UnitTestProject1
{
    [TestClass]
    public class OptionTest
    {
        private Option<int> v1 = 12;
        private Option<int> v2 = 1;

        public static Option<int> Add(Option<int> i1, int i2) => i1.Map(x => x + i2);

        public static Option<int> Add(Option<int> i1, Option<int> i2) => i1.Fmap(x => Add(i2, x));

        [TestMethod]
        public void CheckBasicOption()
        {
            var result0 =
                from x in v1
                select x;

            Assert.IsTrue(result0.HasValue && result0.Value == v1.Value);
        }

        [TestMethod]
        public void CheckOptionSelectWithNone()
        {
            var result =
                from x in v1
                where x < 10
                select x;

            Assert.IsFalse(result.HasValue);
        }

        [TestMethod]
        public void CheckOptionSelect()
        {
            var result =
                from x in v1
                where x > 10
                select x + 2;

            Assert.IsTrue(result.HasValue && result.Value == v1.Value + 2 );
        }

        [TestMethod]
        public void CheckOptionSelectMany()
        {
            var result =
                from x in v1
                from y in v2
                where x > 10
                select x + y;

            Assert.IsTrue(result.HasValue && result.Value == v1.Value + v2.Value);
        }

        [TestMethod]
        public void CheckOptionNone()
        {
            var result =
                from x in new Option<int>()
                where x > 10
                select x;

            Assert.IsFalse(result.HasValue);
        }

        [TestMethod]
        public void CheckFullOption()
        {
            var result =
                 from x in v1
                 from y in Add(x, 2)
                 from z in Add(y, 4)
                 where z + x >= 30
                 select y - 10;

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(result.Value, 4);
        }

    }
}
