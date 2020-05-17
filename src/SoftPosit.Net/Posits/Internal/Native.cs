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

    internal static partial class Native
    {
        private const string SoftPosit = "SoftPosit.x86.dll";

        #region Posit (nbits=8, es=0)

        static bool signP8UI(int x) => (x & Posit8.SignMask) != 0;
        static bool signregP8UI(int x) => (x & (Posit8.SignMask >> 1)) != 0;
        static byte packToP8UI(int regime, int fracA) => (byte)((byte)regime + (byte)fracA);


        public static posit8_t p8_mul(in posit8_t a, in posit8_t b) => throw new NotImplementedException();

        public static posit8_t p8_div(in posit8_t a, in posit8_t b) => throw new NotImplementedException();

        public static posit8_t p8_mod(in posit8_t a, in posit8_t b) => throw new NotImplementedException();

        public static posit8_t p8_sqrt(in posit8_t a) => throw new NotImplementedException();


        public static posit8_t i64_to_p8(in long value) => throw new NotImplementedException();

        public static posit8_t ui64_to_p8(in ulong value) => throw new NotImplementedException();

        public static posit8_t f32_to_p8(in float value) => ConvertDoubleToP8(value);

        public static posit8_t f64_to_p8(in double value) => ConvertDoubleToP8(value);

        [DllImport(SoftPosit, EntryPoint = "convertDoubleToP8")]
        public static extern posit8_t ConvertDoubleToP8(in double value);


        public static long p8_to_i64(in posit8_t value) => throw new NotImplementedException();

        public static ulong p8_to_ui64(in posit8_t value) => throw new NotImplementedException();

        public static float p8_to_f32(in posit8_t value) => (float) ConvertP8ToDouble(value);

        public static double p8_to_f64(in posit8_t value) => ConvertP8ToDouble(value);

        [DllImport(SoftPosit, EntryPoint = "convertP8ToDouble")]
        private static extern double ConvertP8ToDouble(in posit8_t value);


        public static posit8_t p16_to_p8(in posit16_t value) => throw new NotImplementedException();

        public static posit8_t p32_to_p8(in posit32_t value) => throw new NotImplementedException();

        public static posit8_t p64_to_p8(in posit64_t value) => throw new NotImplementedException();


        [DllImport(SoftPosit)]
        public static extern posit8_t q8_to_p8(in quire8_t q);

        public static quire8_t q8_fdp_add(quire8_t q, in posit8_t a, in posit8_t b) => throw new NotImplementedException();

        public static quire8_t q8_fdp_sub(quire8_t q, in posit8_t a, in posit8_t b) => throw new NotImplementedException();

        #endregion

        #region Posit (nbits=16, es=1)

        public static posit16_t p16_add(in posit16_t a, in posit16_t b) => throw new NotImplementedException();

        public static posit16_t p16_sub(in posit16_t a, in posit16_t b) => throw new NotImplementedException();

        public static posit16_t p16_mul(in posit16_t a, in posit16_t b) => throw new NotImplementedException();

        public static posit16_t p16_div(in posit16_t a, in posit16_t b) => throw new NotImplementedException();

        public static posit16_t p16_mod(in posit16_t a, in posit16_t b) => throw new NotImplementedException();

        public static posit16_t p16_sqrt(in posit16_t a) => throw new NotImplementedException();


        public static posit16_t i32_to_p16(in int value) => throw new NotImplementedException();

        public static posit16_t ui32_to_p16(in uint value) => throw new NotImplementedException();

        public static posit16_t i64_to_p16(in long value) => throw new NotImplementedException();

        public static posit16_t ui64_to_p16(in ulong value) => throw new NotImplementedException();

        public static posit16_t f32_to_p16(in float value) => ConvertDoubleToP16(value);

        public static posit16_t f64_to_p16(in double value) => ConvertDoubleToP16(value);

        [DllImport(SoftPosit, EntryPoint = "convertDoubleToP16")]
        private static extern posit16_t ConvertDoubleToP16(in double value);


        public static int p16_to_i32(in posit16_t value) => throw new NotImplementedException();

        public static uint p16_to_ui32(in posit16_t value) => throw new NotImplementedException();

        public static long p16_to_i64(in posit16_t value) => throw new NotImplementedException();

        public static ulong p16_to_ui64(in posit16_t value) => throw new NotImplementedException();

        public static float p16_to_f32(in posit16_t value) => (float)ConvertP16ToDouble(value);

        public static double p16_to_f64(in posit16_t value) => ConvertP16ToDouble(value);

        [DllImport(SoftPosit, EntryPoint = "convertP16ToDouble")]
        private static extern double ConvertP16ToDouble(in posit16_t value);


        public static posit16_t p8_to_p16(in posit8_t value) => throw new NotImplementedException();

        public static posit16_t p32_to_p16(in posit32_t value) => throw new NotImplementedException();

        public static posit16_t p64_to_p16(in posit64_t value) => throw new NotImplementedException();


        [DllImport(SoftPosit)]
        public static extern posit16_t q16_to_p16(in quire16_t q);

        public static quire16_t q16_fdp_add(quire16_t q, in posit16_t a, in posit16_t b) => throw new NotImplementedException();

        public static quire16_t q16_fdp_sub(quire16_t q, in posit16_t a, in posit16_t b) => throw new NotImplementedException();

        #endregion

        #region Posit (nbits=32, es=2)

        public static posit32_t p32_add(in posit32_t a, in posit32_t b) => throw new NotImplementedException();

        public static posit32_t p32_sub(in posit32_t a, in posit32_t b) => throw new NotImplementedException();

        public static posit32_t p32_mul(in posit32_t a, in posit32_t b) => throw new NotImplementedException();

        public static posit32_t p32_div(in posit32_t a, in posit32_t b) => throw new NotImplementedException();

        public static posit32_t p32_mod(in posit32_t a, in posit32_t b) => throw new NotImplementedException();

        public static posit32_t p32_sqrt(in posit32_t a) => throw new NotImplementedException();


        public static posit32_t i32_to_p32(in int value) => throw new NotImplementedException();

        public static posit32_t ui32_to_p32(in uint value) => throw new NotImplementedException();

        public static posit32_t i64_to_p32(in long value) => throw new NotImplementedException();

        public static posit32_t ui64_to_p32(in ulong value) => throw new NotImplementedException();

        public static posit32_t f32_to_p32(in float value) => ConvertDoubleToP32(value);

        public static posit32_t f64_to_p32(in double value) => ConvertDoubleToP32(value);

        [DllImport(SoftPosit, EntryPoint = "convertDoubleToP32")]
        private static extern posit32_t ConvertDoubleToP32(in double value);


        public static int p32_to_i32(in posit32_t value) => throw new NotImplementedException();

        public static uint p32_to_ui32(in posit32_t value) => throw new NotImplementedException();

        public static long p32_to_i64(in posit32_t value) => throw new NotImplementedException();

        public static ulong p32_to_ui64(in posit32_t value) => throw new NotImplementedException();

        public static float p32_to_f32(in posit32_t value) => (float)ConvertP32ToDouble(value);

        public static double p32_to_f64(in posit32_t value) => ConvertP32ToDouble(value);

        [DllImport(SoftPosit, EntryPoint = "convertP32ToDouble")]
        public static extern double ConvertP32ToDouble(in posit32_t value);


        public static posit32_t p8_to_p32(in posit8_t value) => throw new NotImplementedException();

        public static posit32_t p16_to_p32(in posit16_t value) => throw new NotImplementedException();

        public static posit32_t p64_to_p32(in posit64_t value) => throw new NotImplementedException();


        [DllImport(SoftPosit)]
        public static extern posit32_t q32_to_p32(in quire32_t q);

        public static quire32_t q32_fdp_add(quire32_t q, in posit32_t a, in posit32_t b) => throw new NotImplementedException();

        public static quire32_t q32_fdp_sub(quire32_t q, in posit32_t a, in posit32_t b) => throw new NotImplementedException();

        #endregion

        #region Posit (nbits=64, es=3)

        public static posit64_t p64_add(in posit64_t a, in posit64_t b) => throw new NotImplementedException();

        public static posit64_t p64_sub(in posit64_t a, in posit64_t b) => throw new NotImplementedException();

        public static posit64_t p64_mul(in posit64_t a, in posit64_t b) => throw new NotImplementedException();

        public static posit64_t p64_div(in posit64_t a, in posit64_t b) => throw new NotImplementedException();

        public static posit64_t p64_mod(in posit64_t a, in posit64_t b) => throw new NotImplementedException();

        public static posit64_t p64_sqrt(in posit64_t a) => throw new NotImplementedException();


        public static posit64_t i32_to_p64(in int value) => throw new NotImplementedException();

        public static posit64_t ui32_to_p64(in uint value) => throw new NotImplementedException();

        public static posit64_t i64_to_p64(in long value) => throw new NotImplementedException();

        public static posit64_t ui64_to_p64(in ulong value) => throw new NotImplementedException();

        public static posit64_t f32_to_p64(in float value) => ConvertDoubleToP64(value);

        public static posit64_t f64_to_p64(in double value) => ConvertDoubleToP64(value);

        [DllImport(SoftPosit, EntryPoint = "convertDoubleToP64")]
        private static extern posit64_t ConvertDoubleToP64(in double value);


        public static int p64_to_i32(in posit64_t value) => throw new NotImplementedException();

        public static uint p64_to_ui32(in posit64_t value) => throw new NotImplementedException();

        public static long p64_to_i64(in posit64_t value) => throw new NotImplementedException();

        public static ulong p64_to_ui64(in posit64_t value) => throw new NotImplementedException();

        public static float p64_to_f32(in posit64_t value) => (float)ConvertP64ToDouble(value);

        public static double p64_to_f64(in posit64_t value) => ConvertP64ToDouble(value);

        [DllImport(SoftPosit, EntryPoint = "convertP64ToDouble")]
        private static extern double ConvertP64ToDouble(in posit64_t value);


        public static posit64_t p8_to_p64(in posit8_t value) => throw new NotImplementedException();

        public static posit64_t p16_to_p64(in posit16_t value) => throw new NotImplementedException();

        public static posit64_t p32_to_p64(in posit32_t value) => throw new NotImplementedException();


        public static posit64_t q64_to_p64(in quire64_t q) => throw new NotImplementedException();

        public static quire64_t q64_fdp_add(quire64_t q, in posit64_t a, in posit64_t b) => throw new NotImplementedException();

        public static quire64_t q64_fdp_sub(quire64_t q, in posit64_t a, in posit64_t b) => throw new NotImplementedException();

        #endregion
    }
}
