using System;

namespace HanoiTowers
{
    class Menu
    {
        private string? Prompt { get; }
        private int SelectedIndex { get; set; }

        private List<string> Options { get; }
        private List<string> TrueOptions { get; } = new List<string>();
        private int UpperPadding { get; }
        private int PromptPadding { get; }
        private ConsoleColor ForegroundColor { get; }
        private ConsoleColor BackgroundColor { get; }
        private ConsoleColor AccentColor { get; }

        /// <summary>
        /// Constructor for the Menu class. Has default values for padding and colors. Colors can be changed by
        /// passing separate ConsoleColor values.
        /// </summary>
        /// <param name="options">Options to be displayed to the user.</param>
        /// <param name="prompt">Prompt to be displayed at the top of the menu.</param>
        /// <param name="upperPadding">Padding between top of the console and prompt.</param>
        /// <param name="promptPadding">Padding between prompt and first option.</param>
        /// <param name="foregroundColor">Color of text. Default: Gray</param>
        /// <param name="backgroundColor">Color of the background of the console. Default: Black</param>
        /// <param name="accentColor">Color of the selected option and of the promt. Default: White</param>
        public Menu(
            List<string> options,
            string? prompt = null,
            int upperPadding = 2,
            int promptPadding = 1,
            ConsoleColor foregroundColor = ConsoleColor.Gray,
            ConsoleColor backgroundColor = ConsoleColor.Black,
            ConsoleColor accentColor = ConsoleColor.White)
        {
            this.Options = options;
            this.Prompt = prompt;
            this.UpperPadding = upperPadding;
            this.PromptPadding = promptPadding;
            this.ForegroundColor = foregroundColor;
            this.BackgroundColor = backgroundColor;
            this.AccentColor = accentColor;
            foreach(string option in options)
            {
                if(!string.IsNullOrWhiteSpace(option))
                {
                    this.TrueOptions.Add(option);
                }
            }
        }


        /// <summary>
        /// Constructor for the Menu class. Has default values for padding. Colors can be changed by passing an array of ConsoleColor values.
        /// The first color is the text color, the second is the background color, and the third is the accent color.
        /// </summary>
        /// <param name="options">Options to be displayed to the user.</param>
        /// <param name="prompt">Prompt to be displayed at the top of the menu.</param>
        /// <param name="upperPadding">Padding between top of the console and prompt.</param>
        /// <param name="promptPadding">Padding between prompt and first option.</param>
        /// <param name="colors"></param>
        public Menu(
            List<string> options,            
            string? prompt = null,
            int upperPadding = 2, 
            int promptPadding = 1,
            ConsoleColor[] colors = null!)
        {
            this.Options = options;
            this.ForegroundColor = colors[0];
            this.BackgroundColor = colors[1];
            this.AccentColor = colors[2];
            this.Prompt = prompt;
            this.UpperPadding = upperPadding;
            this.PromptPadding = promptPadding;

            foreach(string option in options)
            {
                if(!string.IsNullOrWhiteSpace(option))
                {
                    this.TrueOptions.Add(option);
                }
            }
        }


        /// <summary>
        /// Writes a string centered within the console.
        /// </summary>
        /// <param name="text">Text to be written.</param>
        private static void WriteCenteredText(string? text)
        {
            if(text == null)
            {
                return;
            }
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (text.Length / 2)) + "}", text));
        }


        /// <summary>
        /// Clears the console and displays the menu in its current state.
        /// </summary>
        private void DisplayMenu()
        {
            // Resetting the console.
            Console.Clear();
            Console.BackgroundColor = this.BackgroundColor;

            // Displaying the prompt.
            Console.ForegroundColor = this.AccentColor;
            for (int i = 0; i < this.UpperPadding; i++)
            {
                WriteCenteredText(string.Empty);
            }
            WriteCenteredText(this.Prompt);
            for (int i = 0; i < this.PromptPadding; i++)
            {
                WriteCenteredText(string.Empty);
            }

            // Displaying the options
            for (int i = 0; i < this.Options.Count; i++)
            {
                if(i == this.SelectedIndex)
                {
                    Console.ForegroundColor = this.AccentColor;
                    WriteCenteredText('<' + this.Options[i] + '>');
                }
                else
                {
                    Console.ForegroundColor = this.ForegroundColor;
                    WriteCenteredText(this.Options[i]);
                }
            }
        }


        /// <summary>
        /// Runs the menu and returns the option selected by the user.
        /// </summary>
        /// <returns>
        /// Integer representing the selected option. Ommits whitespace options
        /// </returns>
        public int Run()
        {
            this.SelectedIndex = 0;
            int trueSelectedIndex = 0;
            
            ConsoleKey keyPressed;
            do
            {
                SelectedIndex = Options.IndexOf(TrueOptions[trueSelectedIndex]);
                Console.Clear();
                DisplayMenu();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                // Move selected option depending on input.
                if(keyPressed == ConsoleKey.UpArrow)
                {
                    trueSelectedIndex --;
                }
                else if(keyPressed == ConsoleKey.DownArrow)
                {
                    trueSelectedIndex ++;
                }

                //Move to top or bottom if goes off edge.
                if(trueSelectedIndex == -1)
                {
                    trueSelectedIndex = this.TrueOptions.Count - 1;
                }
                else if(trueSelectedIndex == this.TrueOptions.Count)
                {
                    trueSelectedIndex = 0;
                }
            } while (keyPressed != ConsoleKey.Enter);
            return trueSelectedIndex;
        }
    }
}