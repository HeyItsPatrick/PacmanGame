using System;
using System.Collections.Generic;
using System.Drawing;

namespace PacmanGame
{
    public class DrawingManager
    {
        public const int BLOCK_SIZE = 40;
        private Graphics g { get; set; }

        public DrawingManager(Graphics g) => this.g = g;

        public void DrawBoard()
        {
            for (int r = 0; r < Globals.GameBoard.GetLength(0); r++)
            {
                for (int c = 0; c < Globals.GameBoard.GetLength(1); c++)
                {
                    switch (Globals.GameBoard[r, c])
                    {
                        case (int)Globals.BlockTypes.Pellet:
                            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(c * BLOCK_SIZE, r * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                            g.FillEllipse(new SolidBrush(Color.Yellow), new Rectangle(c * BLOCK_SIZE + (3 * BLOCK_SIZE / 8), r * BLOCK_SIZE + (3 * BLOCK_SIZE / 8), BLOCK_SIZE / 4, BLOCK_SIZE / 4));
                            break;
                        case (int)Globals.BlockTypes.Wall:
                            g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(c * BLOCK_SIZE, r * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                            break;
                        case (int)Globals.BlockTypes.Empty:
                            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(c * BLOCK_SIZE, r * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                            break;
                        case (int)Globals.BlockTypes.Exterior:
                            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(c * BLOCK_SIZE, r * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                            break;
                    }
                }
            }
        }

        public bool UpdateBoard(BoardManager manager)
        {
            //check cardinal dirs off of ghosts and pacman, locally redraw the zone
            var charList = new List<Character>() { manager.Pacman, manager.RedGhost, manager.BlueGhost, manager.OrangeGhost, manager.PinkGhost };
            //Process: frame by frame, step every char in their dir. After frame step, check collision
            for (int i = 1; i <= BLOCK_SIZE; i++)
            {
                foreach (var item in charList)
                {
                    item.IncrementDrawingPosition();
                    //clean square in opposite direction, so there are no trailing afterimages
                    switch (item.OppositeDirection)
                    {
                        case Globals.Directions.Up:
                            RedrawTile(item.Row + 1, item.Column);
                            break;
                        case Globals.Directions.Right:
                            RedrawTile(item.Row, item.Column + 1);
                            break;
                        case Globals.Directions.Down:
                            RedrawTile(item.Row - 1, item.Column);
                            break;
                        case Globals.Directions.Left:
                            RedrawTile(item.Row, item.Column - 1);
                            break;
                    }
                    item.DrawCharacter(g, i);
                }

                DeltaTime(5);
                if (manager.Pacman.HasCollided(charList)) return true;
            }


            if (Globals.GameBoard[manager.Pacman.Row, manager.Pacman.Column] == (int)Globals.BlockTypes.Pellet)
            {
                Globals.GameScore++;
                Globals.GameBoard[manager.Pacman.Row, manager.Pacman.Column] = (int)Globals.BlockTypes.Empty;
            }
            return false;
        }

        private void RedrawTile(int r, int c)
        {
            switch (Globals.GameBoard[r, c])
            {
                case (int)Globals.BlockTypes.Pellet:
                    g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(c * BLOCK_SIZE, r * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                    g.FillEllipse(new SolidBrush(Color.Yellow), new Rectangle(c * BLOCK_SIZE + (3 * BLOCK_SIZE / 8), r * BLOCK_SIZE + (3 * BLOCK_SIZE / 8), BLOCK_SIZE / 4, BLOCK_SIZE / 4));
                    break;
                case (int)Globals.BlockTypes.Wall:
                    g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(c * BLOCK_SIZE, r * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                    break;
                case (int)Globals.BlockTypes.Empty:
                    g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(c * BLOCK_SIZE, r * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                    break;
            }
        }

        private void DeltaTime(double milliseconds = 1000)
        {
            var start = DateTime.Now;
            var end = start.AddMilliseconds(milliseconds);
            while (DateTime.Now <= end) { }
        }
    }
}
