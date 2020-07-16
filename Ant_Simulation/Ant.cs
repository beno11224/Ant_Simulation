using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Ant_Simulation
{
    abstract class Ant
    {
        public Point _location;

        public Random _random;

        private Object _caller;

        private int[] _random_move;

        //public FloorTile generic_floortile = new FloorTile(FloorTile.TileType.Blank);

        public enum Action { None, DropPheremone }; //TODO add more actions here?

        public void HandleMoveEvent(object sender, GameBoardSteppedEventArgs e)
        {
            // get ant position from _gameboard
            //Move();
        }

        public Ant(Object caller, Random rand)
        {
            _random = rand;
            _location = new Point(1, 1);
        }

        public Ant(Object caller, Random rand, Point location)
        {
            _location = location;
        }

        public abstract Point GetLocation();

        public abstract Action Move(AntVision antVision);

        public bool IsTileWalkable(FloorTile.TileType tileType)
        {

            switch (tileType) //Use integer passed back from colour.ToArgb()
            {
                default:
                    {
                        return true;
                    }
                case (FloorTile.TileType.Null):
                    {
                        return false;
                    }
                case (FloorTile.TileType.Special): //TODO - need to determine what a 'special' tile would be...
                    {
                        return false;
                    }
            }
        }
    }

    class NormalAnt : Ant
    {

        private bool carrying_gold = false;

        public NormalAnt(Object caller, Random random) : base(caller,random)
        { }

        public NormalAnt(Object caller, Random random, Point location) : base(caller, random, location)
        { }

        public override Point GetLocation()
        {
            return _location;
        }



        public override Action Move(AntVision antVision) //TODO add in weighted randomisation for movement (weight based on tiles next to it) so like a goal is a higher weight.
        {
            /*
            antVision is 3*3 bitmap.
            tile setup is as follows (numbers refer to array index):
             _____
            |0 3 6|
            |1 4 7| //4 is always walkable (unless code is very broken)
            |2_5_8|
             
            */

            FloorTile.TileType best_tile = FloorTile.TileType.Blank;

            List<bool> walkable_tiles_list = new List<bool>();
            bool[] walkable_tiles;

            int random_int = 0;

            for (int x_count = 0; x_count< 3; x_count++) //TODO change this... //what to?
            {
                for (int y_count = 0; y_count< 3; y_count++)
                {
                    walkable_tiles_list.Add(IsTileWalkable(antVision._antVision[x_count, y_count]));
                }
            }

            //walkable_tiles_list.Add(true); //here to allow the ant to stay still (and to do things with the tile it is standing on)
            walkable_tiles = walkable_tiles_list.ToArray(); //TODO redo this function to allow for preference of direction.

            do
            {
                random_int = _random.Next(walkable_tiles.Length);
            } while (random_int % 2 == 0 || walkable_tiles_list[random_int] == false);

            switch (random_int)
            {
                case (1):
                    {
                        _location.X += -1;
                        break;
                    }
                case (3):
                    {
                        _location.Y += -1;
                        break;
                    }
                case (5):
                    {
                        _location.Y += 1;
                        break;
                    }
                case (7):
                    {
                        _location.X += 1;
                        break;
                    }
            }

            if (carrying_gold)
            {
                if ( antVision._antVision[1,1] == FloorTile.TileType.Home )
                {
                    //-TODO pass ant_vision as array of FloorTile. Pass by reference to allow for ant editing tiles.
                    //TODO increment home tile.
                    carrying_gold = false;

                    return Action.None;
                }

                return Action.DropPheremone;
            }
            else if ( antVision._antVision[1,1] == FloorTile.TileType.Goal )
            {
                carrying_gold = true;
            }

            return Action.None;
        }
    }

    class Pheremone
    {
        private ControlClass _parent;
        private FloorTile _tile;
        private Point _location;
        private double _decayRate;
        private double _current_value;
        private double _initial_value;

        public Pheremone(ControlClass control, Point location, int initialValue, double decayRate)
        {
            if (initialValue > 255)
            {
                throw new ArgumentException("intialValue cannot be greater than 255");
            }
            _initial_value = initialValue;
            _current_value = initialValue;
            _location = location;
            _tile = new FloorTile(control, FloorTile.TileType.Special, initialValue, initialValue);
            _decayRate = decayRate;
            _parent = control;

            _parent.BoardStepped += parent_BoardStepped;
        }

        public double GetValue()
        {
            return _current_value;
        }

        public Point GetLocation()
        {
            return _location;
        }

        public Color GetColour()
        {
            //77 88 99 were the old colours
            double multiplier = 1 - (_current_value / _initial_value);
            int r = 119;
            int g = (int)(136 * multiplier);
            int b = (int)(153 * multiplier);
            return Color.FromArgb(r, g, b);
        }

        private void parent_BoardStepped(object sender, GameBoardSteppedEventArgs e) //should only be called once (and only once) per step.
        {
            _current_value *= _decayRate;

            if (_current_value >= 0.01)
            {
                //All good, carry on
                //return true;
            }
            //return false;//destroy the pheremone
        }
    }
}
