using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECSharp.Test
{
    [TestClass]
    public class UnitTest2
    {
        struct Dodo {
            public int i;
        }

        Dodo[] values = new Dodo[5];
        
        [TestMethod]
        public void TestSpan()
        {
        }
    }
}