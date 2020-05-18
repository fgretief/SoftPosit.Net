﻿// SPDX-License-Identifier: MIT

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
    }
}
