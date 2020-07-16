using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Ant_Simulation
{
    class ControlClass //TODO make this static?
    {
        GameBoard _gameBoard;
        Ant[] _ants;

        Random _random = new Random(); //random for use everywhere

        public ControlClass(int numberOfAnts = 10, int width = 20, int height = 20, int goals = 20)
        {
            _gameBoard = new GameBoard(this, _random, width: width, height: height, homeSquareRect: new Rectangle( 3, 5, 2, 2), numberOfGoalLocations: goals);

            _ants = new NormalAnt[numberOfAnts];

            for (int ant_count = 0; ant_count < _ants.Length; ant_count++)
            {
                _ants[ant_count] = new NormalAnt(this, _random);
            }

            Bitmap board = _gameBoard.ToBitmap();

            OnBoardStep(new GameBoardSteppedEventArgs(board)); //TODO i don't think this is right...
        }

        public void Step(int steps = 1)
        {
            for (int count = 0; count < steps; count++)
            {
                _gameBoard.Step(1); //TODO use events

                foreach (Ant ant in _ants) //TODO Events?
                {

                    Point initial_location = ant._location; //TODO this doesn't show pheremones to the ants.

                    //TODO set all tiles here so if it's a null then we can sort it.
                    FloorTile.TileType zerozero = FloorTile.TileType.Null;
                    FloorTile.TileType zeroone = FloorTile.TileType.Null;
                    FloorTile.TileType zerotwo = FloorTile.TileType.Null;
                    FloorTile.TileType onezero = FloorTile.TileType.Null;
                    FloorTile.TileType oneone = FloorTile.TileType.Null;
                    FloorTile.TileType onetwo = FloorTile.TileType.Null;
                    FloorTile.TileType twozero = FloorTile.TileType.Null;
                    FloorTile.TileType twoone = FloorTile.TileType.Null;
                    FloorTile.TileType twotwo = FloorTile.TileType.Null;
                    List<Pheremone> phzerozero = new List<Pheremone>();
                    List<Pheremone> phzeroone = new List<Pheremone>();
                    List<Pheremone> phzerotwo = new List<Pheremone>();
                    List<Pheremone> phonezero = new List<Pheremone>();
                    List<Pheremone> phoneone = new List<Pheremone>();
                    List<Pheremone> phonetwo = new List<Pheremone>();
                    List<Pheremone> phtwozero = new List<Pheremone>();
                    List<Pheremone> phtwoone = new List<Pheremone>();
                    List<Pheremone> phtwotwo = new List<Pheremone>();

                    try
                    {
                        zerozero = _gameBoard.GetTileAtLocation(initial_location.X - 1, initial_location.Y - 1).GetTileType();
                        phzerozero = _gameBoard.GetPheremonesAtLocation(initial_location.X - 1, initial_location.Y - 1);
                    }
                    catch (Exception e)
                    { }
                    try
                    {
                        zeroone = _gameBoard.GetTileAtLocation(initial_location.X - 1, initial_location.Y).GetTileType();
                        phzeroone = _gameBoard.GetPheremonesAtLocation(initial_location.X - 1, initial_location.Y);
                    }
                    catch (Exception e)
                    { }
                    try
                    {
                        zerotwo = _gameBoard.GetTileAtLocation(initial_location.X - 1, initial_location.Y + 1).GetTileType();
                        phzerotwo = _gameBoard.GetPheremonesAtLocation(initial_location.X - 1, initial_location.Y + 1);
                    }
                    catch (Exception e)
                    { }
                    try
                    {
                        onezero = _gameBoard.GetTileAtLocation(initial_location.X, initial_location.Y - 1).GetTileType();
                        phonezero = _gameBoard.GetPheremonesAtLocation(initial_location.X, initial_location.Y - 1);
                    }
                    catch (Exception e)
                    { }
                    try
                    {
                        oneone = _gameBoard.GetTileAtLocation(initial_location.X, initial_location.Y).GetTileType();
                        phoneone = _gameBoard.GetPheremonesAtLocation(initial_location.X, initial_location.Y);
                    }
                    catch (Exception e)
                    { }
                    try
                    {
                        onetwo = _gameBoard.GetTileAtLocation(initial_location.X, initial_location.Y + 1).GetTileType();
                        phonetwo = _gameBoard.GetPheremonesAtLocation(initial_location.X, initial_location.Y + 1);
                    }
                    catch (Exception e)
                    { }
                    try
                    {
                        twozero = _gameBoard.GetTileAtLocation(initial_location.X + 1, initial_location.Y - 1).GetTileType();
                        phtwozero = _gameBoard.GetPheremonesAtLocation(initial_location.X + 1, initial_location.Y - 1);
                    }
                    catch (Exception e)
                    { }
                    try
                    {
                        twoone = _gameBoard.GetTileAtLocation(initial_location.X + 1, initial_location.Y).GetTileType();
                        phtwoone = _gameBoard.GetPheremonesAtLocation(initial_location.X + 1, initial_location.Y);
                    }
                    catch (Exception e)
                    { }
                    try
                    {
                        twotwo = _gameBoard.GetTileAtLocation(initial_location.X + 1, initial_location.Y + 1).GetTileType();
                        phtwotwo = _gameBoard.GetPheremonesAtLocation(initial_location.X + 1, initial_location.Y + 1);
                    }
                    catch (Exception e)
                    { }

                    AntVision ant_vision = new AntVision( //TODO add pheremone detection here...
                        zerozero,onezero,twozero,
                        zeroone,oneone,twoone,
                        zerotwo,onetwo,twotwo,
                        phzerozero, phonezero, phtwozero,
                        phzeroone, phoneone, phtwoone,
                        phzerotwo, phonetwo, phtwotwo
                        );

                    Ant.Action ant_action = ant.Move(ant_vision);

                    switch (ant_action) //REMEMBER all actions are done to PREVIOUS tile
                    {
                        default:
                            {
                                break;
                            }

                        case (Ant.Action.DropPheremone):
                            {
                                Pheremone ph = new Pheremone(this, initial_location, 255, 0.5);
                                _gameBoard.addPheremone(ph);
                                break;
                            }
                    }
                }
            }

            OnBoardStep(new GameBoardSteppedEventArgs(DrawWholeGameBoard(_gameBoard)));
        }

        public Bitmap DrawWholeGameBoard(GameBoard board)
        {
            Bitmap result = board.ToBitmap();

            foreach (Ant ant in _ants)
            {
                result.SetPixel(ant._location.X, ant._location.Y, Color.Purple); //TODO each time ant is drawn make sure its in the right place!
            }

            return result;
        }

        public event GameBoardSteppedEventHandler BoardStepped;

        private void OnBoardStep(GameBoardSteppedEventArgs e)
        {
            GameBoardSteppedEventHandler board_changed_handler = BoardStepped;
            if (board_changed_handler != null)
            {
                board_changed_handler(this, e);
            }
        }

        public delegate void GameBoardSteppedEventHandler(object sender, GameBoardSteppedEventArgs e);
    }

        #region Events

    class GameBoardSteppedEventArgs : EventArgs
    {
        //Be careful to use this iff the board has stepped once!
        public Bitmap Board;

        public GameBoardSteppedEventArgs (Bitmap board)
        {
            Board = board;
        }

        public GameBoardSteppedEventArgs (GameBoard gameBoard)
        {
            Board = gameBoard.ToBitmap();
        }
    }

    class AntMoveEventArgs : EventArgs
    {
        public Point Location;

        public AntMoveEventArgs(Point location)
        {
            Location = location;
        }
    }

    class AntViewEventArgs : EventArgs
    {
        public FloorTile[] ant_vision = new FloorTile[5];
        
        public AntViewEventArgs(Point Location)
        {
            //TODO generate AntView
        }
    }

    #endregion
}
