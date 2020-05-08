// SPDX-License-Identifier: MIT

using System.Runtime.InteropServices;

namespace System.Numerics
{
    /// <summary>
    /// Quire for 32-bit Posit values
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Quire32
    {
        private fixed ulong m_value[8]; // 512-bits

        /// <summary>
        /// Create a <see cref="Quire32" /> accumulator register.
        /// </summary>
        public static Quire32 Create()
        {
            return new Quire32();
        }

        /// <summary>
        /// Clear the <see cref="Quire32" /> accumulator register to zero.
        /// </summary>
        public void Clear()
        {
            for (var i = 0; i < 8; ++i)
                m_value[i] = 0;
        }

        /// <summary>
        /// Convert the <see cref="Quire32" /> accumulator register to a <see cref="Posit32"/> number.
        /// </summary>
        public Posit32 ToPosit()
        {
            return (Posit32)this;
        }

        /// <summary>
        /// Multiply specified value with one and add to <see cref="Quire32"/> accumulator register.
        /// <para>(result = <paramref name="q"/> + <paramref name="a"/> * 1)</para>
        /// </summary>
        public static Posit32 operator +(Quire32 q, Posit32 a)
        {
            var b = Posit32.One;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multiply specified tuple values and add to <see cref="Quire32"/> accumulator register.
        /// <para>(result = <paramref name="q"/> + a * b)</para>
        /// </summary>
        public static Posit32 operator +(Quire32 q, ValueTuple<Posit32, Posit32> aMulB)
        {
            var (a, b) = aMulB;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multiply specified value with one and subtract from <see cref="Quire32"/> accumulator register.
        /// <para>(result = <paramref name="q"/> - a * 1)</para>
        /// </summary>
        public static Posit32 operator -(Quire32 q, Posit32 a)
        {
            var b = Posit32.One;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multiply specified tuple values and subtract from <see cref="Quire64"/> accumulator register.
        /// <para>(result = <paramref name="q"/> - a * b)</para>
        /// </summary>
        public static Posit32 operator -(Quire32 q, ValueTuple<Posit32, Posit32> aMulB)
        {
            var (a, b) = aMulB;
            throw new NotImplementedException();
        }

        // TODO: add more operators
    }
}