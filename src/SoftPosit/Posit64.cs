// SPDX-License-Identifier: MIT
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=64, es=3)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit64
    {
        internal readonly ulong ui; // unsigned integer value

        public const int nbits = 64;
        public const int es = 3;

        internal Posit64(ulong value) => ui = value;

        // TODO: add operators
    }
}