using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peak
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Peak\n\n");

            GameBoard board = new Peak.GameBoard();
            GameBoard newBoard = new GameBoard(board);
            //newBoard = board;
            Console.WriteLine(board);
            Console.WriteLine("\n");
            Console.WriteLine(board.DebugDirections(3,4));
            Console.WriteLine("\n");
            Console.WriteLine(board.DebugIntermediateDistances(3,4));

            /*
            board.Test();
            Console.WriteLine(board);
            Console.WriteLine("\n");
            Console.WriteLine("Possible Moves of WHITE:\n");
            Console.WriteLine(board.DebugPossibleMoves(Player.White));
            Console.WriteLine("\n");
            Console.WriteLine("Possible Moves of BLACK:\n");
            Console.WriteLine(board.DebugPossibleMoves(Player.Black));
            Console.WriteLine("\n");
            */

            /*
            Move move = new Move(new Position(3, 4, board), new Position(0, 4, board), board);
            board.DoMove(move);
            Console.WriteLine("\n");
            Console.WriteLine(board);
            board.UndoMove(move);
            Console.WriteLine("\n");
            Console.WriteLine(board);

            board.DoMove(new Move(new Position(3, 4, board), new Position(0, 4, board), board));
            Console.WriteLine("\n");
            Console.WriteLine(board);

            board.DoMove(new Move(new Position(0, 5, board), new Position(0, 3, board), board));
            Console.WriteLine("\n");
            Console.WriteLine(board);
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            //Console.WriteLine(newBoard);
            Console.WriteLine("Possible Moves of WHITE:\n");
            Console.WriteLine(board.DebugPossibleMoves(Player.White));
            Console.WriteLine("\n");
            Console.WriteLine("Possible Moves of BLACK:\n");
            Console.WriteLine(board.DebugPossibleMoves(Player.Black));
            Console.WriteLine("\n");
            */

            
            IMove bestMove;

            for (int runningtime = 0; runningtime < 100; runningtime++)
            {
                int count = 0;
                board.MakeMoveList(out count);
                int depth = 1;
                if (count > 0)
                {
                    depth = (int)Math.Round((1.0 / (float)count) * 80.0);
                    if (depth < 2) depth = 2;
                    if (depth > 5) depth = 5;
                }
                
                Algorithmn.MiniMax(board,depth, out bestMove);
                if (bestMove != null)
                {
                    Console.WriteLine($"{runningtime}: {((Move)bestMove).ToString()}");
                    Console.WriteLine("\n");
                    board.DoMove(bestMove);
                    Console.WriteLine("\n");
                    Console.WriteLine(board);
                    Console.WriteLine("\n");
                }
                else
                {
                    Console.WriteLine($"{runningtime}: No move for {board.NextPlayer}");
                    Console.WriteLine("\n");
                    board.ChangePlayer();
                    //break;
                }
            }
           

            Console.ReadKey();
        }
    }
}
