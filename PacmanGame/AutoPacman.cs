using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PacmanGame
{
    //TODO: Character:  Add real ghost logic
    //TODO: Character:  GetMovementPacman: bias in favor of not turning around; may need to turn down currentDir bias along with introduction
    //TODO: Draw:       Figure out a different way to handle the chomp timing, current way isn't great
    //TODO: Game:       Build UI - Start Menu, Win, Loss, HUD
    //TODO: Game:       Set win condition upon all pellets being collected
    //TODO: Game:       Shortcut keys for new game
    //TODO: Game:       Toggle option for user-controlled pacman?

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class AutoPacman : Game
    {
        private int _step = 0;
        private int _increment = 1;

        private int GameScore { get; set; }
        private Globals.GameState GameState { get; set; }
        private GraphicsDeviceManager Graphics { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private SpriteFont ArialFont { get; set; }
        private SpriteFont ArialMenuFont { get; set; }
        private Texture2D SpriteSheet { get; set; }

        private BackgroundTile BkgdEmpty { get; set; }
        private BackgroundTile BkgdPellet { get; set; }
        private BackgroundTile BkgdWall { get; set; }

        private BoardManager Board { get; set; }
        private DrawingManager Drawing { get; set; }

        public AutoPacman()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Graphics.PreferredBackBufferWidth = 1000;
            Graphics.PreferredBackBufferHeight = 1000;
            Graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Window.Title = "Autonomous Pacman";
            GameState = Globals.GameState.Menu;
            GameScore = 0;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteSheet = Content.Load<Texture2D>("PacmanSpriteSheet");
            ArialFont = Content.Load<SpriteFont>("Arial");
            ArialMenuFont = Content.Load<SpriteFont>("ArialMenu");

            Board = new BoardManager(GraphicsDevice, SpriteSheet);
            Drawing = new DrawingManager(GraphicsDevice, SpriteSheet);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GameScore == 150)
                GameState = Globals.GameState.Win;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (GameState)
            {
                case Globals.GameState.Menu:
                    if (Keyboard.GetState().GetPressedKeys().Length!=0 && !Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        GameState = Globals.GameState.Running;
                        goto case Globals.GameState.Running;
                    }
                    break;
                case Globals.GameState.Running:
                    if (Character.HasCollided(new System.Collections.Generic.List<Character> { Board.Pacman, Board.BlueGhost, Board.OrangeGhost, Board.PinkGhost, Board.RedGhost }))
                    {
                        GameState = Globals.GameState.Lose;
                        break;
                    }

                    //Only choose new direction after finishing a full movement to a new tile
                    if (Board.Pacman.X == Board.Pacman.Column * 40 && Board.Pacman.Y == Board.Pacman.Row * 40)
                    {
                        if (Globals.GameBoard[Board.Pacman.Row, Board.Pacman.Column] == (int)Globals.BlockTypes.Pellet)
                        {
                            GameScore++;
                            Globals.GameBoard[Board.Pacman.Row, Board.Pacman.Column] = (int)Globals.BlockTypes.Empty;
                        }
                        Board.Pacman.ChooseMovement();
                    }

                    if (Board.BlueGhost.X == Board.BlueGhost.Column * 40 && Board.BlueGhost.Y == Board.BlueGhost.Row * 40)
                        Board.BlueGhost.ChooseMovement();
                    if (Board.OrangeGhost.X == Board.OrangeGhost.Column * 40 && Board.OrangeGhost.Y == Board.OrangeGhost.Row * 40)
                        Board.OrangeGhost.ChooseMovement();
                    if (Board.PinkGhost.X == Board.PinkGhost.Column * 40 && Board.PinkGhost.Y == Board.PinkGhost.Row * 40)
                        Board.PinkGhost.ChooseMovement();
                    if (Board.RedGhost.X == Board.RedGhost.Column * 40 && Board.RedGhost.Y == Board.RedGhost.Row * 40)
                        Board.RedGhost.ChooseMovement();
                    break;
                default:
                    if (Keyboard.GetState().GetPressedKeys().Length != 0)
                        Exit();
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            switch (GameState)
            {
                case Globals.GameState.Menu:
                    GraphicsDevice.Clear(Color.Black);
                    Drawing.DrawMenu(SpriteBatch, ArialMenuFont);
                    break;
                case Globals.GameState.Running:
                    GraphicsDevice.Clear(Color.Gray);
                    SpriteBatch.DrawString(ArialFont, "Score", new Vector2(850, 0), Color.Black);
                    SpriteBatch.DrawString(ArialFont, GameScore.ToString(), new Vector2(900, 30), Color.Black);
                    Drawing.DrawBoard(SpriteBatch);

                    Board.Pacman.DrawCharacter(SpriteBatch, (int)_step / 4);
                    Board.BlueGhost.DrawCharacter(SpriteBatch);
                    Board.OrangeGhost.DrawCharacter(SpriteBatch);
                    Board.PinkGhost.DrawCharacter(SpriteBatch);
                    Board.RedGhost.DrawCharacter(SpriteBatch);

                    //Uncomment for double time
                    Board.Pacman.IncrementDrawingPosition();
                    Board.BlueGhost.IncrementDrawingPosition();
                    Board.OrangeGhost.IncrementDrawingPosition();
                    Board.PinkGhost.IncrementDrawingPosition();
                    Board.RedGhost.IncrementDrawingPosition();
                    Board.Pacman.IncrementDrawingPosition();
                    Board.BlueGhost.IncrementDrawingPosition();
                    Board.OrangeGhost.IncrementDrawingPosition();
                    Board.PinkGhost.IncrementDrawingPosition();
                    Board.RedGhost.IncrementDrawingPosition();

                    _step += _increment;
                    if (_step == 11) _increment = -1;
                    if (_step == 0) _increment = 1;

                    break;
                case Globals.GameState.Win:
                    Drawing.DrawWin(SpriteBatch,ArialMenuFont);
                    break;
                case Globals.GameState.Lose:
                    Drawing.DrawLose(SpriteBatch,ArialMenuFont);
                    break;
                default:
                    break;
            }
            SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
