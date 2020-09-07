using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace L20200902_Game_Of_Life
{
    class CellControl : Control
    {
        private int row, col;
        private CellControl[,] cells;
        private bool isAlive;
        private Lockable lockable;
        private bool nextGeneration;
        public CellControl(CellControl[,] cells, int row, int col, Lockable lockable)
        {
            
            this.row = row;
            this.col = col;
            this.cells = cells;
            isAlive = false;
            UpdateColor();
            this.Paint += CellControl_Paint;
            this.MouseClick += CellControl_MouseClick;
            this.lockable = lockable;
        }
        public void CalculateNextGeneration()
        {
            int aliveNeighbors = 0;
            int ROWS = cells.GetLength(0);

            //the row above me:
            if(row > 0)
            {
                if (col > 0)
                    if (cells[row - 1, col - 1].isAlive)
                        aliveNeighbors++;
                if (cells[row - 1, col].isAlive)
                    aliveNeighbors++;
                if (col < ROWS-1)//not last column
                    if (cells[row - 1, col + 1].isAlive)
                        aliveNeighbors++;
            }

            //my own row:
            if (col > 0)
                if (cells[row, col - 1].isAlive)
                    aliveNeighbors++;
            if (col < ROWS - 1)//not last column
                if (cells[row, col + 1].isAlive)
                    aliveNeighbors++;

            //the row beneath me
            if (row < ROWS - 1)
            {
                if (col > 0)
                    if (cells[row + 1, col - 1].isAlive)
                        aliveNeighbors++;
                if (cells[row + 1, col].isAlive)
                    aliveNeighbors++;
                if (col < ROWS - 1)//not last column
                    if (cells[row + 1, col + 1].isAlive)
                        aliveNeighbors++;
            }

            nextGeneration = isAlive;
            if (isAlive)
            {
                if (aliveNeighbors <= 1)
                    nextGeneration = false;//die from loneliness

                if (aliveNeighbors > 3)
                    nextGeneration = false;//die from over population.
            }
            else
            {
                if (aliveNeighbors == 3)
                    nextGeneration = true;

            }
        }

        public void MoveToNextGeneration()
        {
            isAlive = nextGeneration;
            UpdateColor();
        }
        private void CellControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (lockable.IsLocked())
                return;
            isAlive = !isAlive;
            UpdateColor();
        }

        

        private void UpdateColor()
        {
            BackColor = isAlive ? Color.Black : Color.White;
        }

        private void CellControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Gray, 0, 0, Width-1, Height-1);
        }
    }
}
