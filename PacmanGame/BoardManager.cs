using Microsoft.Xna.Framework.Graphics;

namespace PacmanGame
{
    public class BoardManager
    {
        public Character Pacman { get; private set; }
        public Character BlueGhost { get; private set; }
        public Character OrangeGhost { get; private set; }
        public Character PinkGhost { get; private set; }
        public Character RedGhost { get; private set; }

        public BoardManager(GraphicsDevice graphicsDevice, Texture2D spriteSheet)
        {
            for (int r = 0; r < Globals.GameBoard.GetLength(0); r++)
            {
                for (int c = 0; c < Globals.GameBoard.GetLength(1); c++)
                {
                    switch (Globals.GameBoard[r, c])
                    {
                        case (int)Globals.BlockTypes.Pacman:
                            Pacman = new Character(graphicsDevice, spriteSheet, 0, 4, 4, 3) { Row = r, Column = c, Y = r * Globals.BLOCK_SIZE, X = c * Globals.BLOCK_SIZE, Type = Globals.BlockTypes.Pacman };
                            goto case -1;
                        case (int)Globals.BlockTypes.BlueGhost:
                            BlueGhost = new Character(graphicsDevice, spriteSheet, 0, 2, 4, 1) { Row = r, Column = c, Y = r * Globals.BLOCK_SIZE, X = c * Globals.BLOCK_SIZE, Type = Globals.BlockTypes.BlueGhost };
                            goto case -1;
                        case (int)Globals.BlockTypes.OrangeGhost:
                            OrangeGhost = new Character(graphicsDevice, spriteSheet, 0, 3, 4, 1) { Row = r, Column = c, Y = r * Globals.BLOCK_SIZE, X = c * Globals.BLOCK_SIZE, Type = Globals.BlockTypes.OrangeGhost };
                            goto case -1;
                        case (int)Globals.BlockTypes.PinkGhost:
                            PinkGhost = new Character(graphicsDevice, spriteSheet, 0, 1, 4, 1) { Row = r, Column = c, Y = r * Globals.BLOCK_SIZE, X = c * Globals.BLOCK_SIZE, Type = Globals.BlockTypes.PinkGhost };
                            goto case -1;
                        case (int)Globals.BlockTypes.RedGhost:
                            RedGhost = new Character(graphicsDevice, spriteSheet, 0, 0, 4, 1) { Row = r, Column = c, Y = r * Globals.BLOCK_SIZE, X = c * Globals.BLOCK_SIZE, Type = Globals.BlockTypes.RedGhost };
                            goto case -1;
                        case -1:
                            Globals.GameBoard[r, c] = (int)Globals.BlockTypes.Empty;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static bool ValidateMovement(Globals.Directions dir, int row, int column)
        {
            int rowInc = 0;
            int colInc = 0;
            switch (dir)
            {
                case Globals.Directions.Up:
                    rowInc--;
                    break;
                case Globals.Directions.Right:
                    colInc++;
                    break;
                case Globals.Directions.Down:
                    rowInc++;
                    break;
                case Globals.Directions.Left:
                    colInc--;
                    break;
            }
            return Globals.GameBoard[row + rowInc, column + colInc] != (int)Globals.BlockTypes.Wall
            && Globals.GameBoard[row + rowInc, column + colInc] != (int)Globals.BlockTypes.Exterior;
        }
    }
}
