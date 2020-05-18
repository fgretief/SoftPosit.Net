// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static float p8_to_f32(in posit8_t value)
        {
            return (float)ConvertP8ToDouble(value);
        }

        public static double p8_to_f64(in posit8_t value)
        {
            return ConvertP8ToDouble(value);
        }

        private static double ConvertP8ToDouble(in posit8_t a)
        {
            var uZ_ui = a.ui;

            if (uZ_ui == 0)
            {
                return 0;
            }
            else if (uZ_ui == 0x7F)
            {
                return 64; // maxpos
            }
            else if (uZ_ui == 0x81)
            {
                return -64; // -maxpos
            }
            else if (uZ_ui == 0x80)
            {
                return double.NaN; // NaR
            }

            const int signMask = Posit8.SignMask;

            var sign = signP8UI(uZ_ui);
            if (sign) uZ_ui = (byte)(-(sbyte)uZ_ui & 0xFF);
            var regS = signregP8UI(uZ_ui);

            int reg;
            sbyte k = 0;
            byte tmp = (byte)((uZ_ui << 2) & 0xFF);
            var shift = 2;
            if (regS)
            {
                while ((tmp & signMask) != 0)
                {
                    k++;
                    shift++;
                    tmp = (byte)((tmp << 1) & 0xFF);
                }
                reg = k + 1;
            }
            else
            {
                k = -1;
                while ((tmp & signMask) == 0)
                {
                    k--;
                    shift++;
                    tmp = (byte)((tmp << 1) & 0xFF);
                }
                tmp &= 0x7F;
                reg = -k;
            }
            var frac = (tmp & 0x7F) >> shift;

            var fraction_max = Math.Pow(2, 6 - reg);
            var d8 = (double)(Math.Pow(2, k) * (1 + ((double)frac / fraction_max)));
            return sign ? -d8 : d8;
        }
    }
}
