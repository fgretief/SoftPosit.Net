// SPDX-License-Identifier: MIT

using System.Runtime.InteropServices;

namespace System.Numerics
{
    using static Posits.Internal.Native;

    /// <summary>
    /// Quire for 64-bit Posit values
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Quire64
    {
        private fixed ulong m_value[32]; // 2048-bits

        /// <summary>
        /// Create a <see cref="Quire64" /> accumulator register.
        /// </summary>
        public static Quire64 Create()
        {
            return new Quire64();
        }

        /// <summary>
        /// Clear the <see cref="Quire64" /> accumulator register to zero.
        /// </summary>
        public void Clear()
        {
            for (var i = 0; i < 32; ++i)
                m_value[i] = 0;
        }

        /// <summary>
        /// Convert the <see cref="Quire64" /> accumulator register to a <see cref="Posit64"/> number.
        /// </summary>
        public Posit64 ToPosit()
        {
            return (Posit64)this;
        }

        /// <summary>
        /// Multiply specified value with one and add to <see cref="Quire64"/> accumulator register.
        /// <para>(result = <paramref name="q"/> + <paramref name="a"/> * 1)</para>
        /// </summary>
        public static Quire64 operator +(Quire64 q, Posit64 a)
        {
            var b = Posit64.One;
            return q64_fdp_add(q, a, b);
        }

        /// <summary>
        /// Multiply specified tuple values and add to <see cref="Quire64"/> accumulator register.
        /// <para>(result = <paramref name="q"/> + a * b)</para>
        /// </summary>
        public static Quire64 operator +(Quire64 q, ValueTuple<Posit64, Posit64> aMulB)
        {
            var (a, b) = aMulB;
            return q64_fdp_add(q, a, b);
        }

        /// <summary>
        /// Multiply specified value with one and subtract from <see cref="Quire64"/> accumulator register.
        /// <para>(result = <paramref name="q"/> - a * 1)</para>
        /// </summary>
        public static Quire64 operator -(Quire64 q, Posit64 a)
        {
            var b = Posit64.One;
            return q64_fdp_sub(q, a, b);
        }

        /// <summary>
        /// Multiply specified tuple values and subtract from <see cref="Quire64"/> accumulator register.
        /// <para>(result = <paramref name="q"/> - a * b)</para>
        /// </summary>
        public static Quire64 operator -(Quire64 q, ValueTuple<Posit64, Posit64> aMulB)
        {
            var (a, b) = aMulB;
            return q64_fdp_sub(q, a, b);
        }

        // TODO: add more operators
    }
}