// SPDX-License-Identifier: MIT

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=64, es=3)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit64 : IEquatable<Posit64>
    {
        internal readonly ulong ui; // unsigned integer value

        /// <summary>
        /// Number of bits.
        /// </summary>
        /// <remarks>
        /// The precision of a posit format, the total number of bits.
        /// </remarks>
        public const int nbits = 64;

        /// <summary>Exponent size.</summary>
        /// <remarks>
        /// The maximum number of bits that are available for expressing the exponent.
        /// </remarks>        
        public const int es = 3;

        /// <summary>unum seed (256) for es=3</summary>
        public const int useed = 1 << (1 << es);

        internal Posit64(ulong value) => ui = value;
        internal Posit64(long value) => ui = (ulong)value;

        //
        // Public Constants
        //

        /// <summary>Zero.</summary>
        public static readonly Posit64 Zero = new Posit64(0);

        /// <summary>Identity.</summary>
        public static readonly Posit64 One = new Posit64(0x4000_0000_0000_0000);

        /// <summary>Two (2).</summary>
        public static readonly Posit64 Two = new Posit64(0x4400_0000_0000_0000);

        /// <summary>Half (½).</summary>
        public static readonly Posit64 Half = new Posit64(0x3400_0000_0000_0000);

        /// <summary>Largest finite value (?value?).</summary>
        public static readonly Posit64 MaxValue = new Posit64(+0x7FFF_FFFF_FFFF_FFFF);

        /// <summary>Smallest finite value (-?value?).</summary>
        public static readonly Posit64 MinValue = new Posit64(-0x7FFF_FFFF_FFFF_FFFF);

        /// <summary>Not-a-Real (NaR).</summary>
        public static readonly Posit64 NaR = new Posit64(0x8000_0000_0000_0000);

        /// <summary>Infinity (±∞).</summary>
        public static readonly Posit64 Infinity = NaR;

        public static bool operator ==(Posit64 a, Posit64 b)
        {
            return (long)a.ui == (long)b.ui;
        }

        public static bool operator !=(Posit64 a, Posit64 b)
        {
            return !(a == b);
        }

        public static bool operator <=(Posit64 a, Posit64 b)
        {
            return (long)a.ui <= (long)b.ui;
        }

        public static bool operator >=(Posit64 a, Posit64 b)
        {
            return !(a >= b);
        }

        public static bool operator <(Posit64 a, Posit64 b)
        {
            return (long)a.ui < (long)b.ui;
        }

        public static bool operator >(Posit64 a, Posit64 b)
        {
            return !(a < b);
        }

        public static explicit operator Posit64(Quire64 value)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Posit64 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is Posit64 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (int)ui ^ (int)(ui >> 32);
            }
        }

        // TODO: add more operators
    }

    public static partial class Posit
    {
        /// <summary>Determines whether the specified value is zero.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZero(Posit64 p) => p.ui == Posit64.Zero.ui;

        /// <summary>Determines whether the specified value is identity (1).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOne(Posit64 p) => p.ui == Posit64.One.ui;

        /// <summary>Determines whether the specified value is NaR.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaR(Posit64 p) => p.ui == Posit64.NaR.ui;

        /// <summary>Determines whether the specified value is infinite.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(Posit64 p) => p.ui == Posit64.Infinity.ui;

        /// <summary>Determines whether the specified value is negative.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegative(Posit64 p) => (short)p.ui < 0;
    }
}