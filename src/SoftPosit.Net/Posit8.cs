﻿// SPDX-License-Identifier: MIT

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics
{
    using static Posits.Internal.Native;

    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=8, es=0)
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerTypeProxy(typeof(Posits.Internal.DebugProxy))]
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
        internal Posit8(bool sign, byte value) => ui = (byte)(sign ? -(sbyte)value : value);

        internal void Deconstruct(out bool sign, out byte uiAbs)
        {
            sign = (ui & SignMask) != 0;
            uiAbs = sign ? (byte) -(sbyte)ui : ui;
        }

        internal void Deconstruct(out bool sign, out sbyte k, out byte tmp)
        {
            sign = (ui & SignMask) != 0;
            tmp = sign ? (byte) -(sbyte)ui : ui;
            var signOfRegime = (tmp & (SignMask >> 1)) != 0;

            tmp <<= 2;
            if (signOfRegime)
            {
                k = 0;
                while ((tmp & SignMask) != 0)
                {
                    k++;
                    tmp <<= 1;
                }
            }
            else
            {
                k = -1;
                Debug.Assert(tmp != 0, "Zero cause infinite loop");
                while ((tmp & SignMask) == 0)
                {
                    k--;
                    tmp <<= 1;
                }
                tmp &= 0x7F;
            }
        }

        //
        // Constants for manipulating the private bit-representation
        //

        internal const byte SignMask = 1 << (nbits - 1);
        internal const int SignShift = nbits - 1;

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

        /// <summary>Minus one (-1).</summary>
        public static readonly Posit8 MinusOne = new Posit8(0xC0);

        /// <summary>Largest finite value (64).</summary>
        public static readonly Posit8 MaxValue = new Posit8(+0x7F);

        /// <summary>Smallest finite value (-64).</summary>
        public static readonly Posit8 MinValue = new Posit8(-0x7F);

        /// <summary>Not-a-Real (NaR).</summary>
        public static readonly Posit8 NaR = new Posit8(0x80);

        /// <summary>Infinity (±∞).</summary>
        public static readonly Posit8 Infinity = NaR;

        /// <summary>
        /// Returns the value of the <see cref="Posit8"/> operand (the sign of the operand is unchanged).
        /// </summary>
        /// <param name="a">The operand to return.</param>
        /// <returns>The value of the operand, <paramref name="a"/>.</returns>
        public static Posit8 operator +(Posit8 a) => a;

        /// <summary>
        /// Increments the <see cref="Posit8"/> operand by 1.
        /// </summary>
        /// <param name="a">The value to increment.</param>
        /// <returns>The value of <paramref name="a"/> incremented by 1.</returns>
        public static Posit8 operator ++(Posit8 a)
        {
            return p8_add(a, One);
        }

        /// <summary>
        /// Adds two specified <see cref="Posit8"/> values.
        /// </summary>
        /// <param name="a">The first value to add.</param>
        /// <param name="b">The second value to add.</param>
        /// <returns>The result of adding <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static Posit8 operator +(Posit8 a, Posit8 b)
        {
            return p8_add(a, b);
        }

        /// <summary>
        /// Negates the value of the specified <see cref="Posit8"/> operand.
        /// </summary>
        /// <param name="a">The value to negate.</param>
        /// <returns>The result of <paramref name="a"/> multiplied by negative one (-1).</returns>
        public static Posit8 operator -(Posit8 a)
        {
            var mask = 1u << (nbits - 1);

            if ((a.ui & ~mask) == 0)
                return a; // Zero or NaR

            return new Posit8((byte)(a.ui ^ mask));
        }

        /// <summary>
        /// Decrements the <see cref="Posit8"/> operand by one.
        /// </summary>
        /// <param name="a">The value to decrement.</param>
        /// <returns>The value of <paramref name="a"/> decremented by 1.</returns>
        public static Posit8 operator --(Posit8 a)
        {
            return p8_sub(a, One);
        }

        /// <summary>
        /// Subtracts two specified <see cref="Posit8"/> values.
        /// </summary>
        /// <param name="a">The minuend.</param>
        /// <param name="b">The subtrahend.</param>
        /// <returns>The result of subtracting <paramref name="a"/> from <paramref name="b"/>.</returns>
        public static Posit8 operator -(Posit8 a, Posit8 b)
        {
            return p8_sub(a, b);
        }

        /// <summary>
        /// Multiplies two specified <see cref="Posit8"/> values.
        /// </summary>
        /// <param name="a">The first value to multiply.</param>
        /// <param name="b">The second value to multiply.</param>
        /// <returns>The result of multiplying <paramref name="a"/> by <paramref name="b"/>.</returns>
        public static Posit8 operator *(Posit8 a, Posit8 b)
        {
            return p8_mul(a, b);
        }

        /// <summary>
        /// Divides two specified <see cref="Posit8"/> values.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The result of dividing <paramref name="a"/> by <paramref name="b"/>.</returns>
        public static Posit8 operator /(Posit8 a, Posit8 b)
        {
            return p8_div(a, b);
        }

        /// <summary>
        /// Returns the remainder resulting from dividing two specified <see cref="Posit8"/> values.
        /// </summary>
        /// <param name="a">The dividend.</param>
        /// <param name="b">The divisor.</param>
        /// <returns>The remainder resulting from dividing d1 by d2.</returns>
        public static Posit8 operator %(Posit8 a, Posit8 b)
        {
            return p8_mod(a, b);
        }

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
        /// Defines an implicit conversion of a 32-bit signed integer to a <see cref="Posit8"/>.
        /// </summary>
        /// <param name="value">The 32-bit signed integer to convert.</param>
        /// <returns>The converted <see cref="Posit8"/> number.</returns>
        public static implicit operator Posit8(int value)
        {
            return i32_to_p8(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a 32-bit unsigned integer to a <see cref="Posit8"/>.
        /// </summary>
        /// <param name="value">The 32-bit unsigned integer to convert.</param>
        /// <returns>The converted <see cref="Posit8"/> number.</returns>
        public static implicit operator Posit8(uint value)
        {
            return ui32_to_p8(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a 64-bit signed integer to a <see cref="Posit8"/>.
        /// </summary>
        /// <param name="value">The 64-bit signed integer to convert.</param>
        /// <returns>The converted <see cref="Posit8"/> number.</returns>
        public static implicit operator Posit8(long value)
        {
            return i64_to_p8(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a 64-bit unsigned integer to a <see cref="Posit8"/>.
        /// </summary>
        /// <param name="value">The 64-bit unsigned integer to convert.</param>
        /// <returns>The converted <see cref="Posit8"/> number.</returns>
        public static implicit operator Posit8(ulong value)
        {
            return ui64_to_p8(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a single-precision floating-point number to a <see cref="Posit8"/>.
        /// </summary>
        /// <param name="value">The single-precision floating-point number to convert.</param>
        /// <returns>The converted single-precision floating point number.</returns>
        public static implicit operator Posit8(float value)
        {
            return f32_to_p8(value);
        }

        /// <summary>
        /// Defines an implicit conversion of a double-precision floating-point number to a <see cref="Posit8"/>.
        /// </summary>
        /// <param name="value">The double-precision floating-point number to convert.</param>
        /// <returns>The converted double-precision floating point number.</returns>
        public static implicit operator Posit8(double value)
        {
            return f64_to_p8(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit8"/> to a 32-bit signed integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 32-bit signed integer that represents the converted <see cref="Posit8"/>.</returns>
        public static explicit operator int(Posit8 value)
        {
            return p8_to_i32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit8"/> to a 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 32-bit unsigned integer that represents the converted <see cref="Posit8"/>.</returns>
        public static explicit operator uint(Posit8 value)
        {
            return p8_to_ui32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit8"/> to a 64-bit signed integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 64-bit signed integer that represents the converted <see cref="Posit8"/>.</returns>
        public static explicit operator long(Posit8 value)
        {
            return p8_to_i64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit8"/> to a 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A 64-bit unsigned integer that represents the converted <see cref="Posit8"/>.</returns>
        public static explicit operator ulong(Posit8 value)
        {
            return p8_to_ui64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit8"/> to a single-precision floating-point number.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A single-precision floating-point number that represents the converted <see cref="Posit8"/>.</returns>
        public static explicit operator float(Posit8 value)
        {
            return p8_to_f32(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit8"/> to a double-precision floating-point number.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A double-precision floating-point number that represents the converted <see cref="Posit8"/>.</returns>
        public static explicit operator double(Posit8 value)
        {
            return p8_to_f64(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit16"/> number
        /// to a <see cref="Posit8"/> number.
        /// </summary>
        /// <param name="value">The posit number to convert.</param>
        /// <returns>The converted posit number.</returns>
        public static explicit operator Posit8(Posit16 value)
        {
            return p16_to_p8(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit32"/> number
        /// to a <see cref="Posit8"/> number.
        /// </summary>
        /// <param name="value">The posit number to convert.</param>
        /// <returns>The converted posit number.</returns>
        public static explicit operator Posit8(Posit32 value)
        {
            return p32_to_p8(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Posit64"/> number
        /// to a <see cref="Posit8"/> number.
        /// </summary>
        /// <param name="value">The posit number to convert.</param>
        /// <returns>The converted posit number.</returns>
        public static explicit operator Posit8(Posit64 value)
        {
            return p64_to_p8(value);
        }

        /// <summary>
        /// Defines an explicit conversion of a <see cref="Quire8"/> accumulator to a <see cref="Posit8"/> number.
        /// </summary>
        public static explicit operator Posit8(Quire8 value)
        {
            return q8_to_p8(value);
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

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of the value of this instance.</returns>
        public override string ToString()
        {
            return $"0x{ui:X2}";
        }
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

        /// <summary>Determines whether the specified value is zero or NaR.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsZeroOrNaR(Posit8 p)
        {
            return (p.ui & (Posit8.SignMask - 1)) == 0;
        }
    }

    /// <summary>
    /// Provides constants and static methods for trigonometric, logarithmic,
    /// and other common mathematical functions.
    /// </summary>
    public static partial class MathP
    {
        /// <summary>
        /// Returns the absolute value of a <see cref="Posit8"/> number.
        /// </summary>
        /// <param name="x">A number that is greater than or equal to MinValue, but less than or equal to MaxValue.</param>
        /// <returns>A <see cref="Posit8"/> number, x, such that 0 ≤ x ≤ MaxValue.</returns>
        public static Posit8 Abs(Posit8 x)
        {
            unchecked
            {
                const int CHAR_BIT = 8;
                // http://graphics.stanford.edu/~seander/bithacks.html#IntegerAbs
                var mask = (sbyte)x.ui >> sizeof(byte) * CHAR_BIT - 1;
                var result = (x.ui ^ mask) - mask;
                return new Posit8((byte)result);
            }
        }

        /// <summary>
        /// Returns the smallest integral value that is greater than
        /// or equal to the specified <see cref="Posit8"/> number.
        /// </summary>
        /// <param name="x">A <see cref="Posit8"/> number.</param>
        /// <returns>
        /// The smallest integral value that is greater than or equal to
        /// <paramref name="x"/>. If <paramref name="x"/> is equal to
        /// <c>NaR</c>, that value is returned. Note that this method
        /// returns a <see cref="Posit8"/> instead of an integral type.
        /// </returns>
        public static Posit8 Ceiling(Posit8 x)
        {
            return p8_ceil(x);
        }

        /// <summary>
        /// Returns a value with the magnitude of <paramref name="x"/> and the sign of <paramref name="y"/>.
        /// </summary>
        /// <param name="x">A number whose magnitude is used in the result.</param>
        /// <param name="y">A number whose sign is the used in the result.</param>
        /// <returns>A value with the magnitude of <paramref name="x"/> and the sign of <paramref name="y"/>.</returns>
        public static Posit8 CopySign(Posit8 x, Posit8 y)
        {
            return ((x.ui ^ y.ui) & Posit8.SignMask) == 0 ? x : -x;
        }

        /// <summary>
        /// Returns <c>e</c> raised to the specified power.
        /// </summary>
        /// <param name="x">A number specifying a power.</param>
        /// <returns>The number <c>e</c> raised to the power <paramref name="x"/>. If <paramref name="x"/> equals NaR, that value is returned.</returns>
        public static Posit8 Exp(Posit8 x)
        {
            return p8_exp(x);
        }

        /// <summary>
        /// Returns the largest integral value less than or equal to the specified <see cref="Posit8"/> number.
        /// </summary>
        /// <param name="x">A <see cref="Posit8"/> number.</param>
        /// <returns>The largest integral value less than or equal to <paramref name="x"/>. If <paramref name="x"/> is equal to <c>NaR</c>, that value is returned.</returns>
        public static Posit8 Floor(Posit8 x)
        {
            return p8_floor(x);
        }

        /// <summary>
        /// Returns the natural (base e) logarithm of a specified number.
        /// </summary>
        /// <param name="x">The number whose logarithm is to be found.</param>
        /// <returns>The natural logarithm of <paramref name="x"/>; that is, ln x, or log e x. </returns>
        /// <remarks>Parameter <paramref name="x"/> is specified as a base 10 number.</remarks>
        public static Posit8 Log(Posit8 x)
        {
            return p8_log(x);
        }

        /// <summary>
        /// Returns the larger of two <see cref="Posit8"/> numbers.
        /// </summary>
        /// <param name="x">The first of two <see cref="Posit8"/> numbers to compare.</param>
        /// <param name="y">The second of two <see cref="Posit8"/> numbers to compare.</param>
        /// <returns>
        /// Parameter <paramref name="x"/> or <paramref name="y"/>,
        /// whichever is larger. If <paramref name="x"/>, or <paramref name="y"/>,
        /// or both <paramref name="x"/> and <paramref name="y"/>
        /// are equal to <c>NaR</c>, that value is returned.
        /// </returns>
        public static Posit8 Max(Posit8 x, Posit8 y)
        {
            if (Posit.IsNaR(x) || Posit.IsNaR(y))
                return Posit8.NaR;

            return x > y ? x : y;
        }

        /// <summary>
        /// Returns the smaller of <see cref="Posit8"/> numbers.
        /// </summary>
        /// <param name="x">The first of two <see cref="Posit8"/> numbers to compare.</param>
        /// <param name="y">The second of two <see cref="Posit8"/> numbers to compare.</param>
        /// <returns>
        /// Parameter <paramref name="x"/> or <paramref name="y"/>,
        /// whichever is smaller. If <paramref name="x"/>, <paramref name="y"/>,
        /// or both <paramref name="x"/> and <paramref name="y"/>
        /// are equal to <c>NaR</c>, that value is returned.
        /// </returns>
        public static Posit8 Min(Posit8 x, Posit8 y)
        {
            if (Posit.IsNaR(x) || Posit.IsNaR(y))
                return Posit8.NaR;

            return x <= y ? x : y;
        }

        /// <summary>
        /// Rounds a <see cref="Posit8"/> number to the nearest integral value,
        /// and rounds midpoint values to the nearest even number.
        /// </summary>
        /// <param name="x">A <see cref="Posit8"/> number to be rounded.</param>
        /// <returns>
        /// The integer nearest <paramref name="x"/>. If the fractional component
        /// of <paramref name="x"/> is halfway between two integers, one of which
        /// is even and the other odd, then the even number is returned. Note that
        /// this method returns a <see cref="Posit8"/> instead of an integral type.
        /// </returns>
        public static Posit8 Round(Posit8 x)
        {
            return p8_roundToInt(x);
        }

        /// <summary>
        /// Returns an integer that indicates the sign of a <see cref="Posit8"/> number.
        /// </summary>
        /// <param name="x">A signed number.</param>
        /// <returns>A number that indicates the sign of <paramref name="x"/></returns>
        public static int Sign(Posit8 x)
        {
            if ((x.ui & ~Posit8.SignMask) == 0)
                return 0; // Zero or NaR

            if ((x.ui & Posit8.SignMask) != 0)
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
        public static Posit8 Sqrt(Posit8 x)
        {
            return p8_sqrt(x);
        }
    }
}