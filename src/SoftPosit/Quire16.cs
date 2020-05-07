// SPDX-License-Identifier: MIT
using System.Runtime.InteropServices;

namespace System.Numerics
{
    /// <summary>
    /// Quire for 16-bit Posit values
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Quire16
    {
        private fixed ulong m_value[2]; // 128-bits

        public static Quire16 Create()
        {
            return new Quire16();
        }

        public void Clear()
        {
            for (var i = 0; i < 2; ++i)
                m_value[i] = 0;
        }

        public Posit16 ToPosit()
        {
            return (Posit16)this;
        }

        // TODO: add more operators
    }
}