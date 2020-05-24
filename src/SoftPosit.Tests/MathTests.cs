// SPDX-License-Identifier: MIT

using NUnit.Framework;

namespace System.Numerics.Posits.Tests
{
    using static MathP;

    public class MathTests
    {
        [Test]
        public void AbsTests_Posit8()
        {
            Assert.That(Abs(Posit8.One), Is.EqualTo(Posit8.One));
            Assert.That(Abs(Posit8.MinusOne), Is.EqualTo(Posit8.One));
            Assert.That(Abs(Posit8.Zero), Is.EqualTo(Posit8.Zero));
            Assert.That(Abs(Posit8.NaR), Is.EqualTo(Posit8.NaR));
        }

        [Test]
        public void AbsTests_Posit16()
        {
            Assert.That(Abs(Posit16.One), Is.EqualTo(Posit16.One));
            Assert.That(Abs(Posit16.MinusOne), Is.EqualTo(Posit16.One));
            Assert.That(Abs(Posit16.Zero), Is.EqualTo(Posit16.Zero));
            Assert.That(Abs(Posit16.NaR), Is.EqualTo(Posit16.NaR));
        }

        [Test]
        public void AbsTests_Posit32()
        {
            Assert.That(Abs(Posit32.One), Is.EqualTo(Posit32.One));
            Assert.That(Abs(Posit32.MinusOne), Is.EqualTo(Posit32.One));
            Assert.That(Abs(Posit32.Zero), Is.EqualTo(Posit32.Zero));
            Assert.That(Abs(Posit32.NaR), Is.EqualTo(Posit32.NaR));
        }

        [Test]
        public void AbsTests_Posit64()
        {
            Assert.That(Abs(Posit64.One), Is.EqualTo(Posit64.One));
            Assert.That(Abs(Posit64.MinusOne), Is.EqualTo(Posit64.One));
            Assert.That(Abs(Posit64.Zero), Is.EqualTo(Posit64.Zero));
            Assert.That(Abs(Posit64.NaR), Is.EqualTo(Posit64.NaR));
        }

        [Test]
        public void CopySignTests_Double()
        {
            Assert.That(Math.CopySign(1, 1), Is.EqualTo(1));
            Assert.That(Math.CopySign(1, -1), Is.EqualTo(-1));
            Assert.That(Math.CopySign(-1, 1), Is.EqualTo(1));
            Assert.That(Math.CopySign(-1, -1), Is.EqualTo(-1));

            Assert.That(Math.CopySign(0, 1), Is.EqualTo(0));
            Assert.That(Math.CopySign(0, -1), Is.EqualTo(0));
            Assert.That(Math.CopySign(1, 0), Is.EqualTo(1));
            Assert.That(Math.CopySign(-1, 0), Is.EqualTo(1));

            Assert.That(Math.CopySign(Double.NaN, 1), Is.EqualTo(Double.NaN));
            Assert.That(Math.CopySign(Double.NaN, -1), Is.EqualTo(Double.NaN));
            Assert.That(Math.CopySign(1, Double.NaN), Is.EqualTo(-1));
            Assert.That(Math.CopySign(-1, Double.NaN), Is.EqualTo(-1));
        }

        [Test]
        public void CopySignTests_Posit8()
        {
            Assert.That(CopySign(Posit8.One, Posit8.One), Is.EqualTo(Posit8.One));
            Assert.That(CopySign(Posit8.One, Posit8.MinusOne), Is.EqualTo(Posit8.MinusOne));
            Assert.That(CopySign(Posit8.MinusOne, Posit8.One), Is.EqualTo(Posit8.One));
            Assert.That(CopySign(Posit8.MinusOne, Posit8.MinusOne), Is.EqualTo(Posit8.MinusOne));

            Assert.That(CopySign(Posit8.Zero, Posit8.One), Is.EqualTo(Posit8.Zero));
            Assert.That(CopySign(Posit8.Zero, Posit8.MinusOne), Is.EqualTo(Posit8.Zero));
            Assert.That(CopySign(Posit8.One, Posit8.Zero), Is.EqualTo(Posit8.One));
            Assert.That(CopySign(Posit8.MinusOne, Posit8.Zero), Is.EqualTo(Posit8.One));

            Assert.That(CopySign(Posit8.NaR, Posit8.One), Is.EqualTo(Posit8.NaR));
            Assert.That(CopySign(Posit8.NaR, Posit8.MinusOne), Is.EqualTo(Posit8.NaR));
            Assert.That(CopySign(Posit8.One, Posit8.NaR), Is.EqualTo(Posit8.MinusOne));
            Assert.That(CopySign(Posit8.MinusOne, Posit8.NaR), Is.EqualTo(Posit8.MinusOne));
        }

        [Test]
        public void CopySignTests_Posit16()
        {
            Assert.That(CopySign(Posit16.One, Posit16.One), Is.EqualTo(Posit16.One));
            Assert.That(CopySign(Posit16.One, Posit16.MinusOne), Is.EqualTo(Posit16.MinusOne));
            Assert.That(CopySign(Posit16.MinusOne, Posit16.One), Is.EqualTo(Posit16.One));
            Assert.That(CopySign(Posit16.MinusOne, Posit16.MinusOne), Is.EqualTo(Posit16.MinusOne));

            Assert.That(CopySign(Posit16.Zero, Posit16.One), Is.EqualTo(Posit16.Zero));
            Assert.That(CopySign(Posit16.Zero, Posit16.MinusOne), Is.EqualTo(Posit16.Zero));
            Assert.That(CopySign(Posit16.One, Posit16.Zero), Is.EqualTo(Posit16.One));
            Assert.That(CopySign(Posit16.MinusOne, Posit16.Zero), Is.EqualTo(Posit16.One));

            Assert.That(CopySign(Posit16.NaR, Posit16.One), Is.EqualTo(Posit16.NaR));
            Assert.That(CopySign(Posit16.NaR, Posit16.MinusOne), Is.EqualTo(Posit16.NaR));
            Assert.That(CopySign(Posit16.One, Posit16.NaR), Is.EqualTo(Posit16.MinusOne));
            Assert.That(CopySign(Posit16.MinusOne, Posit16.NaR), Is.EqualTo(Posit16.MinusOne));
        }

        [Test]
        public void CopySignTests_Posit32()
        {
            Assert.That(CopySign(Posit32.One, Posit32.One), Is.EqualTo(Posit32.One));
            Assert.That(CopySign(Posit32.One, Posit32.MinusOne), Is.EqualTo(Posit32.MinusOne));
            Assert.That(CopySign(Posit32.MinusOne, Posit32.One), Is.EqualTo(Posit32.One));
            Assert.That(CopySign(Posit32.MinusOne, Posit32.MinusOne), Is.EqualTo(Posit32.MinusOne));

            Assert.That(CopySign(Posit32.Zero, Posit32.One), Is.EqualTo(Posit32.Zero));
            Assert.That(CopySign(Posit32.Zero, Posit32.MinusOne), Is.EqualTo(Posit32.Zero));
            Assert.That(CopySign(Posit32.One, Posit32.Zero), Is.EqualTo(Posit32.One));
            Assert.That(CopySign(Posit32.MinusOne, Posit32.Zero), Is.EqualTo(Posit32.One));

            Assert.That(CopySign(Posit32.NaR, Posit32.One), Is.EqualTo(Posit32.NaR));
            Assert.That(CopySign(Posit32.NaR, Posit32.MinusOne), Is.EqualTo(Posit32.NaR));
            Assert.That(CopySign(Posit32.One, Posit32.NaR), Is.EqualTo(Posit32.MinusOne));
            Assert.That(CopySign(Posit32.MinusOne, Posit32.NaR), Is.EqualTo(Posit32.MinusOne));
        }

        [Test]
        public void CopySignTests_Posit64()
        {
            Assert.That(CopySign(Posit64.One, Posit64.One), Is.EqualTo(Posit64.One));
            Assert.That(CopySign(Posit64.One, Posit64.MinusOne), Is.EqualTo(Posit64.MinusOne));
            Assert.That(CopySign(Posit64.MinusOne, Posit64.One), Is.EqualTo(Posit64.One));
            Assert.That(CopySign(Posit64.MinusOne, Posit64.MinusOne), Is.EqualTo(Posit64.MinusOne));

            Assert.That(CopySign(Posit64.Zero, Posit64.One), Is.EqualTo(Posit64.Zero));
            Assert.That(CopySign(Posit64.Zero, Posit64.MinusOne), Is.EqualTo(Posit64.Zero));
            Assert.That(CopySign(Posit64.One, Posit64.Zero), Is.EqualTo(Posit64.One));
            Assert.That(CopySign(Posit64.MinusOne, Posit64.Zero), Is.EqualTo(Posit64.One));

            Assert.That(CopySign(Posit64.NaR, Posit64.One), Is.EqualTo(Posit64.NaR));
            Assert.That(CopySign(Posit64.NaR, Posit64.MinusOne), Is.EqualTo(Posit64.NaR));
            Assert.That(CopySign(Posit64.One, Posit64.NaR), Is.EqualTo(Posit64.MinusOne));
            Assert.That(CopySign(Posit64.MinusOne, Posit64.NaR), Is.EqualTo(Posit64.MinusOne));
        }

        [Test]
        public void TestRoundInt_Posit8()
        {
            Assert.That(Round(Posit8.One), Is.EqualTo(Posit8.One));
            Assert.That(Round(new Posit8((byte)(Posit8.One.ui + 1))), Is.EqualTo(Posit8.One));
            Assert.That(Round(Posit8.Half), Is.EqualTo(Posit8.Zero));
            Assert.That(Round(new Posit8((byte)(Posit8.Half.ui + 1))), Is.EqualTo(Posit8.One));
            Assert.That(Round(Posit8.Half+Posit8.One), Is.EqualTo(Posit8.Two));
            Assert.That(Round(new Posit8((byte)((Posit8.Half + Posit8.One).ui - 1))), Is.EqualTo(Posit8.One));
            Assert.That(Round(new Posit8((byte)((Posit8.Half + Posit8.One).ui + 1))), Is.EqualTo(Posit8.Two));
            Assert.That(Round(Posit8.MinusOne), Is.EqualTo(Posit8.MinusOne));
            Assert.That(Round(new Posit8((byte)(Posit8.MinusOne.ui + 1))), Is.EqualTo(Posit8.MinusOne));
            Assert.That(Round(new Posit8((byte)(Posit8.MinusOne.ui - 1))), Is.EqualTo(Posit8.MinusOne));
        }

        [Test]
        public void TestSign_Double()
        {
            Assert.That(Math.Sign(1.0), Is.EqualTo(+1));
            Assert.That(Math.Sign(-1.0), Is.EqualTo(-1));
            Assert.That(Math.Sign(0.0), Is.EqualTo(0));

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            var ex = Assert.Throws<ArithmeticException>(() => Math.Sign(Double.NaN));
            Assert.That(ex.Message, Is.EqualTo("Function does not accept floating point Not-a-Number values."));
        }

        [Test]
        public void TestSign_Posit8()
        {
            Assert.That(Sign(Posit8.One), Is.EqualTo(+1));
            Assert.That(Sign(Posit8.MinusOne), Is.EqualTo(-1));
            Assert.That(Sign(Posit8.Zero), Is.EqualTo(0));
            Assert.That(Sign(Posit8.NaR), Is.EqualTo(0));
        }

        [Test]
        public void TestSign_Posit16()
        {
            Assert.That(Sign(Posit16.One), Is.EqualTo(+1));
            Assert.That(Sign(Posit16.MinusOne), Is.EqualTo(-1));
            Assert.That(Sign(Posit16.Zero), Is.EqualTo(0));
            Assert.That(Sign(Posit16.NaR), Is.EqualTo(0));
        }

        [Test]
        public void TestSign_Posit32()
        {
            Assert.That(Sign(Posit32.One), Is.EqualTo(+1));
            Assert.That(Sign(Posit32.MinusOne), Is.EqualTo(-1));
            Assert.That(Sign(Posit32.Zero), Is.EqualTo(0));
            Assert.That(Sign(Posit32.NaR), Is.EqualTo(0));
        }

        [Test]
        public void TestSign_Posit64()
        {
            Assert.That(Sign(Posit64.One), Is.EqualTo(+1));
            Assert.That(Sign(Posit64.MinusOne), Is.EqualTo(-1));
            Assert.That(Sign(Posit64.Zero), Is.EqualTo(0));
            Assert.That(Sign(Posit64.NaR), Is.EqualTo(0));
        }

        [Test]
        public void TestSqrt_Posit8()
        {
            Assert.That(Sqrt(Posit8.Zero), Is.EqualTo(Posit8.Zero));
            Assert.That(Sqrt((Posit8)4), Is.EqualTo(Posit8.Two));
            Assert.That(Sqrt((Posit8)16), Is.EqualTo((Posit8)4));
            Assert.That(Sqrt(Posit8.NaR), Is.EqualTo(Posit8.NaR));
            Assert.That(Sqrt(-(Posit8)4), Is.EqualTo(Posit8.NaR));
        }
    }
}