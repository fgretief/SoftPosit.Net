// SPDX-License-Identifier: MIT

using System.Numerics.Posits.Internal;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=64, es=3)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit64 : IComparable, IComparable<Posit64>, IEquatable<Posit64>
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

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Posit64"/> values are equal.
        /// </summary>
        public static bool operator ==(Posit64 a, Posit64 b)
        {
            return (long)a.ui == (long)b.ui;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Posit64"/> values are not equal.
        /// </summary>
        public static bool operator !=(Posit64 a, Posit64 b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit64"/>
        /// value is less than or equal to another specified <see cref="Posit64"/> value.
        /// </summary>
        public static bool operator <=(Posit64 a, Posit64 b)
        {
            return (long)a.ui <= (long)b.ui;
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit64"/>
        /// value is greater than or equal to another specified <see cref="Posit64"/> value.
        /// </summary>
        public static bool operator >=(Posit64 a, Posit64 b)
        {
            return !(a >= b);
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit64"/>
        /// value is less than another specified <see cref="Posit64"/> value.
        /// </summary>
        public static bool operator <(Posit64 a, Posit64 b)
        {
            return (long)a.ui < (long)b.ui;
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit64"/>
        /// value is greater than another specified <see cref="Posit64"/> value.
        /// </summary>
        public static bool operator >(Posit64 a, Posit64 b)
        {
            return !(a < b);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Quire64"/> accumulator to a <see cref="Posit64"/> number.
        /// </summary>
        public static explicit operator Posit64(Quire64 value)
        {
            return Native.q64_to_p64(value);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Posit16"/> number
        /// and returns an integer that indicates whether the value of this
        /// instance is less than, equal to, or greater than the value of the
        /// specified number.
        /// </summary>
        /// <param name="other">A <see cref="Posit16"/> number to compare.</param>
        /// <returns>A signed number indicating the relative values of this instance and value.</returns>
        public int CompareTo(Posit64 other)
        {
            if (this == other) return 0;
            return this < other ? -1 : 1;
        }

        /// <summary>
        /// Compares this instance to a specified object and returns an integer
        /// that indicates whether the value of this instance is less than,
        /// equal to, or greater than the value of the specified object.
        /// </summary>
        /// <param name="obj">An object to compare, or null.</param>
        /// <returns>A signed number indicating the relative values of this instance and value.</returns>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (obj is Posit64 other) return CompareTo(other);
            throw new ArgumentException($"Must be a {nameof(Posit64)}");
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <see cref="Posit32"/> number represent the same value.
        /// </summary>
        /// <param name="other">A <see cref="Posit32"/> number to compare to this instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Posit64 other)
        {
            return this == other;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is an instance of <see cref="Posit64"/> and equals the value of this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is Posit64 other && Equals(other);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
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
        public static bool IsNegative(Posit64 p) => (long)p.ui < 0;
    }

    public static partial class MathP
    {

    }
}