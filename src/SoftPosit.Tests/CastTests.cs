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

        [TestCase(0u, 0)]
        [TestCase(1u, 0x4000)]
        [TestCase(2u, 0b0101_0000_0000_0000)]
        [TestCase(3u, 0b0101_1000_0000_0000)]
        [TestCase(4u, 0b0110_0000_0000_0000)]
        [TestCase(5u, 0b0110_0010_0000_0000)]
        [TestCase(6u, 0b0110_0100_0000_0000)]
        [TestCase(7u, 0b0110_0110_0000_0000)]
        [TestCase(8u, 0b0110_1000_0000_0000)]
        [TestCase(9u, 0b0110_1001_0000_0000)]

        [TestCase(510u, 0x7CFE)]
        [TestCase(511u, 0x7CFF)]
        [TestCase(512u, 0x7D00)]
        [TestCase(513u, 0x7D00)] // round down to 512 (tie)
        [TestCase(514u, 0x7D01)]
        [TestCase(515u, 0x7D02)] // round up to 516 (tie)
        [TestCase(516u, 0x7D02)]

        [TestCase(1022u, 0x7DFF)]
        [TestCase(1023u, 0x7E00)] // round up to 1024 (tie)
        [TestCase(1024u, 0x7E00)]
        [TestCase(1025u, 0x7E00)] // round down to 1024

        [TestCase(25165824u, 0x7FFC)] // tie
        [TestCase(25165825u, 0x7FFD)]
        [TestCase(50331647u, 0x7FFD)]
        [TestCase(50331648u, 0x7FFE)] // tie
        [TestCase(50331649u, 0x7FFE)]
        [TestCase(134217728u, 0x7FFE)]
        [TestCase(167772160u, 0x7FFE)] // tie
        [TestCase(167772161u, 0x7FFF)]
        [TestCase(268435456u, 0x7FFF)] // +maxpos
        [TestCase(uint.MaxValue, 0x7FFF)] // +maxpos
        public void TestUInt32ToPosit16Cast(uint value, int ui)
        {
            var p = new Posit16((ushort)ui);

            var x = (Posit16)value;

            Assert.That(x.ui, Is.EqualTo(p.ui));
        }

        [TestCase(0u, 0x0000_0000u)]
        [TestCase(1u, 0x4000_0000u)]
        [TestCase(2u, 0x4800_0000u)]
        [TestCase(3u, 0x4C00_0000u)]
        [TestCase(4u, 0x5000_0000u)]
        [TestCase(5u, 0x5200_0000u)]
        [TestCase(6u, 0x5400_0000u)]
        [TestCase(7u, 0x5600_0000u)]
        [TestCase(8u, 0x5800_0000u)]
        [TestCase(9u, 0x5900_0000u)]

        [TestCase(510u, 0x71FC_0000u)]
        [TestCase(511u, 0x71FE_0000u)]
        [TestCase(512u, 0x7200_0000u)]
        [TestCase(513u, 0x7201_0000u)]
        [TestCase(514u, 0x7202_0000u)]
        [TestCase(515u, 0x7203_0000u)]
        [TestCase(516u, 0x7204_0000u)]

        [TestCase(2147479552u, 0x7FAF_FFFCu)] // exact
        [TestCase(2147480064u, 0x7FAF_FFFCu)] // tie (round down)
        [TestCase(2147480576u, 0x7FAF_FFFDu)] // exact
        [TestCase(2147481088u, 0x7FAF_FFFEu)] // tie (round up)
        [TestCase(2147481600u, 0x7FAF_FFFEu)] // exact
        [TestCase(2147482112u, 0x7FAF_FFFEu)] // tie (round down)
        [TestCase(2147482624u, 0x7FAF_FFFFu)] // exact
        [TestCase(2147482625u, 0x7FAF_FFFFu)] // round down
        [TestCase(2147483135u, 0x7FAF_FFFFu)] // round down
        [TestCase(2147483136u, 0x7FB0_0000u)] // tie (round up)
        [TestCase((uint)int.MaxValue, 0x7FB0_0000u)] // 2^31

        [TestCase(4294961152u, 0x7FBF_FFFDu)] // exact (0xFFFFE800)
        [TestCase(4294963200u, 0x7FBF_FFFEu)] // exact (0xFFFFF000)
        [TestCase(4294965248u, 0x7FBF_FFFFu)] // exact (0xFFFFF800)
        [TestCase(4294966271u, 0x7FBF_FFFFu)] // round down
        [TestCase(4294966272u, 0x7FC0_0000u)] // tie (round up)
        [TestCase(uint.MaxValue, 0x7FC0_0000u)] // (2^32)-1
        public void TestUInt32ToPosit32Cast(uint value, uint ui)
        {
            var p = new Posit32(ui);

            var x = (Posit32)value;

            Assert.That(x.ui, Is.EqualTo(p.ui));
        }

        [TestCase(0u, 0ul)]
        [TestCase(1u, 0x4000_0000_0000_0000ul)]
        [TestCase(2u, 0x4400_0000_0000_0000ul)]
        [TestCase(3u, 0x4600_0000_0000_0000ul)]
        [TestCase(4u, 0x4800_0000_0000_0000ul)]
        [TestCase(5u, 0x4900_0000_0000_0000ul)]
        [TestCase(512u, 0x6200_0000_0000_0000ul)]
        [TestCase(768u, 0x6300_0000_0000_0000ul)]
        [TestCase(uint.MaxValue / 2, 0x7B7F_FFFF_FE00_0000ul)]
        [TestCase(uint.MaxValue, 0x7BFF_FFFF_FF00_0000ul)]
        public void TestUInt32ToPosit64Cast(uint value, ulong ui)
        {
            var p = new Posit64(ui);

            var x = (Posit64)value;

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

        [TestCase(0, 0b0000_0000)]
        [TestCase(1, 0b0100_0000)]
        [TestCase(2, 0b0110_0000)]
        [TestCase(3, 0b0110_1000)]
        [TestCase(4, 0b0111_0000)]
        [TestCase(5, 0b0111_0010)]
        [TestCase(6, 0b0111_0100)]
        [TestCase(7, 0b0111_0110)]
        [TestCase(8, 0b0111_1000)]
        [TestCase(9, 0b0111_1000)] // rounded down to 8
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
        [TestCase(long.MaxValue, 0b0111_1111)] // rounded down to 64

        [TestCase(-1, 0b1100_0000)]
        [TestCase(-2, 0b1010_0000)]
        [TestCase(-3, 0b1001_1000)]
        [TestCase(-4, 0b1001_0000)]
        [TestCase(-5, 0b1000_1110)]
        [TestCase(-6, 0b1000_1100)]
        [TestCase(-7, 0b1000_1010)]
        [TestCase(-8, 0b1000_1000)]
        [TestCase(-9, 0b1000_1000)] // rounded down to -8
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
        [TestCase(long.MinValue, 0b1000_0001)] // rounded down to -64
        public void TestInt64ToPosit8Cast(long value, byte ui)
        {
            var p = new Posit8(ui);

            var x = (Posit8)value;

            Assert.That(x.ui, Is.EqualTo(p.ui));
        }

        [TestCase(ulong.MinValue, 0b0000_0000)]
        [TestCase(1ul, 0b0100_0000)]
        [TestCase(2ul, 0b0110_0000)]
        [TestCase(3ul, 0b0110_1000)]
        [TestCase(4ul, 0b0111_0000)]
        [TestCase(5ul, 0b0111_0010)]
        [TestCase(6ul, 0b0111_0100)]
        [TestCase(7ul, 0b0111_0110)]
        [TestCase(8ul, 0b0111_1000)]
        [TestCase(9ul, 0b0111_1000)] // rounded down to 8
        [TestCase(10ul, 0b0111_1001)]
        [TestCase(11ul, 0b0111_1010)] // rounded up to 12
        [TestCase(12ul, 0b0111_1010)]
        [TestCase(13ul, 0b0111_1010)] // rounded down to 12
        [TestCase(14ul, 0b0111_1011)]
        [TestCase(15ul, 0b0111_1100)] // rounded up to 16
        [TestCase(16ul, 0b0111_1100)]
        [TestCase(17ul, 0b0111_1100)] // rounded down to 16
        [TestCase(23ul, 0b0111_1101)] // rounded up to 24
        [TestCase(24ul, 0b0111_1101)]
        [TestCase(32ul, 0b0111_1110)]
        [TestCase(33ul, 0b0111_1110)] // rounded down to 32
        [TestCase(48ul, 0b0111_1110)] // rounded down to 32
        [TestCase(49ul, 0b0111_1111)] // rounded up to 64
        [TestCase(63ul, 0b0111_1111)] // rounded up to 64
        [TestCase(64ul, 0b0111_1111)]
        [TestCase(65ul, 0b0111_1111)] // rounded down to 64
        [TestCase(99ul, 0b0111_1111)] // rounded down to 64
        [TestCase(0xffff_fffful, 0b0111_1111)]
        [TestCase(ulong.MaxValue, 0b0111_1111)] // rounded down to 64
        public void TestUInt64ToPosit8Cast(ulong value, byte ui)
        {
            var p = new Posit8(ui);

            var x = (Posit8)value;

            Assert.That(x.ui, Is.EqualTo(p.ui));
        }

        [TestCase(0ul, 0x0000)]
        [TestCase(1ul, 0x4000)]
        [TestCase(2ul, 0x5000)]
        [TestCase(3ul, 0b0101_1000_0000_0000)]
        [TestCase(4ul, 0b0110_0000_0000_0000)]
        [TestCase(5ul, 0b0110_0010_0000_0000)]
        [TestCase(6ul, 0b0110_0100_0000_0000)]
        [TestCase(7ul, 0b0110_0110_0000_0000)]
        [TestCase(8ul, 0b0110_1000_0000_0000)]
        [TestCase(9ul, 0b0110_1001_0000_0000)]

        [TestCase(510ul, 0x7CFE)]
        [TestCase(511ul, 0x7CFF)]
        [TestCase(512ul, 0x7D00)]
        [TestCase(513ul, 0x7D00)] // round down to 512 (tie)
        [TestCase(514ul, 0x7D01)]
        [TestCase(515ul, 0x7D02)] // round up to 516 (tie)
        [TestCase(516ul, 0x7D02)]

        [TestCase(1022ul, 0x7DFF)]
        [TestCase(1023ul, 0x7E00)] // round up to 1024 (tie)
        [TestCase(1024ul, 0x7E00)]
        [TestCase(1025ul, 0x7E00)] // round down to 1024

        [TestCase(25165824ul, 0x7FFC)]  // round down (tie)
        [TestCase(25165825ul, 0x7FFD)]  // round down
        [TestCase(50331648ul, 0x7FFE)]  // round up (tie)
        [TestCase(167772160ul, 0x7FFE)] // round down (tie)
        [TestCase(167772161ul, 0x7FFF)] // round up
        public void TestUInt64ToPosit16Cast(ulong value, int ui)
        {
            var p = new Posit16((ushort)ui);

            var x = (Posit16)value;

            Assert.That(x.ui, Is.EqualTo(p.ui));
        }

        [TestCase(0ul, 0x0000_0000u)]
        [TestCase(1ul, 0x4000_0000u)]
        [TestCase(2ul, 0x4800_0000u)]
        [TestCase(3ul, 0x4C00_0000u)]
        [TestCase(4ul, 0x5000_0000u)]
        [TestCase(5ul, 0x5200_0000u)]
        [TestCase(512ul, 0x7200_0000u)]
        [TestCase(768ul, 0x7300_0000u)]

        [TestCase((uint)int.MaxValue, 0x7FB0_0000u)]
        [TestCase(uint.MaxValue, 0x7FC0_0000u)]

        [TestCase(4611686018427387904ul, 0x7FFF_A000u)]
        [TestCase(9223372036854775808ul, 0x7FFF_B000u)] // 2^63
        [TestCase((ulong)long.MaxValue, 0x7FFF_B000u)]

        [TestCase(18437736874454810624ul, 0x7FFF_BFFCu)] // exact
        [TestCase(18437736874454810625ul, 0x7FFF_BFFCu)] // round down
        [TestCase(18438862774361653248ul, 0x7FFF_BFFCu)] // round down (tie)
        [TestCase(18439988674268495871ul, 0x7FFF_BFFDu)] // round up
        [TestCase(18439988674268495872ul, 0x7FFF_BFFDu)] // exact
        [TestCase(18441114574175338496ul, 0x7FFF_BFFEu)] // round up (tie)
        [TestCase(18442240474082181120ul, 0x7FFF_BFFEu)] // exact
        [TestCase(18443366373989023744ul, 0x7FFF_BFFEu)] // round down (tie)
        [TestCase(18444492273895866368ul, 0x7FFF_BFFFu)] // exact
        [TestCase(18444492273895866369ul, 0x7FFF_BFFFu)] // round down
        [TestCase(18445618173802708991ul, 0x7FFF_BFFFu)] // round down
        [TestCase(18445618173802708992ul, 0x7FFF_C000u)] // round up (tie)
        [TestCase(ulong.MaxValue, 0x7FFF_C000u)] // 18446744073709551616
        public void TestUInt64ToPosit32Cast(ulong value, uint ui)
        {
            var p = new Posit32(ui);

            var x = (Posit32)value;

            Assert.That(x.ui, Is.EqualTo(p.ui));
        }

        [TestCase(0ul, 0ul)]
        [TestCase(1ul, 0x4000_0000_0000_0000ul)]
        [TestCase(2ul, 0x4400_0000_0000_0000ul)]
        [TestCase(3ul, 0x4600_0000_0000_0000ul)]
        [TestCase(4ul, 0x4800_0000_0000_0000ul)]
        [TestCase(5ul, 0x4900_0000_0000_0000ul)]
        [TestCase(512ul, 0x6200_0000_0000_0000ul)]
        [TestCase(768ul, 0x6300_0000_0000_0000ul)]

        [TestCase(4503599627370491ul, 0x7F3F_FFFF_FFFF_FFF6ul)]
        [TestCase(4503599627370492ul, 0x7F3F_FFFF_FFFF_FFF8ul)]
        [TestCase(4503599627370493ul, 0x7F3F_FFFF_FFFF_FFFAul)]
        [TestCase(4503599627370494ul, 0x7F3F_FFFF_FFFF_FFFCul)]
        [TestCase(4503599627370495ul, 0x7F3F_FFFF_FFFF_FFFEul)]
        [TestCase(4503599627370496ul, 0x7F40_0000_0000_0000ul)]
        [TestCase(4503599627370497ul, 0x7F40_0000_0000_0001ul)]
        [TestCase(4503599627370498ul, 0x7F40_0000_0000_0002ul)]
        [TestCase(4503599627370499ul, 0x7F40_0000_0000_0003ul)]

        [TestCase(4611686018427386880ul, 0x7FAF_FFFF_FFFF_FFFFul)] // exact
        [TestCase(4611686018427386881ul, 0x7FAF_FFFF_FFFF_FFFFul)]
        [TestCase(4611686018427387391ul, 0x7FAF_FFFF_FFFF_FFFFul)] // round down
        [TestCase(4611686018427387392ul, 0x7FB0_0000_0000_0000ul)] // tie (round up)
        [TestCase(4611686018427387393ul, 0x7FB0_0000_0000_0000ul)] // round up
        [TestCase(4611686018427387903ul, 0x7FB0_0000_0000_0000ul)]
        [TestCase(4611686018427387904ul, 0x7FB0_0000_0000_0000ul)] // exact

        [TestCase(9223372036854771712ul, 0x7FB7_FFFF_FFFF_FFFEul)] // exact
        [TestCase(9223372036854771713ul, 0x7FB7_FFFF_FFFF_FFFEul)]
        [TestCase(9223372036854772735ul, 0x7FB7_FFFF_FFFF_FFFEul)] // round down
        [TestCase(9223372036854772736ul, 0x7FB7_FFFF_FFFF_FFFEul)] // tie (round down)
        [TestCase(9223372036854772737ul, 0x7FB7_FFFF_FFFF_FFFFul)] // round up
        [TestCase(9223372036854773759ul, 0x7FB7_FFFF_FFFF_FFFFul)]
        [TestCase(9223372036854773760ul, 0x7FB7_FFFF_FFFF_FFFFul)] // exact
        [TestCase(9223372036854773761ul, 0x7FB7_FFFF_FFFF_FFFFul)]
        [TestCase((ulong)long.MaxValue, 0x7FB8_0000_0000_0000ul)] // 2^63-1
        [TestCase(9223372036854775808ul, 0x7FB8_0000_0000_0000ul)] // 2^63

        [TestCase(18446744073709535232ul, 0x7FBF_FFFF_FFFF_FFFCul)] // exact
        [TestCase(18446744073709537280ul, 0x7FBF_FFFF_FFFF_FFFCul)] // tie (round down)
        [TestCase(18446744073709539328ul, 0x7FBF_FFFF_FFFF_FFFDul)] // exact
        [TestCase(18446744073709541375ul, 0x7FBF_FFFF_FFFF_FFFDul)]
        [TestCase(18446744073709541376ul, 0x7FBF_FFFF_FFFF_FFFEul)] // tie (round up)
        [TestCase(18446744073709541377ul, 0x7FBF_FFFF_FFFF_FFFEul)]
        [TestCase(18446744073709543424ul, 0x7FBF_FFFF_FFFF_FFFEul)] // exact
        [TestCase(18446744073709545472ul, 0x7FBF_FFFF_FFFF_FFFEul)] // tie (round down)
        [TestCase(18446744073709547520ul, 0x7FBF_FFFF_FFFF_FFFFul)] // exact
        [TestCase(18446744073709549568ul, 0x7FC0_0000_0000_0000ul)] // tie (round up)
        [TestCase(ulong.MaxValue, 0x7FC0_0000_0000_0000ul)]
        public void TestUInt64ToPosit64Cast(ulong value, ulong ui)
        {
            var p = new Posit64(ui);

            var x = (Posit64)value;

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
        public void TestPosit8ToInt64Cast(byte ui, long value)
        {
            var p = new Posit8(ui);

            var x = (long)p;

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
        public void TestPosit8ToUInt64Cast(byte ui, ulong value)
        {
            var p = new Posit8(ui);

            var x = (ulong)p;

            Assert.That(x, Is.EqualTo(value));
        }

        [TestCase(0.0, 0b0000_0000)]
        [TestCase(1.0, 0b0100_0000)]
        [TestCase(-1.0, 0b1100_0000)]
        [TestCase(double.NaN, 0b1000_0000)]
        [TestCase(double.PositiveInfinity, 0b1000_0000)]
        [TestCase(double.NegativeInfinity, 0b1000_0000)]
        [TestCase(64, 0x7F)]
        [TestCase(65, 0x7F)]
        [TestCase(double.MaxValue, 0x7F)]
        [TestCase(-64, 0b1000_0001)]
        [TestCase(-65, 0b1000_0001)]
        [TestCase(-double.MaxValue, 0b1000_0001)]
        [TestCase(0.015625, 0b0000_0001)] // +minpos
        [TestCase(double.Epsilon, 0b0000_0001)] // +minpos
        [TestCase(-0.015625, 0b1111_1111)] // -minpos
        [TestCase(-double.Epsilon, 0b1111_1111)] // -minpos

        [TestCase(0.515625, 0b0010_0001)]
        [TestCase(0.50001, 0b0010_0000)]
        [TestCase(0.5, 0b0010_0000)]
        [TestCase(0.49999, 0b0010_0000)]
        [TestCase(0.484375, 0b0001_1111)]

        [TestCase(-0.515625, 0b1101_1111)]
        [TestCase(-0.50001, 0b1110_0000)]
        [TestCase(-0.5, 0b1110_0000)]
        [TestCase(-0.49999, 0b1110_0000)]
        [TestCase(-0.484375, 0b1110_0001)]

        [TestCase(0.25, 0b0001_0000)]
        [TestCase(-0.25, 0b1111_0000)]

        [TestCase(1.96875, 0b0101_1111)]
        [TestCase(1.9999, 0b0110_0000)]
        [TestCase(2, 0b0110_0000)]
        [TestCase(2.0001, 0b0110_0000)]
        [TestCase(2.125, 0b0110_0001)]

        [TestCase(63, 0b0111_1111)]
        [TestCase(-63, 0b1000_0001)]
        public void TestFloat64ToPosit8Cast(double value, byte ui)
        {
            var p = (Posit8)value;
            Assert.That(p.ui, Is.EqualTo(ui));
        }

        [TestCase(0f, 0x00)]
        [TestCase(1f, 0x40)]
        [TestCase(-1f, 0xC0)]
        [TestCase(64f, 0x7F)]
        [TestCase(-64f, 0x81)]
        [TestCase(float.NaN, 0x80)] // NaR
        [TestCase(float.PositiveInfinity, 0x80)] // NaR
        [TestCase(float.NegativeInfinity, 0x80)] // NaR
        [TestCase(float.MaxValue, 0x7F)]
        [TestCase(float.MinValue, 0x81)]
        [TestCase(0.015625f, 0b0000_0001)] // +minpos
        [TestCase(float.Epsilon, 0b0000_0001)] // +minpos
        [TestCase(-0.015625f, 0b1111_1111)] // -minpos
        [TestCase(-float.Epsilon, 0b1111_1111)] // -minpos

        [TestCase(0.515625f, 0b0010_0001)]
        [TestCase(0.50001f, 0b0010_0000)]
        [TestCase(0.5f, 0b0010_0000)]
        [TestCase(0.49999f, 0b0010_0000)]
        [TestCase(0.484375f, 0b0001_1111)]

        [TestCase(-0.515625f, 0b1101_1111)]
        [TestCase(-0.50001f, 0b1110_0000)]
        [TestCase(-0.5f, 0b1110_0000)]
        [TestCase(-0.49999f, 0b1110_0000)]
        [TestCase(-0.484375f, 0b1110_0001)]

        [TestCase(1.96875f, 0b0101_1111)]
        [TestCase(1.9999f, 0b0110_0000)]
        [TestCase(2f, 0b0110_0000)]
        [TestCase(2.0001f, 0b0110_0000)]
        [TestCase(2.125f, 0b0110_0001)]

        [TestCase(-1.96875f, 0b1010_0001)]
        [TestCase(-1.9999f, 0b1010_0000)]
        [TestCase(-2f, 0b1010_0000)]
        [TestCase(-2.0001f, 0b1010_0000)]
        [TestCase(-2.125f, 0b1001_1111)]

        public void TestFloat32ToPosit8Cast(float value, byte ui)
        {
            Assert.That(((Posit8)value).ui, Is.EqualTo(ui));
        }

        [TestCase(0x80, double.NaN)] // North
        [TestCase(0x00, 0.0)] // South
        [TestCase(0x40, 1.0)] // East
        [TestCase(0xC0, -1.0)] // West
        [TestCase(0x7F, +64.0)] // +maxpos
        [TestCase(0x81, -64.0)] // -maxpos
        [TestCase(0x01, +0.015625)] // +minpos
        [TestCase(0xFF, -0.015625)] // -minpos

        [TestCase(0b00100000, +0.5)]
        [TestCase(0b01100000, +2)]
        [TestCase(0b10100000, -2)]
        [TestCase(0b11100000, -0.5)]
        public void TestPosit8ToFloat64Cast(byte ui, double value)
        {
            var p = new Posit8(ui);
            Assert.That((double)p, Is.EqualTo(value));
        }

        [TestCase(0x80, float.NaN)] // North
        [TestCase(0x00, 0.0f)] // South
        [TestCase(0x40, 1.0f)] // East
        [TestCase(0xC0, -1.0f)] // West
        [TestCase(0x7F, +64.0f)] // +maxpos
        [TestCase(0x81, -64.0f)] // -maxpos
        [TestCase(0x01, +0.015625f)] // +minpos
        [TestCase(0xFF, -0.015625f)] // -minpos

        [TestCase(0b00100000, +0.5f)]
        [TestCase(0b01100000, +2f)]
        [TestCase(0b10100000, -2f)]
        [TestCase(0b11100000, -0.5f)]
        public void TestPosit8ToFloat32Cast(byte ui, float value)
        {
            var p = new Posit8(ui);
            Assert.That((float)p, Is.EqualTo(value));
        }

        [TestCase(0x00, 0)] // Zero
        [TestCase(0x80, 0x8000)] // NaR
        [TestCase(0x7F, 0b0111100000000000)] // +maxpos
        [TestCase(0x01, 0b0000100000000000)] // +minpos
        [TestCase(0x81, 0b1000100000000000)] // -maxpos
        [TestCase(0xFF, 0b1111100000000000)] // -minpos
        [TestCase(0x40, 0x4000)] // 1
        [TestCase(0xC0, 0xC000)] // -1

        [TestCase(0b00100000, 0x3000)] // 0.5
        [TestCase(0b11100000, 0xD000)] // -0.5
        [TestCase(0b01100000, 0x5000)] // 2
        [TestCase(0b10100000, 0xB000)] // -2
        public void TestPosit8ToPosit16Cast(byte uiA, int uiB)
        {
            var pA = new Posit8(uiA);
            var pB = new Posit16((ushort)uiB);
            var pC = (Posit16)pA;

            Console.WriteLine("A=0x{0:X2}, B=0x{1:X4}, C=0x{2:X4}", pA.ui, pB.ui, pC.ui);

            Assert.That(pC.ui, Is.EqualTo(pB.ui));
        }

        [TestCase(0x00, 0u)] // Zero
        [TestCase(0x80, 0x8000_0000u)] // NaR
        [TestCase(0x7F, 0x6800_0000u)] // +maxpos
        [TestCase(0x01, 0x1800_0000u)] // +minpos
        [TestCase(0x81, 0x9800_0000u)] // -maxpos
        [TestCase(0xFF, 0xE800_0000u)] // -minpos
        [TestCase(0x40, 0x4000_0000u)] // 1
        [TestCase(0xC0, 0xC000_0000u)] // -1

        [TestCase(0b00100000, 0x3800_0000u)] // 0.5
        [TestCase(0b11100000, 0xC800_0000u)] // -0.5
        [TestCase(0b01100000, 0x4800_0000u)] // 2
        [TestCase(0b10100000, 0xB800_0000u)] // -2
        public void TestPosit8ToPosit32Cast(byte uiA, uint uiB)
        {
            var pA = new Posit8(uiA);
            var pB = new Posit32(uiB);
            var pC = (Posit32)pA;

            Console.WriteLine("A=0x{0:X2}, B=0x{1:X8}, C=0x{2:X8}", pA.ui, pB.ui, pC.ui);

            Assert.That(pC.ui, Is.EqualTo(pB.ui));
        }

        [TestCase(0, 0, "0")] // Zero
        [TestCase(0x8000, 0x80, "NaR")]
        [TestCase(0x4000, 0x40, "1")]
        [TestCase(0xC000, 0xC0, "-1")]

        [TestCase(0x7FFF, 0x7F, "+maxpos")]
        [TestCase(0x0001, 0x01, "+minpos")]
        [TestCase(0x8001, 0x81, "-maxpos")]
        [TestCase(0xFFFF, 0xFF, "-minpos")]

        [TestCase(0x3000, 0b00100000, "0.5")]
        [TestCase(0xD000, 0b11100000, "-0.5")]
        [TestCase(0x5000, 0b01100000, "2")]
        [TestCase(0xB000, 0b10100000, "-2")]
        public void TestPosit16ToPosit8Cast(int uiA, int uiB, string op)
        {
            var pA = new Posit16((ushort)uiA);
            var pB = new Posit8((byte)uiB);
            var pC = (Posit8)pA;

            Console.WriteLine("A=0x{0:X4}, B=0x{1:X2}, C=0x{2:X2}", pA.ui, pB.ui, pC.ui);

            Assert.That(pC.ui, Is.EqualTo(pB.ui));
        }

        [TestCase(0, 0u, "Zero")]
        [TestCase(0x8000, 0x8000_0000u, "NaR")]
        [TestCase(0x4000, 0x4000_0000u, "1")]
        [TestCase(0xC000, 0xC000_0000u, "-1")]
        [TestCase(0x7FFF, 0x7F80_0000u, "+maxpos")]
        [TestCase(0x0001, 0x0080_0000u, "+minpos")]
        [TestCase(0x8001, 0x8080_0000u, "-maxpos")]
        [TestCase(0xFFFF, 0xFF80_0000u, "-minpos")]

        [TestCase(0x3000, 0x3800_0000u, "0.5")]
        [TestCase(0xD000, 0xC800_0000u, "-0.5")]
        [TestCase(0x5000, 0x4800_0000u, "2")]
        [TestCase(0xB000, 0xB800_0000u, "-2")]
        public void TestPosit16ToPosit32Cast(int uiA, uint uiB, string op)
        {
            var pA = new Posit16((ushort)uiA);
            var pB = new Posit32(uiB);
            var pC = (Posit32)pA;

            Assert.That(pC.ui, Is.EqualTo(pB.ui));
        }

        [TestCase(0u, 0, "0")] // Zero
        [TestCase(0x8000_0000u, 0x80, "NaR")]
        [TestCase(0x4000_0000u, 0x40, "1")]
        [TestCase(0xC000_0000u, 0xC0, "-1")]

        [TestCase(0x7FFF_FFFFu, 0x7F, "+maxpos")]
        [TestCase(0x0000_0001u, 0x01, "+minpos")]
        [TestCase(0x8000_0001u, 0x81, "-maxpos")]
        [TestCase(0xFFFF_FFFFu, 0xFF, "-minpos")]

        [TestCase(0x3800_0000u, 0x20, "0.5")]
        [TestCase(0xC800_0000u, 0xE0, "-0.5")]
        [TestCase(0x4800_0000u, 0x60, "2")]
        [TestCase(0xB800_0000u, 0xA0, "-2")]
        public void TestPosit32ToPosit8Cast(uint uiA, int uiB, string op)
        {
            var pA = new Posit32(uiA);
            var pB = new Posit8((byte)uiB);
            var pC = (Posit8)pA;

            Console.WriteLine("A=0x{0:X4}, B=0x{1:X2}, C=0x{2:X2}", pA.ui, pB.ui, pC.ui);

            Assert.That(pC.ui, Is.EqualTo(pB.ui));
        }

        [TestCase(0u, 0ul, "0")] // Zero
        [TestCase(0x8000_0000u, 0x8000_0000_0000_0000ul, "NaR")]
        [TestCase(0x4000_0000u, 0x4000_0000_0000_0000ul, "1")]
        [TestCase(0xC000_0000u, 0xC000_0000_0000_0000ul, "-1")]

        [TestCase(0x7FFF_FFFFu, 0x7FFF_8000_0000_0000ul, "+maxpos")]
        [TestCase(0x0000_0001u, 0x0000_8000_0000_0000ul, "+minpos")]
        [TestCase(0x8000_0001u, 0x8000_8000_0000_0000ul, "-maxpos")]
        [TestCase(0xFFFF_FFFFu, 0xFFFF_8000_0000_0000ul, "-minpos")]

        [TestCase(0x7FFF_FFFEu, 0x7FFF_4000_0000_0000ul, "+(maxpos-1)")]
        [TestCase(0x0000_0002u, 0x0000_C000_0000_0000ul, "+(minpos+1)")]
        // ps=32, es=2, k=30, e=0, f=0  <-->  ps=64, es=3, k=15, e=0, f=0
        //  +0111_1111_1111_1111_1111_1111_1111_1111 => 0x7FFF_FFFF
        //  +0111_1111_1111_1111_1000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000 => 0x7FFF_8000_0000_0000
        public void TestPosit32ToPosit64Cast(uint uiA, ulong uiB, string op)
        {
            var pA = new Posit32(uiA);
            var pB = new Posit64(uiB);
            var pC = (Posit64)pA;

            //Console.WriteLine("A=0x{0:X8}, B=0x{1:X16}, C=0x{2:X16}", pA.ui, pB.ui, pC.ui);

            Assert.That(pC.ui, Is.EqualTo(pB.ui));
        }
    }
}
