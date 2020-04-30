using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PacmanGame
{
    public class Character
    {
        //required for DrawCharacter to allow the optional parameter
        public delegate void OptionalParamAction(SpriteBatch spriteBatch, int step = 0);

        private Globals.BlockTypes _type;
        private Texture2D _characterSpriteSheet;

        public Action ChooseMovement;
        public OptionalParamAction DrawCharacter;

        //GameBoard Position
        public int Row { get; set; }
        public int Column { get; set; }

        //Drawing Position
        public int Y { get; set; }
        public int X { get; set; }

        //Movement Directionals
        //This is where the Character intends to move next
        public Globals.Directions Direction { get; set; }
        public Globals.Directions OppositeDirection
        {
            get
            {
                //faster to use a switch than a trig one-liner
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
                        DrawCharacter = DrawGhost;
                        ChooseMovement = GetMovementPacman;
                        break;
                    case Globals.BlockTypes.BlueGhost:
                        DrawCharacter = DrawGhost;
                        ChooseMovement = GetMovementPacman;
                        break;
                    case Globals.BlockTypes.PinkGhost:
                        DrawCharacter = DrawGhost;
                        ChooseMovement = GetMovementPacman;
                        break;
                    case Globals.BlockTypes.OrangeGhost:
                        DrawCharacter = DrawGhost;
                        ChooseMovement = GetMovementPacman;
                        break;
                    default:
                        break;
                }
            }
        }

        public Character(GraphicsDevice graphicsDevice, Texture2D spriteSheet, int spriteStartRow, int spriteStartColumn, int numberOfRows, int numberOfColumns)
        {
            _characterSpriteSheet = new Texture2D(graphicsDevice, numberOfColumns * Globals.BLOCK_SIZE, numberOfRows * Globals.BLOCK_SIZE);
            Color[] extractedRegion = new Color[numberOfColumns * Globals.BLOCK_SIZE * numberOfRows * Globals.BLOCK_SIZE];
            spriteSheet.GetData(0, new Rectangle(spriteStartColumn * Globals.BLOCK_SIZE, spriteStartRow * Globals.BLOCK_SIZE, numberOfColumns * Globals.BLOCK_SIZE, numberOfRows * Globals.BLOCK_SIZE), extractedRegion, 0, extractedRegion.Length);
            _characterSpriteSheet.SetData(extractedRegion);
            Direction = Globals.Directions.Up;
        }

        public void IncrementDrawingPosition()
        {
            switch (Direction)
            {
                case Globals.Directions.Up:
                    Y--;
                    break;
                case Globals.Directions.Right:
                    X++;
                    break;
                case Globals.Directions.Down:
                    Y++;
                    break;
                case Globals.Directions.Left:
                    X--;
                    break;
            }
        }

        public static bool HasCollided(List<Character> charList)
        {
            bool collision = false;

            var pacman = charList.Find(c => c.Type == Globals.BlockTypes.Pacman);
            var ghosts = charList.Where(c => c.Type != Globals.BlockTypes.Pacman).ToList();
            foreach (var item in ghosts)
            {
                if (pacman.X < item.X + Globals.BLOCK_SIZE && pacman.X >= item.X)
                {
                    if (pacman.Y < item.Y + Globals.BLOCK_SIZE && pacman.Y >= item.Y)
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
        private void DrawPacman(SpriteBatch spriteBatch, int step = 0)
        {
            //Since the sprite sheet for pacman is not an even shape
            if (step == 2)
                spriteBatch.Draw(_characterSpriteSheet, new Rectangle(X, Y, Globals.BLOCK_SIZE, Globals.BLOCK_SIZE), new Rectangle(step * 40, 0, 40, 40), Color.White);
            else
                spriteBatch.Draw(_characterSpriteSheet, new Rectangle(X, Y, Globals.BLOCK_SIZE, Globals.BLOCK_SIZE), new Rectangle(step * 40, (int)Direction * 40, 40, 40), Color.White);
        }
        private void DrawGhost(SpriteBatch spriteBatch, int step = 0)
        {
            spriteBatch.Draw(_characterSpriteSheet, new Rectangle(X, Y, Globals.BLOCK_SIZE, Globals.BLOCK_SIZE), new Rectangle(step * 40, (int)Direction * 40, 40, 40), Color.White);
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
                dir = Globals.RandSeed.Next(0, 6);
                //bias in favor of maintaining current direction
                if (dir >= 4) dir = (int)Direction;
                validMove = BoardManager.ValidateMovement((Globals.Directions)dir, Row, Column);
            }
            //found a valid move; update Character
            Direction = (Globals.Directions)dir;
            switch (Direction)
            {
                case Globals.Directions.Up:
                    Row--;
                    break;
                case Globals.Directions.Right:
                    Column++;
                    break;
                case Globals.Directions.Down:
                    Row++;
                    break;
                case Globals.Directions.Left:
                    Column--;
                    break;
            }
        }

        #endregion
    }
}
