using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Model
{
    internal class Field
    {
        private ushort value;
        public ushort Value { get { return value; } set { this.value = value; } }

        private bool solved;
        public bool Solved { get { return solved; } set { solved = value; } }

        private HashSet<ushort> candidates = new HashSet<ushort>();
        public HashSet<ushort> Candidates { get { return candidates; } }

        public Field()
        {
            value = 0;
            solved = false;
            candidates = new HashSet<ushort>();
        }

        public Field(ushort value)
        {
            this.value = value;
            solved = true;
            candidates.Clear();
        }

    }
}
