// SPDX-License-Identifier: MIT

using System.Runtime.InteropServices;

namespace System.Numerics
{
    /// <summary>
    /// Quire for 16-bit Posit values
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Quire16
    {
        private fixed ulong m_value[2]; // 128-bits

        /// <summary>
        /// Create a <see cref="Quire16" /> accumulator register.
        /// </summary>
        public static Quire16 Create()
        {
            return new Quire16();
        }

        /// <summary>
        /// Clear the <see cref="Quire16" /> accumulator register to zero.
        /// </summary>
        public void Clear()
        {
            for (var i = 0; i < 2; ++i)
                m_value[i] = 0;
        }

        /// <summary>
        /// Convert the <see cref="Quire8" /> accumulator register to a <see cref="Posit16"/> number.
        /// </summary>
        public Posit16 ToPosit()
        {
            return (Posit16)this;
        }

        /// <summary>
        /// Multiply specified value with one and add to <see cref="Quire16"/> accumulator register.
        /// <para>(result = <paramref name="q"/> + <paramref name="a"/> * 1)</para>
        /// </summary>
        public static Posit16 operator +(Quire16 q, Posit16 a)
        {
            var b = Posit16.One;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multiply specified tuple values and add to <see cref="Quire16"/> accumulator register.
        /// <para>(result = <paramref name="q"/> + a * b)</para>
        /// </summary>
        public static Posit16 operator +(Quire16 q, ValueTuple<Posit16, Posit16> aMulB)
        {
            var (a, b) = aMulB;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multiply specified value with one and subtract from <see cref="Quire16"/> accumulator register.
        /// <para>(result = <paramref name="q"/> - a * 1)</para>
        /// </summary>
        public static Posit16 operator -(Quire16 q, Posit16 a)
        {
            var b = Posit16.One;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multiply specified tuple values and subtract from <see cref="Quire16"/> accumulator register.
        /// <para>(result = <paramref name="q"/> - a * b)</para>
        /// </summary>
        public static Posit16 operator -(Quire16 q, ValueTuple<Posit16, Posit16> aMulB)
        {
            var (a, b) = aMulB;
            throw new NotImplementedException();
        }

        // TODO: add more operators
    }
}