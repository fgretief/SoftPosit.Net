// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    // ReSharper disable CompareOfFloatsByEqualityOperator

    internal static partial class Native
    {
        public static posit8_t f32_to_p8(in float value)
        {
            if (value == 0)
                return Posit8.Zero;
            if (float.IsNaN(value) || float.IsInfinity(value))
                return Posit8.NaR;

            float f32 = value;

            var sign = float.IsNegative(f32);
            if (sign)
            {
                f32 = -f32;
            }

            byte p8;

            if (f32 == 1)
            {
                p8 = 0x40;
            }
            else if (f32 >= 64)
            {
                Debug.Assert(!float.IsInfinity(f32));
                p8 = 0x7F; // maxpos
            }
            else if (f32 <= 0.015625)
            {
                p8 = 0x01; // minpos
            }
            else if (f32 < 1)
            {
                // regime
                var reg = 0;
                while (f32 < 1)
                {
                    f32 *= 2;
                    reg++;
                }

                // rounding off regime bits
                if (reg > 6)
                {
                    p8 = 0x01; // minpos
                }
                else
                {
                    sbyte fracLength = (sbyte)(6 - reg);
                    var frac = convertFractionP8(f32, fracLength, out var bitNPlusOne, out var bitsMore);
                    byte regime = (byte)(0x40 >> reg);
                    p8 = packToP8UI(regime, frac);
                    if (bitNPlusOne)
                    {
                        p8 += (byte)((p8 & 1) | (bitsMore ? 1 : 0));
                    }
                }
            }
            else if (f32 > 1)
            {
                // regime
                var reg = 1; //because k = m-1; so need to add back 1
                while (f32 >= 2)
                {
                    f32 *= 0.5f;
                    reg++;
                }

                // rounding off regime bits
                if (reg > 6)
                {
                    p8 = 0x7F; // maxpos
                }
                else
                {
                    sbyte fracLength = (sbyte)(6 - reg);
                    var frac = convertFractionP8(f32, fracLength, out var bitNPlusOne, out var bitsMore);
                    byte regime = (byte)(0x7F - (0x7F >> reg));
                    p8 = packToP8UI(regime, frac);
                    if (bitNPlusOne)
                    {
                        p8 += (byte)((p8 & 1) | (bitsMore ? 1 : 0));
                    }
                }
            }
            else
            {
                p8 = 0x7F; // NaR
                sign = false;
            }

            if (sign)
            {
                //p8 = (byte)(-(sbyte)p8 & 0xFF);
                p8 = (byte)(~p8 + 1);
            }

            return new Posit8(p8);
        }
    }
}
