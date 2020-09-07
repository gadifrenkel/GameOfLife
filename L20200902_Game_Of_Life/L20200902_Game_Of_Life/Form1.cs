using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Threading;

namespace L20200902_Game_Of_Life
{
    public partial class Form1 : Form, Lockable
    {
        public const int ROWS = 10;
        private CellControl[,] cells;
        private Button btnStart;
        private bool locked;
        private bool go;

        public Form1()
        {
            cells = new CellControl[ROWS,ROWS];
            locked = false;
            InitializeComponent();
            go = true;
        }



        
        public bool IsLocked()
        {
            return locked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            const int CELL_SIZE = 25;//the size of the cell in pixels
            int leftMargin = (this.Width - CELL_SIZE * ROWS) / 2;
            int y = 50;
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < ROWS; j++)
                {
                    CellControl cellControl = new CellControl(cells, i, j, this);
                    cellControl.Top = y;
                    cellControl.Left = leftMargin + j * CELL_SIZE;
                    cellControl.Width = CELL_SIZE;
                    cellControl.Height = CELL_SIZE;
                    cells[i, j] = cellControl;
                    Controls.Add(cellControl);
                }
                y += CELL_SIZE;
            }
            btnStart = new Button();
            btnStart.Text = "Start";
            btnStart.Top = Height - btnStart.Height - 50;
            btnStart.Left = (Width - btnStart.Width) / 2;
            Controls.Add(btnStart);
            btnStart.Click += BtnStart_Click;



        }

        private void NextGeneration()
        {
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < ROWS; j++)
                {
                    cells[i, j].CalculateNextGeneration();
                }
            }
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < ROWS; j++)
                {
                    cells[i, j].MoveToNextGeneration();
                }
            }

        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            locked = true;
            btnStart.Enabled = false;
            Thread thread = new Thread(()=> {
                while (go)
                {
                    Thread.Sleep(500);
                    Action action = new Action(() => {
                        this.NextGeneration();
                    });
                    //this.Invoke(action);

                    Dispatcher.CurrentDispatcher.Invoke(() =>

                    {
                        action();
                    });

                }
            });
            thread.Start();
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            go = false;
        }
    }

    public interface Lockable
    {
        bool IsLocked();
    }
}
