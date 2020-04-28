using System;
using System.Threading.Tasks;
using System.Windows.Forms;

//TODO: Set draw methods as data members of Character. Use delegates or something
//TODO: Set finding next move as data member of Character
//TODO: Use imagegrid and draw on top of gameBoard, that'll help make it less "flashy"
//      Keep GameBoard matrix for pellets and boundaries
//TODO: Set all the characters on their own threads, but collision detection will be a damn bitch, so maybe not
//TODO: Set win condition upon all pellets being collected

namespace PacmanGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            btnStartGame.Enabled = false;
            Task app = RunGame();
        }

        public async Task RunGame()
        {
            //TODO: after a time, open up the start zone to release other ghosts
            var g = CreateGraphics();
            var game = new GameManager(g);
            await Task.Run(game.StartGame);
            var gameOver = false;
            int counter = 0;
            while (!gameOver)
            {
                lblScoreValue.Text = Globals.GameScore.ToString();

                gameOver = await Task.Run(game.StepGame);
                label1.Text = game.Board.Pacman.ToString();
                //label2.Text = game.Board.RedGhost.ToString();
                //label3.Text = game.Board.BlueGhost.ToString();
                //label4.Text = game.Board.OrangeGhost.ToString();
                //label5.Text = game.Board.PinkGhost.ToString();
                counter++;
                //if (counter > 100) gameOver = true;
            }

            g.Dispose();
        }
    }
}
