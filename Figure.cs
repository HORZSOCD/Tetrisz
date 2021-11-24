using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Féléves_beadandó_Tetrisz_PBVGD1
{

    // minden formát egy tipusként határoztam meg
    public enum Type
    {
        line, square, L, Z, T
    }

    class Figure
    {
        // a figurák formája és tipusa
        public Type type { get; set; }
        public GameElement[,] Shape { get; set; }
        public int[,] Coordinates { get; set; }

        
        // Ez generálja le a figurákat
        public Figure(int n)
        {
            
            this.Shape = new GameElement[4, 4];
            this.Coordinates = new int[4, 2];
            FigureMaker(n);
        }
        // Ez randomizálja, hogy melyik forma jöjjön. Minden formának megvan a saját kezdési koordinátája az első két sorban
        private void FigureMaker(int n)
        {


            if(n == 1) //line
            {
                this.type = Type.line;
                
                for (int i = 0; i < this.Shape.GetLength(0); i++)
                    for (int j = 0; j < this.Shape.GetLength(1); j++)
                    {
                        if (i == 0) 
                            this.Shape[i, j] = new GameElement(ElementType.filled);
                        else
                            this.Shape[i, j] = new GameElement();
                    }
                this.Coordinates[0, 0] = 1;
                this.Coordinates[0, 1] = 4;

                this.Coordinates[1, 0] = 1;
                this.Coordinates[1, 1] = 5;

                this.Coordinates[2, 0] = 1;
                this.Coordinates[2, 1] = 6;

                this.Coordinates[3, 0] = 1;
                this.Coordinates[3, 1] = 7;

            }

            else if(n == 2) //spuare
            {
                this.type = Type.square;
                for (int i = 0; i < this.Shape.GetLength(0); i++)
                    for (int j = 0; j < this.Shape.GetLength(1); j++)
                    {
                        if (i < 2 && j < 2)
                            this.Shape[i, j] = new GameElement(ElementType.filled);
                        else
                            this.Shape[i, j] = new GameElement();
                    }
                this.Coordinates[0, 0] = 1;
                this.Coordinates[0, 1] = 4;

                this.Coordinates[1, 0] = 1;
                this.Coordinates[1, 1] = 5;

                this.Coordinates[2, 0] = 2;
                this.Coordinates[2, 1] = 4;

                this.Coordinates[3, 0] = 2;
                this.Coordinates[3, 1] = 5;
            }
            else if (n == 3)//L rotated
            {

                this.type = Type.L;
                for (int i = 0; i < this.Shape.GetLength(0); i++)
                    for (int j = 0; j < this.Shape.GetLength(1); j++)
                    {
                        if (i == 0 && j < 3)
                            this.Shape[i, j] = new GameElement(ElementType.filled);
                        else if(i == 1 && j == 0)
                            this.Shape[i, j] = new GameElement(ElementType.filled);
                        else
                            this.Shape[i, j] = new GameElement();
                    }
                this.Coordinates[0, 0] = 1;
                this.Coordinates[0, 1] = 4;

                this.Coordinates[1, 0] = 1;
                this.Coordinates[1, 1] = 5;

                this.Coordinates[2, 0] = 1;
                this.Coordinates[2, 1] = 6;

                this.Coordinates[3, 0] = 2;
                this.Coordinates[3, 1] = 4;
            }
            else if (n == 4) //Z rotated
            {
                this.type = Type.Z;
                for (int i = 0; i < this.Shape.GetLength(0); i++)
                {
                    for (int j = 0; j < this.Shape.GetLength(1); j++)
                    {
                        if(i==0 && j==1)
                            this.Shape[i, j] = new GameElement(ElementType.filled);
                        else if(i == 0 && j == 2) 
                            this.Shape[i, j] = new GameElement(ElementType.filled);
                        else if(i==1 && j<2) 
                            this.Shape[i, j] = new GameElement(ElementType.filled);
                        else
                            this.Shape[i, j] = new GameElement();
                    }
                }
                this.Coordinates[0, 0] = 1;
                this.Coordinates[0, 1] = 5;

                this.Coordinates[1, 0] = 1;
                this.Coordinates[1, 1] = 6;

                this.Coordinates[2, 0] = 2;
                this.Coordinates[2, 1] = 4;

                this.Coordinates[3, 0] = 2;
                this.Coordinates[3, 1] = 5;
            }
            else if (n == 5)//T
            {
                this.type = Type.T;
                for (int i = 0; i < this.Shape.GetLength(0); i++)
                {
                    for (int j = 0; j < this.Shape.GetLength(1); j++)
                    {
                        if (i == 0 && j < 3) 
                            this.Shape[i, j] = new GameElement(ElementType.filled);
                        else if (i == 1 && j == 1) 
                            this.Shape[i, j] = new GameElement(ElementType.filled);
                        else
                            this.Shape[i, j] = new GameElement();
                    }
                }
                this.Coordinates[0, 0] = 1;
                this.Coordinates[0, 1] = 4;

                this.Coordinates[1, 0] = 1;
                this.Coordinates[1, 1] = 5;

                this.Coordinates[2, 0] = 1;
                this.Coordinates[2, 1] = 6;

                this.Coordinates[3, 0] = 2;
                this.Coordinates[3, 1] = 5;
            }
        }
    }
}
