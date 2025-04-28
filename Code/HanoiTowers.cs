using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace HanoiTowers
{
    class HanoiTowers
    {
        public int Degree { get; }
        public int MinimumMoves { get; }
        public int CurrentMove { get; set; } = 0;
        private int SelectedCollumn { get; set; } = 0;
        private int? MovingCollumn { get; set; } = null;
        private Stack<int>[] Columns { get; }
        private Stack<int> SolvedColumn { get; }


        /// <summary>
        /// Initializes a new instance of the HanoiTowers class with the specified number of blocks.
        /// </summary>
        /// <param name="degree">Number of blocks in the tower.</param>
        public HanoiTowers(int degree)
        {
            this.Degree = degree;
            this.MinimumMoves = Convert.ToInt32(Math.Pow(2, degree)) - 1;
            this.SolvedColumn = new Stack<int>();
            Stack<int> nullStack = new();
            
            for (int i = 0; i < degree; i++)
            {
                this.SolvedColumn.Push(this.Degree - i);
            }

            this.Columns = new Stack<int>[3] {this.SolvedColumn, new Stack<int>(degree), new Stack<int>(degree)};
        }

        //--------------------------Game display--------------------------\\


        /// <summary>
        /// Displays a block of the tower and its left and right padding.
        /// </summary>
        /// <param name="blockSize">Determines the size of the block.</param>
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


        /// <summary>
        /// Displays the first rows of the towers without any blocks.
        /// This is used so that the user can more easily see the position of the towers.
        /// </summary>
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


        /// <summary>
        /// Displays a row of the towers.
        /// </summary>
        /// <param name="rowNumber">Determines which row is displayed.</param>
        private void DisplayRow(int rowNumber)
        {
            int rowsFromBot = this.Degree - rowNumber;
            // Writing leftmost padding.
            Console.ForegroundColor = ConsoleColor.Black;
            for (int j = 0; j < 3; j++)
            {
                Console.Write("\u2588");
            }


            // Displaying collumn 1.
            if(rowsFromBot <= this.Columns[0].Count)
            {
                DisplayBlock(this.Columns[0].ElementAt(this.Columns[0].Count - rowsFromBot));
            }
            else
            {
                DisplayBlock(0);
            }


            // Writing center-left padding.
            Console.ForegroundColor = ConsoleColor.Black;
            for (int j = 0; j < 3; j++)
            {
                Console.Write("\u2588");
            }


            // Displaying the second collumn.
            if(rowsFromBot <= this.Columns[1].Count)
            {
                DisplayBlock(this.Columns[1].ElementAt(this.Columns[1].Count - rowsFromBot));
            }
            else
            {
                DisplayBlock(0);
            }


            // Writing center-right padding.
            Console.ForegroundColor = ConsoleColor.Black;
            for (int j = 0; j < 3; j++)
            {
                Console.Write("\u2588");
            }


            // Displaying the last collumn.
            if(rowsFromBot <= this.Columns[2].Count)
            {
                DisplayBlock(this.Columns[2].ElementAt(this.Columns[2].Count - rowsFromBot));
            }
            else
            {
                DisplayBlock(0);
            }
        }



        /// <summary>
        /// Displays the towers and their blocks.
        /// </summary>
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


        /// <summary>
        /// Displays the towers and their numbers so the user can select a move to perform.
        /// </summary>
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


        /// <summary>
        /// Runs the menu where the user selects a move to perform. This is the main display method of the game.
        /// </summary>
        /// <returns>An integer array that describes the move selected by the user.</returns>
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

        //--------------------------Game logic--------------------------\\


        /// <summary>
        /// Determines if the move selected by the user is legal.
        /// </summary>
        /// <param name="move">Array describing the move selected by the user.</param>
        /// <returns>Boolean describing wether the move is legal.</returns>
        private bool IsValidMove(int[] move)
        {
            int movingBlock = this.Columns[move[0]].Peek();
            int targetBlock;

            if(this.Columns[move[1]].Count < 1) // Avoiding Stack.Empty return.
            {
                targetBlock = 0;
            }
            else
            {
                targetBlock = this.Columns[move[1]].Peek();
            }
            
            // Checking if the move is legal.
            if (movingBlock < targetBlock || targetBlock == 0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Checks if the towers are solved in their current arrangement.
        /// </summary>
        /// <returns>Boolean describing wether the game is solved.</returns>
        public bool IsSolved()
        {
            return this.Columns[2].Count == this.Degree;
        }


        /// <summary>
        /// Performs the move selceted by the user.
        /// </summary>
        /// <param name="move">Array describing the move.</param>
        private void Move(int[] move)
        {
            int movingBlock = this.Columns[move[0]].Pop();
            this.Columns[move[1]].Push(movingBlock);
        }


        /// <summary>
        /// Runs the game for a user until it is solved.
        /// </summary>
        public void Run()
        {
            while(!IsSolved())
            {
                Move(Display());
                this.CurrentMove ++;
                Console.ForegroundColor = ConsoleColor.White;
            }

            // Displaying the towers for the last time.
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(this.CurrentMove + "/" + this.MinimumMoves);
            Console.WriteLine();
            DisplayTowers();
            Thread.Sleep(1500);

            // Creating win message.
            string winMessage = string.Format("Congrats, you completed the puzzle in {0} moves!", this.CurrentMove);
            string movesAboveMin = string.Format("That's {0} moves above the minimum.", this.CurrentMove - this.MinimumMoves);

            // Writing win message.
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.WriteLine("\n\n");
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (winMessage.Length / 2)) + "}", winMessage));
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (movesAboveMin.Length / 2)) + "}", movesAboveMin));
            Console.ReadKey();
        }


        /// <summary>
        /// Finds a solution to the given arrangement of towers.
        /// </summary>
        /// <param name="degree">The number of blocks in the tower.</param>
        /// <param name="initialColumn">The collumn on which the blocks are stacked.</param>
        /// <param name="targetColumn">The collumn to which they should be moved.</param>
        /// <returns>List of moves to be performed in order to solve the towers.</returns>
        private List<int[]> SolveHanoi(int degree, int initialColumn, int targetColumn)
        {
            List<int[]> solution = new();
            int openColumn = 0;

            // Handling degree 1.
            if(degree == 1)
            {
                solution.Add(new int[] {initialColumn, targetColumn});
                return solution;
            }

            
            // Finding the open column.
            for (int i = 0; i < 3; i++)
            {
                if(i != initialColumn && i != targetColumn)
                {
                    openColumn = i;
                    break;
                }
            }

            // Handling high degree.
            solution.AddRange(SolveHanoi(degree - 1, initialColumn, openColumn));
            solution.Add(new int[] {initialColumn, targetColumn});
            solution.AddRange(SolveHanoi(degree - 1, openColumn, targetColumn));

            return solution;
        }


        /// <summary>
        /// Runs and displays the shortest solution to the towers.
        /// </summary>
        /// <param name="moveDelay">Time in miliseconds to wait between the displaying of each move.</param>
        public void SolveSelf(int moveDelay = 1000)
        {
            List<int[]> solution = SolveHanoi(this.Degree, 0, 2);

            // Looping through the moves in the solution.
            foreach (int[] move in solution)
            {
                Console.Clear();
                DisplayTowers();
                Move(move);
                Thread.Sleep(moveDelay);              
            }

            // Displaying the towers for the last time.
            Console.Clear();
            DisplayTowers();
            Thread.Sleep(2000);

            // Displaying a final message.
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            string finalMessage = "Towers were solved in " + this.MinimumMoves + " moves";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (finalMessage.Length / 2)) + "}", finalMessage));
        }
    }
}