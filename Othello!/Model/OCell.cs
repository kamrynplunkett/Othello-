using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_.Model
{
    enum CellState
    {
        Empty,
        Black,
        White
    }
    internal class OCell
    {
        private CellState State;
        
        public OCell()
        {
            State = CellState.Empty;
        }

        public CellState GetState()
        {
            return State;
        }

        public void Place(CellState state)
        {
            State = state;
        }

        public void Flip()
        {
            if (State == CellState.Black)
                State = CellState.White;
            else if (State == CellState.White)
                State = CellState.Black;
            else if (State == CellState.Empty)
                throw new InvalidOperationException("Cannot flip an empty cell. BAD DEV:(");
        }
    }
}
