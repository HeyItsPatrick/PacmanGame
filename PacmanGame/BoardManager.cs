using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace PacmanGame
{
    //Handles board data; player positions, refreshing graphics
    public class BoardManager
    {
        public Graphics g { get; set; }
        public Character Pacman { get; private set; }
        public Character BlueGhost { get; private set; }
        public Character OrangeGhost { get; private set; }
        public Character PinkGhost { get; private set; }
        public Character RedGhost { get; private set; }

        public BoardManager(Graphics g) => this.g = g;

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
                            Pacman.DrawCharacter = (g,a)=>DrawingManager.DrawPacMan(g,Pacman.X,Pacman.Y,a);
                            goto case -1;
                        case (int)Globals.BlockTypes.RedGhost:
                            RedGhost = new Character { Row = r, Column = c, Y = r * DrawingManager.BLOCK_SIZE, X = c * DrawingManager.BLOCK_SIZE, Type = Globals.BlockTypes.RedGhost,CharacterColor=Color.Red };
                            goto case -1;
                        case (int)Globals.BlockTypes.BlueGhost:
                            BlueGhost = new Character { Row = r, Column = c, Y = r * DrawingManager.BLOCK_SIZE, X = c * DrawingManager.BLOCK_SIZE, Type = Globals.BlockTypes.BlueGhost,CharacterColor=Color.Blue};
                            goto case -1;
                        case (int)Globals.BlockTypes.PinkGhost:
                            PinkGhost = new Character { Row = r, Column = c, Y = r * DrawingManager.BLOCK_SIZE, X = c * DrawingManager.BLOCK_SIZE, Type = Globals.BlockTypes.PinkGhost,CharacterColor=Color.Pink };
                            goto case -1;
                        case (int)Globals.BlockTypes.OrangeGhost:
                            OrangeGhost = new Character { Row = r, Column = c, Y = r * DrawingManager.BLOCK_SIZE, X = c * DrawingManager.BLOCK_SIZE, Type = Globals.BlockTypes.OrangeGhost,CharacterColor=Color.Orange };
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

        public void MoveCharacter(Character c) 
        {
            //Choose direction
            bool validMove = false;
            int dir = 0;
            //validate the move
            //if valid, move and update board position
            //if invalid, check another direction
            while (!validMove)
            {
                dir = Globals.RandSeed.Next(1, 10);
                //bias in favor of maintaining current direction
                if (dir >= 5) dir = (int)c.Direction;
                //TODO: bias in favor of not turning around; may need to turn down currentDir bias along with introduction
                validMove = ValidateMovement((Globals.Directions)dir, c);
            }
            //found a valid move; update Character
            c.Direction = (Globals.Directions)dir;
            switch (c.Direction)
            {
                case Globals.Directions.Up:
                    c.Row++;
                    break;
                case Globals.Directions.Right:
                    c.Column++;
                    break;
                case Globals.Directions.Down:
                    c.Row--;
                    break;
                case Globals.Directions.Left:
                    c.Column--;
                    break;
            }
        }

        private bool ValidateMovement(Globals.Directions dir, Character c)
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
            return Globals.GameBoard[c.Row + rowInc, c.Column + colInc] != (int)Globals.BlockTypes.Wall
            && Globals.GameBoard[c.Row + rowInc, c.Column + colInc] != (int)Globals.BlockTypes.Exterior;
        }
    }
}
