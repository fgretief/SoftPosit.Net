// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t p8_add(in posit8_t a, in posit8_t b)
        {
            // zero or infinity
            if (a.ui == 0 || b.ui == 0)
            { // Not required but put here for speed
                return new Posit8((byte)(a.ui | b.ui));
            }
            else if (a.ui == Posit8.SignMask || b.ui == Posit8.SignMask)
            {
                return Posit8.NaR;
            }

            // different signs
            if (((a.ui ^ b.ui) & Posit8.SignMask) != 0)
                return softposit_subMagsP8(a, b);
            else
                return softposit_addMagsP8(a, b);
        }
    }
}
