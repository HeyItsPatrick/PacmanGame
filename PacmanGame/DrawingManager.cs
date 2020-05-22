using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanGame
{
        //drawInfo (timer,score,etc)
    public class DrawingManager
    {
        private BackgroundTile BkgdEmpty { get; set; }
        private BackgroundTile BkgdPellet { get; set; }
        private BackgroundTile BkgdWall { get; set; }

        public DrawingManager(GraphicsDevice graphicsDevice, Texture2D spriteSheet)
        {
            BkgdEmpty = new BackgroundTile(graphicsDevice, spriteSheet, 3, 6);
            BkgdPellet = new BackgroundTile(graphicsDevice, spriteSheet, 2, 6);
            BkgdWall = new BackgroundTile(graphicsDevice, spriteSheet, 1, 6);
        }

        public void DrawBoard(SpriteBatch spriteBatch)
        {
            for (int r = 0; r < Globals.GameBoard.GetLength(0); r++)
            {
                for (int c = 0; c < Globals.GameBoard.GetLength(1); c++)
                {
                    switch (Globals.GameBoard[r, c])
                    {
                        case (int)Globals.BlockTypes.Pellet:
                            BkgdPellet.Draw(spriteBatch, c * Globals.BLOCK_SIZE, r * Globals.BLOCK_SIZE);
                            break;
                        case (int)Globals.BlockTypes.Wall:
                            BkgdWall.Draw(spriteBatch, c * Globals.BLOCK_SIZE, r * Globals.BLOCK_SIZE);
                            break;
                        case (int)Globals.BlockTypes.Empty:
                        case (int)Globals.BlockTypes.Exterior:
                            BkgdEmpty.Draw(spriteBatch, c * Globals.BLOCK_SIZE, r * Globals.BLOCK_SIZE);
                            break;
                    }
                }
            }

        }

        public void DrawMenu(SpriteBatch spriteBatch, SpriteFont font)
        {
                    spriteBatch.DrawString(font, "Autonomous", new Vector2(290, 400), Color.White);
                    spriteBatch.DrawString(font, "PacMan", new Vector2(360, 450), Color.White);
                    spriteBatch.DrawString(font, "Press any key to start\nPress ESC to quit", new Vector2(200, 600), Color.White);
        }

        public void DrawWin(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, "You Win!", new Vector2(400, 400), Color.White);
        }

        public void DrawLose(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, "You Lose!", new Vector2(400, 400), Color.White);
        }
    }
}
