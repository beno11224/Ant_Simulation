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

        public virtual void Move(Bitmap antVision) //TODO add in the drop pheremone into move when '4' is a pheremone tile. Also add in weighted randomisation for movement (weight based on tiles next to it)
                                                   //so like a goal is a higher weight.
        {
            /*
                antVision is 3*3 bitmap.
                tile setup is as follows (numbers refer to array index):
                _ _ _
                |/ 1 /|
                |0 4 3| //4 is always true
                |/_2_/|
             
                */

            List<bool> walkable_tiles_list = new List<bool>();
            bool[] walkable_tiles;

            int random_int = 0;

            for (int x_count = 0; x_count < 3; x_count++)
            {
                for (int y_count = 0; y_count < 3; y_count++)
                {
                    if ((x_count + y_count) % 2 != 0)
                    {
                        walkable_tiles_list.Add(IsTileWalkable(antVision.GetPixel(x_count, y_count)));
                    }
                }
            }

            walkable_tiles_list.Add(true); //here to allow the ant to stay still (I think).
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
        }

        //TODO add in a create pheremone function...

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

        private bool carrying_gold = true; //TODO change this to false

        public NormalAnt(Random random) : base(random)
        { }

        public NormalAnt(Random random, Point location) : base(random, location)
        { }

        public override Point GetLocation()
        {
            return _location;
        }

        public override void Move(Bitmap antVision)
        {
            if (!carrying_gold)
            {
                base.Move(antVision); //do any base move stuff... (//TODO decide how much we do in the base class)
                //TODO test if they are on a goal tile, then set carrying_gold
            }
            else
            {
                Pheremone ph = new Pheremone(_location, 250, 0.1);
                //TODO add_pheremone event...
                
                //TODO follow pheremone trails
            }

        }
    }

    class Pheremone
    {
        private FloorTile _tile;
        private Point _location;
        private double _decayRate;
        private double _current_value;
        private int lifeSpan = 0; //lifespan always = 0; // TODO why did I write this?

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
        //TODO find a way of decrementing all tiles correctly...

        public double GetValue()
        {
            return _current_value;
        }

        public void Decay() //should only be called once (and only once) per step. //TODO is there a way to enforce this?
        {
            _current_value *= _decayRate;
        }
    }
}
