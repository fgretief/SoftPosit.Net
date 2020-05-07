// SPDX-License-Identifier: MIT
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=64, es=3)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit64 : IEquatable<Posit64>
    {
        internal readonly ulong ui; // unsigned integer value

        public const int nbits = 64;
        public const int es = 3;

        internal Posit64(ulong value) => ui = value;

        public static bool operator ==(Posit64 a, Posit64 b)
        {
            return (long)a.ui == (long)b.ui;
        }

        public static bool operator !=(Posit64 a, Posit64 b)
        {
            return !(a == b);
        }

        public static bool operator <=(Posit64 a, Posit64 b)
        {
            return (long)a.ui <= (long)b.ui;
        }

        public static bool operator >=(Posit64 a, Posit64 b)
        {
            return !(a >= b);
        }

        public static bool operator <(Posit64 a, Posit64 b)
        {
            return (long)a.ui < (long)b.ui;
        }

        public static bool operator >(Posit64 a, Posit64 b)
        {
            return !(a < b);
        }

        public static explicit operator Posit64(Quire64 value)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Posit64 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is Posit64 other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (int)ui ^ (int)(ui >> 32);
            }
        }

        // TODO: add more operators
    }
}