// SPDX-License-Identifier: MIT

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    using static Posits.Internal.Native;

    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=32, es=2)
    /// </summary>
    [Serializable]
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
        // Constants for manipulating the private bit-representation
        //

        internal const uint SignMask = 1u << (nbits - 1);
        internal const int SignShift = nbits - 1;

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

        /// <summary>Minus one (-1).</summary>
        public static readonly Posit32 MinusOne = new Posit32(0xC000_0000);

        /// <summary>Largest finite value (?value?).</summary>
        public static readonly Posit32 MaxValue = new Posit32(+0x7FFF_FFFF);

        /// <summary>Smallest finite value (-?value?).</summary>
        public static readonly Posit32 MinValue = new Posit32(-0x7FFF_FFFF);

        /// <summary>Not-a-Real (NaR).</summary>
        public static readonly Posit32 NaR = new Posit32(0x8000_0000);

        /// <summary>Infinity (±∞).</summary>
        public static readonly Posit32 Infinity = NaR;

        /// <summary>
        /// Returns the value of the <see cref="Posit32"/> operand (the sign of the operand is unchanged).
        /// </summary>
        /// <param name="a">The operand to return.</param>
        /// <returns>The value of the operand, <paramref name="a"/>.</returns>
        public static Posit32 operator +(Posit32 a) => a;

        /// <summary>
        /// Increments the <see cref="Posit32"/> operand by 1.
        /// </summary>
        /// <param name="a">The value to increment.</param>
        /// <returns>The value of <paramref name="a"/> incremented by 1.</returns>
        public static Posit32 operator ++(Posit32 a)
        {
            return p32_add(a, One);
        }

        /// <summary>
        /// Adds two specified <see cref="Posit32"/> values.
        /// </summary>
        /// <param name="a">The first value to add.</param>
        /// <param name="b">The second value to add.</param>
        /// <returns>The result of adding <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static Posit32 operator +(Posit32 a, Posit32 b)
        {
            return p32_add(a, b);
        }

        /// <summary>
        /// Negates the value of the specified <see cref="Posit32"/> operand.
        /// </summary>
        /// <param name="a">The value to negate.</param>
        /// <returns>The result of <paramref name="a"/> multiplied by negative one (-1).</returns>
        public static Posit32 operator -(Posit32 a)
        {
            var mask = 1u << (nbits - 1);

            if ((a.ui & ~mask) == 0)
                return a; // Zero or NaR

            return new Posit32(a.ui ^ mask);
        }

        /// <summary>
        /// Decrements the <see cref="Posit32"/> operand by one.
        /// </summary>
        /// <param name="a">The value to decrement.</param>
        /// <returns>The value of <paramref name="a"/> decremented by 1.</returns>
        public static Posit32 operator --(Posit32 a)
        {
            return p32_sub(a, One);
        }

        /// <summary>
        /// Subtracts two specified <see cref="Posit32"/> values.
        /// </summary>
        /// <param name="a">The minuend.</param>
        /// <param name="b">The subtrahend.</param>
        /// <returns>The result of subtracting <paramref name="a"/> from <paramref name="b"/>.</returns>
        public static Posit32 operator -(Posit32 a, Posit32 b)
        {
            return p32_sub(a, b);
        }

        /// <summary>
        /// Multiplies two specified <see cref="Posit32"/> values.
        /// </summary>
        /// <param name="a">The first value to multiply.</param>
        /// <param name="b">The second value to multiply.</param>
        /// <returns>The result of multiplying <paramref name="a"/> by <paramref name="b"/>.</returns>
        public static Posit32 operator *(Posit32 a, Posit32 b)
        {
            return p32_mul(a, b);
        }

        /// <summary>
        /// Divides two specified <see cref="Posit32"/> values.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The result of dividing <paramref name="a"/> by <paramref name="b"/>.</returns>
        public static Posit32 operator /(Posit32 a, Posit32 b)
        {
            return p32_div(a, b);
        }

        /// <summary>
        /// Returns the remainder resulting from dividing two specified <see cref="Posit32"/> values.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The remainder resulting from dividing d1 by d2.</returns>
        public static Posit32 operator %(Posit32 a, Posit32 b)
        {
            return p32_mod(a, b);
        }


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
        /// Defines an implicit conversion of a 32-bit signed integer to a <see cref="Posit32"/>.
        /// </summary>
        /// <param name="value">The 32-bit signed integer to convert.</param>
        /// <returns>The converted <see cref="Posit32"/> number.</returns>
        public static implicit operator Posit32(int value)
        {
            return i32_to_p32(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a 32-bit unsigned integer to a <see cref="Posit32"/>.
        /// </summary>
        /// <param name="value">The 32-bit unsigned integer to convert.</param>
        /// <returns>The converted <see cref="Posit32"/> number.</returns>
        public static implicit operator Posit32(uint value)
        {
            return ui32_to_p32(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a 64-bit signed integer to a <see cref="Posit32"/>.
        /// </summary>
        /// <param name="value">The 64-bit signed integer to convert.</param>
        /// <returns>The converted <see cref="Posit32"/> number.</returns>
        public static implicit operator Posit32(long value)
        {
            return i64_to_p32(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a 64-bit unsigned integer to a <see cref="Posit32"/>.
        /// </summary>
        /// <param name="value">The 64-bit unsigned integer to convert.</param>
        /// <returns>The converted <see cref="Posit32"/> number.</returns>
        public static implicit operator Posit32(ulong value)
        {
            return ui64_to_p32(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a single-precision floating-point number to a <see cref="Posit32"/>.
        /// </summary>
        /// <param name="value">The single-precision floating-point number to convert.</param>
        /// <returns>The converted single-precision floating point number.</returns>
        public static implicit operator Posit32(float value)
        {
            return f32_to_p32(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a double-precision floating-point number to a <see cref="Posit32"/>.
        /// </summary>
        /// <param name="value">The double-precision floating-point number to convert.</param>
        /// <returns>The converted double-precision floating point number.</returns>
        public static implicit operator Posit32(double value)
        {
            return f64_to_p32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit32"/> to a 32-bit signed integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 32-bit signed integer that represents the converted <see cref="Posit32"/>.</returns>
        public static explicit operator int(Posit32 value)
        {
            return p32_to_i32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit32"/> to a 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 32-bit unsigned integer that represents the converted <see cref="Posit32"/>.</returns>
        public static explicit operator uint(Posit32 value)
        {
            return p32_to_ui32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit32"/> to a 64-bit signed integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 64-bit signed integer that represents the converted <see cref="Posit32"/>.</returns>
        public static explicit operator long(Posit32 value)
        {
            return p32_to_i64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit32"/> to a 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 64-bit unsigned integer that represents the converted <see cref="Posit32"/>.</returns>
        public static explicit operator ulong(Posit32 value)
        {
            return p32_to_ui64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit32"/> to a single-precision floating-point number.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A single-precision floating-point number that represents the converted <see cref="Posit32"/>.</returns>
        public static explicit operator float(Posit32 value)
        {
            return p32_to_f32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit32"/> to a double-precision floating-point number.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A double-precision floating-point number that represents the converted <see cref="Posit32"/>.</returns>
        public static explicit operator double(Posit32 value)
        {
            return p32_to_f64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit8"/> number
        /// to a <see cref="Posit32"/> number.
        /// </summary>
        /// <param name="value">The posit number to convert.</param>
        /// <returns>The converted posit number.</returns>
        public static explicit operator Posit32(Posit8 value)
        {
            return p8_to_p32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit16"/> number
        /// to a <see cref="Posit32"/> number.
        /// </summary>
        /// <param name="value">The posit number to convert.</param>
        /// <returns>The converted posit number.</returns>
        public static explicit operator Posit32(Posit16 value)
        {
            return p16_to_p32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit64"/> number
        /// to a <see cref="Posit32"/> number.
        /// </summary>
        /// <param name="value">The posit number to convert.</param>
        /// <returns>The converted posit number.</returns>
        public static explicit operator Posit32(Posit64 value)
        {
            return p64_to_p32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Quire32"/> accumulator to a <see cref="Posit32"/> number.
        /// </summary>
        public static explicit operator Posit32(Quire32 value)
        {
            return q32_to_p32(value);
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

        /// <summary>Determines whether the specified value is zero or NaR.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZeroOrNaR(Posit32 p)
        {
            return (p.ui & (Posit32.SignMask - 1)) == 0;
        }
    }

    public static partial class MathP
    {

    }
}
