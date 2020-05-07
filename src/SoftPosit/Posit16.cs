// SPDX-License-Identifier: MIT
using System.Runtime.InteropServices;

namespace System.Numerics
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Posit (nbits=16, es=1)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Posit16
    {
        internal readonly ushort ui; // unsigned integer value

        public const int nbits = 16;
        public const int es = 1;

        internal Posit16(ushort value) => ui = value;

        // TODO: add operators
    }
}