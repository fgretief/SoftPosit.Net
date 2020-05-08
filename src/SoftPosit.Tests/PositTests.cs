using NUnit.Framework;

namespace System.Numerics.Posits.Tests
{
    public class PositTests
    {
        [Test]
        public void TestSizeOf()
        {
            unsafe
            {
                Assert.That(sizeof(Posit8), Is.EqualTo(sizeof(byte)));
                Assert.That(sizeof(Posit16), Is.EqualTo(sizeof(ushort)));
                Assert.That(sizeof(Posit32), Is.EqualTo(sizeof(uint)));
                Assert.That(sizeof(Posit64), Is.EqualTo(sizeof(ulong)));
            }
        }
    }
}