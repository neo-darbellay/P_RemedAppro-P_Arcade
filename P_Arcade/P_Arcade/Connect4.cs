using System;

namespace P_Arcade
{
    /// <summary>
    /// The classic Connect 4 game, ported to C# Console.
    /// This was originally made a year ago for the I319 module, but modified this year
    /// </summary>
    internal class Connect4 : Game
    {
        /// <summary>
        /// The current player
        /// </summary>
        static byte bytPlayer = 1;

        // Constants used for min/max of rows/columns
        const byte VAL_MIN_ROWS = 5;
        const byte VAL_MAX_ROWS = 13;
        const byte VAL_MIN_COLUMNS = 6;
        const byte VAL_MAX_COLUMNS = 16;

        // User input for rows and columns
        static byte bytRow = 0;
        static byte bytColumn = 0;

        /// <summary>
        /// Variables used as a fix to windows 11's terminal not having infinite scroll
        /// </summary>
        static byte bytLastRow = 0;

        /// <summary>
        /// Whether or not there is a second player
        /// </summary>
        static bool blnTwoPlayers = true;

        /// <summary>
        /// The second player (bot)'s level of thinking
        /// </summary>
        static byte bytBotSmartness = 1;

        // The game piece's X and Y position
        static byte bytCursorPosX = 0;
        const byte CURSOR_POS_Y = 6;

        /// <summary>
        /// A counter used to keep track of how many pieces have been placed during the game
        /// </summary>
        static byte bytCounter = 0;

        /// <summary>
        /// The Connect 4 game's constructor
        /// </summary>
        public Connect4() : base("Connect 4", false) { }

        /// <summary>
        /// Small fix for windows 11's terminal
        /// </summary>
        private void Windows11TerminalFix()
        {
            if (Console.CursorTop == Console.BufferHeight)
            {
                Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + 3);
            }
            else if (Console.CursorTop == Console.BufferHeight - 1)
            {
                Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + 2);
            }
            else if (Console.CursorTop == Console.BufferHeight - 2)
            {
                Console.SetBufferSize(Console.BufferWidth, Console.BufferHeight + 1);
            }
        }

        /// <summary>
        /// Getting the user input
        /// </summary>
        private void GetUserInput()
        {
            Arcade.ShowTitle(Name);

            // Ask the user for the number of rows they want
            Console.Write("   Please enter the number of rows you want.\nThe value needs to be greater than");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(VAL_MIN_ROWS);
            Console.ResetColor();

            Console.Write(" and smaller than ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(VAL_MAX_ROWS);
            Console.ResetColor();

            // Get the correct input
            GetInputInBounds(out bytRow, VAL_MIN_ROWS, VAL_MAX_ROWS);


            // Ask the user for the number of columns they want
            Console.Write("\n   Please enter the number of columns you want.\nThe value needs to be greater than ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(VAL_MIN_COLUMNS);
            Console.ResetColor();

            Console.Write(" and smaller than ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(VAL_MAX_COLUMNS);
            Console.ResetColor();

            // Get the correct input
            GetInputInBounds(out bytColumn, VAL_MIN_COLUMNS, VAL_MAX_COLUMNS);


            /* Bot related stuff, not useful right now

            // Ask the user if they want to play with two players
            bool blnVerification = false;

            while (!blnVerification)
            {
                Console.WriteLine("\n   Would you like to play with a second player? (Y / N)");

                Console.Write("   Your input: ");

                char chrAnswer = 'X';

                char.TryParse(Console.ReadLine(), out chrAnswer);


                switch (char.ToUpper(chrAnswer))
                {
                    case 'o':
                    case 'O':
                        blnTwoPlayers = true;
                        blnVerification = true;
                        break;

                    case 'n':
                    case 'N':
                        blnTwoPlayers = false;
                        blnVerification = true;
                        break;
                    default:
                        Console.WriteLine("\n   Wrong character found. Please type Y for yes, or N for no.");
                        break;
                }

                Windows11TerminalFix();
                
            }

            // Ask the user for the level of difficulty if they want to play against a bot
            if (!blnTwoPlayers)
            {
                Console.Write("\n   What level do you want the computer to be at?");

                bool blnVerification = false;

                while (!blnVerification)
                {
                    Console.WriteLine("\nPlease enter a number between 1 and 10, where 1 is the easiest level, and 10 is the hardest");

                    Console.Write("   Your input : ");

                    string strConsoleLine = Console.ReadLine();

                    if (byte.TryParse(strConsoleLine, out bytBotSmartness))
                    {
                        if (Enumerable.Range(1, 10).Contains(bytBotSmartness))
                        {
                            blnVerification = true;
                        }
                    }

                    Windows11TerminalFix();
                }
            }*/
        }

        public override void Start()
        {
            GetUserInput();

            // Clear the screen and add the title back
            Arcade.ShowTitle(Name);

            // Display the navigation grid (where the game piece moves before dropping)
            {
                // Initialize the last row with 5, as that's the number of free space the
                bytLastRow = 5;

                // (1/3)
                Console.Write("   ╔");
                for (byte x = 1; x < bytColumn; x++)
                {
                    Console.Write("═══╦");
                }
                Console.Write("═══╗\t    User guide");

                bytLastRow++;

                // (2/3)
                Console.Write("\n   ║");
                for (byte x = 1; x < bytColumn; x++)
                {
                    Console.Write("   ║");
                }
                Console.Write("   ║\t    -------------------");

                bytLastRow++;

                // (3/3)
                Console.Write("\n   ╚");
                for (byte x = 1; x < bytColumn; x++)
                {
                    Console.Write("═══╩");
                }
                Console.Write("═══╝\t        Movement\tDirectional keys");

                bytLastRow++;

                // Space used to for the user guide
                Console.Write("\n   ");
                for (byte x = 1; x < bytColumn; x++)
                {
                    Console.Write("    ");
                }
                Console.Write("    \t        Shoot\t\tSpacebar or Enter\n");

                bytLastRow++;

            }


            // First line of the grid
            {
                Console.Write("\t╔");
                for (byte x = 1; x < bytColumn; x++)
                {
                    Console.Write("═══╦");
                }
                Console.Write("═══╗\t        Quit\t\tEscape");

                bytLastRow++;
            }


            // Middle part of the grid
            for (byte x = 1; x < byte.MaxValue; x++)
            {
                if (x < bytRow)
                {
                    bytLastRow++;

                    Console.Write("\n   ║");
                    for (byte y = 0; y < bytColumn; y++)
                    {
                        Console.Write("   ║");
                    }

                    Console.Write("\n   ");
                    bytLastRow++;

                    Console.Write("╠");
                    for (byte y = 1; y < bytColumn; y++)
                    {
                        Console.Write("═══╬");
                    }
                    Console.Write("═══╣");

                }
                else if (x < 10)
                {

                    Console.Write("\n    ");
                    for (byte y = 0; y < bytColumn; y++)
                    {
                        Console.Write("    ");
                    }

                    Console.Write("\n\t ");
                    for (byte y = 1; y < bytColumn; y++)
                    {
                        Console.Write("    ");
                    }
                    Console.Write("    ");
                }

                // Continue the user guide on specific rows
                switch (x)
                {
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\t        Player 1: █");
                        if (blnTwoPlayers)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        }
                        Console.Write("\tPlayer 2: █");
                        Console.ResetColor();
                        break;

                    case 3:
                        if (!blnTwoPlayers)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\t\t\t\tComputer level");
                            Console.ResetColor();
                        }
                        break;

                    case 4:
                        if (!blnTwoPlayers)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\t\t\t\t----------------------");
                            Console.ResetColor();
                        }
                        break;

                    case 5:
                        if (!blnTwoPlayers)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\t\t\t\tEasy (1-3)");
                            Console.ResetColor();
                        }
                        break;

                    case 6:
                        if (!blnTwoPlayers)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\t\t\t\tMedium (4-6)");
                            Console.ResetColor();
                        }
                        break;

                    case 7:
                        if (!blnTwoPlayers)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\t\t\t\tHard (7+)");
                            Console.ResetColor();
                        }
                        break;

                    case 9:
                        if (!blnTwoPlayers)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\t\t\t\tCurrent computer level : {0}", bytBotSmartness);
                            Console.ResetColor();
                        }
                        Console.WriteLine("\n");
                        Console.SetCursorPosition(0, bytLastRow - 1);
                        break;

                    default:
                        break;
                }
            }

            // Last line of the grid
            {
                bytLastRow++;

                Console.Write("\n   ║");
                for (byte x = 0; x < bytColumn; x++)
                {
                    Console.Write("   ║");
                }

                Console.Write("\n   ╚═══");
                for (byte x = 1; x < bytColumn; x++)
                {
                    Console.Write("╩═══");
                }
                Console.Write("╝");
            }

            // The 2D grid used for the game's logic
            byte[,] GameGrid = new byte[bytRow, bytColumn];

            // Put the cursor inside of the navigation grid
            Console.SetCursorPosition(10, 6);
            Console.CursorVisible = false;

            // Reset variables used for the game
            Console.ForegroundColor = (bytPlayer == 1 ? ConsoleColor.Red : ConsoleColor.Yellow);

            bytPlayer = 1;
            bytCursorPosX = 0;
            bytCounter = 0;

            // Insert the piece
            Console.Write("█");

            // Game loop
            while (true)
            {
                // Check if there's two players, or if its player 1's turn
                if (blnTwoPlayers || bytPlayer == 1)
                {
                    ConsoleKey keyPressed = Console.ReadKey(true).Key;

                    bool blnPiecePlaced = false;

                    // Movement handling
                    switch (keyPressed)
                    {
                        case ConsoleKey.Backspace:
                        case ConsoleKey.Escape:
                            Console.SetCursorPosition(0, bytLastRow);
                            Console.Write("\n\n\n\n");
                            Restart_Game();
                            break;

                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            // If the cursor isn't all the way on the left
                            if (bytCursorPosX > 0)
                            {
                                // Erase previous piece
                                Console.SetCursorPosition(10 + bytCursorPosX * 4, CURSOR_POS_Y);
                                Console.Write("  ");

                                // Add new piece to the left
                                bytCursorPosX--;
                                Console.SetCursorPosition(10 + bytCursorPosX * 4, CURSOR_POS_Y);
                                Console.Write("█");
                            }
                            else
                            {
                                // Move the piece all the way to the right
                                Console.SetCursorPosition(10 + bytCursorPosX * 4, CURSOR_POS_Y);
                                Console.Write("  ");
                                bytCursorPosX = (byte)(bytColumn - 1);
                                Console.SetCursorPosition(10 + bytCursorPosX * 4, CURSOR_POS_Y);
                                Console.Write("█");
                            }
                            break;

                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            // If the cursor isn't all the way on the right
                            if (bytCursorPosX < bytColumn - 1)
                            {
                                // Erase previous piece
                                Console.SetCursorPosition(10 + bytCursorPosX * 4, CURSOR_POS_Y);
                                Console.Write("  ");

                                // Add new piece to the right
                                bytCursorPosX++;
                                Console.SetCursorPosition(10 + bytCursorPosX * 4, CURSOR_POS_Y);
                                Console.Write("█");
                            }
                            else
                            {
                                // Move the piece all the way to the left
                                Console.SetCursorPosition(10 + bytCursorPosX * 4, CURSOR_POS_Y);
                                Console.Write("  ");
                                bytCursorPosX = 0;
                                Console.SetCursorPosition(10 + bytCursorPosX * 4, CURSOR_POS_Y);
                                Console.Write("█");
                            }
                            break;

                        // Piece throwing system
                        case ConsoleKey.Spacebar:
                        case ConsoleKey.DownArrow:
                        case ConsoleKey.S:
                        case ConsoleKey.Enter:
                            // Drop the piece in the first empty line
                            for (int i = bytRow - 1; i >= 0; i--)
                            {
                                if (GameGrid[i, bytCursorPosX] == 0)
                                {
                                    GameGrid[i, bytCursorPosX] = bytPlayer;
                                    Console.SetCursorPosition(10 + bytCursorPosX * 4, 10 + (i * 2));
                                    Console.Write("█");

                                    blnPiecePlaced = true;
                                    break;
                                }
                            }
                            break;

                    }

                    // Victory check
                    if (blnPiecePlaced)
                    {
                        // Increment the piece counter
                        bytCounter++;
                        if (Check_Victory(GameGrid, bytPlayer, bytRow, bytColumn))
                        {
                            Console.SetCursorPosition(0, bytLastRow);
                            Console.Write("\n\n\n\n");
                            Console.ResetColor();

                            Console.Write("   Congratulations,  ");

                            Console.ForegroundColor = (bytPlayer == 1 ? ConsoleColor.Red : ConsoleColor.Yellow);
                            Console.Write("Player " + bytPlayer);
                            Console.ResetColor();

                            Console.Write(" ! You have won in" + bytCounter + " turns!\n");
                            Restart_Game();
                            break;
                        }

                        if (Grid_Full(GameGrid, bytRow, bytColumn))
                        {
                            Console.SetCursorPosition(0, bytLastRow);
                            Console.WriteLine("\n\n\n");
                            Console.ResetColor();

                            Console.Write("   It's a tie! The game grid is full.\n\n");
                            Restart_Game();
                            break;
                        }

                        // Alternate between player 1 and 2
                        bytPlayer = (byte)(bytPlayer == 1 ? 2 : 1);

                        // Update the piece's color
                        Console.ForegroundColor = (bytPlayer == 1 ? ConsoleColor.Red : blnTwoPlayers ? ConsoleColor.Yellow : ConsoleColor.Cyan);
                        Console.SetCursorPosition(10 + bytCursorPosX * 4, 6);
                        Console.Write("█");
                    }
                }

                // Bot's turn
                else
                {
                    /*

                        To be re-implemented

                    */
                }
            }
        }

        /// <summary>
        /// Get the user's input, and verify if it is in bound
        /// </summary>
        /// <param name="bytAnswer">The variable that will get changed</param>
        /// <param name="MIN_VALUE">Minimal value</param>
        /// <param name="MAX_VALUE">Maximum value</param>
        private void GetInputInBounds(out byte bytAnswer, byte MIN_VALUE, byte MAX_VALUE)
        {
            bool blnVerification = false;

            do
            {
                Console.Write("   Your input: ");

                bool blnResult = byte.TryParse(Console.ReadLine(), out bytAnswer);
                bool blnResultInBound = blnResult && (bytAnswer > MIN_VALUE) && (bytAnswer < MAX_VALUE);

                // Check if the value is correct
                if (blnResultInBound)
                {
                    blnVerification = true;
                }
                else if (blnResult)
                {
                    Console.Write("\n   Your value isn't between {0} and {1}, please retry.\n\n", MIN_VALUE, MAX_VALUE);
                }
                else
                {
                    Console.Write("\n   Your value isn't a number, bplease retry.\n\n");
                }

                Windows11TerminalFix();

            }
            while (!blnVerification);
        }

        /// <summary>
        /// Méthode utilisée pour la vérification de la victoire d'un des deux joueurs
        /// </summary>
        /// <param name="GameGrid">Le tableau bidirectionnel utilisé pour le fonctionnement du jeu</param>
        /// <param name="bytPlayer">Nombre du joueur actuel (1 ou 2)</param>
        /// <param name="bytAnswerLine">Nombre de lignes total</param>
        /// <param name="bytAnswerColumn">Nombre de colonnes total</param>
        /// <returns>Retourne true si un des deux joueurs ont gagné, sinon false</returns>
        private bool Check_Victory(byte[,] GameGrid, byte bytPlayer, byte bytAnswerLine, byte bytAnswerColumn)
        {
            // Vérifie si la taille de la grille permet une victoire
            if (bytAnswerLine < VAL_CONNECT || bytAnswerColumn < VAL_CONNECT)
                return false;



            /// Vérification horizontale ///

            // Parcourir chaque ligne de la grille
            for (byte bytLines = 0; bytLines < bytAnswerLine; bytLines++)
            {
                // Vérifier chaque colonne et s'arrête à bytAnswerColumn - (VAL_CONNECT - 1) pour ne pas
                // dépasser les limites du tableau en vérifiant les pions nécessaires
                for (byte bytColumns = 0; bytColumns <= bytAnswerColumn - VAL_CONNECT; bytColumns++)
                {
                    bool blnVictory = true; // Variable utilisée pour indiquer si les pions sont alignés horizontalement

                    for (int x = 0; x < VAL_CONNECT; x++)
                    {
                        // Si un pion ne correspond pas à celui du joueur actuel, l'alignement est interrompu
                        if (GameGrid[bytLines, bytColumns + x] != bytPlayer)
                        {
                            blnVictory = false;
                            break;
                        }
                    }
                    // Si tous les pions nécessaires sont alignés, retourne le booléen true
                    if (blnVictory) return true;
                }
            }

            /// Vérification verticale ///

            // Parcourir chaque colonne de la grille
            for (byte bytColumns = 0; bytColumns < bytAnswerColumn; bytColumns++)
            {
                // Vérifier chaque ligne et s'arrête à bytAnswerLine - (VAL_CONNECT - 1) pour ne pas
                // dépasser les limites du tableau en vérifiant les pions nécessaires
                for (byte bytLines = 0; bytLines <= bytAnswerLine - VAL_CONNECT; bytLines++)
                {
                    bool blnVictory = true; // Variable utilisée pour indiquer si les pions sont alignés verticalement
                    for (int x = 0; x < VAL_CONNECT; x++)
                    {
                        // Si un pion ne correspond pas à celui du joueur actuel, l'alignement est interrompu
                        if (GameGrid[bytLines + x, bytColumns] != bytPlayer)
                        {
                            blnVictory = false;
                            break;
                        }
                    }
                    // Si tous les pions nécessaires sont alignés, retourne "true"
                    if (blnVictory) return true;
                }
            }

            /// Vérification diagonale ///

            // Vérification de la diagonale haut-gauche à bas-droit
            for (byte bytLines = 0; bytLines <= bytAnswerLine - VAL_CONNECT; bytLines++)
            {
                // Vérifier chaque colonne et s'arrête à bytAnswerColumn - (VAL_CONNECT - 1) pour ne pas
                // dépasser les limites du tableau en vérifiant les pions nécessaires
                for (byte bytColumns = 0; bytColumns <= bytAnswerColumn - VAL_CONNECT; bytColumns++)
                {
                    bool blnVictory = true; // Variable utilisée pour indiquer si les pions sont alignés diagonalement

                    for (int x = 0; x < VAL_CONNECT; x++)
                    {
                        // Si un pion ne correspond pas à celui du joueur actuel, l'alignement est interrompu
                        if (GameGrid[bytLines + x, bytColumns + x] != bytPlayer)
                        {
                            blnVictory = false;
                            break;
                        }
                    }
                    // Si tous les pions nécessaires sont alignés, retourne "true"
                    if (blnVictory) return true;
                }
            }

            // Vérification de la diagonale haut-droite à bas-gauche
            for (byte bytLines = 0; bytLines <= bytAnswerLine - VAL_CONNECT; bytLines++)
            {
                // Vérifier chaque colonne et s'arrête avant VAL_CONNECT pour éviter les erreurs
                for (byte bytColumns = (byte)(VAL_CONNECT - 1); bytColumns < bytAnswerColumn; bytColumns++)
                {
                    bool blnVictory = true; // Variable utilisée pour indiquer si les pions sont alignés diagonalement

                    for (int x = 0; x < VAL_CONNECT; x++)
                    {
                        // Si un pion ne correspond pas à celui du joueur actuel, l'alignement est interrompu
                        if (GameGrid[bytLines + x, bytColumns - x] != bytPlayer)
                        {
                            blnVictory = false;
                            break;
                        }
                    }
                    // Si tous les pions nécessaires sont alignés, retourne "true"
                    if (blnVictory) return true;
                }
            }

            // Si aucune combinaison gagnante n'est trouvée, retourne "false"
            return false;

        }

        /// <summary>
        /// Méthode utilisée pour verifier que la grile n'est pas remplie complêtement
        /// </summary>
        /// <param name="GameGrid">Le tableau bidirectionnel utilisé pour le fonctionnement du jeu</param>
        /// <param name="bytAnswerLine">Nombre de lignes total</param>
        /// <param name="bytAnswerColumn">Nombre de colonnes total</param>
        /// <returns>Retourne true si la grile du jeu est remplie</returns>
        private bool Grid_Full(byte[,] GameGrid, byte bytAnswerLine, byte bytAnswerColumn)
        {
            // Parcourt chaque ligne de la grille
            for (byte bytLines = 0; bytLines < bytAnswerLine; bytLines++)
            {
                // Parcourt chaque colonne de la grille
                for (byte bytColumns = 0; bytColumns < bytAnswerColumn; bytColumns++)
                {
                    // Si une case est vide, cela signifie que la grille n'est pas pleine
                    if (GameGrid[bytLines, bytColumns] == 0)
                    {
                        return false;
                    }
                }
            }
            // Si aucune case vide n'est trouvée retourne true, (donc la grille est pleine)
            return true;

        }

        /// <summary>
        /// Méthode utilisée pour recommencer le jeu
        /// </summary>
        private void Restart_Game()
        {
            // Affiche le message pour savoir si l'utilisateur veut recommencer (fin du programme)
            Console.ResetColor();
            Console.CursorVisible = true;

            // Affichage de la question
            Console.Write("\tVoulez-vous recommencer ? (o / n): ");

            // Boucle do while qui permet de lire la réponse de l'utilisateur (pour recommencer le jeu)

            blnVerification = false;

            do
            {
                // Sauvegarde de la position du curseur avant la saisie
                int intLeft = Console.CursorLeft;
                int intTop = Console.CursorTop;

                // Si l'utilisateur appuie sur une touche, lire le caractère lié à la touche
                char chrRestartAnswer = Console.ReadKey().KeyChar;

                // Si l'utilisateur décide de recommencer le jeu
                if (char.ToUpper(chrRestartAnswer) == 'O')
                    Main();

                // Si l'utilisateur décide de ne pas recommencer le jeu (Fin du programme)
                else if (char.ToUpper(chrRestartAnswer) == 'N')
                {
                    Console.CursorVisible = false;
                    Console.Write("\n\tMerci d'avoir utilisé le programme.                                      \n");

                    // Ferme la console
                    Environment.Exit(0);
                }

                else
                {
                    Console.CursorVisible = false;

                    Console.Write("\n\tChoix invalide. Veuillez saisir 'o' pour recommencer ou 'n' pour quitter.");

                    // Revenir à la position (sauvegarde faite précédemment) pour ne pas réecrire choix invalide à chaque fois.
                    Console.SetCursorPosition(intLeft, intTop);

                    // Effacer l'ancien caractère
                    Console.Write(" ");

                    // Revenir à la position
                    Console.SetCursorPosition(intLeft, intTop);

                    Console.CursorVisible = true;
                }

                // Répeter tant que valueOkRestart = false (tant que l'utilisateur n'a pas appuyer sur  'o' 'O' ou 'n' 'N')
            } while (!blnVerification);
        }
    }
}
