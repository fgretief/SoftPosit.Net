// SPDX-License-Identifier: MIT

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    using static Posits.Internal.Native;

    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=64, es=3)
    /// </summary>
    [Serializable]
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
        // Constants for manipulating the private bit-representation
        //

        internal const ulong SignMask = 1ul << (nbits - 1);
        internal const int SignShift = nbits - 1;

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

        /// <summary>Minus one (-1).</summary>
        public static readonly Posit64 MinusOne = new Posit64(0xC000_0000_0000_0000);

        /// <summary>Largest finite value (?value?).</summary>
        public static readonly Posit64 MaxValue = new Posit64(+0x7FFF_FFFF_FFFF_FFFF);

        /// <summary>Smallest finite value (-?value?).</summary>
        public static readonly Posit64 MinValue = new Posit64(-0x7FFF_FFFF_FFFF_FFFF);

        /// <summary>Not-a-Real (NaR).</summary>
        public static readonly Posit64 NaR = new Posit64(0x8000_0000_0000_0000);

        /// <summary>Infinity (±∞).</summary>
        public static readonly Posit64 Infinity = NaR;

        /// <summary>
        /// Returns the value of the <see cref="Posit64"/> operand (the sign of the operand is unchanged).
        /// </summary>
        /// <param name="a">The operand to return.</param>
        /// <returns>The value of the operand, <paramref name="a"/>.</returns>
        public static Posit64 operator +(Posit64 a) => a;

        /// <summary>
        /// Increments the <see cref="Posit64"/> operand by 1.
        /// </summary>
        /// <param name="a">The value to increment.</param>
        /// <returns>The value of <paramref name="a"/> incremented by 1.</returns>
        public static Posit64 operator ++(Posit64 a)
        {
            return p64_add(a, One);
        }

        /// <summary>
        /// Adds two specified <see cref="Posit64"/> values.
        /// </summary>
        /// <param name="a">The first value to add.</param>
        /// <param name="b">The second value to add.</param>
        /// <returns>The result of adding <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static Posit64 operator +(Posit64 a, Posit64 b)
        {
            return p64_add(a, b);
        }

        /// <summary>
        /// Negates the value of the specified <see cref="Posit64"/> operand.
        /// </summary>
        /// <param name="a">The value to negate.</param>
        /// <returns>The result of <paramref name="a"/> multiplied by negative one (-1).</returns>
        public static Posit64 operator -(Posit64 a)
        {
            var mask = 1ul << (nbits - 1);

            if ((a.ui & ~mask) == 0)
                return a; // Zero or NaR

            return new Posit64(a.ui ^ mask);
        }

        /// <summary>
        /// Decrements the <see cref="Posit64"/> operand by one.
        /// </summary>
        /// <param name="a">The value to decrement.</param>
        /// <returns>The value of <paramref name="a"/> decremented by 1.</returns>
        public static Posit64 operator --(Posit64 a)
        {
            return p64_sub(a, One);
        }

        /// <summary>
        /// Subtracts two specified <see cref="Posit64"/> values.
        /// </summary>
        /// <param name="a">The minuend.</param>
        /// <param name="b">The subtrahend.</param>
        /// <returns>The result of subtracting <paramref name="a"/> from <paramref name="b"/>.</returns>
        public static Posit64 operator -(Posit64 a, Posit64 b)
        {
            return p64_sub(a, b);
        }

        /// <summary>
        /// Multiplies two specified <see cref="Posit64"/> values.
        /// </summary>
        /// <param name="a">The first value to multiply.</param>
        /// <param name="b">The second value to multiply.</param>
        /// <returns>The result of multiplying <paramref name="a"/> by <paramref name="b"/>.</returns>
        public static Posit64 operator *(Posit64 a, Posit64 b)
        {
            return p64_mul(a, b);
        }

        /// <summary>
        /// Divides two specified <see cref="Posit64"/> values.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The result of dividing <paramref name="a"/> by <paramref name="b"/>.</returns>
        public static Posit64 operator /(Posit64 a, Posit64 b)
        {
            return p64_div(a, b);
        }

        /// <summary>
        /// Returns the remainder resulting from dividing two specified <see cref="Posit64"/> values.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The remainder resulting from dividing d1 by d2.</returns>
        public static Posit64 operator %(Posit64 a, Posit64 b)
        {
            return p64_mod(a, b);
        }

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
        /// Defines an implicit conversion of a 32-bit signed integer to a <see cref="Posit64"/>.
        /// </summary>
        /// <param name="value">The 32-bit signed integer to convert.</param>
        /// <returns>The converted <see cref="Posit64"/> number.</returns>
        public static implicit operator Posit64(int value)
        {
            return i32_to_p64(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a 32-bit unsigned integer to a <see cref="Posit64"/>.
        /// </summary>
        /// <param name="value">The 32-bit unsigned integer to convert.</param>
        /// <returns>The converted <see cref="Posit64"/> number.</returns>
        public static implicit operator Posit64(uint value)
        {
            return ui32_to_p64(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a 64-bit signed integer to a <see cref="Posit64"/>.
        /// </summary>
        /// <param name="value">The 64-bit signed integer to convert.</param>
        /// <returns>The converted <see cref="Posit64"/> number.</returns>
        public static implicit operator Posit64(long value)
        {
            return i64_to_p64(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a 64-bit unsigned integer to a <see cref="Posit64"/>.
        /// </summary>
        /// <param name="value">The 64-bit unsigned integer to convert.</param>
        /// <returns>The converted <see cref="Posit64"/> number.</returns>
        public static implicit operator Posit64(ulong value)
        {
            return ui64_to_p64(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a single-precision floating-point number to a <see cref="Posit64"/>.
        /// </summary>
        /// <param name="value">The single-precision floating-point number to convert.</param>
        /// <returns>The converted single-precision floating point number.</returns>
        public static implicit operator Posit64(float value)
        {
            return f32_to_p64(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a double-precision floating-point number to a <see cref="Posit64"/>.
        /// </summary>
        /// <param name="value">The double-precision floating-point number to convert.</param>
        /// <returns>The converted double-precision floating point number.</returns>
        public static implicit operator Posit64(double value)
        {
            return f64_to_p64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit64"/> to a 32-bit signed integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 32-bit signed integer that represents the converted <see cref="Posit64"/>.</returns>
        public static explicit operator int(Posit64 value)
        {
            return p64_to_i32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit64"/> to a 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 32-bit unsigned integer that represents the converted <see cref="Posit64"/>.</returns>
        public static explicit operator uint(Posit64 value)
        {
            return p64_to_ui32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit64"/> to a 64-bit signed integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 64-bit signed integer that represents the converted <see cref="Posit64"/>.</returns>
        public static explicit operator long(Posit64 value)
        {
            return p64_to_i64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit64"/> to a 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 64-bit unsigned integer that represents the converted <see cref="Posit64"/>.</returns>
        public static explicit operator ulong(Posit64 value)
        {
            return p64_to_ui64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit64"/> to a single-precision floating-point number.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A single-precision floating-point number that represents the converted <see cref="Posit64"/>.</returns>
        public static explicit operator float(Posit64 value)
        {
            return p64_to_f32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit64"/> to a double-precision floating-point number.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A double-precision floating-point number that represents the converted <see cref="Posit64"/>.</returns>
        public static explicit operator double(Posit64 value)
        {
            return p64_to_f64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit8"/> number
        /// to a <see cref="Posit64"/> number.
        /// </summary>
        /// <param name="value">The posit number to convert.</param>
        /// <returns>The converted posit number.</returns>
        public static explicit operator Posit64(Posit8 value)
        {
            return p8_to_p64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit16"/> number
        /// to a <see cref="Posit64"/> number.
        /// </summary>
        /// <param name="value">The posit number to convert.</param>
        /// <returns>The converted posit number.</returns>
        public static explicit operator Posit64(Posit16 value)
        {
            return p16_to_p64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit32"/> number
        /// to a <see cref="Posit64"/> number.
        /// </summary>
        /// <param name="value">The posit number to convert.</param>
        /// <returns>The converted posit number.</returns>
        public static explicit operator Posit64(Posit32 value)
        {
            return p32_to_p64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Quire64"/> accumulator to a <see cref="Posit64"/> number.
        /// </summary>
        public static explicit operator Posit64(Quire64 value)
        {
            return q64_to_p64(value);
        }

        /// <summary>
        /// Compares this instance to a specified <see cref="Posit64"/> number
        /// and returns an integer that indicates whether the value of this
        /// instance is less than, equal to, or greater than the value of the
        /// specified number.
        /// </summary>
        /// <param name="other">A <see cref="Posit64"/> number to compare.</param>
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

        /// <summary>Determines whether the specified value is zero or NaR.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZeroOrNaR(Posit64 p)
        {
            return (p.ui & (Posit64.SignMask - 1)) == 0;
        }
    }

    public static partial class MathP
    {
        /// <summary>
        /// Returns the absolute value of a <see cref="Posit64"/> number.
        /// </summary>
        /// <param name="x">A number that is greater than or equal to MinValue, but less than or equal to MaxValue.</param>
        /// <returns>A <see cref="Posit64"/> number, x, such that 0 ≤ x ≤ MaxValue.</returns>
        public static Posit64 Abs(Posit64 x)
        {
            unchecked
            {
                const int CHAR_BIT = 8;
                // http://graphics.stanford.edu/~seander/bithacks.html#IntegerAbs
                var mask = (long)x.ui >> sizeof(ulong) * CHAR_BIT - 1;
                var result = ((long)x.ui ^ mask) - mask;
                return new Posit64(result);
            }
        }
    }
}