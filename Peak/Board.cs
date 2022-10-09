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
        public Position(Position pos)
        {
            X = pos.X;
            Y = pos.Y;
            MyBoard = pos.MyBoard;
        }
        public override string ToString()
        {
            string output = $"Row[{Y}]Col[{X}]";

            return output;
        }
        public bool IsEqual(Position pos)
        {
            return (pos.X == X && pos.Y == Y);
        }
    }
    class Move : IMove
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
        public int PreValueFrom { get; set; }
        public int PostValueFrom { get; set; }
        public int PreValueTo { get; set; }
        public int PostValueTo { get; set; }
        public bool Applied { get; set; }

        public Move(Position from, Position to, GameBoard g)
        {
            MyBoard = g;
            From = from;
            To = to;
            PreValueFrom = 0;
            PostValueFrom = 0;
            PreValueTo = 0;
            PostValueTo = 0;
            Applied = false;
        }
        public Move(Move move)
        {
            MyBoard = move.MyBoard;
            From = new Position(move.From);
            To = new Position(move.To);
            
            PreValueFrom = move.PreValueFrom;
            PostValueFrom = move.PostValueFrom;
            PreValueTo = move.PreValueTo;
            PostValueTo = move.PostValueTo;
            Applied = move.Applied;
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
        public override string ToString()
        {
            string output = $"{From.ToString()} -> {To.ToString()} {MyBoard.Board[To.Y,To.X]}+1";

            return output;
        }
        public bool IsEqual(IMove _move)
        {
            Move move = (Move)_move;
            return (move.From.IsEqual(From) && move.To.IsEqual(To));
        }
        public void PreMove()
        {
            PreValueFrom = MyBoard.Board[From.Y, From.X];
            PreValueTo = MyBoard.Board[To.Y, To.X];
        }
        public void PostMove()
        {
            PostValueFrom = MyBoard.Board[From.Y, From.X];
            PostValueTo = MyBoard.Board[To.Y, To.X];
        }
        public void Undo()
        {
            MyBoard.Board[From.Y, From.X] = PreValueFrom;
            MyBoard.Board[To.Y, To.X] = PreValueTo;
        }
    }
    class GameBoard : IBoard
    {
        private const int TOTAL = 2;
        
        private const int DIM = 6;
        public int[,] Board { get; set; }
        public int[] Scoring { get; set; }
        public int[] Passing { get; set; }
        public Stack<IMove> LastMoves { get; set; }
        public Player NextPlayer { get; set; }
        public int GetDim()
        {
            return DIM;
        }
        public void Test()
        {
            /*
            Board = new int[DIM, DIM]
                {
                { 0,0,0,2,0,0 },
                { 0,0,0,-2,-3,0},
                { 0,-2,2,-3,-2,0 },
                { 0,2,0,-3,2,-1},
                { 1,3,2,0,0,1},
                { 0,-1,1,1,0,2}
            };
            */
            Board = new int[DIM, DIM]
                {
                { 0,0,0,2,-2,0 },
                { 0,0,0,-2,-3,-1},
                { 0,1,0,-1,1,1 },
                { 0,2,1,-2,-1,2},
                { -2,1,2,-1,1,1},
                { -1,-1,1,1,-1,-1}
            };
            NextPlayer = Player.Black;
        }

        public GameBoard(Player nextPlayer = Player.White)
        {
            Board = new int[DIM, DIM];
            LastMoves = new Stack<IMove>();
            Scoring = new int[3];
            Passing = new int[2] { 0, 0 };
            Init();
            Evaluation();
            NextPlayer = nextPlayer;
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
            LastMoves = new Stack<IMove>(g.LastMoves);
            Scoring = new int[3];
            Passing = new int[2] { g.Passing[0], g.Passing[1] };
            Evaluation();
            NextPlayer = g.NextPlayer;
        }
        public void Pass(Player player,bool setPass=true)
        {
            if (setPass)
            {
                Passing[(int)player]++;
            }
            else
            {
                Passing[(int)player] = 0;
            }
            
        }
        public bool CheckWin(Player player)
        {
            /*
            Player otherPlayer = (player == Player.White) ? Player.Black : Player.White;
            if (Passing[(int)otherPlayer] >= 2) return true;

            List<IMove> moves = GetPossibleMoves(otherPlayer);
            if (moves.Count == 0 && Passing[(int)otherPlayer] > 0)
            {
                int score = Evaluation();
                if (player == Player.White && score > 0) return true;
                if (player == Player.Black && score < 0) return true;
            }
            */
            return false;
        }
        public bool GameOver(out Player winner, out int score)
        {
            winner = Player.None;
            score = 0;
            if (Passing[(int)Player.White] > 1 || Passing[(int)Player.Black] > 1 || (Passing[(int)Player.White]==1 && Passing[(int)Player.Black] == 1))
            {
                if (Math.Abs(Scoring[(int)Player.White]) > Math.Abs(Scoring[(int)Player.Black]))
                {
                    winner = Player.White;
                    score = Math.Abs(Scoring[(int)Player.White]);
                    return true;
                }
                if (Math.Abs(Scoring[(int)Player.White]) < Math.Abs(Scoring[(int)Player.Black]))
                {
                    winner = Player.Black;
                    score = Math.Abs(Scoring[(int)Player.Black]);
                    return true;
                }
                else
                {
                    winner = Player.None;
                    score = Math.Abs(Scoring[(int)Player.White]);
                    return true;
                }
            }
            return false;
        }

        public int GetInitValue(Player player)
        {
            return (player == Player.White) ? 1 : -1;
        }
        public void ChangePlayer()
        {
            NextPlayer = (NextPlayer == Player.White) ? Player.Black : Player.White;
        }
        public List<IMove> MakeMoveList(out int count)
        {
            List<IMove> moves = GetPossibleMoves(NextPlayer);
            count = moves.Count;
            return moves;
        }
        public List<IMove> GetPossibleMoves(Player player)
        {
            List<IMove> possibleMoves = new List<IMove>();

            int startChip = GetInitValue(player);

            for (int r = 0; r < DIM; r++)
            {
                for (int c = 0; c < DIM; c++)
                {
                    if (Board[r,c] == startChip)
                    {
                        List<IMove> movesFromPosition = GetPossibleMoves(player, new Position(r, c, this));
                        possibleMoves.AddRange(movesFromPosition);
                    }
                }
            }

            return possibleMoves;
        }
        public List<IMove> GetPossibleMoves(Player player, Position pos)
        {
            List<IMove> possibleMoves = new List<IMove>();

            for (int r = 0; r < DIM; r++)
            {
                for (int c = 0; c < DIM; c++)
                {
                    IMove move = new Move(pos, new Position(r, c, this), this);
                    if (move.IsValid())
                    {
                        possibleMoves.Add(move);
                    }
                }
            }
            return possibleMoves;
        }

        public void Init()
        {
            NextPlayer = Player.White;
            int chip = GetInitValue(NextPlayer);
            for (int r = 0; r < DIM; r++)
            {
                for (int c = 0; c < DIM; c++)
                {
                    if (r % 2 == 1 && c == 0)
                    {
                        NextPlayer = Player.White;
                    }
                    else if (r % 2 == 0 && c == 0)
                    {
                        NextPlayer = Player.Black;
                    }
                    ChangePlayer();
                    chip = GetInitValue(NextPlayer);
                    Board[r, c] = chip;
                    Board[r, c+1] = chip;
                    c++;
                }
            }
        }
        void ApplyMove(IMove _move)
        {
            Move move = (Move)_move;
            int sign = (Board[move.From.Y, move.From.X] >= 0) ? 1 : -1;
            Board[move.From.Y, move.From.X] = 0;
            int currentWeight = Board[move.To.Y, move.To.X];
            Board[move.To.Y, move.To.X] = (Math.Abs(currentWeight) + 1) * sign;
        }
        public void DoMove(IMove move)
        {
            if (move.IsValid() && move.Applied == false)
            {
                move.PreMove();
                ApplyMove(move);
                move.PostMove();
                move.Applied = true;
                LastMoves.Push(move);
                ChangePlayer();
            }
        }
        public void UndoMove(IMove move)
        {
            if (LastMoves.Count > 0 && move.Applied == true && move.IsEqual(LastMoves.Peek()))
            {
                move.Undo();
                move.Applied = false;
                LastMoves.Pop();
                ChangePlayer();
            }
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
                                    
            if (Math.Abs(Board[row,col]) != 1 || Math.Abs(Board[To.Y, To.X]) == 0)
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

        public int Evaluation()
        {
            Scoring[(int)Player.White] = 0;
            Scoring[(int)Player.Black] = 0;
            Scoring[TOTAL] = 0;
            for (int r = 0; r < DIM; r++)
            {
                for (int c = 0; c < DIM; c++)
                {
                    Scoring[TOTAL] += Board[r, c];
                    if (Board[r, c] >= 0)
                    {
                        Scoring[(int)Player.White] += Board[r, c];
                    }
                    else
                    {
                        Scoring[(int)Player.Black] += Math.Abs(Board[r, c]);
                    }
                }
            }
            return Scoring[TOTAL];
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

        public string DebugPossibleMoves(Player player, int row, int col)
        {
            Position fromPos = new Position(row, col, this);
            string output = "";

            List<IMove> possibleMoves = GetPossibleMoves(player, fromPos);

            output = DebugMoves(possibleMoves);
            
            return output;
        }
        public string DebugPossibleMoves(Player player)
        {
            string output = "";

            List<IMove> possibleMoves = GetPossibleMoves(player);

            output = DebugMoves(possibleMoves);

            return output;
        }
        public string DebugMoves(List<IMove> moves)
        {
            string output = "";

            foreach(IMove move in moves)
            {
                output += move.ToString() + "\n";
            }
            
            return output;
        }
    }
}
