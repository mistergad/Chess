using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessLib;

namespace ChessDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            Chess chess = new Chess(); // ("rnbqkbnr/pp1111pp/8/8/8/8/PP11111P/RNBQKBNR w KQkq - 0 1");
            List<string> listMoves;
            while (true)
            {
                listMoves = chess.GetAllMoves();
                Console.WriteLine(chess.fen);
                Print(ChessToASCII(chess));
                Console.WriteLine(chess.isCheck() ? "CHECK" : "-");
                foreach (string moves in listMoves)
                    Console.Write(moves + "\t");
                Console.WriteLine();
                Console.Write("> ");
                string move = Console.ReadLine();
                if (move == "q") break;
                if (move == "") move = listMoves[random.Next(listMoves.Count)];
                chess = chess.Move(move);
            }
        }

        static string ChessToASCII(Chess chess)
        {
            string text = "  +-----------------+\n";
            for (int y = 7; y >= 0; y--)
            {
                text += y + 1;
                text += " | ";
                for (int x = 0; x < 8; x++)
                    text += chess.GetFigureAt(x, y) + " ";
                text += "|\n";
            }
            text += "  +-----------------+\n";
            text += "    a b c d e f g h\n";
            return text;
        }

        static void Print(string text)
        {
            ConsoleColor oldForeColor = Console.ForegroundColor;
            foreach(char x in text)
            {
                if (x >= 'a' && x <= 'z')
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (x >= 'A' && x <= 'Z')
                    Console.ForegroundColor = ConsoleColor.White;
                else
                    Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(x);
            }
            Console.ForegroundColor = oldForeColor;
        }
    }
}