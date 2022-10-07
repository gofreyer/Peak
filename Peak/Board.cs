using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peak
{
    class Position
    {
        private GameBoard MyBoard { set; get; }
        private int _x;
        public int X
        {
            get { return _x; }
            set
            {
                if (value < MyBoard.GetDim())
                {
                    _x = value;
                }
                else
                {
                    _x = -1;
                }
            }
        }
        private int _y;
        public int Y
        {
            get { return _y; }
            set
            {
                if (value < MyBoard.GetDim())
                {
                    _y = value;
                }
                else
                {
                    _y = -1;
                }
            }
        }
        public bool IsValid()
        {
            return (X >= 0 && X < MyBoard.GetDim() && Y >= 0 && Y < MyBoard.GetDim());
        }
        public Position(int y, int x, GameBoard g)
        {
            MyBoard = g;
            X = x;
            Y = y;
        }
    }
    class Move
    {
        public enum Direction
        {
            NONE,
            INVALID,
            N,E,W,S,NE,NW,SE,SW
        }
        private GameBoard MyBoard { set; get; }
        public Position From { get; set; }
        public Position To { get; set; }
        public Move(Position from, Position to, GameBoard g)
        {
            MyBoard = g;
            From = from;
            To = to;
        }
        public bool IsValid()
        {
            Direction dir = GetDirection();
            if (dir == Direction.INVALID || dir == Direction.NONE)
            {
                return false;
            }
            int intermediateDistance = MyBoard.GetIntermediateDistance(From, To, dir);
            
            return (intermediateDistance==2);
        }
        public Direction GetDirection()
        {
            if (!From.IsValid()
                || !To.IsValid()
                || (From.X != To.X && From.Y != To.Y && Math.Abs(From.X - To.X) != Math.Abs(From.Y - To.Y))) return Direction.INVALID;
            else if (From == To) return Direction.NONE;
            else if (From.Y == To.Y && From.X < To.X) return Direction.E;
            else if (From.Y == To.Y && From.X > To.X) return Direction.W;
            else if (From.X == To.X && From.Y < To.Y) return Direction.S;
            else if (From.X == To.X && From.Y > To.Y) return Direction.N;
            else if (From.X < To.X && From.Y > To.Y) return Direction.NE;
            else if (From.X > To.X && From.Y > To.Y) return Direction.NW;
            else if (From.X < To.X && From.Y < To.Y) return Direction.SE;
            else if (From.X > To.X && From.Y < To.Y) return Direction.SW;
            else return Direction.INVALID;
        }
    }
    class GameBoard
    {
        public enum Player
        {
            White,
            Black
        }
        
        private const int DIM = 10;
        public int[,] Board { get; set; }
        public int GetDim()
        {
            return DIM;
        }

        public GameBoard()
        {
            Board = new int[DIM,DIM];
            Init();
        }
        public GameBoard(GameBoard g)
        {
            Board = new int[DIM, DIM];
            for (int r = 0; r < DIM; r++)
            {
                for (int c = 0; c < DIM; c++)
                {
                    Board[r, c] = g.Board[r, c];
                }
            }
        }
        
        public int GetInitValue(Player player)
        {
            return (player == Player.White) ? 1 : -1;
        }
        public Player ChangePlayer(Player player)
        {
            return (player == Player.White) ? Player.Black : Player.White;
        }

        public void Init()
        {
            Player player = Player.White;
            int chip = GetInitValue(player);
            for (int r = 0; r < DIM; r++)
            {
                for (int c = 0; c < DIM; c++)
                {
                    if (r % 2 == 1 && c == 0)
                    {
                        player = Player.White;
                    }
                    else if (r % 2 == 0 && c == 0)
                    {
                        player = Player.Black;
                    }
                    player = ChangePlayer(player);
                    chip = GetInitValue(player);
                    Board[r, c] = chip;
                    Board[r, c+1] = chip;
                    c++;
                }
            }
        }
        public bool ApplyMove(Move move)
        {
            if (move.IsValid())
            {
                int sign = (Board[move.From.Y, move.From.X] >= 0) ? 1 : -1;
                Board[move.From.Y, move.From.X] = 0;
                int currentWeight = Board[move.To.Y, move.To.X];
                Board[move.To.Y, move.To.X] = (Math.Abs(currentWeight) + 1) * sign;
                return true;
            }
            return false;
        }
        public int GetIntermediateDistance(Position From, Position To, Move.Direction dir)
        {
            int row = From.Y;
            int col = From.X;
            int intermediateDistance = 0;
            if (!From.IsValid())
            {
                return 0;
            }
                                    
            if (Math.Abs(Board[row,col]) != 1)
            {
                return 0;
            }
            switch (dir)
            {
                case Move.Direction.NONE:
                case Move.Direction.INVALID:
                    break;
                case Move.Direction.N:
                    col = From.X;
                    for (row = From.Y-1; row > To.Y; row--)
                    {
                        intermediateDistance += Math.Abs(Board[row, col]);
                    }
                    break;
                case Move.Direction.E:
                    row = From.Y;
                    for (col = From.X + 1; col < To.X; col++)
                    {
                        intermediateDistance += Math.Abs(Board[row, col]);
                    }
                    break;
                case Move.Direction.S:
                    col = From.X;
                    for (row = From.Y + 1; row < To.Y; row++)
                    {
                        intermediateDistance += Math.Abs(Board[row, col]);
                    }
                    break;
                case Move.Direction.W:
                    row = From.Y;
                    for (col = From.X - 1; col > To.X; col--)
                    {
                        intermediateDistance += Math.Abs(Board[row, col]);
                    }
                    break;
                case Move.Direction.NE:
                    col = From.X+1;
                    row = From.Y-1;
                    while (col < To.X && row > To.Y)
                    {
                        intermediateDistance += Math.Abs(Board[row, col]);
                        col++;
                        row--;
                    }
                    break;
                case Move.Direction.NW:
                    col = From.X - 1;
                    row = From.Y - 1;
                    while (col > To.X && row > To.Y)
                    {
                        intermediateDistance += Math.Abs(Board[row, col]);
                        col--;
                        row--;
                    }
                    break;
                case Move.Direction.SE:
                    col = From.X + 1;
                    row = From.Y + 1;
                    while (col < To.X && row < To.Y)
                    {
                        intermediateDistance += Math.Abs(Board[row, col]);
                        col++;
                        row++;
                    }
                    break;
                case Move.Direction.SW:
                    col = From.X - 1;
                    row = From.Y + 1;
                    while (col > To.X && row < To.Y)
                    {
                        intermediateDistance += Math.Abs(Board[row, col]);
                        col--;
                        row++;
                    }
                    break;
                default:
                    break;
            }
            return intermediateDistance;
        }
        public override string ToString()
        {
            string output = "";
            for (int r = 0; r < DIM; r++)
            {
                for (int c = 0; c < DIM; c++)
                {
                    if (Board[r,c]<0)
                    {
                        output += $" {Board[r, c]} ";
                    }
                    else
                    {
                        output += $"  {Board[r, c]} ";
                    }
                }
                output += "\n";
            }

            return output;
        }

        public string DebugDirections(int row, int col)
        {
            Position fromPos = new Position(row, col, this);
            string output = "";
            Move.Direction dir;
            for (int r = 0; r < DIM; r++)
            {
                for (int c = 0; c < DIM; c++)
                {
                    if (r == fromPos.Y && c == fromPos.X)
                    {
                        output += " XX ";
                    }
                    else
                    {
                        Position toPos = new Position(r, c, this);
                        Move move = new Move(fromPos, toPos, this);
                        dir = move.GetDirection();

                        switch (dir)
                        {
                            case Move.Direction.NONE:
                            case Move.Direction.INVALID:
                                output += " 00 ";
                                break;
                            case Move.Direction.N:
                                output += " NN ";
                                break;
                            case Move.Direction.E:
                                output += " EE ";
                                break;
                            case Move.Direction.S:
                                output += " SS ";
                                break;
                            case Move.Direction.W:
                                output += " WW ";
                                break;
                            case Move.Direction.NE:
                                output += " NE ";
                                break;
                            case Move.Direction.NW:
                                output += " NW ";
                                break;
                            case Move.Direction.SE:
                                output += " SE ";
                                break;
                            case Move.Direction.SW:
                                output += " SW ";
                                break;
                            default:
                                break;
                        }
                    }
                }
                output += "\n";
            }

            return output;
        }
        public string DebugIntermediateDistances(int row, int col)
        {
            Position fromPos = new Position(row, col, this);
            Move.Direction dir;
            string output = "";
            int intermediateDistance = 0;
            for (int r = 0; r < DIM ; r++)
            {
                for (int c = 0; c < DIM; c++)
                {
                    if (r == fromPos.Y && c == fromPos.X)
                    {
                        output += "    ";
                    }
                    else
                    {
                        Position toPos = new Position(r, c, this);
                        Move move = new Move(fromPos, toPos, this);
                        dir = move.GetDirection();
                        intermediateDistance = GetIntermediateDistance(fromPos, toPos, dir);
                        output += $"  {intermediateDistance} ";
                    }
                }
                output += "\n";
            }

            return output;
        }
    }
}
