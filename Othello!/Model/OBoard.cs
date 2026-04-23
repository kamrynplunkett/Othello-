using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello_.Model
{
    internal class OBoard
    {
        private OCell[,] Cells = new OCell[8, 8];

        public OBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Cells[i, j] = new OCell();
                }
            }
            Cells[3, 3].Place(CellState.White);
            Cells[4, 4].Place(CellState.White);
            Cells[3, 4].Place(CellState.Black);
            Cells[4, 3].Place(CellState.Black);
        }

        public OBoard(string OBState)
        {
            if (string.IsNullOrWhiteSpace(OBState) || OBState.Trim().Length != 64)
                throw new ArgumentException("Invalid board state string. Must be exactly 64 characters long.");

            OBState = OBState.Trim().ToUpper();


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    char c = OBState[i * 8 + j];
                    if (c == 'E')
                        Cells[i, j] = new OCell();
                    else if (c == 'B')
                    {
                        Cells[i, j] = new OCell();
                        Cells[i, j].Place(CellState.Black);
                    }
                    else if (c == 'W')
                    {
                        Cells[i, j] = new OCell();
                        Cells[i, j].Place(CellState.White);
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid character '{c}' in board state string. Use 'E' for empty, 'B' for black, and 'W' for white.");
                    }
                }
            }
        }

        public static String Serialize(OBoard Board)
        {
            String BState = "";
            foreach (OCell cell in Board.Cells)
            {
                if (cell.GetState() == CellState.Empty)
                {
                    BState += 'E';
                }
                else if (cell.GetState() == CellState.Black)
                {
                    BState += 'B';
                }
                else if (cell.GetState() == CellState.White)
                {
                    BState += 'W';
                }
            }
            return BState;
        }

        public static OBoard Deserialize(string OBState)
        {
            if (string.IsNullOrWhiteSpace(OBState) || OBState.Trim().Length != 64)
                throw new ArgumentException("Invalid board state string. Must be exactly 64 characters long.");

            OBState = OBState.Trim().ToUpper();
            OBoard board = new OBoard();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    char c = OBState[i * 8 + j];

                    board.Cells[i, j] = new OCell();

                    if (c == 'B')
                    {
                        board.Cells[i, j].Place(CellState.Black);
                    }
                    else if (c == 'W')
                    {
                        board.Cells[i, j].Place(CellState.White);
                    }
                    else if (c != 'E')
                    {
                        throw new ArgumentException($"Invalid character '{c}' in board state string. Use 'E' for empty, 'B' for black, and 'W' for white.");
                    }
                }
            }

            return board; 
        }

        public List<int> GetMoves(CellState player)
        {
            if (player == CellState.Empty)
                throw new ArgumentException("Player cannot be empty.");
            List<int> moves = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Cells[i, j].GetState() == CellState.Empty && IsValidMove(i, j, player))
                    {
                        moves.Add(i * 8 + j);
                    }
                }
            }
            return moves;
        }

        public bool IsValidMove(int x, int y, CellState player)
        {
            if (x < 0 || x >= 8 || y < 0 || y >= 8)
                return false;

            if (Cells[x, y].GetState() != CellState.Empty)
                return false;

            if (player == CellState.Empty)
                throw new ArgumentException("Player cannot be empty.");

            CellState opponent = (player == CellState.Black) ? CellState.White : CellState.Black;

            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int dir = 0; dir < 8; dir++)
            {
                int nx = x + dx[dir];
                int ny = y + dy[dir];
                bool hasOpponentBetween = false;
                while (nx >= 0 && nx < 8 && ny >= 0 && ny < 8)
                {
                    if (Cells[nx, ny].GetState() == opponent)
                    {
                        hasOpponentBetween = true;
                    }
                    else if (Cells[nx, ny].GetState() == player)
                    {
                        if (hasOpponentBetween)
                            return true;
                        else
                            break;
                    }
                    else
                    {
                        break;
                    }
                    nx += dx[dir];
                    ny += dy[dir];
                }
            }
            return false;
        }

        public CellState GetCellState(int row, int col)
        {
            return Cells[row, col].GetState();
        }

        public bool ApplyMove(int x, int y, CellState player)
        {
            if (!IsValidMove(x, y, player))
                return false;

            CellState opponent = (player == CellState.Black) ? CellState.White : CellState.Black;
            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            Cells[x, y].Place(player);

            for (int dir = 0; dir < 8; dir++)
            {
                int nx = x + dx[dir];
                int ny = y + dy[dir];
                List<OCell> toFlip = new List<OCell>();

                while (nx >= 0 && nx < 8 && ny >= 0 && ny < 8)
                {
                    if (Cells[nx, ny].GetState() == opponent)
                    {
                        toFlip.Add(Cells[nx, ny]);
                    }
                    else if (Cells[nx, ny].GetState() == player)
                    {
                        if (toFlip.Count > 0)
                        {
                            foreach (OCell cell in toFlip)
                            {
                                cell.Flip();
                            }
                        }
                        break;
                    }
                    else
                    {
                        break;
                    }

                    nx += dx[dir];
                    ny += dy[dir];
                }
            }

            return true;
        }

        public int CountPieces(CellState player)
        {
            int count = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Cells[i, j].GetState() == player)
                        count++;
                }
            }

            return count;
        }

        public bool IsFull()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Cells[i, j].GetState() == CellState.Empty)
                        return false;
                }
            }

            return true;
        }

        public bool HasAnyMoves(CellState player)
        {
            return GetMoves(player).Count > 0;
        }
    }
}
