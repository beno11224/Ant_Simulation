using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Ant_Simulation
{
    class FloorTile
    {
        public enum TileType { Null, Blank, Home, Goal, Special, Pheremone }; //special tiles are child classes of FloorTile.

        public ControlClass _control;

        private TileType _tileType;
        private Color _colour;
        private int _value; //TODO make this double?
        private int _maxValue;
        private double _modifer;

        public FloorTile(ControlClass control, TileType tileType)
        {
            _control = control;

            //_control.BoardStepped += Decay(); //TODO

            _tileType = tileType;
            _value = 0;

            switch (tileType)
            {
                case (TileType.Blank):
                    {
                        _colour = Color.LightSlateGray;
                        break;
                    }
                case (TileType.Goal):
                    {
                        _colour = Color.Yellow;
                        break;
                    }
                case (TileType.Home):
                    {
                        _colour = Color.Green;
                        break;
                    }
                case (TileType.Special):
                    {
                        _colour = Color.Orange; //TODO will this work??
                        break;
                    }
                case (TileType.Pheremone):
                    {
                        _colour = Color.Blue;
                        break;
                    }
            }
        }

        

        public void Decay(object sender, GameBoardSteppedEventArgs e)
        {

        }

        public FloorTile(ControlClass control, TileType tileType, int value) : this(control, tileType) //the "this:" at the end runs the above constructor too
        {
            _value = value;
            if (tileType == TileType.Pheremone)
            {
                if (value > 255)
                {
                    throw new ArgumentException("Value of a pheremone cannot be greater than 255");
                }

                _colour = Color.FromArgb(100,100,value);
            }
        }

        public FloorTile(ControlClass control, TileType tileType, int value, int maxValue) : this(control, tileType, value) //the "this:" at the end runs the above constructor too
        {
            _maxValue = maxValue;
        }

        public FloorTile(ControlClass control, TileType tileType, int value, double modifier) : this(control, tileType, value)
        {
            _modifer = modifier;
        }

        public int GetValue()
        {
            return _value;
        }

        public TileType GetTileType()
        {
            return _tileType;
        }

        public TileType GetTileTypeFromColour(Color colour)
        {
            switch (colour.ToArgb())
            {
                case (-256): //TODO
                    {
                        return TileType.Goal;
                    }
                case (-16744448):
                    {
                        return TileType.Home;
                    }
                default:
                    {
                        if (colour.R == 100 && colour.G == 100) //determine what colour a pheremone is (data is stored in the blue value of the pheremone)
                        {
                            return TileType.Special;
                        }
                        else
                        {
                            return TileType.Blank;
                        }
                    }
            }
        }

        public bool IncrementValue(int increment)
        {
            int result = _value + increment;
            if (result < 0 || result > _maxValue)
            {
                return false;
            }
            else
            {
                _value = result;
                return true;
            }
        }

        public bool ModifyValue()
        {
            double temp_value = (double) _value * _modifer;
            _value = (int)temp_value;

            if (_value < 25)
            {
                return false; //colour is now 'black'
            }

            int colour_int = (_value > 255)? 255 : _value;

            _colour = Color.FromArgb(0,0,colour_int);
            return true;
        }

        public Color GetColour()
        {
            return _colour;
        }

        public void SetColour(Color colour)
        {
            if (_tileType != TileType.Special)
            {
                throw (new Exception("Tile must be of type 'special' to give it a custom colour"));
                return;
            }
            else
            {
                _colour = colour;
            }
        }
    }
}
