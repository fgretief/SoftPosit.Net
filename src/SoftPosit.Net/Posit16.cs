// SPDX-License-Identifier: MIT

using System.Numerics.Posits.Internal;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=16, es=1)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit16 : IComparable, IComparable<Posit16>, IEquatable<Posit16>
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

        /// <summary>
        /// Returns the value of the <see cref="Posit16"/> operand (the sign of the operand is unchanged).
        /// </summary>
        /// <param name="a">The operand to return.</param>
        /// <returns>The value of the operand, <paramref name="a"/>.</returns>
        public static Posit16 operator +(Posit16 a) => a;

        /// <summary>
        /// Increments the <see cref="Posit16"/> operand by 1.
        /// </summary>
        /// <param name="a">The value to increment.</param>
        /// <returns>The value of <paramref name="a"/> incremented by 1.</returns>
        public static Posit16 operator ++(Posit16 a)
        {
            return Native.p16_add(a, One);
        }

        /// <summary>
        /// Adds two specified <see cref="Posit16"/> values.
        /// </summary>
        /// <param name="a">The first value to add.</param>
        /// <param name="b">The second value to add.</param>
        /// <returns>The result of adding <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static Posit16 operator +(Posit16 a, Posit16 b)
        {
            return Native.p16_add(a, b);
        }

        /// <summary>
        /// Negates the value of the specified <see cref="Posit16"/> operand.
        /// </summary>
        /// <param name="a">The value to negate.</param>
        /// <returns>The result of <paramref name="a"/> multiplied by negative one (-1).</returns>
        public static Posit16 operator -(Posit16 a)
        {
            var mask = 1u << (nbits - 1);

            if ((a.ui & ~mask) == 0)
                return a; // Zero or NaR

            return new Posit16((ushort)(a.ui ^ mask));
        }

        /// <summary>
        /// Decrements the <see cref="Posit16"/> operand by one.
        /// </summary>
        /// <param name="a">The value to decrement.</param>
        /// <returns>The value of <paramref name="a"/> decremented by 1.</returns>
        public static Posit16 operator --(Posit16 a)
        {
            return Native.p16_sub(a, One);
        }

        /// <summary>
        /// Subtracts two specified <see cref="Posit16"/> values.
        /// </summary>
        /// <param name="a">The minuend.</param>
        /// <param name="b">The subtrahend.</param>
        /// <returns>The result of subtracting <paramref name="a"/> from <paramref name="b"/>.</returns>
        public static Posit16 operator -(Posit16 a, Posit16 b)
        {
            return Native.p16_sub(a, b);
        }

        /// <summary>
        /// Multiplies two specified <see cref="Posit16"/> values.
        /// </summary>
        /// <param name="a">The first value to multiply.</param>
        /// <param name="b">The second value to multiply.</param>
        /// <returns>The result of multiplying <paramref name="a"/> by <paramref name="b"/>.</returns>
        public static Posit16 operator *(Posit16 a, Posit16 b)
        {
            return Native.p16_mul(a, b);
        }

        /// <summary>
        /// Divides two specified <see cref="Posit16"/> values.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The result of dividing <paramref name="a"/> by <paramref name="b"/>.</returns>
        public static Posit16 operator /(Posit16 a, Posit16 b)
        {
            return Native.p16_div(a, b);
        }

        /// <summary>
        /// Returns the remainder resulting from dividing two specified <see cref="Posit16"/> values.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The remainder resulting from dividing d1 by d2.</returns>
        public static Posit16 operator %(Posit16 a, Posit16 b)
        {
            return Native.p16_mod(a, b);
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Posit16"/> values are equal.
        /// </summary>
        public static bool operator ==(Posit16 a, Posit16 b)
        {
            return (short)a.ui == (short)b.ui;
        }

        /// <summary>
        /// Returns a value that indicates whether two specified <see cref="Posit16"/> values are not equal.
        /// </summary>
        public static bool operator !=(Posit16 a, Posit16 b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit16"/>
        /// value is less than or equal to another specified <see cref="Posit16"/> value.
        /// </summary>
        public static bool operator <=(Posit16 a, Posit16 b)
        {
            return (short)a.ui <= (short)b.ui;
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit16"/>
        /// value is greater than or equal to another specified <see cref="Posit16"/> value.
        /// </summary>
        public static bool operator >=(Posit16 a, Posit16 b)
        {
            return !(a >= b);
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit16"/>
        /// value is less than another specified <see cref="Posit16"/> value.
        /// </summary>
        public static bool operator <(Posit16 a, Posit16 b)
        {
            return (short)a.ui < (short)b.ui;
        }

        /// <summary>
        /// Returns a value that indicates whether a specified <see cref="Posit16"/>
        /// value is greater than another specified <see cref="Posit16"/> value.
        /// </summary>
        public static bool operator >(Posit16 a, Posit16 b)
        {
            return !(a < b);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Quire16"/> accumulator to a <see cref="Posit16"/> number.
        /// </summary>
        public static explicit operator Posit16(Quire16 value)
        {
            return Native.q16_to_p16(value);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Posit16"/> number
        /// and returns an integer that indicates whether the value of this
        /// instance is less than, equal to, or greater than the value of the
        /// specified number.
        /// </summary>
        /// <param name="other">A <see cref="Posit16"/> number to compare.</param>
        /// <returns>A signed number indicating the relative values of this instance and value.</returns>
        public int CompareTo(Posit16 other)
        {
            if (Posit.IsNaR(this) || Posit.IsNaR(other)) return 0;
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
            if (obj is Posit16 other) return CompareTo(other);
            throw new ArgumentException($"Must be a {nameof(Posit16)}");
        }

        /// <summary>
        /// Returns a value indicating whether this instance and a specified <see cref="Posit16"/> number represent the same value.
        /// </summary>
        /// <param name="other">A <see cref="Posit16"/> number to compare to this instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Posit16 other)
        {
            return this == other;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is an instance of <see cref="Posit16"/> and equals the value of this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is Posit16 other && Equals(other);
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

    public static partial class MathP
    {

    }
}