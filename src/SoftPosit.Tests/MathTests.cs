// SPDX-License-Identifier: MIT

using NUnit.Framework;

namespace System.Numerics.Posits.Tests
{
    public class MathTests
    {
        [Test]
        public void Posit8_AbsTests()
        {
            Assert.That(MathP.Abs(Posit8.One), Is.EqualTo(Posit8.One));
            Assert.That(MathP.Abs(Posit8.MinusOne), Is.EqualTo(Posit8.One));
            Assert.That(MathP.Abs(Posit8.Zero), Is.EqualTo(Posit8.Zero));
            Assert.That(MathP.Abs(Posit8.NaR), Is.EqualTo(Posit8.NaR));
        }

        [Test]
        public void Posit16_AbsTests()
        {
            Assert.That(MathP.Abs(Posit16.One), Is.EqualTo(Posit16.One));
            Assert.That(MathP.Abs(Posit16.MinusOne), Is.EqualTo(Posit16.One));
            Assert.That(MathP.Abs(Posit16.Zero), Is.EqualTo(Posit16.Zero));
            Assert.That(MathP.Abs(Posit16.NaR), Is.EqualTo(Posit16.NaR));
        }

        [Test]
        public void Posit32_AbsTests()
        {
            Assert.That(MathP.Abs(Posit32.One), Is.EqualTo(Posit32.One));
            Assert.That(MathP.Abs(Posit32.MinusOne), Is.EqualTo(Posit32.One));
            Assert.That(MathP.Abs(Posit32.Zero), Is.EqualTo(Posit32.Zero));
            Assert.That(MathP.Abs(Posit32.NaR), Is.EqualTo(Posit32.NaR));
        }

        [Test]
        public void Posit64_AbsTests()
        {
            Assert.That(MathP.Abs(Posit64.One), Is.EqualTo(Posit64.One));
            Assert.That(MathP.Abs(Posit64.MinusOne), Is.EqualTo(Posit64.One));
            Assert.That(MathP.Abs(Posit64.Zero), Is.EqualTo(Posit64.Zero));
            Assert.That(MathP.Abs(Posit64.NaR), Is.EqualTo(Posit64.NaR));
        }
    }
}