using System;
using System.Diagnostics;

namespace PacmanGame
{
    public class Character
    {
        //GameBoard Position
        public int Row { get; set; }
        public int Column { get; set; }
        //Drawing Position
        public int Y { get; set; }
        public int X { get; set; }
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
        public Globals.BlockTypes Type { get; set; }
        public System.Drawing.Color CharacterColor { get; set; }
        public bool HasCollided { get { return false; } }
        public Action<System.Drawing.Graphics,int> DrawCharacter;
        public Action ChooseMovement;
        public Action Move;

        public Character()
        {
            Row = 0;
            Column = 0;
            Y = 0;
            X = 0;
            Direction = Globals.Directions.Up;
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

        public override string ToString()
        {
            return string.Format("{0,-12} {1} {2} {3} {4} {5,-6} {6,-6}",
                Type.ToString(),Row.ToString("D2"),Column.ToString("D2"),
                X.ToString("D2"),Y.ToString("D2"),
                Direction.ToString(),OppositeDirection.ToString());
        }
    }
}
