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
            Board = new BoardManager(g);
        }

        public void StartGame()
        {
            Board.Setup();
            Drawing.DrawBoard();
        }

        public bool StepGame()
        {
            Board.MoveCharacter(Board.Pacman);
            Board.MoveCharacter(Board.RedGhost);
            Board.MoveCharacter(Board.BlueGhost);
            Board.MoveCharacter(Board.PinkGhost);
            Board.MoveCharacter(Board.OrangeGhost);
            return Drawing.UpdateBoard(Board);
            //DeltaTime(500);
        }

        public void DeltaTime(double milliseconds = 1000)
        {
            var start = DateTime.Now;
            var end = start.AddMilliseconds(milliseconds);
            while (DateTime.Now <= end) { }
        }
    }
}
