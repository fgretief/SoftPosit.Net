// SPDX-License-Identifier: MIT
using System.Runtime.InteropServices;

namespace System.Numerics
{
    /// <summary>
    /// Quire for 8-bit Posit values
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Quire8
    {
        private uint m_value; // 32-bits

        public static Quire8 Create()
        {
            return new Quire8();
        }

        public void Clear()
        {
            m_value = 0;
        }

        // TODO: add operators
    }
}