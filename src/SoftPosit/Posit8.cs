// SPDX-License-Identifier: MIT
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=8, es=0)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit8
    {
        internal readonly byte ui; // unsigned integer value 

        public const int nbits = 8;
        public const int es = 0;

        internal Posit8(byte value) => ui = value;

        // TODO: add operators
    }
}