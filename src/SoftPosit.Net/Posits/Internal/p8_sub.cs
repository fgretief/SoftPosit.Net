// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t p8_sub(in posit8_t a, in posit8_t b)
        {
            //infinity
            if (Posit.IsNaR(a) || Posit.IsNaR(b))
            {
                return Posit8.NaR;
            }

            //Zero
            if (Posit.IsZero(a) || Posit.IsZero(b))
            {
                return new Posit8((byte)(a.ui | -(sbyte)b.ui));
            }

            //different signs
            if (signP8UI(a.ui ^ b.ui))
                return softposit_addMagsP8(a.ui, (byte)(-(sbyte)b.ui));
            else
                return softposit_subMagsP8(a.ui,  (byte)(-(sbyte)b.ui));
        }
    }
}
