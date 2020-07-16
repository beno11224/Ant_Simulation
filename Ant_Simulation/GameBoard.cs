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
        public ControlClass _control;

        private FloorTile[,] _board; //THIS is stored as (X,Y)
        private List<Pheremone>[,] _pheremone_locations;

        private int _minGoalScore = 10;
        private int _maxGoalScore = 30;

        private Rectangle _homeSquare;
        private Point[] _goals;
        
        private Random _random;

        #region Constructors

        public GameBoard(ControlClass control, Random random, int width, int height, Rectangle homeSquareRect)
        {
            _control = control;
            _random = random;

            _board = new FloorTile[width, height];
            _pheremone_locations = new List<Pheremone>[width,height];
            _homeSquare = homeSquareRect;

            for (int width_count = 0; width_count < width; width_count++)
            {
                for (int height_count = 0; height_count < height; height_count++)
                {
                    _board[width_count, height_count] = new FloorTile(_control, FloorTile.TileType.Blank);
                    _pheremone_locations[width_count, height_count] = new List<Pheremone>();
                }
            }

            for (int home_square_width_count = 0; home_square_width_count < homeSquareRect.Width; home_square_width_count++)
            {
                for (int home_square_height_count = 0; home_square_height_count < homeSquareRect.Height; home_square_height_count++)
                {
                    _board[home_square_width_count + homeSquareRect.X, home_square_height_count + homeSquareRect.Y] = new FloorTile(_control, FloorTile.TileType.Home, 10); //TODO remove magic number
                }
            }
        }

        public GameBoard(ControlClass control, Random random, int width, int height, Rectangle homeSquareRect, int numberOfGoalLocations) : this(control, random, width, height, homeSquareRect)
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

                _board[board_x, board_y] = new FloorTile(_control, FloorTile.TileType.Goal, value: _random.Next(_minGoalScore, _maxGoalScore));

                _goals[goal_count] = new Point(board_x, board_y);
            }
        }

        public GameBoard(ControlClass control, Random random, int width, int height, Rectangle homeSquareRect, Point[] goalLocations) : this(control, random, width, height, homeSquareRect)
        {
            _goals = goalLocations;

            foreach (Point goal_location in goalLocations)
            {
                _board[goal_location.X, goal_location.Y] = new FloorTile(_control, FloorTile.TileType.Goal, value: _random.Next(_maxGoalScore, _maxGoalScore));
            }
        }

        #endregion

        public Bitmap ToBitmap() //returns bitmap of CORRECT size - add border later on in code.
        {
            int width = _board.GetLength(0);
            int height = _board.GetLength(1);
            Bitmap result = new Bitmap(width, height);

            for (int board_width_count = 0; board_width_count < width; board_width_count++)
            {
                for (int board_height_count = 0; board_height_count < height; board_height_count++)
                {
                    List<Pheremone> pheremones = GetPheremonesAtLocation(board_width_count, board_height_count);
                    
                    if (false == (pheremones == null || pheremones.Count == 0))
                    {
                        double current_max = 0;
                        int current_index = 0;
                        for (int pheremone_count = 0; pheremone_count < pheremones.Count; pheremone_count++)
                        {
                            //TODO decide if using the 'newest' pheremone or using a scaled sum of all the pheremones
                            //if using scaled sum saturation may be an issue
                            //newest
                            if (current_max < pheremones[pheremone_count].GetValue())
                            {
                                current_max = pheremones[pheremone_count].GetValue();
                                current_index = pheremone_count;
                            }                            
                        }
                        //draw the pheremone.
                        result.SetPixel((board_width_count), (board_height_count), pheremones[current_index].GetColour());//TODO need to look at multiple pheremones
                    }
                    else
                    {
                        //draw as a normal tile
                        result.SetPixel((board_width_count), (board_height_count), _board[board_width_count, board_height_count].GetColour());
                    }
                }
            }

            //TODO call EVENT to draw anything else here

            return result;
        }    

        public void Step(int numberOfSteps) //TODO Move into FloorTile and use events!
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
                                        _board[x_count, y_count] = new FloorTile(_control, FloorTile.TileType.Blank);
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

        public FloorTile GetTileAtLocation(int x, int y)
        {
            try
            {
                return _board[x, y];
            }
            catch ( IndexOutOfRangeException )
            {
                return null;
            }
        }

        public List<Pheremone> GetPheremonesAtLocation(int x, int y)
        {
            try
            {
                return _pheremone_locations[x, y];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        public int GetWidth()
        {
            return _board.GetLength(0);
        }

        public int GetHeight()
        {
            return _board.GetLength(1);
        }

        public void addPheremone(Pheremone pheremone)
        {
            _pheremone_locations[pheremone.GetLocation().X, pheremone.GetLocation().Y].Add(pheremone);
        }
    }
}
