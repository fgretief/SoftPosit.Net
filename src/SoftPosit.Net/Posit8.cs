// SPDX-License-Identifier: MIT

using System.Numerics.Posits.Internal;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=8, es=0)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit8 : IComparable, IComparable<Posit8>, IEquatable<Posit8>
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

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Posit8"/> values are equal.
        /// </summary>
        public static bool operator ==(Posit8 a, Posit8 b)
        {
            return (sbyte)a.ui == (sbyte)b.ui;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Posit8"/> values are not equal.
        /// </summary>
        public static bool operator !=(Posit8 a, Posit8 b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit8"/>
        /// value is less than or equal to another specified <see cref="Posit8"/> value.
        /// </summary>
        public static bool operator <=(Posit8 a, Posit8 b)
        {
            return (sbyte)a.ui <= (sbyte)b.ui;
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit8"/>
        /// value is greater than or equal to another specified <see cref="Posit8"/> value.
        /// </summary>
        public static bool operator >=(Posit8 a, Posit8 b)
        {
            return !(a >= b);
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit8"/>
        /// value is less than another specified <see cref="Posit8"/> value.
        /// </summary>
        public static bool operator <(Posit8 a, Posit8 b)
        {
            return (sbyte)a.ui < (sbyte)b.ui;
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit8"/>
        /// value is greater than another specified <see cref="Posit8"/> value.
        /// </summary>
        public static bool operator >(Posit8 a, Posit8 b)
        {
            return !(a < b);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Quire8"/> accumulator to a <see cref="Posit8"/> number.
        /// </summary>
        public static explicit operator Posit8(Quire8 value)
        {
            return Native.q8_to_p8(value);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Posit8"/> number
        /// and returns an integer that indicates whether the value of this
        /// instance is less than, equal to, or greater than the value of the
        /// specified number.
        /// </summary>
        /// <param name="other">A <see cref="Posit8"/> number to compare.</param>
        /// <returns>A signed number indicating the relative values of this instance and value.</returns>
        public int CompareTo(Posit8 other)
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
            if (obj is Posit8 other) return CompareTo(other);
            throw new ArgumentException($"Must be a {nameof(Posit8)}");
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <see cref="Posit8"/> number represent the same value.
        /// </summary>
        /// <param name="other">A <see cref="Posit8"/> number to compare to this instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Posit8 other)
        {
            return this == other;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is an instance of <see cref="Posit8"/> and equals the value of this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is Posit8 other && Equals(other);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return ui;
        }

        // TODO: add more operators
    }

    /// <summary>
    /// Utility class with static methods for Posit numbers.
    /// </summary>
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
        public static bool IsNegative(Posit8 p) => (sbyte)p.ui < 0;
    }

    /// <summary>
    /// Provides constants and static methods for trigonometric, logarithmic,
    /// and other common mathematical functions.
    /// </summary>
    public static partial class MathP
    {

    }
}