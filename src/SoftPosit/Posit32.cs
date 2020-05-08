// SPDX-License-Identifier: MIT

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=32, es=2)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit32 : IEquatable<Posit32>
    {
        internal readonly uint ui; // unsigned integer value

        /// <summary>Number of bits.</summary>
        /// <remarks>
        /// The precision of a posit format, the total number of bits.
        /// </remarks>
        public const int nbits = 32;

        /// <summary>Exponent size.</summary>
        /// <remarks>
        /// The maximum number of bits that are available for expressing the exponent.
        /// </remarks>
        public const int es = 2;

        /// <summary>
        /// unum seed (16) for es=2
        /// </summary>
        public const int useed = 1 << (1 << es); // 16

        internal Posit32(uint value) => ui = value;
        internal Posit32(int value) => ui = (uint)value;

        //
        // Public Constants
        //

        /// <summary>Zero.</summary>
        public static readonly Posit32 Zero = new Posit32(0);

        /// <summary>Identity.</summary>
        public static readonly Posit32 One = new Posit32(0x4000_0000);

        /// <summary>Two (2).</summary>
        public static readonly Posit32 Two = new Posit32(0x4800_0000);

        /// <summary>Half (½).</summary>
        public static readonly Posit32 Half = new Posit32(0x3800_0000);

        /// <summary>Largest finite value (?value?).</summary>
        public static readonly Posit32 MaxValue = new Posit32(+0x7FFF_FFFF);

        /// <summary>Smallest finite value (-?value?).</summary>
        public static readonly Posit32 MinValue = new Posit32(-0x7FFF_FFFF);

        /// <summary>Not-a-Real (NaR).</summary>
        public static readonly Posit32 NaR = new Posit32(0x8000_0000);

        /// <summary>Infinity (±∞).</summary>
        public static readonly Posit32 Infinity = NaR;

        public static bool operator ==(Posit32 a, Posit32 b)
        {
            return (int)a.ui == (int)b.ui;
        }

        public static bool operator !=(Posit32 a, Posit32 b)
        {
            return !(a == b);
        }

        public static bool operator <=(Posit32 a, Posit32 b)
        {
            return (int)a.ui <= (int)b.ui;
        }

        public static bool operator >=(Posit32 a, Posit32 b)
        {
            return !(a >= b);
        }

        public static bool operator <(Posit32 a, Posit32 b)
        {
            return (int)a.ui < (int)b.ui;
        }

        public static bool operator >(Posit32 a, Posit32 b)
        {
            return !(a < b);
        }

        public static explicit operator Posit32(Quire32 value)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Posit32 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is Posit32 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int)ui;
        }

        // TODO: add more operators
    }

    public static partial class Posit
    {
        /// <summary>Determines whether the specified value is zero.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(Posit32 p) => p.ui == Posit32.Zero.ui;

        /// <summary>Determines whether the specified value is identity (1).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOne(Posit32 p) => p.ui == Posit32.One.ui;

        /// <summary>Determines whether the specified value is NaR.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaR(Posit32 p) => p.ui == Posit32.NaR.ui;

        /// <summary>Determines whether the specified value is infinite.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(Posit32 p) => p.ui == Posit32.Infinity.ui;

        /// <summary>Determines whether the specified value is negative.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegative(Posit32 p) => (short)p.ui < 0;
    }
}
