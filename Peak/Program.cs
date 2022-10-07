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
            board.ApplyMove(new Move(new Position(3, 4,board), new Position(0, 4,board),board));
            Console.WriteLine("\n");
            Console.WriteLine(board);
            board.ApplyMove(new Move(new Position(0, 5, board), new Position(0, 3, board), board));
            Console.WriteLine("\n");
            Console.WriteLine(board);
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine(newBoard);

            Console.ReadKey();
        }
    }
}
