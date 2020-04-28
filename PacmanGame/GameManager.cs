using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanGame
{
    //Handles running the game; Starting, ending, timing, score, input
    class GameManager
    {
        private Graphics g { get; set; }
        private DrawingManager Drawing;
        public BoardManager Board;

        public GameManager(Graphics g)
        {
            this.g = g;
            Drawing = new DrawingManager(g);
            Board = new BoardManager();
        }

        public void StartGame()
        {
            Board.Setup();
            Drawing.DrawBoard();
            Board.Pacman.DrawCharacter(g);
            Board.RedGhost.DrawCharacter(g);
            Board.BlueGhost.DrawCharacter(g);
            Board.OrangeGhost.DrawCharacter(g);
            Board.PinkGhost.DrawCharacter(g);
        }

        public bool StepGame()
        {
            Board.Pacman.ChooseMovement();
            Board.RedGhost.ChooseMovement();
            Board.BlueGhost.ChooseMovement();
            Board.OrangeGhost.ChooseMovement();
            Board.PinkGhost.ChooseMovement();
            return Drawing.UpdateBoard(Board);
        }
    }
}
