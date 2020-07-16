using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ant_Simulation
{
    class AntVision
    {
        public FloorTile.TileType[,] _antVision = new FloorTile.TileType[3, 3];
        public List<Pheremone>[,] _antSmell = new List<Pheremone>[3, 3];

        /*
         * set up as follows:
         * 
         * (x coord, y coord)
         * 
         * -----------------
         * |(0,0)(1,0)(2,0)|
         * |(0,1)(1,1)(2,1)|
         * |(0,2)(1,2)(2,2)|
         * -----------------
         * 
         * for legacy (access by passing int rather than point): //TODO make sure this isn't anywhere in the code
         *  |/ 1 /|
         *  |0 4 3|
         *  |/_2_/|
         * 
        */

        public AntVision (FloorTile.TileType zerozero = FloorTile.TileType.Blank, FloorTile.TileType onezero = FloorTile.TileType.Blank, FloorTile.TileType twozero = FloorTile.TileType.Blank,
                        FloorTile.TileType zeroone = FloorTile.TileType.Blank, FloorTile.TileType oneone = FloorTile.TileType.Blank, FloorTile.TileType twoone = FloorTile.TileType.Blank,
                        FloorTile.TileType zerotwo = FloorTile.TileType.Blank, FloorTile.TileType onetwo = FloorTile.TileType.Blank, FloorTile.TileType twotwo = FloorTile.TileType.Blank,
                        List<Pheremone> phzerozero = null, List<Pheremone> phonezero = null, List<Pheremone> phtwozero = null,
                        List<Pheremone> phzeroone = null, List<Pheremone> phoneone = null, List<Pheremone> phtwoone = null,
                        List<Pheremone> phzerotwo = null, List<Pheremone> phonetwo = null, List<Pheremone> phtwotwo = null
                        )
        {
            _antVision[0, 0] = zerozero;
            _antVision[0, 1] = zeroone;
            _antVision[0, 2] = zerotwo;
            _antVision[1, 0] = onezero;
            _antVision[1, 1] = oneone;
            _antVision[1, 2] = onetwo;
            _antVision[2, 0] = twozero;
            _antVision[2, 1] = twoone;
            _antVision[2, 2] = twotwo;
            _antSmell[0, 0] = phzerozero;
            _antSmell[0, 1] = phzeroone;
            _antSmell[0, 2] = phzerotwo;
            _antSmell[1, 0] = phonezero;
            _antSmell[1, 1] = phoneone;
            _antSmell[1, 2] = phonetwo;
            _antSmell[2, 0] = phtwozero;
            _antSmell[2, 1] = phtwoone;
            _antSmell[2, 2] = phtwotwo;
        }

        //OLD VERSION
        public AntVision(FloorTile.TileType zero, FloorTile.TileType one, FloorTile.TileType two, FloorTile.TileType three, FloorTile.TileType four)
        {
            _antVision[0, 0] = FloorTile.TileType.Null;
            _antVision[0, 1] = one;
            _antVision[0, 2] = FloorTile.TileType.Null;
            _antVision[1, 0] = zero;
            _antVision[1, 1] = four;
            _antVision[1, 2] = three;
            _antVision[2, 0] = FloorTile.TileType.Null;
            _antVision[2, 1] = two;
            _antVision[2, 2] = FloorTile.TileType.Null;
        }
    }
}
