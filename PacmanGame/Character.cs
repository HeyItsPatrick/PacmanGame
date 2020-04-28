using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PacmanGame
{

    public class Character
    {
        public delegate void OptionalParamAction(Graphics g, int step = 0);

        private Globals.BlockTypes _type;

        public Color CharacterColor { get; set; }
        public Action ChooseMovement;
        public OptionalParamAction DrawCharacter;

        //GameBoard Position
        public int Row { get; set; }
        public int Column { get; set; }

        //Drawing Position
        public int Y { get; set; }
        public int X { get; set; }

        //Movement Directionals; This is where the Character intends to move
        public Globals.Directions Direction { get; set; }
        public Globals.Directions OppositeDirection
        {
            get
            {
                //return (Globals.Directions)((int)Direction + (2 * ((int)Math.Sin((((int)Direction - 1) * Math.PI) / 2) + (int)Math.Cos((((int)Direction - 1) * Math.PI) / 2))));
                //faster to use a switch than a bunch of trig
                switch (Direction)
                {
                    case Globals.Directions.Up:
                        return Globals.Directions.Down;
                    case Globals.Directions.Right:
                        return Globals.Directions.Left;
                    case Globals.Directions.Down:
                        return Globals.Directions.Up;
                    case Globals.Directions.Left:
                        return Globals.Directions.Right;
                    default:
                        return Globals.Directions.Up;
                }
            }
        }

        //Actually need real mutators so the "set" operations can be done
        //Otherwise, there is a StackOverflow from recursive "set" calls, it seems
        public Globals.BlockTypes Type
        {
            get => _type;
            set
            {
                _type = value;
                switch (Type)
                {
                    case Globals.BlockTypes.Pacman:
                        DrawCharacter = DrawPacman;
                        ChooseMovement = GetMovementPacman;
                        break;
                    case Globals.BlockTypes.RedGhost:
                        DrawCharacter = DrawPacman;
                        ChooseMovement = GetMovementPacman;
                        break;
                    case Globals.BlockTypes.BlueGhost:
                        DrawCharacter = DrawPacman;
                        ChooseMovement = GetMovementPacman;
                        break;
                    case Globals.BlockTypes.PinkGhost:
                        DrawCharacter = DrawPacman;
                        ChooseMovement = GetMovementPacman;
                        break;
                    case Globals.BlockTypes.OrangeGhost:
                        DrawCharacter = DrawPacman;
                        ChooseMovement = GetMovementPacman;
                        break;
                    default:
                        break;
                }
            }
        }

        public Character()
        {
            Row = 0;
            Column = 0;
            Y = 0;
            X = 0;
        }

        public void IncrementDrawingPosition()
        {
            switch (Direction)
            {
                case Globals.Directions.Up:
                    Y++;
                    break;
                case Globals.Directions.Right:
                    X++;
                    break;
                case Globals.Directions.Down:
                    Y--;
                    break;
                case Globals.Directions.Left:
                    X--;
                    break;
            }
        }

        public bool HasCollided(List<Character> charList)
        {
            bool collision = false;

            var pacman = charList.Find(c => c.Type == Globals.BlockTypes.Pacman);
            var ghosts = charList.Where(c => c.Type != Globals.BlockTypes.Pacman).ToList();
            foreach (var item in ghosts)
            {
                if (pacman.X < item.X + DrawingManager.BLOCK_SIZE && pacman.X >= item.X)
                {
                    if (pacman.Y < item.Y + DrawingManager.BLOCK_SIZE && pacman.Y >= item.Y)
                    {
                        collision = true;
                        break;
                    }
                }

            }
            return collision;
        }

        public override string ToString()
        {
            return string.Format("{0,-12} {1} {2} {3} {4} {5,-6} {6,-6}",
                Type.ToString(), Row.ToString("D2"), Column.ToString("D2"),
                X.ToString("D2"), Y.ToString("D2"),
                Direction.ToString(), OppositeDirection.ToString());
        }

        #region Private Action Methods
        private void DrawPacman(Graphics g, int step = 0)
        {
            g.FillRectangle(new SolidBrush(CharacterColor), new Rectangle(X, Y, DrawingManager.BLOCK_SIZE, DrawingManager.BLOCK_SIZE));
        }
        private void DrawGhost(Graphics g, int step=0)
        {
            g.FillRectangle(new SolidBrush(CharacterColor), new Rectangle(X, Y, DrawingManager.BLOCK_SIZE, DrawingManager.BLOCK_SIZE));
            //Below works, but there is too much imprecision to the way it is drawn, so colors and old positions bleed through
            //Just use an image grid for graphics with all characters
            //Maybe pass down the image from the Form level, if there is an issue with System.Drawing not just laying on top

            //var blockSize = DrawingManager.BLOCK_SIZE;
            //var brush = new SolidBrush(CharacterColor);
            //g.FillEllipse(brush, new Rectangle(X, Y, blockSize, blockSize));
            //g.FillRectangle(brush, new Rectangle(X, Y + (blockSize / 2), blockSize, blockSize / 2));
            //brush = new SolidBrush(Color.Black);
            //for (int i = 0; i <= 2 * blockSize / 3; i += (blockSize / 3))
            //{
            //    Point[] pointArr = { new Point(X + i, Y + blockSize), new Point(X + i + (blockSize / 6), Y + blockSize - (blockSize / 3)), new Point(X + i + (blockSize / 3), Y + blockSize) };
            //    g.FillPolygon(brush, pointArr);
            //}
            //brush = new SolidBrush(Color.White);
            //for (int i = 2 * blockSize / 8; i <= 6 * blockSize / 8; i += 3 * blockSize / 8)
            //{
            //    g.FillRectangle(brush, new Rectangle(X + i, Y + (blockSize / 4), blockSize / 5, blockSize / 5));
            //    var x = (int)Math.Cos((((int)Direction) * Math.PI) / 2) * (blockSize / 20);
            //    var y = (int)Math.Sin((((int)Direction) * Math.PI) / 2) * (blockSize / 20);
            //    g.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(X + i + (blockSize / 20) - x, Y + (blockSize / 4) + (blockSize / 20) - y, blockSize / 10, blockSize / 10));
            //}
        }

        private void GetMovementPacman()
        {
            //Choose direction
            bool validMove = false;
            int dir = 0;
            //validate the move
            //if valid, move and update board position
            //if invalid, check another direction
            while (!validMove)
            {
                dir = Globals.RandSeed.Next(1, 6);
                //bias in favor of maintaining current direction
                if (dir >= 5) dir = (int)Direction;
                //TODO: bias in favor of not turning around; may need to turn down currentDir bias along with introduction
                validMove = BoardManager.ValidateMovement((Globals.Directions)dir, Row, Column);
            }
            //found a valid move; update Character
            Direction = (Globals.Directions)dir;
            switch (Direction)
            {
                case Globals.Directions.Up:
                    Row++;
                    break;
                case Globals.Directions.Right:
                    Column++;
                    break;
                case Globals.Directions.Down:
                    Row--;
                    break;
                case Globals.Directions.Left:
                    Column--;
                    break;
            }
        }

        #endregion
    }
}
