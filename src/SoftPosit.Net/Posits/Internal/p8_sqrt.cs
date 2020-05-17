﻿// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        static readonly byte[] p8Sqrt =
        {
            0, 8, 11, 14, 16, 18, 20, 21, 23, 24, 25, 27, 28, 29, 30, 31, 32,
            33, 34, 35, 36, 37, 38, 38, 39, 40, 41, 42, 42, 43, 44, 45, 45, 46,
            47, 47, 48, 49, 49, 50, 51, 51, 52, 52, 53, 54, 54, 55, 55, 56, 57,
            57, 58, 58, 59, 59, 60, 60, 61, 61, 62, 62, 63, 63, 64, 64, 65, 65,
            66, 66, 67, 67, 68, 68, 69, 69, 70, 70, 70, 71, 71, 72, 72, 72, 73,
            73, 74, 74, 74, 75, 75, 75, 76, 76, 77, 77, 77, 79, 80, 81, 83, 84,
            85, 86, 87, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 100,
            101, 102, 103, 105, 108, 110, 112, 114, 115, 120
        };

        public static posit8_t p8_sqrt(in posit8_t pA)
        {
            var uA = pA.ui < 0x80 ? p8Sqrt[pA.ui] : 0x80;
            return new Posit8((byte)uA);
        }
    }
}
