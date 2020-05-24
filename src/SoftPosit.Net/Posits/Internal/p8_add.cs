// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t p8_add(in posit8_t a, in posit8_t b)
        {
            if (Posit.IsNaR(a) || Posit.IsNaR(b))
            {
                return Posit8.NaR;
            }

            if (Posit.IsZero(a) || Posit.IsZero(b))
            {   // Not required but put here for speed
                return new Posit8((byte)(a.ui | b.ui));
            }

            // different signs
            if (((a.ui ^ b.ui) & Posit8.SignMask) != 0)
                return softposit_subMagsP8(a, b);
            else
                return softposit_addMagsP8(a, b);
        }
    }
}
