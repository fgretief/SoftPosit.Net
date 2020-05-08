// SPDX-License-Identifier: MIT

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

                Assert.That(sizeof(Quire8), Is.EqualTo(32 / 8));
                Assert.That(sizeof(Quire16), Is.EqualTo(128 / 8));
                Assert.That(sizeof(Quire32), Is.EqualTo(512 / 8));
                Assert.That(sizeof(Quire64), Is.EqualTo(2048 / 8));
            }
        }
    }
}