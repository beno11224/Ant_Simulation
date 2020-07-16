using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ant_Simulation
{
    public partial class Form1 : Form
    {
        ControlClass _control;

        public int _numberOfNormalAnts = 5;

        public int _boardWidth = 20;
        public int _boardHeight = 20;
        public int _goalTiles = 20;

        public Form1()
        {
            InitializeComponent();

            _control = new ControlClass(_numberOfNormalAnts); //TODO get from correct bit of UI

            _control.BoardStepped += _control_BoardStepped;
        }

        private void _control_BoardStepped(object sender, GameBoardSteppedEventArgs e)
        {
            UpdateGameBoardView(e.Board);
        }

        #region Image-Based

        public void UpdateGameBoardView(Bitmap board)
        {
            GameBoardBox.Image = ResizeBitmapToPictureBox(board);
            GameBoardBox.Refresh();
        }

        private Bitmap ResizeBitmapToPictureBox(Bitmap inputBitmap) //always keeps aspect ratio and adds border
        {
            int border_width = 1; //always 1 'pixel' thick.

            double adjusted_input_width = inputBitmap.Width + border_width * 2;
            double adjusted_input_height = inputBitmap.Height + border_width * 2;

            Bitmap result = new Bitmap(GameBoardBox.Width , GameBoardBox.Height );

            double width_ratio = adjusted_input_width/ (double)result.Width;
            double height_ratio = adjusted_input_height / (double)result.Height;
            double scale_ratio = (width_ratio > height_ratio) ? width_ratio : height_ratio;
            int width = (int)(adjusted_input_width / scale_ratio);
            int height = (int)(adjusted_input_height / scale_ratio);
            int x_offset = (result.Width - width) / 2;
            int y_offset = (result.Height - height) / 2;

            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.Clear(Color.Black);
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.DrawImage(inputBitmap, new Rectangle(x_offset, y_offset, width, height));
            }

            return result;
        }

        #endregion

        #region Movement
       
        public void Step_Click_1(object sender, EventArgs e)
        {
            _control.Step(steps: 1);
        }

        public void Step5_Click(object sender, EventArgs e)
        {
            _control.Step(steps: 5);
        }

        public void Step10_Click(object sender, EventArgs e)
        {
            _control.Step(steps: 10);
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateNewBoard new_board = new GenerateNewBoard(this);
            new_board.ShowDialog();
            _control = new ControlClass(_numberOfNormalAnts,_boardWidth, _boardHeight, _goalTiles);//TODO is this correct?

            _control.BoardStepped += _control_BoardStepped;
            //TODO draw the board?
        }
    }
}
