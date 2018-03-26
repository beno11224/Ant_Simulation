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

        private int[] _random_move;

        public FloorTile generic_floortile = new FloorTile(FloorTile.TileType.Blank);

        public enum Action { None, DropPheremone }; //TODO add more actions here?

        public Ant(Random rand)
        {
            _random = rand;
            _location = new Point(1, 1);
        }

        public Ant(Random rand, Point location)
        {
            _location = location;
        }

        public abstract Point GetLocation();

        public abstract Action Move(FloorTile[] antVision);

        public bool IsTileWalkable(Color colour)
        {
            int colour_int = colour.ToArgb();

            switch (colour_int) //Use integer passed back from colour.ToArgb()
            {
                default:
                    {
                        return true;
                    }
                //Add more cases in here for non-walkable tiles.
                case (169):
                    {
                        return false;
                    }
                case (-16777216):
                    {
                        return false;
                    }
                case (-65536):
                    {
                        return false;
                    }
            }
        }
    }

    class NormalAnt : Ant
    {

        private bool carrying_gold = false; //TODO change this to false

        public NormalAnt(Random random) : base(random)
        { }

        public NormalAnt(Random random, Point location) : base(random, location)
        { }

        public override Point GetLocation()
        {
            return _location;
        }


        public override Action Move(FloorTile[] antVision) //TODO add in weighted randomisation for movement (weight based on tiles next to it) so like a goal is a higher weight.
        {
            /*
            antVision is 3*3 bitmap.
            tile setup is as follows (numbers refer to array index):
            _ _ _
            |/ 1 /|
            |0 4 3| //4 is always walkable (unless code is very broken)
            |/_2_/|
             
            */

        FloorTile.TileType best_tile = FloorTile.TileType.Blank;

        List<bool> walkable_tiles_list = new List<bool>();
        bool[] walkable_tiles;

        int random_int = 0;

            for (int x_count = 0; x_count< 3; x_count++) //TODO change this...
            {
                for (int y_count = 0; y_count< 3; y_count++)
                {
                    if ((x_count + y_count) % 2 != 0)
                    {
                        switch (generic_floortile.GetTileTypeFromColour(antVision.GetPixel(x_count, y_count)))
                        {
                            default: //TODO decide what walk to do for base ant 
                            {
                                walkable_tiles_list.Add(IsTileWalkable(antVision.GetPixel(x_count, y_count)));
                                break;
                            }

                            case (FloorTile.TileType.Pheremone): //TODO could I use red-value to determine how many pheremones are on that tile? (100 levels would be enough)...
                            {
                                    walkable_tiles_list.Add(IsTileWalkable(antVision.GetPixel(x_count, y_count)));
                                    break;
                            }

                            case (FloorTile.TileType.Goal):
                            {
                                if (!carrying_gold)
                                {
                                    walkable_tiles_list.Add(IsTileWalkable(antVision.GetPixel(x_count, y_count)));
                                    carrying_gold = true;
                                }
                                break;
                            }

                            case (FloorTile.TileType.Home):
                            {
                                if (carrying_gold)
                                {
                                    walkable_tiles_list.Add(IsTileWalkable(antVision.GetPixel(x_count, y_count)));
                                }
                                break;
                            }
                        }
                    }
                }
            }

            walkable_tiles_list.Add(true); //here to allow the ant to stay still (and to do things with the tile it is standing on)
            walkable_tiles = walkable_tiles_list.ToArray();

            do
            {
                random_int = _random.Next(walkable_tiles.Length);
            } while (walkable_tiles_list[random_int] == false);

            switch (random_int)
            {
                case (0):
                    {
                        _location.X += -1;
                        break;
                    }
                case (1):
                    {
                        _location.Y += -1;
                        break;
                    }
                case (2):
                    {
                        _location.Y += 1;
                        break;
                    }
                case (3):
                    {
                        _location.X += 1;
                        break;
                    }
            }

            if (carrying_gold)
            {
                if ( generic_floortile.GetTileTypeFromColour(antVision.GetPixel(1,1)) == FloorTile.TileType.Home )
                {
                    //TODO pass ant_vision as array of FloorTile. Pass by reference to allow for ant editing tiles.
                    //TODO increment home tile.
                    carrying_gold = false;
                }

                return Action.DropPheremone;
            }
            else
            {
                return Action.None;
            }
        }
    }

    class Pheremone
    {
        private FloorTile _tile;
        private Point _location;
        private double _decayRate;
        private double _current_value;

        public Pheremone(Point location, int initialValue, double decayRate)
        {
            if (initialValue > 255)
            {
                throw new ArgumentException("intialValue cannot be greater than 255");
            }
            _location = location;
            _tile = new FloorTile(FloorTile.TileType.Special, initialValue, initialValue);
            _decayRate = decayRate;
        }

        public double GetValue()
        {
            return _current_value;
        }

        public Point GetLocation()
        {
            return _location;
        }

        public void Decay() //should only be called once (and only once) per step.
        {
            _current_value *= _decayRate;
        }
    }
}
