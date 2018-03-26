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
        GameBoard _gameBoard;
        NormalAnt[] _ants = new NormalAnt[1]; //change this value to change the number of ants

        Random _random = new Random();


        public Form1()
        {
            InitializeComponent();

            _gameBoard = new GameBoard(_random, width: 20, height: 20, homeSquareRect: new Rectangle(10, 10, 3, 5), numberOfGoalLocations: 5);
            for (int ant_count = 0; ant_count < _ants.Length; ant_count++)
            {
                _ants[ant_count] = new NormalAnt(_random);
            }
        }

        #region Image-Based

        private Bitmap ResizeBitmapToPictureBox(Bitmap inputBitmap)
        {
            return ResizeBitmapToPictureBox(inputBitmap, keepAspectRatio: true);
        }

        private Bitmap ResizeBitmapToPictureBox(Bitmap inputBitmap, bool keepAspectRatio)
        {
            Bitmap result = new Bitmap(PictureBox.Width, PictureBox.Height);

            int width;
            int height;
            int x_offset;
            int y_offset;

            if (keepAspectRatio)
            {
                double width_ratio = (double)inputBitmap.Width / (double)result.Width;
                double height_ratio = (double)inputBitmap.Height / (double)result.Height;
                double scale_ratio = (width_ratio > height_ratio) ? width_ratio : height_ratio;
                width = (int)((double)inputBitmap.Width / scale_ratio);
                height = (int)((double)inputBitmap.Height / scale_ratio);
                x_offset = (result.Width - width) / 2;
                y_offset = (result.Height - height) / 2;
            }
            else
            {
                width = result.Width;
                height = result.Height;
                x_offset = 0;
                y_offset = 0;
            }

            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.DrawImage(inputBitmap, new Rectangle(x_offset, y_offset, width, height));
            }

            return result;
        }

        #endregion

        #region Event-Based

        private void Step_Click(object sender, EventArgs e)
        {
            StepXTimes(steps: 1);
        }

        private void FiveStep_Click(object sender, EventArgs e)
        {
            StepXTimes(steps: 5);
        }

        private void TenStep_Click(object sender, EventArgs e)
        {
            StepXTimes(steps: 10);
        }

        private void StepXTimes(int steps)
        {
            for (int count = 0; count < steps; count++)
            {
                _gameBoard.Step(1);
                Bitmap board = _gameBoard.ToBitmap();

                foreach (Ant ant in _ants)
                {
                    Bitmap ant_vision = new Bitmap(3, 3); //should ant_vision be an array of FloorTile? otherwise FloorTile seems pretty useless...
                    //FloorTile[] ant_vision = new FloorTile[5];
                    using (Graphics graphics = Graphics.FromImage(ant_vision))
                    {
                        graphics.Clear(Color.Red);
                        graphics.DrawImage(image: _gameBoard.ToBitmap(),
                            destRect: new Rectangle(0, 0, 3, 3),
                            srcRect: new Rectangle((ant.GetLocation().X - 1), (ant.GetLocation().Y - 1), 3, 3),
                            srcUnit: GraphicsUnit.Pixel);
                        //I can use -1 as the left-most pixel of the board is 1,1
                    }
                    Ant.Action ant_action = ant.Move(ant_vision);
                    Point location = ant.GetLocation();

                    switch (ant_action)
                    {
                        default: //or Action.None
                            {
                                break;
                            }

                        case (Ant.Action.DropPheremone):
                            {
                                Point pheremone_location = location;
                                pheremone_location.X--;
                                pheremone_location.Y--;
                                Pheremone ph = new Pheremone(pheremone_location,255,0.5); //TODO work out if there is a better way to do this.
                                _gameBoard.addPheremone(ph);
                                break;
                            }
                    }
                    
                    board.SetPixel(location.X, location.Y, Color.Purple);
                }

                PictureBox.Image = ResizeBitmapToPictureBox(board);
                PictureBox.Refresh();
            }
        }

        #endregion
    }
}
