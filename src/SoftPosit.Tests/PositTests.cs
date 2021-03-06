﻿// SPDX-License-Identifier: MIT

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

    public class QuireTests
    {
        [Test]
        public void TestQuire8Add()
        {
            var qA = Quire8.Create();
            qA += (1, 1);
            var pA = qA.ToPosit();
            Assert.That(pA, Is.EqualTo(Posit8.One));
        }

        [Test]
        public void TestQuire8Sub()
        {
            var qA = Quire8.Create();
            qA -= (1, 1);
            var pA = qA.ToPosit();
            Assert.That(pA, Is.EqualTo(Posit8.MinusOne));
        }
    }
}