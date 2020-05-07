// SPDX-License-Identifier: MIT
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

        public const int nbits = 16;
        public const int es = 1;

        internal Posit16(ushort value) => ui = value;

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
}