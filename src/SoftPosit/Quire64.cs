// SPDX-License-Identifier: MIT
using System.Runtime.InteropServices;

namespace System.Numerics
{
    /// <summary>
    /// Quire for 64-bit Posit values
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Quire64
    {
        private fixed ulong m_value[32]; // 2048-bits

        public static Quire64 Create()
        {
            return new Quire64();
        }

        public void Clear()
        {
            for (var i = 0; i < 32; ++i)
                m_value[i] = 0;
        }

        public Posit64 ToPosit()
        {
            return (Posit64)this;
        }

        // TODO: add more operators
    }
}