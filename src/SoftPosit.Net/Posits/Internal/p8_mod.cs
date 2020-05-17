// SPDX-License-Identifier: MIT

namespace System.Numerics.Posits.Internal
{
    using posit8_t = Posit8;

    internal static partial class Native
    {
        public static posit8_t p8_mod(in posit8_t a, in posit8_t b)
        {
            if (Posit.IsNaR(a) || Posit.IsZeroOrNaR(b))
            {
                return Posit8.NaR;
            }
            else if (Posit.IsZero(a) || a.ui == b.ui)
            {
                return Posit8.Zero;
            }

            throw new NotImplementedException();
        }
    }
}
