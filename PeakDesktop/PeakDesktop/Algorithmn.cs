using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace PeakDesktop
{
    public enum Player
    {
        White = 0,
        Black = 1,
		None = 2
    }
    public interface IMove
	{
		bool IsValid();
        bool Applied { get; set; }
		void PreMove();
		void PostMove();
		bool IsEqual(IMove move);
		void Undo();
    }
    public interface IBoard
	{
		List<IMove> MakeMoveList(out int count);
		void DoMove(IMove move);
		void UndoMove(IMove move);
		int Evaluation();
		bool CheckWin(Player player);
		void Pass(Player player,bool setPass = true);
        Player NextPlayer { get; set; }
    }

	internal class Algorithmn
    {
		private static Random random = new Random();
        public static int MiniMax(IBoard board, int depth, out IMove bestmove)
		{
            bestmove = null;
			int value = 0;
			
            int bestValue = board.NextPlayer == Player.White ? int.MinValue : int.MaxValue;
            Player otherPlayer = board.NextPlayer == Player.White ? Player.Black : Player.White;

            if (board.CheckWin(otherPlayer))
			{
				return bestValue;
			}
            if (depth == 0)
			{
				return board.Evaluation();
			}
			
            int n = 0;
			List<IMove> nextMoves = board.MakeMoveList(out n);
            List<IMove> bestMoves = new List<IMove>();
            if (n == 0)
			{
				return bestValue;
            }
            for (int i = 0; i < n; i++)
            {
				IMove move = nextMoves[i];
				board.DoMove(move);
				IMove nextbestmove;
                value = MiniMax(board, depth - 1,out nextbestmove);
                board.UndoMove(move);

				if (board.NextPlayer == Player.White)
				{
					if (value > bestValue)
					{
						bestValue = value;
						bestmove = move;
						bestMoves = new List<IMove>();
						bestMoves.Add(move);
					}
					else if (value == bestValue)
					{
						bestMoves.Add(move);
					}
				}
				else
				{
					if (value < bestValue)
					{
						bestValue = value;
						bestmove = move;
						bestMoves = new List<IMove>();
						bestMoves.Add(move);
					}
					else if (value == bestValue)
					{
						bestMoves.Add(move);
					}
				}
            }
			int bm = bestMoves.Count;
            if (bm > 0)
			{
				int m = 0;
				if (bm > 1)
				{
                    m = random.Next(bm);     // creates a number between 0 and bm
                }
				bestmove = bestMoves[m];
            }
			else
			{
				bestmove = null;
			}
			return bestValue;
        }
    }
}

