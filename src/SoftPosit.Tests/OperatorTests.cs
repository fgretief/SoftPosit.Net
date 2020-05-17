// SPDX-License-Identifier: MIT

using NUnit.Framework;

namespace System.Numerics.Posits.Tests
{
    public class OperatorTests
    {
        [TestCase(0x80, 0x80, 0x80, "NaR + NaR = NaR")]
        [TestCase(0x80, 0x00, 0x80, "NaR + 0 = NaR")]
        [TestCase(0x80, 0x40, 0x80, "NaR + 1 = NaR")]
        [TestCase(0x80, 0xC0, 0x80, "NaR + -1 = NaR")]
        public void TestAddSubNaR(byte a, byte b, byte c, string op)
        {
            var pA = new Posit8(a);
            var pB = new Posit8(b);
            var pC = new Posit8(c);

            Console.WriteLine(op);

            Assert.That((pA + pB).ui, Is.EqualTo(pC.ui));
            Assert.That((pB + pA).ui, Is.EqualTo(pC.ui));
            Assert.That((pC - pB).ui, Is.EqualTo(pA.ui));
        }

        [TestCase(0x00, 0x00, 0x00, "0 + 0 = 0")]
        [TestCase(0x00, 0x40, 0x40, "0 + 1 = 1")]
        [TestCase(0x00, 0xC0, 0xC0, "0 + -1 = -1")]

        [TestCase(0x60, 0x60, 0x70, "2 + 2 = 4")]
        [TestCase(0x60, 0xA0, 0x00, "2 + -2 = 0")]

        [TestCase(0x40, 0b0010_0000, 0b0101_0000, "1 + 0.5 = 1.5")]
        [TestCase(0x40, 0b0001_0000, 0b0100_1000, "1 + 0.25 = 0.25")]
        [TestCase(0x40, 0b0000_1000, 0b0100_0100, "1 + 0.125 = 1.125")]
        [TestCase(0x40, 0b0000_0100, 0b0100_0010, "1 + 0.0625 = 1.0625")]

        [TestCase(0x40, 0b0100_0000, 0b0110_0000, "1 + 1 = 2")]
        [TestCase(0x40, 0b0101_0000, 0b0110_0100, "1 + 1.5 = 2.5")]
        [TestCase(0x40, 0b0100_1000, 0b0110_0010, "1 + 1.25 = 2.25")]
        [TestCase(0x40, 0b0100_0100, 0b0110_0001, "1 + 1.125 = 2.125")]

        [TestCase(0x40, 0b1100_0000, 0x0000_0000, "1 + -1 =  0")]
        [TestCase(0x40, 0b1011_0000, 0b1110_0000, "1 + -1.5 = -0.5")]
        [TestCase(0x40, 0b1011_1000, 0b1111_0000, "1 + -1.25 = -0.25")]
        [TestCase(0x40, 0b1011_1100, 0b1111_1000, "1 + -1.125 = -0.125")]
        [TestCase(0x40, 0b1011_1110, 0b1111_1100, "1 + -1.125 = -0.0625")]
        [TestCase(0x40, 0b1010_0000, 0b1100_0000, "1 + -2 = -1")]

        [TestCase(0x40, 0b1110_0000, 0b0010_0000, "1 + -0.5 = 0.5")]
        [TestCase(0x40, 0b1111_0000, 0b0011_0000, "1 + -0.25 = 0.75")]
        [TestCase(0x40, 0b1111_1000, 0b0011_1000, "1 + -0.125 = 0.875")]
        [TestCase(0x40, 0b1111_1100, 0b0011_1100, "1 + -0.0625 = 0.9375")]

        [TestCase(0b0000_1000, 0b0000_0100, 0b0000_1100, "0.125 + 0.0625 = 0.1875")]
        [TestCase(0b0000_1000, 0b1111_1100, 0b0000_0100, "0.125 + -0.0625 = 0.0625")]

        public void TestAddSub(byte a, byte b, byte c, string op)
        {
            var pA = new Posit8(a);
            var pB = new Posit8(b);
            var pC = new Posit8(c);

            Console.WriteLine(op);

            Assert.That((pA + pB).ui, Is.EqualTo(pC.ui));
            Assert.That((pB + pA).ui, Is.EqualTo(pC.ui));

            Assert.That((pC - pA).ui, Is.EqualTo(pB.ui));
            Assert.That((pC - pB).ui, Is.EqualTo(pA.ui));
        }

        [TestCase(0b01111111, 0b01111110, 0b01111111, "64 + 32 = 64")]
        [TestCase(0b01111111, 0b00000001, 0b01111111, "maxpos + minpos = maxpos")]
        [TestCase(0b01000000, 0b00000001, 0b01000000, "1 + minpos = 1")] // bitNPlusOne
        public void TestAdd(byte a, byte b, byte c, string op)
        {
            var pA = new Posit8(a);
            var pB = new Posit8(b);
            var pC = new Posit8(c);

            Console.WriteLine(op);

            Assert.That((pA + pB).ui, Is.EqualTo(pC.ui));
            Assert.That((pB + pA).ui, Is.EqualTo(pC.ui));
        }

        [TestCase(0b10000001, 0b01111110, 0b10000001, "-64 - 32 = -64")]
        [TestCase(0b01111111, 0b01111110, 0b01111110, "64 - 32 = 32")]
        [TestCase(0b01111110, 0b01111111, 0b10000010, "32 - 64 = -32")]
        [TestCase(0b00000001, 0b01000001, 0b11000000, "minpos - 1.03125 = -1")] // bitNPlusOne
        public void TestSub(byte a, byte b, byte c, string op)
        {
            var pA = new Posit8(a);
            var pB = new Posit8(b);
            var pC = new Posit8(c);

            Console.WriteLine(op);

            Assert.That((pA - pB).ui, Is.EqualTo(pC.ui));
        }

        [TestCase(0b1000_0000, 0b1000_0000, 0b1000_0000, "NaR * NaR = NaR")]
        [TestCase(0b1000_0000, 0b0000_0000, 0b1000_0000, "NaR * 0 = NaR")]
        [TestCase(0b1000_0000, 0b0100_0000, 0b1000_0000, "NaR * 1 = NaR")]
        [TestCase(0b1000_0000, 0b1100_0000, 0b1000_0000, "NaR * -1 = NaR")]

        [TestCase(0b0000_0000, 0b0000_0000, 0b0000_0000, "0 * 0 = 0")]
        [TestCase(0b0000_0000, 0b0100_0000, 0b0000_0000, "0 * 1 = 0")]
        [TestCase(0b0000_0000, 0b1100_0000, 0b0000_0000, "0 * -1 = 0")]

        [TestCase(0b0100_0000, 0b0100_0000, 0b0100_0000, "1 * 1 = 1")]
        [TestCase(0b0100_0000, 0b0010_0000, 0b0010_0000, "1 * 0.5 = 0.5")]
        [TestCase(0b0100_0000, 0b0001_0000, 0b0001_0000, "1 * 0.25 = 0.25")]

        [TestCase(0b0100_0000, 0b1100_0000, 0b1100_0000, "1 * -1 = -1")]
        [TestCase(0b0100_0000, 0b1110_0000, 0b1110_0000, "1 * -0.5 = -0.5")]
        [TestCase(0b0100_0000, 0b1111_0000, 0b1111_0000, "1 * -0.25 = -0.25")]

        [TestCase(0b1100_0000, 0b1100_0000, 0b0100_0000, "-1 * -1 = 1")]
        [TestCase(0b0001_0000, 0b0001_0000, 0b0000_0100, "0.25 * 0.25 = 0.0625")]
        [TestCase(0b0111_1000, 0b0111_1000, 0b0111_1111, "8 * 8 = 64")]

        [TestCase(0x50, 0x01, 0x02, "1.5 * 0.015625 = 0.03125")]
        [TestCase(0x03, 0x03, 0x01, "0.046875 * 0.046875 = 0.015625")]
        public void TestMul(byte a, byte b, byte c, string op)
        {
            var pA = new Posit8(a);
            var pB = new Posit8(b);
            var pC = new Posit8(c);

            Console.WriteLine(op);

            Assert.That((pA * pB).ui, Is.EqualTo(pC.ui));
            Assert.That((pB * pA).ui, Is.EqualTo(pC.ui));
        }

        [TestCase(0b1000_0000, 0b1000_0000, 0b1000_0000, "NaR / NaR = NaR")]
        [TestCase(0b1000_0000, 0b0000_0000, 0b1000_0000, "NaR / 0 = NaR")]
        [TestCase(0b1000_0000, 0b0100_0000, 0b1000_0000, "NaR / 1 = NaR")]
        [TestCase(0b1000_0000, 0b1100_0000, 0b1000_0000, "NaR / -1 = NaR")]

        [TestCase(0b0000_0000, 0b0000_0000, 0b1000_0000, "0 / 0 = NaR")]
        [TestCase(0b0000_0000, 0b0100_0000, 0b0000_0000, "0 / 1 = 0")]
        [TestCase(0b0000_0000, 0b1100_0000, 0b0000_0000, "0 / -1 = 0")]

        [TestCase(0b0100_0000, 0b0000_0000, 0b1000_0000, "1 / 0 = NaR")]
        [TestCase(0b0100_0000, 0b0100_0000, 0b0100_0000, "1 / 1 = 1")]
        [TestCase(0b0100_0000, 0b1100_0000, 0b1100_0000, "1 / -1 = -1")]
        [TestCase(0b0100_0000, 0b0010_0000, 0b0110_0000, "1 / 0.5 = 2")]
        [TestCase(0b0100_0000, 0b0001_0000, 0b0111_0000, "1 / 0.25 = 4")]
        [TestCase(0b0100_0000, 0b0110_0000, 0b0010_0000, "1 / 2 = 0.5")]
        [TestCase(0b0100_0000, 0b0111_0000, 0b0001_0000, "1 / 4 = 0.25")]

        [TestCase(0b0010_0000, 0b0100_0000, 0b0010_0000, "0.5 / 1 = 0.5")]
        [TestCase(0b0001_0000, 0b0100_0000, 0b0001_0000, "0.25 / 1 = 0.25")]
        [TestCase(0b0110_0000, 0b0100_0000, 0b0110_0000, "2 / 1 = 2")]
        [TestCase(0b0111_0000, 0b0100_0000, 0b0111_0000, "4 / 1 = 4")]

        [TestCase(0b1100_0000, 0b0000_0000, 0b1000_0000, "-1 / 0 = NaR")]
        [TestCase(0b1100_0000, 0b0100_0000, 0b1100_0000, "-1 / 1 = -1")]
        [TestCase(0b1100_0000, 0b1100_0000, 0b0100_0000, "-1 / -1 = 1")]

        [TestCase(0x01, 0x05, 0b00001101, "0.015625 / 0.078125 = 0.203125")]
        [TestCase(0x01, 0x41, 0b00000001, "0.015625 / 1.03125 = 0.015625")]
        public void TestDiv(byte a, byte b, byte c, string op)
        {
            var pA = new Posit8(a);
            var pB = new Posit8(b);
            var pC = new Posit8(c);

            Console.WriteLine(op);

            Assert.That((pA / pB).ui, Is.EqualTo(pC.ui));
        }

        [TestCase(0b1000_0000, 0b1000_0000, 0b1000_0000, "NaR % NaR = NaR")]
        [TestCase(0b1000_0000, 0b0000_0000, 0b1000_0000, "NaR % 0 = NaR")]
        [TestCase(0b1000_0000, 0b0100_0000, 0b1000_0000, "NaR % 1 = NaR")]
        [TestCase(0b1000_0000, 0b1100_0000, 0b1000_0000, "NaR % -1 = NaR")]

        [TestCase(0b0000_0000, 0b0000_0000, 0b1000_0000, "0 % 0 = NaR")]
        [TestCase(0b0000_0000, 0b0100_0000, 0b0000_0000, "0 % 1 = 0")]
        [TestCase(0b0000_0000, 0b1100_0000, 0b0000_0000, "0 % -1 = 0")]

        [TestCase(0b0100_0000, 0b0100_0000, 0b0000_0000, "1 % 1 = 0")]
        [TestCase(0b0010_0000, 0b0010_0000, 0b0000_0000, "0.5 % 0.5 = 0")]
        [TestCase(0b0001_0000, 0b0001_0000, 0b0000_0000, "0.25 % 0.25 = 0")]
        [TestCase(0b1100_0000, 0b1100_0000, 0b0000_0000, "-1 % -1 = 0")]

        // TODO: more test-cases here
        public void TestMod(byte a, byte b, byte c, string op)
        {
            var pA = new Posit8(a);
            var pB = new Posit8(b);
            var pC = new Posit8(c);

            Console.WriteLine(op);

            Assert.That((pA % pB).ui, Is.EqualTo(pC.ui));
        }
    }
}
