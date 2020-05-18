// SPDX-License-Identifier: MIT

using System.Diagnostics;

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    // ReSharper disable CompareOfFloatsByEqualityOperator

    internal static partial class Native
    {
        public static posit8_t f64_to_p8(in double value)
        {
            return ConvertDoubleToP8(value);
        }

        static posit8_t ConvertDoubleToP8(in double value)
        {
            double f8 = value; // f8 or f64?

            var sign = value < 0;

            // sign: 1 bit, frac: 8 bits, mantisa: 23 bits
            //sign = a.parts.sign;
            //frac = a.parts.fraction;
            //exp = a.parts.exponent;

            if (f8 == 0)
            {
                return Posit8.Zero;
            }
            else if (double.IsInfinity(f8) || double.IsNaN(f8))
            //else if(f8 == INFINITY || f8 == -INFINITY || f8 == NAN)
            {
                return Posit8.NaR;
            }
            else if (f8 == 1)
            {
                return Posit8.One;
            }
            else if (f8 == -1)
            {
                return Posit8.MinusOne;
            }
            else if (f8 >= 64)
            {
                return new Posit8(0x7F); // +maxpos
            }
            else if (f8 <= -64)
            {
                return new Posit8(0x81); // -maxpos
            }
            else if (f8 <= 0.015625 && !sign)
            //else if (0 < f8 && f8 <= 0.015625)
            {
                return new Posit8(0x01); // +minpos
            }
            else if (f8 >= -0.015625 && sign)
            //else if (0 > f8 && f8 >= -0.015625)
            {
                return new Posit8(0xFF); // -minpos
            }
            else if (f8 > 1 || f8 < -1)
            {
                if (sign)
                {
                    //Make negative numbers positive for easier computation
                    f8 = -f8;
                }

                byte uZ_ui;
                var reg = 1; //because k = m-1; so need to add back 1

                // minpos
                if (f8 <= 0.015625)
                {
                    uZ_ui = 1;
                }
                else
                {
                    //regime
                    while (f8 >= 2)
                    {
                        f8 *= 0.5;
                        reg++;
                    }

                    //rounding off regime bits
                    if (reg > 6)
                    {
                        uZ_ui = 0x7F;
                    }
                    else
                    {
                        sbyte fracLength = (sbyte)(6 - reg);
                        var frac = convertFractionP8(f8, fracLength, out var bitNPlusOne, out var bitsMore);
                        byte regime = (byte)(0x7F - (0x7F >> reg));
                        uZ_ui = packToP8UI(regime, frac);
                        if (bitNPlusOne)
                        {
                            uZ_ui += (byte)((uZ_ui & 1) | (bitsMore?1:0));
                        }
                    }

                    if (sign)
                    {
                        uZ_ui = (byte)(-(sbyte)uZ_ui & 0xFF);
                    }
                }

                return new Posit8(uZ_ui);
            }
            else if (f8 < 1 || f8 > -1)
            {
                if (sign)
                {
                    //Make negative numbers positive for easier computation
                    f8 = -f8;
                }

                byte uZ_ui;
                var reg = 0;

                //regime
                while (f8 < 1)
                {
                    f8 *= 2;
                    reg++;
                }
                //rounding off regime bits
                if (reg > 6)
                {
                    uZ_ui = 0x1;
                }
                else
                {
                    sbyte fracLength = (sbyte)(6 - reg);
                    var frac = convertFractionP8(f8, fracLength, out var bitNPlusOne, out var bitsMore);
                    byte regime = (byte)(0x40 >> reg);
                    uZ_ui = packToP8UI(regime, frac);
                    if (bitNPlusOne)
                    {
                        uZ_ui += (byte)((uZ_ui & 1) | (bitsMore?1:0));
                    }
                }

                if (sign)
                {
                    uZ_ui = (byte)(-(sbyte)uZ_ui & 0xFF);
                }
                return new Posit8(uZ_ui);
            }
            else
            {
                //NaR - for NaN, INF and all other combinations
                return Posit8.NaR;
            }
        }

        static ushort convertFractionP8(double f8, sbyte fracLength, out bool bitsNPlusOne, out bool bitsMore)
        {
            bitsNPlusOne = bitsMore = false;

            byte frac = 0;

            if (f8 == 0)
            {
                return 0;
            }
            else if (double.IsInfinity(f8))
            {
                return 0x80;
            }

            f8 -= 1; //remove hidden bit
            if (fracLength == 0)
            {
                checkExtraTwoBitsP8(f8, 1.0, ref bitsNPlusOne, ref bitsMore);
            }
            else
            {
                double temp = 1;
                while (true)
                {
                    temp /= 2;
                    if (temp <= f8)
                    {
                        f8 -= temp;
                        fracLength--;
                        frac = (byte)((frac << 1) + 1); //shift in one
                        if (f8 == 0)
                        {
                            //put in the rest of the bits
                            frac <<= (byte)fracLength;
                            break;
                        }

                        if (fracLength == 0)
                        {
                            checkExtraTwoBitsP8(f8, temp, ref bitsNPlusOne, ref bitsMore);
                            break;
                        }
                    }
                    else
                    {
                        frac <<= 1; //shift in a zero
                        fracLength--;
                        if (fracLength == 0)
                        {
                            checkExtraTwoBitsP8(f8, temp, ref bitsNPlusOne, ref bitsMore);
                            break;
                        }
                    }
                }
            }

            //Console.WriteLine("convertfloat: frac:{0} f16: {1:f26}  bitsNPlusOne: {2}, bitsMore: {3}", frac, f16, bitsNPlusOne, bitsMore);

            return frac;
        }

        static void checkExtraTwoBitsP8(double f8, double temp, ref bool bitsNPlusOne, ref bool bitsMore)
        {
            temp /= 2;

            if (temp <= f8)
            {
                bitsNPlusOne = true;
                f8 -= temp;
            }

            if (f8 > 0)
            {
                bitsMore = true;
            }
        }
    }
}
