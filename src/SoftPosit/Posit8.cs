// SPDX-License-Identifier: MIT

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=8, es=0)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit8 : IEquatable<Posit8>
    {
        internal readonly byte ui; // unsigned integer value

        /// <summary>Number of bits.</summary>
        /// <remarks>
        /// The precision of a posit format, the total number of bits.
        /// </remarks>
        public const int nbits = 8;

        /// <summary>Exponent size.</summary>
        /// <remarks>
        /// The maximum number of bits that are available for expressing the exponent.
        /// </remarks>
        public const int es = 0;

        /// <summary>unum seed (2) for es=0</summary>
        public const int useed = 1 << (1 << es); // 2

        internal Posit8(byte value) => ui = value;
        internal Posit8(sbyte value) => ui = (byte)value;

        //
        // Public Constants
        //

        /// <summary>Zero.</summary>
        public static readonly Posit8 Zero = new Posit8(0);

        /// <summary>Identity.</summary>
        public static readonly Posit8 One = new Posit8(0x40);

        /// <summary>Two (2).</summary>
        public static readonly Posit8 Two = new Posit8(0x60);

        /// <summary>Half (½).</summary>
        public static readonly Posit8 Half = new Posit8(0x20);

        /// <summary>Largest finite value (64).</summary>
        public static readonly Posit8 MaxValue = new Posit8(+0x7F);

        /// <summary>Smallest finite value (-64).</summary>
        public static readonly Posit8 MinValue = new Posit8(-0x7F);

        /// <summary>Not-a-Real (NaR).</summary>
        public static readonly Posit8 NaR = new Posit8(0x80);

        /// <summary>Infinity (±∞).</summary>
        public static readonly Posit8 Infinity = NaR;

        public static bool operator ==(Posit8 a, Posit8 b)
        {
            return (sbyte)a.ui == (sbyte)b.ui;
        }

        public static bool operator !=(Posit8 a, Posit8 b)
        {
            return !(a == b);
        }

        public static bool operator <=(Posit8 a, Posit8 b)
        {
            return (sbyte)a.ui <= (sbyte)b.ui;
        }

        public static bool operator >=(Posit8 a, Posit8 b)
        {
            return !(a >= b);
        }

        public static bool operator <(Posit8 a, Posit8 b)
        {
            return (sbyte)a.ui < (sbyte)b.ui;
        }

        public static bool operator >(Posit8 a, Posit8 b)
        {
            return !(a < b);
        }

        public static explicit operator Posit8(Quire8 value)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Posit8 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is Posit8 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return ui;
        }

        // TODO: add more operators
    }

    public static partial class Posit
    {
        /// <summary>Determines whether the specified value is zero.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(Posit8 p) => p.ui == Posit8.Zero.ui;

        /// <summary>Determines whether the specified value is identity (1).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOne(Posit8 p) => p.ui == Posit8.One.ui;

        /// <summary>Determines whether the specified value is NaR.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaR(Posit8 p) => p.ui == Posit8.NaR.ui;

        /// <summary>Determines whether the specified value is infinite.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(Posit8 p) => p.ui == Posit8.Infinity.ui;

        /// <summary>Determines whether the specified value is negative.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegative(Posit8 p) => (short)p.ui < 0;
    }
}