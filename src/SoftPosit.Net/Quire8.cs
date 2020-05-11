// SPDX-License-Identifier: MIT

using System.Runtime.InteropServices;

namespace System.Numerics
{
    /// <summary>
    /// Quire for 8-bit Posit values
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Quire8
    {
        private uint m_value; // 32-bits

        /// <summary>
        /// Create a <see cref="Quire8" /> accumulator register.
        /// </summary>
        public static Quire8 Create()
        {
            return new Quire8();
        }

        /// <summary>
        /// Clear the <see cref="Quire8" /> accumulator register to zero.
        /// </summary>
        public void Clear()
        {
            m_value = 0;
        }

        /// <summary>
        /// Convert the <see cref="Quire8" /> accumulator register to a <see cref="Posit8"/> number.
        /// </summary>
        public Posit8 ToPosit()
        {
            return (Posit8)this;
        }

        /// <summary>
        /// Multiply specified value with one and add to <see cref="Quire8"/> accumulator register.
        /// <para>(result = <paramref name="q"/> + <paramref name="a"/> * 1)</para>
        /// </summary>
        public static Posit8 operator +(Quire8 q, Posit8 a)
        {
            var b = Posit8.One;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multiply specified tuple values and add to <see cref="Quire8"/> accumulator register.
        /// <para>(result = <paramref name="q"/> + a * b)</para>
        /// </summary>
        public static Posit8 operator +(Quire8 q, ValueTuple<Posit8, Posit8> aMulB)
        {
            var (a, b) = aMulB;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multiply specified value with one and subtract from <see cref="Quire8"/> accumulator register.
        /// <para>(result = <paramref name="q"/> - a * 1)</para>
        /// </summary>
        public static Posit8 operator -(Quire8 q, Posit8 a)
        {
            var b = Posit8.One;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multiply specified tuple values and subtract from <see cref="Quire8"/> accumulator register.
        /// <para>(result = <paramref name="q"/> - a * b)</para>
        /// </summary>
        public static Posit8 operator -(Quire8 q, ValueTuple<Posit8, Posit8> aMulB)
        {
            var (a, b) = aMulB;
            throw new NotImplementedException();
        }

        // TODO: add more operators
    }
}