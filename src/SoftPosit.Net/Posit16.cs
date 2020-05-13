// SPDX-License-Identifier: MIT

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    using static Posits.Internal.Native;

    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=16, es=1)
    /// </summary>
    [Serializable]
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
        // Constants for manipulating the private bit-representation
        //

        internal const ushort SignMask = 1 << (nbits - 1);
        internal const int SignShift = nbits - 1;

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

        /// <summary>Minus one (-1).</summary>
        public static readonly Posit16 MinusOne = new Posit16(0xC000);

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
            return p16_add(a, One);
        }

        /// <summary>
        /// Adds two specified <see cref="Posit16"/> values.
        /// </summary>
        /// <param name="a">The first value to add.</param>
        /// <param name="b">The second value to add.</param>
        /// <returns>The result of adding <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static Posit16 operator +(Posit16 a, Posit16 b)
        {
            return p16_add(a, b);
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
            return p16_sub(a, One);
        }

        /// <summary>
        /// Subtracts two specified <see cref="Posit16"/> values.
        /// </summary>
        /// <param name="a">The minuend.</param>
        /// <param name="b">The subtrahend.</param>
        /// <returns>The result of subtracting <paramref name="a"/> from <paramref name="b"/>.</returns>
        public static Posit16 operator -(Posit16 a, Posit16 b)
        {
            return p16_sub(a, b);
        }

        /// <summary>
        /// Multiplies two specified <see cref="Posit16"/> values.
        /// </summary>
        /// <param name="a">The first value to multiply.</param>
        /// <param name="b">The second value to multiply.</param>
        /// <returns>The result of multiplying <paramref name="a"/> by <paramref name="b"/>.</returns>
        public static Posit16 operator *(Posit16 a, Posit16 b)
        {
            return p16_mul(a, b);
        }

        /// <summary>
        /// Divides two specified <see cref="Posit16"/> values.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The result of dividing <paramref name="a"/> by <paramref name="b"/>.</returns>
        public static Posit16 operator /(Posit16 a, Posit16 b)
        {
            return p16_div(a, b);
        }

        /// <summary>
        /// Returns the remainder resulting from dividing two specified <see cref="Posit16"/> values.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The remainder resulting from dividing d1 by d2.</returns>
        public static Posit16 operator %(Posit16 a, Posit16 b)
        {
            return p16_mod(a, b);
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
        /// Defines an implicit conversion of a 32-bit signed integer to a <see cref="Posit16"/>.
        /// </summary>
        /// <param name="value">The 32-bit signed integer to convert.</param>
        /// <returns>The converted <see cref="Posit16"/> number.</returns>
        public static implicit operator Posit16(int value)
        {
            return i32_to_p16(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a 32-bit unsigned integer to a <see cref="Posit16"/>.
        /// </summary>
        /// <param name="value">The 32-bit unsigned integer to convert.</param>
        /// <returns>The converted <see cref="Posit16"/> number.</returns>
        public static implicit operator Posit16(uint value)
        {
            return ui32_to_p16(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a 64-bit signed integer to a <see cref="Posit16"/>.
        /// </summary>
        /// <param name="value">The 64-bit signed integer to convert.</param>
        /// <returns>The converted <see cref="Posit16"/> number.</returns>
        public static implicit operator Posit16(long value)
        {
            return i64_to_p16(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a 64-bit unsigned integer to a <see cref="Posit16"/>.
        /// </summary>
        /// <param name="value">The 64-bit unsigned integer to convert.</param>
        /// <returns>The converted <see cref="Posit16"/> number.</returns>
        public static implicit operator Posit16(ulong value)
        {
            return ui64_to_p16(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a single-precision floating-point number to a <see cref="Posit16"/>.
        /// </summary>
        /// <param name="value">The single-precision floating-point number to convert.</param>
        /// <returns>The converted single-precision floating point number.</returns>
        public static implicit operator Posit16(float value)
        {
            return f32_to_p16(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a double-precision floating-point number to a <see cref="Posit16"/>.
        /// </summary>
        /// <param name="value">The double-precision floating-point number to convert.</param>
        /// <returns>The converted double-precision floating point number.</returns>
        public static implicit operator Posit16(double value)
        {
            return f64_to_p16(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit16"/> to a 32-bit signed integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 32-bit signed integer that represents the converted <see cref="Posit16"/>.</returns>
        public static explicit operator int(Posit16 value)
        {
            return p16_to_i32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit16"/> to a 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 32-bit unsigned integer that represents the converted <see cref="Posit16"/>.</returns>
        public static explicit operator uint(Posit16 value)
        {
            return p16_to_ui32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit16"/> to a 64-bit signed integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 64-bit signed integer that represents the converted <see cref="Posit16"/>.</returns>
        public static explicit operator long(Posit16 value)
        {
            return p16_to_i64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit16"/> to a 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 64-bit unsigned integer that represents the converted <see cref="Posit16"/>.</returns>
        public static explicit operator ulong(Posit16 value)
        {
            return p16_to_ui64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit16"/> to a single-precision floating-point number.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A single-precision floating-point number that represents the converted <see cref="Posit16"/>.</returns>
        public static explicit operator float(Posit16 value)
        {
            return p16_to_f32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit16"/> to a double-precision floating-point number.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A double-precision floating-point number that represents the converted <see cref="Posit16"/>.</returns>
        public static explicit operator double(Posit16 value)
        {
            return p16_to_f64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit8"/> number
        /// to a <see cref="Posit16"/> number.
        /// </summary>
        /// <param name="value">The posit number to convert.</param>
        /// <returns>The converted posit number.</returns>
        public static explicit operator Posit16(Posit8 value)
        {
            return p8_to_p16(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit32"/> number
        /// to a <see cref="Posit16"/> number.
        /// </summary>
        /// <param name="value">The posit number to convert.</param>
        /// <returns>The converted posit number.</returns>
        public static explicit operator Posit16(Posit32 value)
        {
            return p32_to_p16(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit32"/> number
        /// to a <see cref="Posit16"/> number.
        /// </summary>
        /// <param name="value">The posit number to convert.</param>
        /// <returns>The converted posit number.</returns>
        public static explicit operator Posit16(Posit64 value)
        {
            return p64_to_p16(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Quire16"/> accumulator to a <see cref="Posit16"/> number.
        /// </summary>
        public static explicit operator Posit16(Quire16 value)
        {
            return q16_to_p16(value);
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

        /// <summary>Determines whether the specified value is zero or NaR.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZeroOrNaR(Posit16 p)
        {
            return (p.ui & (Posit16.SignMask - 1)) == 0;
        }
    }

    public static partial class MathP
    {
        /// <summary>
        /// Returns the absolute value of a <see cref="Posit16"/> number.
        /// </summary>
        /// <param name="x">A number that is greater than or equal to MinValue, but less than or equal to MaxValue.</param>
        /// <returns>A <see cref="Posit16"/> number, x, such that 0 ≤ x ≤ MaxValue.</returns>
        public static Posit16 Abs(Posit16 x)
        {
            unchecked
            {
                const int CHAR_BIT = 8;
                // http://graphics.stanford.edu/~seander/bithacks.html#IntegerAbs
                var mask = (short)x.ui >> sizeof(ushort) * CHAR_BIT - 1;
                var result = (x.ui ^ mask) - mask;
                return new Posit16((ushort)result);
            }
        }

        /// <summary>
        /// Returns a value with the magnitude of <paramref name="x"/> and the sign of <paramref name="y"/>.
        /// </summary>
        /// <param name="x">A number whose magnitude is used in the result.</param>
        /// <param name="y">A number whose sign is the used in the result.</param>
        /// <returns>A value with the magnitude of <paramref name="x"/> and the sign of <paramref name="y"/>.</returns>
        public static Posit16 CopySign(Posit16 x, Posit16 y)
        {
            return ((x.ui ^ y.ui) & Posit16.SignMask) == 0 ? x : -x;
        }

        /// <summary>
        /// Returns an integer that indicates the sign of a <see cref="Posit16"/> number.
        /// </summary>
        /// <param name="x">A signed number.</param>
        /// <returns>A number that indicates the sign of <paramref name="x"/></returns>
        public static int Sign(Posit16 x)
        {
            if ((x.ui & ~Posit16.SignMask) == 0)
                return 0; // Zero or NaR

            if ((x.ui & Posit16.SignMask) != 0)
                return -1; // Negative

            return 1;
        }

        /// <summary>
        /// Returns the square root of a specified number.
        /// </summary>
        /// <param name="x">The number whose square root is to be found.</param>
        /// <returns>One of the values in the following table.
        /// <list type="table">
        ///   <listheader>
        ///     <term><paramref name="x"/> parameter</term>
        ///     <term>Return value</term>
        ///   </listheader>
        ///   <item>
        ///     <term>Zero or positive</term>
        ///     <term>The positive square root of <paramref name="x"/>.</term>
        ///   </item>
        ///   <item>
        ///     <term>Negative</term>
        ///     <term>NaR</term>
        ///   </item>
        ///   <item>
        ///     <term>NaR</term>
        ///     <term>NaR</term>
        ///   </item>
        /// </list>
        /// </returns>
        public static Posit16 Sqrt(Posit16 x)
        {
            return p16_sqrt(x);
        }
    }
}