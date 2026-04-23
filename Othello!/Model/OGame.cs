using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_.Model
{
    internal class OGame
    {
        public OBoard Board { get; private set; }
        public CellState CurrentPlayer { get; private set; }

        public OGame()
        {
            Board = new OBoard();
            CurrentPlayer = CellState.Black;
        }

        public void ResetGame()
        {
            Board = new OBoard();
            CurrentPlayer = CellState.Black;
        }

        public bool PlayMove(int x, int y)
        {
            if (!Board.ApplyMove(x, y, CurrentPlayer))
                return false;

            SwitchTurn();

            if (!Board.HasAnyMoves(CurrentPlayer))
            {
                SwitchTurn();

                if (!Board.HasAnyMoves(CurrentPlayer))
                {
                    // neither player has a move. IsGameOver() catches
                }
            }

            return true;
        }

        public void SwitchTurn()
        {
            if (CurrentPlayer == CellState.Black)
            {
                CurrentPlayer = CellState.White;
            }
            else
            {
                CurrentPlayer = CellState.Black;
            }
        }

        public bool IsGameOver()
        {
            return Board.IsFull() ||
                   (!Board.HasAnyMoves(CellState.Black) && !Board.HasAnyMoves(CellState.White));
        }

        public int GetBlackScore()
        {
            return Board.CountPieces(CellState.Black);
        }

        public int GetWhiteScore()
        {
            return Board.CountPieces(CellState.White);
        }

        public CellState GetWinner()
        {
            int black = GetBlackScore();
            int white = GetWhiteScore();

            if (black > white)
            {
                return CellState.Black;
            }
            else if (white > black)
            {
                return CellState.White;
            }
            else
            {
                return CellState.Empty;
            }
        }

        public int GetScore(CellState player)
        {
            return Board.CountPieces(player);
        }
    }
}
