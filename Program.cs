using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Féléves_beadandó_Tetrisz_PBVGD1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //Alap tulajdonságok a táblának
            Console.Title = "Tetrisz mady By PBVGD1";
            Console.WindowWidth = 50;
            Random r = new Random();
            //igy lehetne nagyobb is
            GameBoard gameBoard = new GameBoard(20, 10);
            int scores = 0;
            int highScore = 0;
            bool gameIsOver = false;
            bool started = false;
            
            Start();

            //Egy Do-While ciklusban fut a játék, hogy frissítsen.
            do
            {
                
                gameBoard.RotatedLeft = false;
                gameBoard.RotatedRight = false;
                bool figureIsDown = false;
                // Az öt figura közül egy random 
                Figure figure = new Figure(r.Next(1,6)); 
                gameIsOver = gameBoard.InsertFigure(figure);
                do
                {

                    Console.Clear();
                    StreamReader sr = new StreamReader("highScore.txt");
                    while (!sr.EndOfStream)
                    {
                        highScore = int.Parse(sr.ReadLine());
                    }
                    sr.Close();
                    if (scores > highScore)
                    {
                        highScore = scores;
                    }
                    gameBoard.DrawGameBoard(highScore,scores);
                    figureIsDown = gameBoard.Move(Console.ReadKey().Key, ref figure);
                    
                } while (!figureIsDown);
                scores += 10;
                scores += gameBoard.GetScore();
            } while (!gameIsOver);
            Console.Clear();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game over!");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Your score is: " + scores);
            Console.ForegroundColor = ConsoleColor.White;


            // Egy külön txt fileban elmenti az legmagasabb pontot feltéve, ha a játékos meghaladja az előzőt.
            StreamWriter sw = new StreamWriter("highScore.txt");
            sw.WriteLine(scores);
            sw.Close();
            Console.ReadKey();

        }
       
        
        // Itt lehet a "start" kulcsszóval elindítani a játékot. Csak kisbetűsen beírva fogadja el.
        static void Start()
        {
            string parancs = "";
            StreamReader sr2 = new StreamReader("highScore.txt");
            while (!sr2.EndOfStream)
            {
                int HighScore = int.Parse(sr2.ReadLine());
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Your Best Score: " + HighScore + "\n");
                
            }
            sr2.Close();
            
            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Type 'start' to enter the game!");
                Console.ForegroundColor = ConsoleColor.White;

                parancs = Console.ReadLine();
            } while (parancs != "start");

        }
        
        
        
        
    }
}
