using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PacmanGame
{
    public class BackgroundTile
    {
        private Texture2D Texture { get; set; }

        public BackgroundTile(GraphicsDevice graphicsDevice, Texture2D spriteSheet, int spriteRow, int spriteColumn)
        {
            Texture = new Texture2D(graphicsDevice, Globals.BLOCK_SIZE, Globals.BLOCK_SIZE);
            Color[] extractedRegion = new Color[Globals.BLOCK_SIZE * Globals.BLOCK_SIZE];
            spriteSheet.GetData(0, new Rectangle(spriteColumn * Globals.BLOCK_SIZE, spriteRow * Globals.BLOCK_SIZE, Globals.BLOCK_SIZE, Globals.BLOCK_SIZE), extractedRegion, 0, extractedRegion.Length);
            Texture.SetData(extractedRegion);
        }
        
        public void Draw(SpriteBatch spriteBatch, int x, int y) => spriteBatch.Draw(Texture, new Rectangle(x, y, Globals.BLOCK_SIZE, Globals.BLOCK_SIZE), Color.White);
    }
}
