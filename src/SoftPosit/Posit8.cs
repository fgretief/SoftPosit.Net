// SPDX-License-Identifier: MIT
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=8, es=0)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit8 : IEquatable<Posit8>
    {
        internal readonly byte ui; // unsigned integer value

        public const int nbits = 8;
        public const int es = 0;

        internal Posit8(byte value) => ui = value;

        public static bool operator ==(Posit8 a, Posit8 b)
        {
            return (sbyte)a.ui == (sbyte)b.ui;
        }

        public static bool operator !=(Posit8 a, Posit8 b)
        {
            return !(a == b);
        }

        public static bool operator <=(Posit8 a, Posit8 b)
        {
            return (sbyte)a.ui <= (sbyte)b.ui;
        }

        public static bool operator >=(Posit8 a, Posit8 b)
        {
            return !(a >= b);
        }

        public static bool operator <(Posit8 a, Posit8 b)
        {
            return (sbyte)a.ui < (sbyte)b.ui;
        }

        public static bool operator >(Posit8 a, Posit8 b)
        {
            return !(a < b);
        }

        public static explicit operator Posit8(Quire8 value)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Posit8 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is Posit8 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return ui;
        }

        // TODO: add more operators
    }
}