using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Graphics;

using FreeMoO.Collections;
using FreeMoO.Game;
using FreeMoO.Screens;
using FreeMoO.Service;

namespace FreeMoO.Controls
{
    public class ShipPathLine
        : AbstractControl
    {
        private ShipPathColor _pathColor;
        private int _xPosStart;
        private int _yPosStart;
        private int _xPosEnd;
        private int _yPosEnd;

        private int _animationFrame;
        private int _totalFrames;
        private double _lastUpdate;
        public MainStarmap StarMap {get; set;}
        
        private static readonly int[] GreenColors = { 0x002400, 0x004900, 0x007100, 0x009A00, 0x1CAE1C };
        private static readonly int[] RedColors = { 0xB21C1C, 0x9E1818, 0x8A1414, 0xD72020, 0xC72020 };
        private static readonly int[] GrayColors = { 0x45456D, 0x38385D, 0x28284D, 0x1C1C3C, 0x55557D  };

        private const double ANIMATION_INTERVAL = 200.0; // animation interval in milliseconds

        private List<PointF> _linePath = new List<PointF>();

        public Point StartPoint
        {
            set
            {
                _xPosStart = value.X;
                _yPosStart = value.Y;
                RecalculateLinePoints();
            }
        }

        public Point EndPoint
        {
            set
            {
                _xPosEnd = value.X;
                _yPosEnd = value.Y;
                RecalculateLinePoints();
            }
        }

        public ShipPathColor PathColor
        {
            get
            {
                return _pathColor;
            }

            set
            {
                _pathColor = value;
                _animationFrame = 0;
                _totalFrames = GetColorArrayLength();
            }
        }

        private void RecalculateLinePoints()
        {
            //_linePath.Clear();
            if (_linePath.Count > 0)
            {
                foreach(var l in _linePath)
                {
                    if (l != null) ObjectPool.PointFObjPool.PutObject(l);
                }
                _linePath.Clear();
            }

            float xStart = (float)_xPosStart;
            float yStart = (float)_yPosStart;
            float xEnd = (float)_xPosEnd;
            float yEnd = (float)_yPosEnd;

            float rise = (yEnd - yStart);
            float run = (xEnd - xStart);
            var slope = rise / run;
            var xstep = run >= 0 ? 1 : -1;

            var p = ObjectPool.PointFObjPool.GetObject();
            p.X = xStart;
            p.Y = yStart;
            _linePath.Add(p);
            while (!Arrived(_linePath[_linePath.Count - 1].X, xEnd, xstep >= 0) || !Arrived(_linePath[_linePath.Count - 1].Y, yEnd, slope >= 0))
            {
                var tmpx = !Arrived(_linePath[_linePath.Count - 1].X, xEnd, xstep >= 0) ?  _linePath[_linePath.Count - 1].X + xstep : _linePath[_linePath.Count - 1].X;
                var tmpy = !Arrived(_linePath[_linePath.Count - 1].Y, yEnd, slope >= 0) ? _linePath[_linePath.Count - 1].Y + slope : _linePath[_linePath.Count - 1].Y;
                var np = ObjectPool.PointFObjPool.GetObject();
                np.X = tmpx; np.Y = tmpy;
                _linePath.Add(np);
            }

        }

        private bool Arrived(float curr, float dest, bool up)
        {
            if (up)
            {
                return curr >= dest;
            }
            return curr <= dest;
        }

        public void Reinitialize(int xstart, int ystart, int xend, int yend, ShipPathColor col, double currMili, MainStarmap map)
        {
            _xPosEnd = xend; _xPosStart = xstart; _yPosEnd = yend; _yPosStart = ystart;
            _pathColor = col;
            _animationFrame = 0;
            _lastUpdate = currMili;
            StarMap = map;
            RecalculateLinePoints();
        }

        public override void Update(Timer timer)
        {
            // update animation counter
            if (timer.TotalMilliseconds - _lastUpdate >= ANIMATION_INTERVAL)
            {
                _lastUpdate = timer.TotalMilliseconds;
                //_animationFrame = _animationFrame >= GetColorArrayLength() ? 0 : _animationFrame + 1;
                _animationFrame++;
                if (_animationFrame >= GetColorArrayLength()) _animationFrame = 0;
            }
        }

        public override void Draw(Timer timer, GuiService guiService)
        {
            var bounds = StarMap.GetBoundingRectangle();
            int colorIdx = _animationFrame;
            int colArrLength = GetColorArrayLength();
            for(int i = 0; i < _linePath.Count; i++)
            {
                if(bounds.Contains((int)_linePath[i].X, (int)_linePath[i].Y))
                {
                    var color = Color.FromArgb(GetColorAtIndex(colorIdx));
                    var x = (int)_linePath[i].X - StarMap.View_X;
                    var y = (int)_linePath[i].Y - StarMap.View_Y;
                    guiService.drawRect(x, y, 1, 1, color);
                    
                }
                colorIdx++;
                if (colorIdx >= colArrLength) colorIdx = 0;
            }            
        }

        private int GetColorArrayLength()
        {
            switch (_pathColor)
            {
                case ShipPathColor.Green:
                    return GreenColors.Length;
                case ShipPathColor.Red:
                    return RedColors.Length;
                case ShipPathColor.Gray:
                    return GrayColors.Length;
            }
            return 0;
        }

        private int GetColorAtIndex(int idx)
        {
            switch(_pathColor)
            {
                case ShipPathColor.Green:
                    return GreenColors[idx];
                case ShipPathColor.Red:
                    return RedColors[idx];
                case ShipPathColor.Gray:
                    return GrayColors[idx];
            }
            return 0;
        }


    }
}
