using System.Drawing;

namespace PacmanGame
{
    //Handles board data; player positions, refreshing graphics
    public class BoardManager
    {
        public Character Pacman { get; private set; }
        public Character BlueGhost { get; private set; }
        public Character OrangeGhost { get; private set; }
        public Character PinkGhost { get; private set; }
        public Character RedGhost { get; private set; }

        //Build Characters and set positions
        public void Setup()
        {
            for (int r = 0; r < Globals.GameBoard.GetLength(0); r++)
            {
                for (int c = 0; c < Globals.GameBoard.GetLength(1); c++)
                {
                    switch (Globals.GameBoard[r, c])
                    {
                        case (int)Globals.BlockTypes.Pacman:
                            Pacman = new Character { Row = r, Column = c, Y = r * DrawingManager.BLOCK_SIZE, X = c * DrawingManager.BLOCK_SIZE, Type = Globals.BlockTypes.Pacman, CharacterColor = Color.Yellow };
                            goto case -1;
                        case (int)Globals.BlockTypes.RedGhost:
                            RedGhost = new Character { Row = r, Column = c, Y = r * DrawingManager.BLOCK_SIZE, X = c * DrawingManager.BLOCK_SIZE, Type = Globals.BlockTypes.RedGhost, CharacterColor = Color.Red };
                            goto case -1;
                        case (int)Globals.BlockTypes.BlueGhost:
                            BlueGhost = new Character { Row = r, Column = c, Y = r * DrawingManager.BLOCK_SIZE, X = c * DrawingManager.BLOCK_SIZE, Type = Globals.BlockTypes.BlueGhost, CharacterColor = Color.DarkBlue };
                            goto case -1;
                        case (int)Globals.BlockTypes.PinkGhost:
                            PinkGhost = new Character { Row = r, Column = c, Y = r * DrawingManager.BLOCK_SIZE, X = c * DrawingManager.BLOCK_SIZE, Type = Globals.BlockTypes.PinkGhost, CharacterColor = Color.Pink };
                            goto case -1;
                        case (int)Globals.BlockTypes.OrangeGhost:
                            OrangeGhost = new Character { Row = r, Column = c, Y = r * DrawingManager.BLOCK_SIZE, X = c * DrawingManager.BLOCK_SIZE, Type = Globals.BlockTypes.OrangeGhost, CharacterColor = Color.Orange };
                            goto case -1;
                        case -1:
                            Globals.GameBoard[r, c] = (int)Globals.BlockTypes.Empty;
                            break;
                        default:
                            break;
                    }
                }
            }

            Globals.GameScore = 0;
        }

        public static bool ValidateMovement(Globals.Directions dir, int row, int column)
        {
            int rowInc = 0;
            int colInc = 0;
            switch (dir)
            {
                case Globals.Directions.Up:
                    rowInc++;
                    break;
                case Globals.Directions.Right:
                    colInc++;
                    break;
                case Globals.Directions.Down:
                    rowInc--;
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
