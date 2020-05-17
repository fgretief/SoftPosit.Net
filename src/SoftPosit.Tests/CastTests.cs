// SPDX-License-Identifier: MIT

using NUnit.Framework;

namespace System.Numerics.Posits.Tests
{
    public class CastTests
    {
        [TestCase( 0, 0b0000_0000)]
        [TestCase( 1, 0b0100_0000)]
        [TestCase( 2, 0b0110_0000)]
        [TestCase( 3, 0b0110_1000)]
        [TestCase( 4, 0b0111_0000)]
        [TestCase( 5, 0b0111_0010)]
        [TestCase( 6, 0b0111_0100)]
        [TestCase( 7, 0b0111_0110)]
        [TestCase( 8, 0b0111_1000)]
        [TestCase( 9, 0b0111_1000)] // rounded down to 8
        [TestCase(10, 0b0111_1001)]
        [TestCase(11, 0b0111_1010)] // rounded up to 12
        [TestCase(12, 0b0111_1010)]
        [TestCase(13, 0b0111_1010)] // rounded down to 12
        [TestCase(14, 0b0111_1011)]
        [TestCase(15, 0b0111_1100)] // rounded up to 16
        [TestCase(16, 0b0111_1100)]
        [TestCase(24, 0b0111_1101)]
        [TestCase(32, 0b0111_1110)]
        [TestCase(33, 0b0111_1110)] // rounded down to 32
        [TestCase(48, 0b0111_1110)] // rounded down to 32
        [TestCase(49, 0b0111_1111)] // rounded up to 64
        [TestCase(63, 0b0111_1111)] // rounded up to 64
        [TestCase(64, 0b0111_1111)]
        [TestCase(65, 0b0111_1111)] // rounded down to 64
        [TestCase(99, 0b0111_1111)] // rounded down to 64
        [TestCase(int.MaxValue, 0b0111_1111)] // rounded down to 64

        [TestCase( -1, 0b1100_0000)]
        [TestCase( -2, 0b1010_0000)]
        [TestCase( -3, 0b1001_1000)]
        [TestCase( -4, 0b1001_0000)]
        [TestCase( -5, 0b1000_1110)]
        [TestCase( -6, 0b1000_1100)]
        [TestCase( -7, 0b1000_1010)]
        [TestCase( -8, 0b1000_1000)]
        [TestCase( -9, 0b1000_1000)] // rounded down to -8
        [TestCase(-10, 0b1000_0111)]
        [TestCase(-11, 0b1000_0110)] // rounded up to 12
        [TestCase(-12, 0b1000_0110)]
        [TestCase(-13, 0b1000_0110)] // rounded down to 12
        [TestCase(-14, 0b1000_0101)]
        [TestCase(-15, 0b1000_0100)] // rounded up to 16
        [TestCase(-16, 0b1000_0100)]
        [TestCase(-24, 0b1000_0011)]
        [TestCase(-32, 0b1000_0010)]
        [TestCase(-33, 0b1000_0010)] // rounded down to -32
        [TestCase(-48, 0b1000_0010)] // rounded down to -32
        [TestCase(-49, 0b1000_0001)] // rounded up to -64
        [TestCase(-63, 0b1000_0001)] // rounded up to -64
        [TestCase(-64, 0b1000_0001)]
        [TestCase(-65, 0b1000_0001)] // rounded down to -64
        [TestCase(-99, 0b1000_0001)] // rounded down to -64
        [TestCase(int.MinValue, 0b1000_0001)] // rounded down to -64
        public void TestInt32ToPosit8Cast(int value, byte ui)
        {
            var p = new Posit8(ui);

            var x = (Posit8)value;

            Assert.That(x.ui, Is.EqualTo(p.ui));
        }

        [TestCase(uint.MinValue, 0b0000_0000)]
        [TestCase( 1u, 0b0100_0000)]
        [TestCase( 2u, 0b0110_0000)]
        [TestCase( 3u, 0b0110_1000)]
        [TestCase( 4u, 0b0111_0000)]
        [TestCase( 5u, 0b0111_0010)]
        [TestCase( 6u, 0b0111_0100)]
        [TestCase( 7u, 0b0111_0110)]
        [TestCase( 8u, 0b0111_1000)]
        [TestCase( 9u, 0b0111_1000)] // rounded down to 8
        [TestCase(10u, 0b0111_1001)]
        [TestCase(11u, 0b0111_1010)] // rounded up to 12
        [TestCase(12u, 0b0111_1010)]
        [TestCase(13u, 0b0111_1010)] // rounded down to 12
        [TestCase(14u, 0b0111_1011)]
        [TestCase(15u, 0b0111_1100)] // rounded up to 16
        [TestCase(16u, 0b0111_1100)]
        [TestCase(17u, 0b0111_1100)] // rounded down to 16
        [TestCase(23u, 0b0111_1101)] // rounded up to 24
        [TestCase(24u, 0b0111_1101)]
        [TestCase(32u, 0b0111_1110)]
        [TestCase(33u, 0b0111_1110)] // rounded down to 32
        [TestCase(48u, 0b0111_1110)] // rounded down to 32
        [TestCase(49u, 0b0111_1111)] // rounded up to 64
        [TestCase(63u, 0b0111_1111)] // rounded up to 64
        [TestCase(64u, 0b0111_1111)]
        [TestCase(65u, 0b0111_1111)] // rounded down to 64
        [TestCase(99u, 0b0111_1111)] // rounded down to 64
        [TestCase(uint.MaxValue, 0b0111_1111)] // rounded down to 64
        public void TestUInt32ToPosit8Cast(uint value, byte ui)
        {
            var p = new Posit8(ui);

            var x = (Posit8)value;

            Assert.That(x.ui, Is.EqualTo(p.ui));
        }

        [TestCase(0b1000_0000, 0)] // NaR

        [TestCase(0b0000_0000, 0)] // Zero
        [TestCase(0b0100_0000, 1)]
        [TestCase(0b0110_0000, 2)]
        [TestCase(0b0110_1000, 3)]
        [TestCase(0b0111_0000, 4)]
        [TestCase(0b0111_0010, 5)]
        [TestCase(0b0111_0100, 6)]
        [TestCase(0b0111_0101, 6)] // 6.5 (rounded down to 6)
        [TestCase(0b0111_0110, 7)]
        [TestCase(0b0111_0111, 8)] // 7.5 (rounded up to 8)
        [TestCase(0b0111_1000, 8)]
        [TestCase(0b0111_1001, 10)]
        [TestCase(0b0111_1010, 12)]
        [TestCase(0b0111_1011, 14)]
        [TestCase(0b0111_1100, 16)]
        [TestCase(0b0111_1101, 24)]
        [TestCase(0b0111_1110, 32)]
        [TestCase(0b0111_1111, 64)]

        [TestCase(0b00000001, 0)] // 0.015625 (minpos) (MinValue)
        [TestCase(0b11100000, 0)] // 0.5 (rounded down to 0)
        [TestCase(0b00100001, 1)] // 0.515625 (rounded up to 1)
        [TestCase(0b00111111, 1)] // 0.984375

        [TestCase(0b1100_0000, -1)]
        [TestCase(0b1010_0000, -2)]
        [TestCase(0b1001_1000, -3)]
        [TestCase(0b1001_0000, -4)]
        [TestCase(0b1000_1110, -5)]
        [TestCase(0b1000_1100, -6)]
        [TestCase(0b1000_1011, -6)] // -6.5 (rounded down to -6)
        [TestCase(0b1000_1010, -7)]
        [TestCase(0b1000_1001, -8)] // -7.5 (rounded up to -8)
        [TestCase(0b1000_1000, -8)]
        [TestCase(0b1000_0111, -10)]
        [TestCase(0b1000_0110, -12)]
        [TestCase(0b1000_0101, -14)]
        [TestCase(0b1000_0100, -16)]
        [TestCase(0b1000_0011, -24)]
        [TestCase(0b1000_0010, -32)]
        [TestCase(0b1000_0001, -64)]

        [TestCase(0b11111111, 0)] // -0.015625 (minpos) (MinValue)
        [TestCase(0b11100000, 0)] // -0.5 (rounded down to 0)
        [TestCase(0b11011111, -1)] // -0.515625 (rounded up to -1)
        [TestCase(0b11000001, -1)] // -0.984375
        public void TestPosit8ToInt32Cast(byte ui, int value)
        {
            var p = new Posit8(ui);

            var x = (int)p;

            Assert.That(x, Is.EqualTo(value));
        }

        [TestCase(0b1000_0000, uint.MinValue)] // NaR
        [TestCase(0b1000_0001, 0u)] // -64 (-maxpos)
        [TestCase(0b1100_0000, 0u)] // -1
        [TestCase(0b1111_1111, 0u)] // -0.015625 (-minpos)

        [TestCase(0b0000_0000, 0u)] // Zero
        [TestCase(0b0000_0001, 0u)] // +minpos
        [TestCase(0b0010_0000, 0u)] // 0.5
        [TestCase(0b0010_0001, 1u)] // 0.515625 (rounded up to 1)
        [TestCase(0b0011_1111, 1u)] // 0.984375
        [TestCase(0b0100_0000, 1u)]
        [TestCase(0b0110_0000, 2u)]
        [TestCase(0b0110_1000, 3u)]
        [TestCase(0b0111_0000, 4u)]
        [TestCase(0b0111_0010, 5u)]
        [TestCase(0b0111_0100, 6u)]
        [TestCase(0b0111_0101, 6u)] // 6.5 (rounded down to 6)
        [TestCase(0b0111_0110, 7u)]
        [TestCase(0b0111_0111, 8u)] // 7.5 (rounded up to 8)
        [TestCase(0b0111_1000, 8u)]
        [TestCase(0b0111_1001, 10u)]
        [TestCase(0b0111_1010, 12u)]
        [TestCase(0b0111_1011, 14u)]
        [TestCase(0b0111_1100, 16u)]
        [TestCase(0b0111_1101, 24u)]
        [TestCase(0b0111_1110, 32u)]
        [TestCase(0b0111_1111, 64u)] // +maxpos
        public void TestPosit8ToUInt32Cast(byte ui, uint value)
        {
            var p = new Posit8(ui);

            var x = (uint)p;

            Assert.That(x, Is.EqualTo(value));
        }
    }
}
