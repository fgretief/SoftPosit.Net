// SPDX-License-Identifier: MIT

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=16, es=1)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit16 : IEquatable<Posit16>
    {
        internal readonly ushort ui; // unsigned integer value

        /// <summary>Number of bits.</summary>
        /// <remarks>
        /// The precision of a posit format, the total number of bits.
        /// </remarks>
        public const int nbits = 16;

        /// <summary>Exponent size.</summary>
        /// <remarks>
        /// The maximum number of bits that are available for expressing the exponent.
        /// </remarks>
        public const int es = 1;

        /// <summary>unum seed (4) for es=1</summary>
        public const int useed = 1 << (1 << es); // 4

        internal Posit16(ushort value) => ui = value;
        internal Posit16(short value) => ui = (ushort)value;

        //
        // Public Constants
        //

        /// <summary>Zero.</summary>
        public static readonly Posit16 Zero = new Posit16(0);

        /// <summary>Identity.</summary>
        public static readonly Posit16 One = new Posit16(0x4000);

        /// <summary>Two (2).</summary>
        public static readonly Posit16 Two = new Posit16(0x5000);

        /// <summary>Half (½).</summary>
        public static readonly Posit16 Half = new Posit16(0x3000);

        /// <summary>Largest finite value (268435456).</summary>
        public static readonly Posit16 MaxValue = new Posit16(+0x7FFF);

        /// <summary>Smallest finite value (-268435456).</summary>
        public static readonly Posit16 MinValue = new Posit16(-0x7FFF);

        /// <summary>Not-a-Real (NaR).</summary>
        public static readonly Posit16 NaR = new Posit16(0x8000);

        /// <summary>Infinity (±∞).</summary>
        public static readonly Posit16 Infinity = NaR;


        public static bool operator ==(Posit16 a, Posit16 b)
        {
            return (short)a.ui == (short)b.ui;
        }

        public static bool operator !=(Posit16 a, Posit16 b)
        {
            return !(a == b);
        }

        public static bool operator <=(Posit16 a, Posit16 b)
        {
            return (short)a.ui <= (short)b.ui;
        }

        public static bool operator >=(Posit16 a, Posit16 b)
        {
            return !(a >= b);
        }

        public static bool operator <(Posit16 a, Posit16 b)
        {
            return (short)a.ui < (short)b.ui;
        }

        public static bool operator >(Posit16 a, Posit16 b)
        {
            return !(a < b);
        }

        public static explicit operator Posit16(Quire16 value)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Posit16 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is Posit16 other && Equals(other);
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
        public static bool IsZero(Posit16 p) => p.ui == Posit16.Zero.ui;

        /// <summary>Determines whether the specified value is identity (1).</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOne(Posit16 p) => p.ui == Posit16.One.ui;

        /// <summary>Determines whether the specified value is NaR.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaR(Posit16 p) => p.ui == Posit16.NaR.ui;

        /// <summary>Determines whether the specified value is infinite.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinity(Posit16 p) => p.ui == Posit16.Infinity.ui;

        /// <summary>Determines whether the specified value is negative.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegative(Posit16 p) => (short)p.ui < 0;
    }
}