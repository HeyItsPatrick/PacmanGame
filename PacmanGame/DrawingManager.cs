using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanGame
{
    //Handles drawing characters and the board
    public class DrawingManager
    {
        public const int BLOCK_SIZE = 24;
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
                        case (int)Globals.BlockTypes.Pacman:
                            //DrawPacMan(c*BLOCK_SIZE,r*BLOCK_SIZE);
                            break;
                        case (int)Globals.BlockTypes.Empty:
                            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(c * BLOCK_SIZE, r * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                            break;
                        case (int)Globals.BlockTypes.RedGhost:
                            g.FillRectangle(new SolidBrush(Color.Red), new Rectangle(c * BLOCK_SIZE, r * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                            break;
                        case (int)Globals.BlockTypes.BlueGhost:
                            g.FillRectangle(new SolidBrush(Color.Aqua), new Rectangle(c * BLOCK_SIZE, r * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                            break;
                        case (int)Globals.BlockTypes.PinkGhost:
                            g.FillRectangle(new SolidBrush(Color.Pink), new Rectangle(c * BLOCK_SIZE, r * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                            break;
                        case (int)Globals.BlockTypes.OrangeGhost:
                            g.FillRectangle(new SolidBrush(Color.Orange), new Rectangle(c * BLOCK_SIZE, r * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
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
                }

                //DrawPacMan(manager.Pacman.X, manager.Pacman.Y, i);
                manager.Pacman.DrawCharacter(g,i);
                DrawGhost(manager.RedGhost);
                DrawGhost(manager.BlueGhost);
                DrawGhost(manager.OrangeGhost);
                DrawGhost(manager.PinkGhost);
                //DeltaTime(7);
                DeltaTime(5);
                var collision = CheckCollision(charList);
                if (collision) return true;
            }

            
                if (Globals.GameBoard[manager.Pacman.Row, manager.Pacman.Column] == (int)Globals.BlockTypes.Pellet)
                {
                    Globals.GameScore++;
                    Globals.GameBoard[manager.Pacman.Row, manager.Pacman.Column] = (int)Globals.BlockTypes.Empty;
                }
            return false;
        }

        private bool CheckCollision(List<Character> charList)
        {
            //not dir dependent since youre checking the whole hitbox space
            bool collision = false;

            var pacman = charList.Find(c => c.Type == Globals.BlockTypes.Pacman);
            var ghosts = charList.Where(c => c.Type != Globals.BlockTypes.Pacman).ToList();
            foreach (var item in ghosts)
            {
                if (pacman.X < item.X + BLOCK_SIZE && pacman.X >= item.X)
                {
                    if (pacman.Y < item.Y + BLOCK_SIZE && pacman.Y >= item.Y)
                    {
                        collision = true;
                        break;
                    }
                }

            }
            return collision;
        }

        private void RedrawTile(int r, int c)
        {
            switch (Globals.GameBoard[r, c])
            {
                case (int)Globals.BlockTypes.Pellet:
                    g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(c*BLOCK_SIZE, r*BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                    g.FillEllipse(new SolidBrush(Color.Yellow), new Rectangle(c * BLOCK_SIZE + (3 * BLOCK_SIZE / 8), r * BLOCK_SIZE + (3 * BLOCK_SIZE / 8), BLOCK_SIZE / 4, BLOCK_SIZE / 4));
                    break;
                case (int)Globals.BlockTypes.Wall:
                    g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(c*BLOCK_SIZE, r*BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                    break;
                case (int)Globals.BlockTypes.Empty:
                    g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(c*BLOCK_SIZE,r*BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE));
                    break;
            }
        }

        public static void DrawPacMan(Graphics g, int x, int y, int step = 0)
        {
            g.FillRectangle(new SolidBrush(Color.Yellow), new Rectangle(x, y, BLOCK_SIZE, BLOCK_SIZE));
        }

        public void DrawGhost(Character ghost)
        {
            g.FillRectangle(new SolidBrush(ghost.CharacterColor), new Rectangle(ghost.X, ghost.Y, BLOCK_SIZE, BLOCK_SIZE));
            return;
            //var brush = new SolidBrush(ghost.CharacterColor);
            //g.FillEllipse(brush, new Rectangle(ghost.X, ghost.Y, BLOCK_SIZE, BLOCK_SIZE));
            //g.FillRectangle(brush, new Rectangle(ghost.X,ghost.Y+ (BLOCK_SIZE / 2), BLOCK_SIZE, BLOCK_SIZE / 2));
            //brush = new SolidBrush(Color.Black);
            //for (int i = 0; i <= 2 * BLOCK_SIZE / 3; i += (BLOCK_SIZE / 3))
            //{
            //    Point[] pointArr = { new Point(ghost.X+i, ghost.Y+BLOCK_SIZE), new Point(ghost.X+i+(BLOCK_SIZE/6), ghost.Y+BLOCK_SIZE - (BLOCK_SIZE / 3)), new Point(ghost.X+i+(BLOCK_SIZE / 3),ghost.Y+ BLOCK_SIZE) };
            //    g.FillPolygon(brush,pointArr);
            //}
            //brush = new SolidBrush(Color.White);
            //for (int i = 2 * BLOCK_SIZE / 8; i <= 6 * BLOCK_SIZE / 8; i += 3 * BLOCK_SIZE / 8)
            //{
            //    g.FillRectangle(brush, new Rectangle(ghost.X+i,ghost.Y+ (BLOCK_SIZE / 4), BLOCK_SIZE / 5, BLOCK_SIZE / 5));
            //    var x = (int)Math.Cos((((int)ghost.Direction) * Math.PI) / 2) * (BLOCK_SIZE / 20);
            //    var y = (int)Math.Sin((((int)ghost.Direction) * Math.PI) / 2) * (BLOCK_SIZE / 20);
            //    g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(ghost.X+i + (BLOCK_SIZE / 20) - x, ghost.Y+(BLOCK_SIZE / 4) + (BLOCK_SIZE / 20) - y, BLOCK_SIZE / 10, BLOCK_SIZE / 10));
            //}

        }
        public void DeltaTime(double milliseconds = 1000)
        {
            var start = DateTime.Now;
            var end = start.AddMilliseconds(milliseconds);
            while (DateTime.Now <= end) { }
        }

    }
}
