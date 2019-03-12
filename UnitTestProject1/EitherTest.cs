using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleMonad;

namespace UnitTestProject1
{
    [TestClass]
    public class EitherTest
    {
        Either<int, string> GetValue(bool isString)
        {
            if (isString)
            {
                return "string";
            }
            else
            {
                return 0;
            }
        }

        [TestMethod]
        public void CheckEitherMatch()
        {
            var either = this.GetValue(true);
            var result = either.Match(
                 onInt => false,
                 onString => true);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckEitherLeftMap()
        {
            var either = this.GetValue(false);
            var result = either.LeftMap(x => x + 1);

            Assert.IsTrue(result.IsLeft);
            Assert.AreEqual(result.Left, 1);
        }
    }
}