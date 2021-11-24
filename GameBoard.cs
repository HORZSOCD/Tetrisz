using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Féléves_beadandó_Tetrisz_PBVGD1
{
    class GameBoard
    {
        public bool RotatedRight = false;
        public bool RotatedLeft = false;
        

        
        public GameElement[,] Gamespace { get; set; }
        public int[,] ToBeMoved { get; set; }

        public GameBoard(int a, int b)
        {
            this.Gamespace = new GameElement[a + 2, b + 2];
            this.ToBeMoved = new int[4, 2];
            for (int i = 0; i < this.Gamespace.GetLength(0); i++)
            {
                for (int j = 0; j < this.Gamespace.GetLength(1); j++)
                {
                    if (i == 0 || i == this.Gamespace.GetLength(0) - 1 ||
                        j == 0 || j == this.Gamespace.GetLength(1) - 1)
                        this.Gamespace[i, j] = new GameElement(ElementType.wall);
                    else
                        this.Gamespace[i, j] = new GameElement();
                }
            }
        }

        // Ez rajzolja ki a pályát
        public void DrawGameBoard(int highScore, int scores) 
        {
            for (int i = 0; i < this.Gamespace.GetLength(0); i++)
            {
                for (int j = 0; j < this.Gamespace.GetLength(1); j++)
                {
                    if (this.Gamespace[i, j] != null) 
                        if (this.Gamespace[i, j].EType == ElementType.wall)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write('#');
                        }

                        else if (Gamespace[i, j].EType == ElementType.filled)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write('X');
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(' ');
                        }

                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            HowToUse();
            Console.WriteLine("\nPlayer highscore " + highScore);
            Console.WriteLine("Player scores: " + scores);
            Console.ForegroundColor = ConsoleColor.White;
        }

        // Ez írja ki hogy mivel hogyan lehet mozogni.
        static void HowToUse()
        {
            Console.Write("S -> DOWN \t");
            Console.Write("A -> LEFT\t");
            Console.WriteLine("D -> RIGHT");
            Console.WriteLine("Q -> ROTATE LEFT\t");
            Console.WriteLine("E -> ROTATE RIGHT");
            Console.WriteLine("ESC -> EXIT (Your score will not be saved.)");
        }
        public bool InsertFigure(Figure f)
        {
            for (int i = 1; i <= 4; i++)
            {
                for (int j = 4; j <= 7; j++)
                {
                    if (this.Gamespace[i, j].EType != ElementType.filled)
                    {
                        this.Gamespace[i, j] = f.Shape[i - 1, j - 4];
                    }
                    else
                    {
                        return true; //game over
                    }
                }
            }
            return false; 
        }


        // Ez kezeli a lenyomott gombokat
        public bool Move(ConsoleKey key, ref Figure f)
        {
            bool output = false;
            switch (key)
            {
                case ConsoleKey.S:
                    output = MoveDown(ref f);
                    break;
                case ConsoleKey.A:
                    MoveLeft(ref f);
                    break;
                case ConsoleKey.D:
                    MoveRight(ref f);
                    break;
                case ConsoleKey.E:
                    RotateRight(ref f);
                    break;
                case ConsoleKey.Q:
                    RotateLeft(ref f);
                    break;
                case ConsoleKey.Escape:
                    Escape(ref f);
                    break;
                default:
                    break;
            }
            return output;
        }

        // Lefelé mozgás
        private bool MoveDown(ref Figure f)
        {
            bool cantGoDown = false;
            for (int i = 0; i < 4; i++)
                if (this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].EType == ElementType.wall ||
                    (this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].EType == ElementType.filled &&
                    this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].IsDown == true))
                    cantGoDown = true;

            if (cantGoDown)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.Gamespace[
                        f.Coordinates[i, 0],
                        f.Coordinates[i, 1]
                        ].IsDown = true;
                }
                return cantGoDown;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    this.Gamespace[
                        f.Coordinates[i, 0],
                        f.Coordinates[i, 1]
                        ] = new GameElement();
                    f.Coordinates[i, 0]++;
                }
                for (int i = 0; i < 4; i++)
                {
                    this.Gamespace[
                        f.Coordinates[i, 0],
                        f.Coordinates[i, 1]
                        ] = new GameElement(ElementType.filled);
                }
            }
            return cantGoDown;
        }

        // Balra mozgás
        private void MoveLeft(ref Figure f)
        {
            bool cantGoLeft = false;
            for (int i = 0; i < 4; i++)
                if (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 1].EType == ElementType.wall ||
                    (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 1].EType == ElementType.filled &&
                    this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 1].IsDown == true))
                    cantGoLeft = true;

            if (!cantGoLeft)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.Gamespace[
                        f.Coordinates[i, 0],
                        f.Coordinates[i, 1]
                        ] = new GameElement();
                    f.Coordinates[i, 1]--;
                }
                for (int i = 0; i < 4; i++)
                {
                    this.Gamespace[
                        f.Coordinates[i, 0],
                        f.Coordinates[i, 1]
                        ] = new GameElement(ElementType.filled);
                }
            }
        }
        // Jobbra mozgás
        private void MoveRight(ref Figure f)
        {
            bool cantGoRight = false;
            for (int i = 0; i < 4; i++)
                if (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].EType == ElementType.wall ||
                    (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].EType == ElementType.filled &&
                    this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].IsDown == true))
                    cantGoRight = true;

            if (!cantGoRight)
            {
                for (int i = 0; i < 4; i++)
                {
                    this.Gamespace[
                        f.Coordinates[i, 0],
                        f.Coordinates[i, 1]
                        ] = new GameElement();
                    f.Coordinates[i, 1]++;
                }
                for (int i = 0; i < 4; i++)
                {
                    this.Gamespace[
                        f.Coordinates[i, 0],
                        f.Coordinates[i, 1]
                        ] = new GameElement(ElementType.filled);
                }
            }
        }
        // Forma jobbra forgatása
        private void RotateRight(ref Figure f)
        {
            bool CantRotate = false;
            if (f.type == Type.line)
            {
                if (RotatedLeft == true)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 2].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 2].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 2].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
                else if (RotatedLeft == false)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0] + 3, f.Coordinates[i, 1]].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0] + 3, f.Coordinates[i, 1]].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0] + 3, f.Coordinates[i, 1]].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
            }
            if (f.type == Type.L)
            {
                if (RotatedLeft == true)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1]+1].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
                else if (RotatedLeft == false)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0] - 1, f.Coordinates[i, 1]].EType == ElementType.wall )
                        {
                            CantRotate = true;
                        }
                    }
                }
            }
            if (f.type == Type.Z)
            {
                if (RotatedLeft == true)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
                else if (RotatedLeft == false)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
            }
            if (f.type == Type.T)
            {
                if (RotatedLeft == true)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
                else if (RotatedLeft == false)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
            }


            if (!CantRotate && RotatedRight == false )
            {

                if(f.type == Type.line)
                {


                    if (RotatedLeft == true)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] -= 0;
                        f.Coordinates[i, 1] -= 0;
                        f.Coordinates[i + 1, 0] -= 1;
                        f.Coordinates[i + 1, 1] += 1;
                        f.Coordinates[i + 2, 0] -= 2;
                        f.Coordinates[i + 2, 1] += 2;
                        f.Coordinates[i + 3, 0] -= 3;
                        f.Coordinates[i + 3, 1] += 3;


                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }
                        RotatedLeft = false;
                    }
                    else if (RotatedLeft == false)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += 3;
                        f.Coordinates[i, 1] += 3;
                        f.Coordinates[i + 1, 0] += 2;
                        f.Coordinates[i + 1, 1] += 2;
                        f.Coordinates[i + 2, 0] += 1;
                        f.Coordinates[i + 2, 1] += 1;
                        f.Coordinates[i + 3, 0] += 0;
                        f.Coordinates[i + 3, 1] += 0;

                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }
                        RotatedRight = true;
                        RotatedLeft = false;

                    }



                }
                else if (f.type == Type.L)
                {
                    if (RotatedLeft == true)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += -1;
                        f.Coordinates[i, 1] += 0;
                        f.Coordinates[i + 1, 0] += 0;
                        f.Coordinates[i + 1, 1] += +1;
                        f.Coordinates[i + 2, 0] += +1;
                        f.Coordinates[i + 2, 1] += +2;
                        f.Coordinates[i + 3, 0] += 0;
                        f.Coordinates[i + 3, 1] += -1;


                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }

                        RotatedLeft = false;
                    }
                    else if (RotatedLeft == false)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += -1;
                        f.Coordinates[i, 1] += 1;
                        f.Coordinates[i + 1, 0] += 0;
                        f.Coordinates[i + 1, 1] += 0;
                        f.Coordinates[i + 2, 0] += 1;
                        f.Coordinates[i + 2, 1] += -1;
                        f.Coordinates[i + 3, 0] += -2;
                        f.Coordinates[i + 3, 1] += 0;



                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }

                        RotatedRight = true;
                        RotatedLeft = false;
                    }


                    
                }
                else if(f.type == Type.Z)
                {
                    if (RotatedLeft == true)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += -1;
                        f.Coordinates[i, 1] += 1;
                        f.Coordinates[i + 1, 0] += 0;
                        f.Coordinates[i + 1, 1] += 2;
                        f.Coordinates[i + 2, 0] += -1;
                        f.Coordinates[i + 2, 1] += -1;
                        f.Coordinates[i + 3, 0] += 0;
                        f.Coordinates[i + 3, 1] += 0;


                       



                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }

                        RotatedLeft = false;
                    }
                    else if (RotatedLeft == false)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += 1;
                        f.Coordinates[i, 1] += 1;
                        f.Coordinates[i + 1, 0] += 2;
                        f.Coordinates[i + 1, 1] += 0;
                        f.Coordinates[i + 2, 0] += -1;
                        f.Coordinates[i + 2, 1] += 1;
                        f.Coordinates[i + 3, 0] += 0;
                        f.Coordinates[i + 3, 1] += 0;



                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }

                        RotatedRight = true;
                        RotatedLeft = false;
                    }
                        
                }
                else if (f.type == Type.T)
                {
                    if (RotatedLeft == true)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += -2;
                        f.Coordinates[i, 1] += 0;
                        f.Coordinates[i + 1, 0] += -1;
                        f.Coordinates[i + 1, 1] += 1;
                        f.Coordinates[i + 2, 0] += 0;
                        f.Coordinates[i + 2, 1] += 2;
                        f.Coordinates[i + 3, 0] += 0;
                        f.Coordinates[i + 3, 1] += 0;

                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }

                        RotatedLeft = false;
                    }
                    else if (true)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += 0;
                        f.Coordinates[i, 1] += 2;
                        f.Coordinates[i + 1, 0] += 1;
                        f.Coordinates[i + 1, 1] += 1;
                        f.Coordinates[i + 2, 0] += 2;
                        f.Coordinates[i + 2, 1] += 0;
                        f.Coordinates[i + 3, 0] += 0;
                        f.Coordinates[i + 3, 1] += 0;



                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }

                        RotatedRight = true;
                        RotatedLeft = false;
                    }
                        
                }

            }
        }
        // Forma jobbra forgatása. Sajnos nem minden irányba tud forogni csak alaphelyzet, balra és jobbra tehát fejjel lefelé sajnos nem tudtam megoldani.
        //Azért is ilyen hosszú a fogatás hiszen csak formánként tudtam megoldani a sajátos forgásukat. Itt néha kiadja a hibát, hogy kifut a tömbön, sajnos ezzel nem tudtam mit kezdeni.

        private void RotateLeft(ref Figure f)
        {
            
            bool CantRotate = false;
            if (f.type == Type.line)
            {
                if (RotatedRight == true)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] -2].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 2].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 2].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
                else if (RotatedRight == false)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0] + 3, f.Coordinates[i, 1]].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0] + 3, f.Coordinates[i, 1]].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0] + 3, f.Coordinates[i, 1]].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
            }
            if (f.type == Type.L)
            {
                if (RotatedRight == true)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 1].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 1].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 1].IsDown == true))
                        {
                            CantRotate = true;
                        }else if (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].EType == ElementType.wall ||
                       (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].EType == ElementType.filled &&
                       this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] + 1].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
                else if (RotatedRight == false)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (((this.Gamespace[f.Coordinates[i, 0] +1, f.Coordinates[i, 1]].EType == ElementType.wall) ||
                            (this.Gamespace[f.Coordinates[i, 0] -1, f.Coordinates[i, 1]].EType == ElementType.wall)) ||
                        (this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
            }
            if (f.type == Type.Z)
            {
                if (RotatedRight == true)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 1].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 1].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 1].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
                else if (RotatedRight == false)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
            }
            if (f.type == Type.T)
            {
                if (RotatedRight == true)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 1].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 1].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0], f.Coordinates[i, 1] - 1].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
                else if (RotatedRight == false)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].EType == ElementType.wall ||
                        (this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].EType == ElementType.filled &&
                        this.Gamespace[f.Coordinates[i, 0] + 1, f.Coordinates[i, 1]].IsDown == true))
                        {
                            CantRotate = true;
                        }
                    }
                }
            }



            if (!CantRotate && this.RotatedLeft == false)
            {
                
                if (f.type == Type.line )
                {
                    if (RotatedRight == true)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] -= 3;
                        f.Coordinates[i, 1] -= 3;
                        f.Coordinates[i + 1, 0] -= 2;
                        f.Coordinates[i + 1, 1] -= 2;
                        f.Coordinates[i + 2, 0] -= 1;
                        f.Coordinates[i + 2, 1] -= 1;
                        f.Coordinates[i + 3, 0] -= 0;
                        f.Coordinates[i + 3, 1] -= 0;

                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }
                        RotatedRight = false;
                    }else if(RotatedRight == false)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += 0;
                        f.Coordinates[i, 1] += 0;
                        f.Coordinates[i + 1, 0] += 1;
                        f.Coordinates[i + 1, 1] -= 1;
                        f.Coordinates[i + 2, 0] += 2;
                        f.Coordinates[i + 2, 1] -= 2;
                        f.Coordinates[i + 3, 0] += 3;
                        f.Coordinates[i + 3, 1] -= 3;



                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }
                        RotatedLeft = true;
                        RotatedRight = false;
                    }
                    


                    
                }
                else if (f.type == Type.L)
                {
                    if (RotatedRight == true)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += 1;
                        f.Coordinates[i, 1] += -1;
                        f.Coordinates[i + 1, 0] += 0;
                        f.Coordinates[i + 1, 1] += 0;
                        f.Coordinates[i + 2, 0] += -1;
                        f.Coordinates[i + 2, 1] += 1;
                        f.Coordinates[i + 3, 0] += 2;
                        f.Coordinates[i + 3, 1] += 0;



                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }

                        RotatedRight = false;
                    }
                    else if(RotatedRight == false)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += 1;
                        f.Coordinates[i, 1] += 0;
                        f.Coordinates[i + 1, 0] += 0;
                        f.Coordinates[i + 1, 1] += -1;
                        f.Coordinates[i + 2, 0] += -1;
                        f.Coordinates[i + 2, 1] += -2;
                        f.Coordinates[i + 3, 0] += 0;
                        f.Coordinates[i + 3, 1] += 1;

                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }

                        RotatedLeft = true;
                        RotatedRight = false;
                    }
                        
                }
                else if (f.type == Type.Z)
                {
                    if (RotatedRight== true)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += -1;
                        f.Coordinates[i, 1] += -1;
                        f.Coordinates[i + 1, 0] += -2;
                        f.Coordinates[i + 1, 1] += 0;
                        f.Coordinates[i + 2, 0] += 1;
                        f.Coordinates[i + 2, 1] += -1;
                        f.Coordinates[i + 3, 0] += 0;
                        f.Coordinates[i + 3, 1] += 0;




                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }

                        RotatedRight = false;
                    }
                    else if (RotatedRight == false)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += 1;
                        f.Coordinates[i, 1] += -1;
                        f.Coordinates[i + 1, 0] += 0;
                        f.Coordinates[i + 1, 1] += -2;
                        f.Coordinates[i + 2, 0] += 1;
                        f.Coordinates[i + 2, 1] += 1;
                        f.Coordinates[i + 3, 0] += 0;
                        f.Coordinates[i + 3, 1] += 0;

                       

                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }

                        RotatedLeft = true;
                        RotatedRight = false;
                    }
                    
                }
                else if (f.type == Type.T)
                {

                    if (RotatedRight == true)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += 0;
                        f.Coordinates[i, 1] += -2;
                        f.Coordinates[i + 1, 0] += -1;
                        f.Coordinates[i + 1, 1] += -1;
                        f.Coordinates[i + 2, 0] += -2;
                        f.Coordinates[i + 2, 1] += 0;
                        f.Coordinates[i + 3, 0] += 0;
                        f.Coordinates[i + 3, 1] += 0;




                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }

                        RotatedRight = false;
                    }
                    else if(RotatedRight == false)
                    {
                        int i = 0;
                        for (int x = 0; x < 4; x++)
                        {
                            this.Gamespace[
                           f.Coordinates[x, 0],
                           f.Coordinates[x, 1]
                           ] = new GameElement();
                        }

                        f.Coordinates[i, 0] += 2;
                        f.Coordinates[i, 1] += 0;
                        f.Coordinates[i + 1, 0] += 1;
                        f.Coordinates[i + 1, 1] += -1;
                        f.Coordinates[i + 2, 0] += 0;
                        f.Coordinates[i + 2, 1] += -2;
                        f.Coordinates[i + 3, 0] += 0;
                        f.Coordinates[i + 3, 1] += 0;



                        for (int j = 0; j < 4; j++)
                        {
                            this.Gamespace[
                                f.Coordinates[j, 0],
                                f.Coordinates[j, 1]
                                ] = new GameElement(ElementType.filled);
                        }

                        RotatedLeft = true;
                        RotatedRight = false;
                    }
                    
                }

            }
        }



        public int GetScore()
        {
            int k = 0;
            int l = 0;
            int m = this.Gamespace.GetLength(0);
            int n = this.Gamespace.GetLength(1);
            int[] linesToBeDeleted = new int[n];

            for (int i = 1; i < m - 1; i++)
            {
                for (int j = 1; j < n - 1; j++)
                {
                    if (this.Gamespace[i, j].EType == ElementType.filled)
                        k++;
                    if (k == 10)
                        linesToBeDeleted[l++] = i;
                }
                k = 0;
            }
            
            int Output = NotZero(linesToBeDeleted);

            if (Output != 0)
                DeleteLines(linesToBeDeleted);

            return Output;
        }
        // Betels sorok törlése. Sajnos csak egyet tud leesni a felette lévő sor. Ezt sajnos nem volt időm befejezni.
        private void DeleteLines(int[] toBeDeleted)
        {
            int m = this.Gamespace.GetLength(0);
            int n = this.Gamespace.GetLength(1);
            for (int i = 0; i < toBeDeleted.Length; i++)
            {
                if (toBeDeleted[i] != 0)
                {
                    for (int j = 1; j < n - 1; j++)
                    {
                        this.Gamespace[toBeDeleted[i], j] = new GameElement();
                    }
                }
            }

            
            for (int i = m - 2; i > 0; i--) 
            {
                for (int j = 1; j < n - 1; j++) 
                {
                    if (this.Gamespace[i, j].EType == ElementType.filled)
                    {
                        if (this.Gamespace[i + 1, j].EType == ElementType.empty)
                        {
                            this.Gamespace[i + 1, j] = new GameElement(ElementType.filled, true);
                            this.Gamespace[i, j] = new GameElement();
                        }
                    }
                }
            }
        }
        private int NotZero(int[] inPut)
        {
            int outPut = 0;
            for (int i = 0; i < inPut.Length; i++)
                if (inPut[i] != 0)
                    outPut+=100;

            return outPut;
        }
        private bool DownLineIsEmpty() 
        {
            for (int i = 1; i < this.Gamespace.GetLength(1) - 1; i++)
            {
                if (this.Gamespace[this.Gamespace.GetLength(0) - 1, i].EType == ElementType.filled)
                {
                    return false;
                }
            }
            return true;
        }

        // A játék megszakításáért felel az ESC megnyomásával
        public void Escape(ref Figure f)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game over! (Game was cancelled by the player)");
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(-1);
        }
    }
}
