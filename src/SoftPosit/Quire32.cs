// SPDX-License-Identifier: MIT
using System.Runtime.InteropServices;

namespace System.Numerics
{
    /// <summary>
    /// Quire for 32-bit Posit values
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Quire32
    {
        private fixed ulong m_value[8]; // 512-bits

        public static Quire32 Create()
        {
            return new Quire32();
        }

        public void Clear()
        {
            for (var i = 0; i < 8; ++i)
                m_value[i] = 0;
        }

        public Posit32 ToPosit()
        {
            return (Posit32)this;
        }

        // TODO: add more operators
    }
}