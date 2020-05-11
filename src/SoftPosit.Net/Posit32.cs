// SPDX-License-Identifier: MIT

using System.Numerics.Posits.Internal;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=32, es=2)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit32 : IComparable, IComparable<Posit32>, IEquatable<Posit32>
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

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Posit32"/> values are equal.
        /// </summary>
        public static bool operator ==(Posit32 a, Posit32 b)
        {
            return (int)a.ui == (int)b.ui;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Posit32"/> values are not equal.
        /// </summary>
        public static bool operator !=(Posit32 a, Posit32 b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit32"/>
        /// value is less than or equal to another specified <see cref="Posit32"/> value.
        /// </summary>
        public static bool operator <=(Posit32 a, Posit32 b)
        {
            return (int)a.ui <= (int)b.ui;
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit32"/>
        /// value is greater than or equal to another specified <see cref="Posit32"/> value.
        /// </summary>
        public static bool operator >=(Posit32 a, Posit32 b)
        {
            return !(a >= b);
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit32"/>
        /// value is less than another specified <see cref="Posit32"/> value.
        /// </summary>
        public static bool operator <(Posit32 a, Posit32 b)
        {
            return (int)a.ui < (int)b.ui;
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit32"/>
        /// value is greater than another specified <see cref="Posit32"/> value.
        /// </summary>
        public static bool operator >(Posit32 a, Posit32 b)
        {
            return !(a < b);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Quire32"/> accumulator to a <see cref="Posit32"/> number.
        /// </summary>
        public static explicit operator Posit32(Quire32 value)
        {
            return Native.q32_to_p32(value);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Posit32"/> number
        /// and returns an integer that indicates whether the value of this
        /// instance is less than, equal to, or greater than the value of the
        /// specified number.
        /// </summary>
        /// <param name="other">A <see cref="Posit32"/> number to compare.</param>
        /// <returns>A signed number indicating the relative values of this instance and value.</returns>
        public int CompareTo(Posit32 other)
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
            if (obj is Posit32 other) return CompareTo(other);
            throw new ArgumentException($"Must be a {nameof(Posit32)}");
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <see cref="Posit32"/> number represent the same value.
        /// </summary>
        /// <param name="other">A <see cref="Posit32"/> number to compare to this instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Posit32 other)
        {
            return this == other;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is an instance of <see cref="Posit32"/> and equals the value of this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is Posit32 other && Equals(other);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
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
        public static bool IsNegative(Posit32 p) => (int)p.ui < 0;
    }

    public static partial class MathP
    {

    }
}
