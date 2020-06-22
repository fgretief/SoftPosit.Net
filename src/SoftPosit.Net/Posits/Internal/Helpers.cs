using System.Diagnostics;
using System.Text;

namespace System.Numerics.Posits.Internal
{
    internal static class Helpers
    {
        public static string TopBits(this sbyte ui) => TopBits(ui, 8);
        public static string TopBits(this sbyte ui, int n) => TopBits(ui, n, n);
        public static string TopBits(this sbyte ui, int c, int n) => TopBits((ulong)ui, c, n);

        public static string TopBits(this byte ui) => TopBits(ui, 8);
        public static string TopBits(this byte ui, int n) => TopBits(ui, n, n);
        public static string TopBits(this byte ui, int c, int n) => TopBits((ulong)ui, c, n);

        public static string TopBits(this short ui) => TopBits(ui, 16);
        public static string TopBits(this short ui, int n) => TopBits(ui, n, n);
        public static string TopBits(this short ui, int c, int n) => TopBits((ulong)ui, c, n);

        public static string TopBits(this ushort ui) => TopBits(ui, 16);
        public static string TopBits(this ushort ui, int n) => TopBits(ui, n, n);
        public static string TopBits(this ushort ui, int c, int n) => TopBits((ulong)ui, c, n);

        public static string TopBits(this int ui) => TopBits(ui, 32);
        public static string TopBits(this int ui, int n) => TopBits(ui, n, n);
        public static string TopBits(this int ui, int c, int n) => TopBits((ulong)ui, n, n);

        public static string TopBits(this uint ui) => TopBits(ui, 32);
        public static string TopBits(this uint ui, int n) => TopBits(ui, n, n);
        public static string TopBits(this uint ui, int c, int n) => TopBits((ulong)ui, c, n);

        public static string TopBits(this long ui) => TopBits(ui, 64);
        public static string TopBits(this long ui, int n) => TopBits(ui, n, n);
        public static string TopBits(this long ui, int c, int n) => TopBits((ulong)ui, c, n);

        public static string TopBits(this ulong ui) => TopBits(ui, 64);
        public static string TopBits(this ulong ui, int n) => TopBits(ui, n, n);
        public static string TopBits(this ulong r, int c, int n)
        {
            if (!(0 < n && n <= 64))
                throw new ArgumentOutOfRangeException(nameof(n), n, "Must be less or equal to 64");
            if (!(0 < c && c <= n))
                throw new ArgumentOutOfRangeException(nameof(c), c, "Count must be less or equal to number of bits");

            var sb = new StringBuilder("0b");
            var m = 1ul << (n - 1);
            for (var i = 0; i < c; ++i, m >>= 1)
                sb.Append((r & m) != 0 ? '1' : '0');
            if (c < n) sb.Append("..");
            return sb.ToString();
        }

        public static string ToDecodedBits(this ulong value, int ps, int es, bool html = false)
        {
            if (!(2 < ps && ps <= 64))
                throw new ArgumentOutOfRangeException(nameof(ps), ps, "Must be less or equal to 64");
            if (es < 0)
                throw new ArgumentOutOfRangeException(nameof(es), es, "Must be a non-negative value");

            var signStyle = "color:#FF2000";
            var regimeStyle = "color:#CC9933";
            var regimeTermStyle = "color:#996633;text-decoration: overline";
            var exponentStyle = "color:#4080FF";
            var inlineStyle = true;

            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            var sb = new StringBuilder();
            //if (html) sb.AppendLine("<!DOCTYPE html><html><head>");
            //if (html) sb.AppendLine("<style>.sign {color:#FF2000} .regime {color:#CC9933} .regime_terminal {color:#996633;text-decoration:overline} .exponent{color:#4080FF} .fraction {}</style>");
            //if (html) sb.AppendLine("</head><body><pre>");

            var mask = 1ul << (ps - 1);
            var sign = (value & mask) != 0;
            var ui = sign ? ~value + 1 : value;

            // sign
            if (html) sb.Append($"<span class=\"sign\"{(inlineStyle ? $" style=\"{signStyle}\"" : null)}>");
            sb.Append(sign ? '-' : '+'); mask >>= 1;
            if (html) sb.Append("</span>");

            // regime
            if (html) sb.Append($"<span class=\"regime\"{(inlineStyle ? $" style =\"{regimeStyle}\"" : null)}>");
            var signOfRegime = (ui & mask) != 0;
            sb.Append(signOfRegime ? '1' : '0'); mask >>= 1;
            for (; mask != 0 && (((ui & mask) == 0) ^ signOfRegime); mask >>= 1)
                sb.Append((ui & mask) != 0 ? '1' : '0');
            if (html) sb.Append("</span>");

            if (mask != 0)
            {
                // regime (termination)
                if (html) sb.Append($"<span class=\"regime_terminal\"{(inlineStyle ? $" style =\"{regimeTermStyle}\"" : null)}>");
                sb.Append((ui & mask) != 0 ? '1' : '0'); mask >>= 1;
                if (html) sb.Append("</span>");
            }

            if (mask != 0)
            {
                // exponent
                if (html) sb.Append($"<span class=\"exponent\"{(inlineStyle ? $" style =\"{exponentStyle}\"" : null)}>");
                for (var ex = 0; ex < es && mask != 0; mask >>= 1, ++ex)
                    sb.Append((ui & mask) != 0 ? '1' : '0');
                if (html) sb.Append("</span>");
            }

            if (mask != 0)
            {
                // fraction
                if (html) sb.Append("<span class=\"fraction\">");
                for (; mask != 0; mask >>= 1)
                    sb.Append((ui & mask) != 0 ? '1' : '0');
                if (html) sb.Append("</span>");
            }

            //if (html) sb.AppendLine().AppendLine("</pre>");
            //if (html) sb.AppendLine("</body></html>");

            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            return sb.ToString();
        }

        public static string ToFormula(this ulong ui, int ps, int es)
        {
            if (!(2 < ps && ps <= 64))
                throw new ArgumentOutOfRangeException(nameof(ps), ps, "Must be less or equal to 64");
            if (es < 0)
                throw new ArgumentOutOfRangeException(nameof(es), es, "Must be a non-negative value");
            if (ui == 0)
                return "0";
            var signMask = 1ul << (ps - 1);
            if (ui == signMask)
                return "inf";
            var (sign, k, tmp) = ui.Deconstruct(signMask);
            var exp = (sbyte)((tmp >> (ps - 1 - es)) & ((1ul << es) - 1));
            var frac = (tmp << es) & (signMask - 1);
            return ToFormulaImpl(sign, k, exp, frac, ps, es);
        }

        public static string ToFormula(bool s, int k, sbyte e, ulong f, int ps, int es)
        {
            if (!(2 < ps && ps <= 64))
                throw new ArgumentOutOfRangeException(nameof(ps));
            if (es < 0)
                throw new ArgumentOutOfRangeException(nameof(es), es, "Must be a non-negative value");

            return ToFormulaImpl(s, k, e, f, ps, es);
        }

        private static string ToFormulaImpl(bool s, int k, sbyte e, ulong f, int ps, int es)
        {
            var useed = 1 << (1 << es);
            ulong fn = f, fd = 1ul << (ps - 1);
            while ((fn & 1) == 0 && fd > 1) { fn >>= 1; fd >>= 1; }
            return $"{(s ? '-' : '+')} {useed}^({k}) · 2^({e}) · (1 + {fn}/{fd})";
        }

        public static (bool, sbyte, ulong) Deconstruct(this ulong ui, int ps)
        {
            if (!(2 < ps && ps <= 64))
                throw new ArgumentOutOfRangeException(nameof(ps));

            return Deconstruct(ui, 1ul << (ps - 1));
        }

        public static (bool, sbyte, ulong) Deconstruct(this ulong ui, ulong signMask)
        {
            var sign = (ui & signMask) != 0;

            if ((ui & (signMask - 1)) == 0)
                throw new ArgumentOutOfRangeException(nameof(ui), ui, "Must be a non-zero");

            var tmp = sign ? (ulong)-(long)ui : ui;
            var signOfRegime = (tmp & (signMask >> 1)) != 0;

            sbyte k;
            tmp <<= 2;
            if (signOfRegime)
            {
                k = 0;
                while ((tmp & signMask) != 0)
                {
                    k++;
                    tmp <<= 1;
                }
            }
            else
            {
                k = -1;
                Debug.Assert(tmp != 0, "Zero cause infinite loop");
                while ((tmp & signMask) == 0)
                {
                    k--;
                    tmp <<= 1;
                }
                tmp ^= signMask;
            }

            return (sign, k, tmp);
        }

        public static string QuireMask(int nbits, string prefix = null)
        {
            int cs = nbits - 1; // carry size (in bits)
            int nq = (nbits * nbits) / 4 - nbits / 2;
            int qbits = 1 + cs + nq + nq; // number of bits in quire
            var sb = new StringBuilder();
            if (prefix != null) sb.Append(prefix);
            sb.Append('S');
            for (var i = 0; i < cs; i++)
                sb.Append('c'); // carry
            for (var i = 0; i < nq; i++)
                sb.Append('i'); // integer
            for (var i = 0; i < nq; i++)
                sb.Append('f'); // fragment
            Debug.Assert(sb.Length == (qbits + (prefix?.Length ?? 0)));
            return sb.ToString();
        }
    }
}
