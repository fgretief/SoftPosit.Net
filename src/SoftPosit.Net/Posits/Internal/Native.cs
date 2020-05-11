using System.Runtime.InteropServices;

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

        [DllImport(SoftPosit)]
        public static extern posit8_t q8_to_p8(quire8_t q);

        [DllImport(SoftPosit)]
        public static extern posit16_t q16_to_p16(quire16_t q);

        [DllImport(SoftPosit)]
        public static extern posit32_t q32_to_p32(quire32_t q);

        public static posit64_t q64_to_p64(quire64_t q) => throw new NotImplementedException();
    }
}
