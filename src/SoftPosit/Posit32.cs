// SPDX-License-Identifier: MIT
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=32, es=2)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit32 : IEquatable<Posit32>
    {
        internal readonly uint ui; // unsigned integer value

        public const int nbits = 32;
        public const int es = 2;

        internal Posit32(uint value) => ui = value;

        public static bool operator ==(Posit32 a, Posit32 b)
        {
            return (int)a.ui == (int)b.ui;
        }

        public static bool operator !=(Posit32 a, Posit32 b)
        {
            return !(a == b);
        }

        public static bool operator <=(Posit32 a, Posit32 b)
        {
            return (int)a.ui <= (int)b.ui;
        }

        public static bool operator >=(Posit32 a, Posit32 b)
        {
            return !(a >= b);
        }

        public static bool operator <(Posit32 a, Posit32 b)
        {
            return (int)a.ui < (int)b.ui;
        }

        public static bool operator >(Posit32 a, Posit32 b)
        {
            return !(a < b);
        }

        public static explicit operator Posit32(Quire32 value)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Posit32 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is Posit32 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int)ui;
        }

        // TODO: add more operators
    }
}
