using System;
using System.Collections.Generic;

namespace HanoiTowers
{
    class HanoiTowers
    {
        int Degree { get; }
        //List<ConsoleColor> ColorScheme { get; }
        int MinimumMoves { get; }
        int CurrentMove { get; set; } = 0;
        bool IsSolved { get; set; } = false;
        int SelectedCollumn { get; set; } = 0;
        int? MovingCollumn { get; set; } = null;
        Stack<int>[] Columns { get; }
        Stack<int> SolvedColumn { get; }

        public HanoiTowers(int degree/*, List<ConsoleColor> colorScheme*/)
        {
            this.Degree = degree;
            //this.ColorScheme = colorScheme;
            this.MinimumMoves = Convert.ToInt32(Math.Pow(2, degree)) - 1;
            this.SolvedColumn = new Stack<int>();
            Stack<int> nullStack = new();
            
            for (int i = 0; i < degree; i++)
            {
                this.SolvedColumn.Push(this.Degree - i);
                nullStack.Push(0);
            }

            this.Columns = new Stack<int>[3] {this.SolvedColumn, nullStack, nullStack};
        }

        private void DisplayBlock(int blockSize)
        {
            // Writing left padding.
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i < this.Degree - blockSize; i++)
            {
                Console.Write("\u2588");
            }

            // Writing block.
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < blockSize*2 + 1; i++)
            {
                Console.Write("\u2588");                
            }

            // Writing right padding.
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i < this.Degree - blockSize; i++)
            {
                Console.Write("\u2588");
            }
        }

        private void DisplayUpperPadding()
        {
            // Looping once for each row.
            for (int i = 0; i < 2; i++)
            {
                // Writing leftmost padding.
                Console.ForegroundColor = ConsoleColor.Black;
                for (int j = 0; j < 3; j++)
                {
                    Console.Write("\u2588");
                }

                // Writing the first collumn.
                DisplayBlock(0);

                // Writing center-left padding.
                Console.ForegroundColor = ConsoleColor.Black;
                for (int j = 0; j < 3; j++)
                {
                    Console.Write("\u2588");
                }

                // Writing the second collumn.
                DisplayBlock(0);

                // Writing center-right padding.
                Console.ForegroundColor = ConsoleColor.Black;
                for (int j = 0; j < 3; j++)
                {
                    Console.Write("\u2588");
                }

                //Writing the third collumn.
                DisplayBlock(0);
                Console.WriteLine();
            } 
        }

        private void DisplayRow(int rowNumber)
        {
            // Writing leftmost padding.
            Console.ForegroundColor = ConsoleColor.Black;
            for (int j = 0; j < 3; j++)
            {
                Console.Write("\u2588");
            }


            // Displaying collumn 1.
            DisplayBlock(this.Columns[0].ElementAt(rowNumber));


            // Writing center-left padding.
            Console.ForegroundColor = ConsoleColor.Black;
            for (int j = 0; j < 3; j++)
            {
                Console.Write("\u2588");
            }


            // Displaying the second collumn.
            DisplayBlock(this.Columns[1].ElementAt(rowNumber));


            // Writing center-right padding.
            Console.ForegroundColor = ConsoleColor.Black;
            for (int j = 0; j < 3; j++)
            {
                Console.Write("\u2588");
            }


            // Displaying the last collumn.
            DisplayBlock(this.Columns[2].ElementAt(rowNumber));
        }

        private void DisplayTowers()
        {
            
            // Displaying the upper padding.
            DisplayUpperPadding();

            // Displaying each row.
            for (int i = 0; i < this.Degree; i++)
            {
                DisplayRow(i);
                Console.WriteLine();
            }
        }

        private void DisplayColumnMenu()
        {
            for (int i = 0; i < 3; i++)
            {
                if (i == SelectedCollumn)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    for (int j = 0; j < this.Degree + 2; j++)
                    {
                        Console.Write("\u2588");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write('<');
                    if (i == this.MovingCollumn)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    Console.Write(i + 1);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write('>');

                    Console.ForegroundColor = ConsoleColor.Black;
                    for (int j = 0; j < this.Degree - 1; j++)
                    {
                        Console.Write("\u2588");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    for (int j = 0; j < this.Degree + 3; j++)
                    {
                        Console.Write("\u2588");
                    }
                    if (i == this.MovingCollumn)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write(i + 1);
                    Console.ForegroundColor = ConsoleColor.Black;
                    for (int j = 0; j < this.Degree; j++)
                    {
                        Console.Write("\u2588");
                    }
                }
            }
        }

        public int[] Display()
        {
            this.MovingCollumn = null;

            while(true)
            {
                // Displaying the game.
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(this.CurrentMove + "/" + this.MinimumMoves);
                Console.WriteLine();
                DisplayTowers();
                DisplayColumnMenu();

                // Running the movement menu.
                ConsoleKey keyPressed;
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                switch (keyPressed)
                {
                    case ConsoleKey.Enter:
                        if (this.MovingCollumn == null)
                        {
                            this.MovingCollumn = this.SelectedCollumn;
                        }
                        else if (this.MovingCollumn == this.SelectedCollumn)
                        {
                            this.MovingCollumn = null;
                        }
                        else
                        {
                            if (IsValidMove([Convert.ToInt32(MovingCollumn), SelectedCollumn]))
                            {
                                return [Convert.ToInt32(MovingCollumn), SelectedCollumn];
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("This is an illegal move.");
                                Console.ReadKey();
                            }
                            this.MovingCollumn = null;
                            this.SelectedCollumn = 0;
                        }
                    break;

                    case ConsoleKey.RightArrow:
                        this.SelectedCollumn ++;
                        if (this.SelectedCollumn == 3)
                        {
                            this.SelectedCollumn = 0;
                        }
                    break;

                    case ConsoleKey.LeftArrow:
                        this.SelectedCollumn --;
                        if (this.SelectedCollumn == -1)
                        {
                            this.SelectedCollumn = 2;
                        }
                    break;
                }
            }
        }

        //Game logic

        private bool IsValidMove(int[] move)
        {
            int movingBlock = this.Columns[move[0]].Peek();
            int targetBlock = this.Columns[move[1]].Peek();

            if (movingBlock < targetBlock || targetBlock == 0)
            {
                return true;
            }
            return false;
        }
    }
}