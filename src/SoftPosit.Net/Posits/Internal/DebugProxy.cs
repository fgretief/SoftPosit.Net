// SPDX-License-Identifier: MIT

using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;

namespace System.Numerics.Posits.Internal
{
    // ReSharper disable InconsistentNaming

    internal class DebugProxy
    {
        private readonly ulong ui;
        private readonly int nbits;
        private readonly int max_es;
        private readonly int useed;

        private readonly ulong signMask;
        private readonly int signShift;

        public DebugProxy(ulong ui, int nbits, int max_es)
        {
            if (nbits < 2)
                throw new ArgumentException("Must greater or equal to 2.", nameof(nbits));
            if (max_es < 0)
                throw new ArgumentException("Must be non-negative.", nameof(max_es));
            Contract.EndContractBlock();
            this.ui = ui;
            this.nbits = nbits;
            this.max_es = max_es;
            this.useed = 1 << (1 << max_es);
            this.signShift = nbits - 1;
            this.signMask = (1ul << signShift);
        }

        public DebugProxy(Posit8 value)
            : this(value.ui, Posit8.nbits, Posit8.es)
        {}

        public DebugProxy(Posit16 value)
            : this(value.ui, Posit16.nbits, Posit16.es)
        {}

        public DebugProxy(Posit32 value)
            : this(value.ui, Posit32.nbits, Posit32.es)
        {}

        public DebugProxy(Posit64 value)
            : this(value.ui, Posit64.nbits, Posit64.es)
        {}

        public enum SignEnum { Positive, Negative }
        public SignEnum Sign => (ui & signMask) == 0 ? SignEnum.Positive : SignEnum.Negative;
        //public char Sign => (ui & signMask) == 0 ? '+' : '-';
        //public int Sign => (ui >> signShift) & 1;

        public string DebugView
        {
            get
            {
                return ui == 0 ? "0"
                    : ui == signMask ? "±∞"
                    : $"0x{ui:X}";
            }
        }
    }
}