// SPDX-License-Identifier: MIT
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=32, es=2)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit32
    {
        internal readonly uint ui; // unsigned integer value

        public const int nbits = 32;
        public const int es = 2;

        internal Posit32(uint value) => ui = value;

        // TODO: add operators
    }
}
