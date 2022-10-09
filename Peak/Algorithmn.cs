using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using static Peak.Algorithmn;

namespace Peak
{
    public enum Player
    {
        White = 0,
        Black = 1
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
        Player NextPlayer { get; set; }
    }

	internal class Algorithmn
    {
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
					}
				}
				else
				{
                    if (value < bestValue)
                    {
                        bestValue = value;
                        bestmove = move;
                    }
                }
				if (i == n-1 && bestmove == null)
				{
					bestmove = nextMoves[n / 2];
				}
            }
			return bestValue;
        }
    }
}

/*
 int makemovelist(POSITION *p, MOVE list[MAXMOVES])
 domove(MOVE *m, POSITION *p)
 undomove(MOVE *m, POSITION *p)
int evaluation(POSITION *p)
 int minimax(POSITION  *p, int depth)
	{
	MOVE list[MAXMOVES];
	int i,n,bestvalue,value;
	
	if(checkwin(p)) 
		{
		if (p->color == WHITE) 
			return -INFINITY;
		else 
			return INFINITY;
		}

	if(depth == 0)	
		return evaluation(p);

	if(p->color==WHITE) 
		bestvalue = -INFINITY;
	else 
		bestvalue = INFINITY;
	
	n = makemovelist(p,list);
	if(n == 0) 
		return handlenomove(p);
	
	for(i=0; i<n; i++)
		{
		domove(&list[i],p);
		value = minimax(p,d-1);
		undomove(&list[i],p);
		if(color == WHITE) 
			bestvalue = max(value,bestvalue);
		else 
			bestvalue = min(value,bestvalue);
		}
  
	return bestvalue;
	}
 
 */