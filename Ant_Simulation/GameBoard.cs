using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Ant_Simulation
{
    class GameBoard
    {
        private FloorTile[,] _board;

        private int _minGoalScore = 10;
        private int _maxGoalScore = 30;

        private Rectangle _homeSquare;
        private Point[] _goals;
        //private List<Point> _pheremone_locations = new List<Point>();

        private Random _random;

        #region Constructors

        public GameBoard(Random random, int width, int height, Rectangle homeSquareRect)
        {
            _random = random;

            _board = new FloorTile[width, height];
            _homeSquare = homeSquareRect;

            for (int width_count = 0; width_count < width; width_count++)
            {
                for (int height_count = 0; height_count < height; height_count++)
                    _board[width_count, height_count] = new FloorTile(FloorTile.TileType.Blank);
            }

            for (int home_square_width_count = 0; home_square_width_count < homeSquareRect.Width; home_square_width_count++)
            {
                for (int home_square_height_count = 0; home_square_height_count < homeSquareRect.Height; home_square_height_count++)
                {
                    _board[home_square_width_count + homeSquareRect.X, home_square_height_count + homeSquareRect.Y] = new FloorTile(FloorTile.TileType.Home, 10); //TODO remove magic number
                }
            }
        }

        public GameBoard(Random random, int width, int height, Rectangle homeSquareRect, int numberOfGoalLocations) : this(random, width, height, homeSquareRect)
        {
            _goals = new Point[numberOfGoalLocations];

            for (int goal_count = 0; goal_count < numberOfGoalLocations; goal_count++)
            {
                int board_x;
                int board_y;
                do
                {
                    board_x = _random.Next(width);
                    board_y = _random.Next(height);
                }

                while (board_x > _homeSquare.X &&
                    board_x < (_homeSquare.X + _homeSquare.Width) &&
                    board_y > _homeSquare.Y &&
                    board_y < (_homeSquare.Y + _homeSquare.Width));
                //While loop stops goals spawning in home_square

                _board[board_x, board_y] = new FloorTile(FloorTile.TileType.Goal, value: _random.Next(_minGoalScore, _maxGoalScore));

                _goals[goal_count] = new Point(board_x, board_y);
            }
        }

        public GameBoard(Random random, int width, int height, Rectangle homeSquareRect, Point[] goalLocations) : this(random, width, height, homeSquareRect)
        {
            _goals = goalLocations;

            foreach (Point goal_location in goalLocations)
            {
                _board[goal_location.X, goal_location.Y] = new FloorTile(FloorTile.TileType.Goal, value: _random.Next(_maxGoalScore, _maxGoalScore));
            }
        }

        #endregion

        public Bitmap ToBitmap()
        {
            int width = _board.GetLength(0);
            int height = _board.GetLength(1);
            Bitmap result = new Bitmap(width + 2, height + 2);

            using (Graphics g = Graphics.FromImage(result))
            {
                g.Clear(Color.Black);
            }

            for (int board_width_count = 0; board_width_count < width; board_width_count++)
            {
                for (int board_height_count = 0; board_height_count < height; board_height_count++)
                {
                    result.SetPixel((board_width_count + 1), (board_height_count + 1), _board[board_width_count, board_height_count].GetColour());
                }
            }

            return result;
        }

        public void Step()
        {
            Step(1);
        }

        public void Step(int numberOfSteps)
        {
            for (int step_count = 0; step_count < numberOfSteps; step_count++)
            {
                int home_score = GetHomeSquareValue();
                //TODO - here decrement any decrementable tiles
                for (int x_count = 0; x_count < _board.GetLength(0); x_count++)
                {
                    for (int y_count = 0; y_count < _board.GetLength(1); y_count++)
                    {
                        FloorTile current_tile = _board[x_count, y_count];
                        switch(current_tile.GetTileType())
                        {
                            case (FloorTile.TileType.Pheremone):
                                {
                                    if (!_board[x_count, y_count].ModifyValue())
                                    {
                                        _board[x_count, y_count] = new FloorTile(FloorTile.TileType.Blank);
                                    }
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        
                    }
                }
            }
        }

        public int GetHomeSquareValue()
        {
            int home_square_x = _homeSquare.X + _homeSquare.Width;
            int result = 0;
            for (int x_count = _homeSquare.X; x_count < home_square_x; x_count++)
            {
                int home_square_y = _homeSquare.X + _homeSquare.Height;
                for (int y_count = 0; y_count < home_square_y; y_count++)
                {
                    result += _board[(x_count), (y_count)].GetValue();
                }
            }
            return result;
        }

        public void addPheremone(Pheremone pheremone)
        {
            _board[pheremone.GetLocation().X, pheremone.GetLocation().Y] = new FloorTile(FloorTile.TileType.Pheremone, 255, 0.9); //TODO magic number?
            //_pheremone_locations.Add(pheremone.GetLocation());
        }
    }
}
