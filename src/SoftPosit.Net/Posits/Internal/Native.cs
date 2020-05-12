// SPDX-License-Identifier: MIT

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: InternalsVisibleTo("SoftPosit.Tests")]

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;
    using posit16_t = Posit16;
    using posit32_t = Posit32;
    using posit64_t = Posit64;

    using quire8_t = Quire8;
    using quire16_t = Quire16;
    using quire32_t = Quire32;
    using quire64_t = Quire64;

    internal static class Native
    {
        private const string SoftPosit = "SoftPosit.x86.dll";

        #region Posit (nbits=8, es=0)

        public static posit8_t p8_add(in posit8_t a, in posit8_t b) => throw new NotImplementedException();

        public static posit8_t p8_sub(in posit8_t a, in posit8_t b) => throw new NotImplementedException();

        public static posit8_t p8_mul(in posit8_t a, in posit8_t b) => throw new NotImplementedException();

        public static posit8_t p8_div(in posit8_t a, in posit8_t b) => throw new NotImplementedException();

        public static posit8_t p8_mod(in posit8_t a, in posit8_t b) => throw new NotImplementedException();

        #endregion

        #region Posit (nbits=16, es=1)

        public static posit16_t p16_add(in posit16_t a, in posit16_t b) => throw new NotImplementedException();

        public static posit16_t p16_sub(in posit16_t a, in posit16_t b) => throw new NotImplementedException();

        public static posit16_t p16_mul(in posit16_t a, in posit16_t b) => throw new NotImplementedException();

        public static posit16_t p16_div(in posit16_t a, in posit16_t b) => throw new NotImplementedException();

        public static posit16_t p16_mod(in posit16_t a, in posit16_t b) => throw new NotImplementedException();

        #endregion

        #region Posit (nbits=32, es=2)

        public static posit32_t p32_add(in posit32_t a, in posit32_t b) => throw new NotImplementedException();

        public static posit32_t p32_sub(in posit32_t a, in posit32_t b) => throw new NotImplementedException();

        public static posit32_t p32_mul(in posit32_t a, in posit32_t b) => throw new NotImplementedException();

        public static posit32_t p32_div(in posit32_t a, in posit32_t b) => throw new NotImplementedException();

        public static posit32_t p32_mod(in posit32_t a, in posit32_t b) => throw new NotImplementedException();

        #endregion

        #region Posit (nbits=64, es=3)

        public static posit64_t p64_add(in posit64_t a, in posit64_t b) => throw new NotImplementedException();

        public static posit64_t p64_sub(in posit64_t a, in posit64_t b) => throw new NotImplementedException();

        public static posit64_t p64_mul(in posit64_t a, in posit64_t b) => throw new NotImplementedException();

        public static posit64_t p64_div(in posit64_t a, in posit64_t b) => throw new NotImplementedException();

        public static posit64_t p64_mod(in posit64_t a, in posit64_t b) => throw new NotImplementedException();

        #endregion

        [DllImport(SoftPosit)]
        public static extern posit8_t q8_to_p8(in quire8_t q);

        [DllImport(SoftPosit)]
        public static extern posit16_t q16_to_p16(in quire16_t q);

        [DllImport(SoftPosit)]
        public static extern posit32_t q32_to_p32(in quire32_t q);

        public static posit64_t q64_to_p64(in quire64_t q) => throw new NotImplementedException();
    }
}
