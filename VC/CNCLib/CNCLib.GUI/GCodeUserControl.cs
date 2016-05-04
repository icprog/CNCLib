﻿////////////////////////////////////////////////////////
/*
  This file is part of CNCLib - A library for stepper motors.

  Copyright (c) 2013-2016 Herbert Aitenbichler

  CNCLib is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  CNCLib is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.
  http://www.gnu.org/licenses/
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using Framework.Tools.Drawing;
using CNCLib.GCode.Commands;
using Framework.Arduino;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace CNCLib.GUI
{
	public partial class GCodeUserControl : UserControl , IOutputCommand
	{
		#region crt

		public GCodeUserControl()
		{
            InitializeComponent();

            SetStyle(ControlStyles.DoubleBuffer, true);
            InitPen();
        }

        #endregion

        #region Properties

        public decimal SizeX { get { return _sizeX; } set { _sizeX = value; CalcRatio(); } }
        public decimal SizeY { get { return _sizeY; } set { _sizeY = value; CalcRatio(); } }

        public bool KeepRatio { get { return _keepRatio; } set { _keepRatio = value; ReInitDraw(); } }

        public double Zoom { get { return _zoom; } set { _zoom = value; ReInitDraw(); } }
		public decimal OffsetX { get { return _offsetX; } set { _offsetX = value; ReInitDraw(); } }
		public decimal OffsetY { get { return _offsetY; } set { _offsetY = value; ReInitDraw(); } }
        public double CutterSize { get { return _cutterSize; } set { _cutterSize = value; ReInitDraw(); } }
        public double LaserSize { get { return _laserSize; } set { _laserSize = value; ReInitDraw(); } }

        public Color MachineColor { get { return _machineColor; } set { _machineColor = value; ReInitDraw(); } }
        public Color LaserColor { get { return _laserColor; } set { _laserColor = value; ReInitDraw(); } }

        public CommandList Commands { get { return _commands; } }

		public delegate void GCodeEventHandler(object o, GCoderUserControlEventArgs info);

		public event GCodeEventHandler GCodeMousePosition;
        public event GCodeEventHandler ZoomOffsetChanged;

        #endregion

        #region private Members

        bool    _keepRatio = true;
        double  _zoom = 1;
        decimal _sizeX = 130.000m;
        decimal _sizeY = 45.000m;
        decimal _offsetX = 0;
		decimal _offsetY = 0;
        double  _cutterSize = 0;
        double  _laserSize = 0.254;
        double  _ratioX = 1;
        double  _ratioY = 1;
        Color   _machineColor = Color.Black;
        Color   _laserColor = Color.Red;

        CommandList _commands = new CommandList();

		private ArduinoSerialCommunication Com
		{
			get { return Framework.Tools.Pattern.Singleton<ArduinoSerialCommunication>.Instance; }
		}

		#endregion

		#region Convert Coordinats

		decimal AdjustX(decimal xx)
		{
			// x: 0...
			return xx + OffsetX;
		}

		decimal AdjustY(decimal yy)
		{
			// y: 0...
			return SizeY - (yy + OffsetY);
		}

		Point3D FromClient(Point pt)
		{
			// with e.g.  867
			// max pt.X = 686 , pt.x can be 0
			return new Point3D(
							AdjustX((decimal) Math.Round(pt.X / _ratioX / Zoom, 3)),
                            AdjustY((decimal) Math.Round(pt.Y / _ratioY / Zoom, 3)),
							0);
		}

		Point ToClient(Point3D pt)
		{
			return new Point(
                ToClientXInt((double) (pt.X??0)), 
                ToClientYInt((double)(pt.Y ?? 0)));
		}

        double ToClientX(double val)
        {
            double x = (double)(val - (double) OffsetX) * Zoom;
            return _ratioX * x;
        }
        double ToClientY(double val)
        {
            double y = (double)(((double) SizeY - (val + (double)OffsetY))) * Zoom;
            return _ratioY * y;
        }
        int ToClientXInt(double val)
        {
            return (int)Math.Round(ToClientX(val), 0);
        }
        int ToClientYInt(double val)
        {
            return (int)Math.Round(ToClientY(val), 0);
        }

        const double SignX = 1.0;
        const double SignY = -1.0;

        int ToClientSizeX(double X)
        {
            double x = X * Zoom;
            return (int)Math.Round(_ratioX * x, 0);
        }
        int ToClientSizeY(double Y)
        {
            double y = Y * Zoom;
            return (int)Math.Round(_ratioY * y, 0);
        }

        #endregion

        #region Drag/Drop

        private bool _isdragging = false;
        private Point3D _mouseDown;
        private decimal _mouseDownOffsetX;
        private decimal _mouseDownOffsetY;
        private Stopwatch _sw = new Stopwatch();

        private void GCodeUserControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                _zoom *= 1.1;
            else
                _zoom /= 1.1;

            OnZoomOffsetChanged();
            Invalidate();
        }

        private void GCodeUserControl_MouseDown(object sender, MouseEventArgs e)
        {
            {
                if (!_isdragging)
                {
                    _mouseDown = FromClient(e.Location);
                    _mouseDownOffsetX = _offsetX;
                    _mouseDownOffsetY = _offsetY;
                    _sw.Start();
                }
                _isdragging = true;
            }
        }

        private void GCodeUserControl_MouseMove(object sender, MouseEventArgs e)
		{
			if (GCodeMousePosition != null)
			{
				GCodeMousePosition(this, new GCoderUserControlEventArgs() { GCodePosition = FromClient(e.Location) });
			}
            if (_isdragging)
            {
                _offsetX = _mouseDownOffsetX;
                _offsetY = _mouseDownOffsetY;
                Point3D c = FromClient(e.Location);
                decimal newX = _mouseDownOffsetX - (c.X.Value - _mouseDown.X.Value);
                decimal newY = _mouseDownOffsetY + (c.Y.Value - _mouseDown.Y.Value);
                _offsetX = newX;
                _offsetY = newY;
                OnZoomOffsetChanged();
                if (_sw.ElapsedMilliseconds > 300)
                {
                    _sw.Start();
                    Invalidate();
                }
            }
        }

        private void GCodeUserControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (_isdragging)
            {
                Invalidate();
                OnZoomOffsetChanged();
            }
            _isdragging = false;
        }

        private void OnZoomOffsetChanged()
        {
            if (ZoomOffsetChanged != null)
            {
                InitPen();
                ZoomOffsetChanged(this, new GCoderUserControlEventArgs() );
            }
        }

        #endregion

        #region private

        private void ReInitDraw()
        {
            InitPen();
            Invalidate();
        }
        private void InitPen()
        {
            Color cutColor = MachineColor == Color.White ? Color.Black : Color.White;
            float cutsize = CutterSize > 0 ? (float)ToClient(new Point3D(OffsetX + (decimal)CutterSize, 0m, 0m)).X : 2;
            float fastSize = 0.5f;

            _cutLine = new Pen(cutColor, cutsize);
            _cutLine.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            _cutLine.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            _cutDot     = new Pen(Color.Blue, cutsize);
            _cutEllipse = new Pen(Color.Cyan, cutsize);

            _cut = new Pen[] { _cutLine, _cutDot, _cutEllipse };

            _fastLine = new Pen(Color.Green, fastSize);
            _NoMove = new Pen(Color.Blue, fastSize);
            _laserCutLine = new Pen(LaserColor, ToClient(new Point3D(OffsetX+(decimal)LaserSize,0m,0m)).X);
            _laserCutLine.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            _laserCutLine.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            _laserFastLine = new Pen(Color.Orange, (float)(fastSize / 2.0));
            _machineLine = new Pen(Color.LightBlue, 1);

            _helpLine = new Pen(Color.Yellow, 1);
        }

        private void PlotterUserControl_Paint(object sender, PaintEventArgs e)
		{
            //Create a Bitmap object with the size of the form
            Bitmap curBitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            //Create a temporary Graphics object from the bitmap
            Graphics g1 = Graphics.FromImage(curBitmap);
            g1.InterpolationMode = InterpolationMode.NearestNeighbor;
            g1.SmoothingMode = SmoothingMode.None;
            g1.PixelOffsetMode = PixelOffsetMode.None;
            g1.CompositingQuality = CompositingQuality.HighSpeed;
            g1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;

            //Draw lines on the temporary Graphics object

            var ee = new PaintEventArgs(g1, new Rectangle());

            Point from = ToClient(new Point3D(0, SizeY, 0));
            Point to = ToClient(new Point3D(SizeX, 0, 0m));
            Size sz = new Size(to.X - from.X, to.Y - from.Y);
            Rectangle rc = new Rectangle(from, sz);

            g1.FillRectangle(new SolidBrush(MachineColor), rc);

            _commands.Paint(this, ee);

            //Call DrawImage of Graphics and draw bitmap
            e.Graphics.DrawImage(curBitmap, 0, 0);
            //Dispose of objects
            g1.Dispose();
            curBitmap.Dispose();
       }

        private void CalcRatio()
        {
            _ratioX = ClientSize.Width / (double) SizeX;
            _ratioY = ClientSize.Height / (double) SizeY;

            if (KeepRatio)
            {
                if (_ratioX > _ratioY) _ratioX = _ratioY;
                else if (_ratioX < _ratioY) _ratioY = _ratioX;
            }
       }

        private Size _lastsize;
		private void PlotterUserControl_Resize(object sender, EventArgs e)
		{
			if (_lastsize.Height != 0 && Size.Width > 0 && Size.Height > 0)
			{
				//RecalcClientCoord();
			}
			_lastsize = Size;
            CalcRatio();
            Invalidate();
		}

        #endregion

        #region IOutput 

        Pen _NoMove;
        Pen _cutEllipse;
        Pen _cutLine;
        Pen _cutDot;
        Pen[] _cut;
        Pen _fastLine;
        Pen _laserCutLine;
        Pen _laserFastLine;
        Pen _machineLine;
        Pen _helpLine;

        public void DrawLine(Command cmd, object param, DrawType drawtype, Point3D ptFrom, Point3D ptTo)
        {
            if (drawtype == DrawType.NoDraw) return;

            PaintEventArgs e = (PaintEventArgs)param;

            Point from = ToClient(ptFrom);
            Point to = ToClient(ptTo);

            if (PreDrawLineOrArc(param, drawtype, from, to))
            {
                e.Graphics.DrawLine(GetPen(drawtype, LineDrawType.Line), from, to);
            }
        }

        private bool PreDrawLineOrArc(object param, DrawType drawtype, Point from, Point to)
        {
            PaintEventArgs e = (PaintEventArgs)param;

            if (from.Equals(to))
            {
                if (drawtype == DrawType.LaserCut)
                    e.Graphics.DrawEllipse(GetPen(drawtype, LineDrawType.Dot), from.X, from.Y, 1, 1);
                else
                    e.Graphics.DrawEllipse(GetPen(drawtype, LineDrawType.Dot), from.X, from.Y, 4, 4);

                return false;
            }
            return true;
        }

        public void DrawEllipse(Command cmd, object param, DrawType drawtype, Point3D ptFrom, int xradius, int yradius)
		{
            if (drawtype == DrawType.NoDraw) return;

			PaintEventArgs e = (PaintEventArgs)param;
			Point from = ToClient(ptFrom);
			e.Graphics.DrawEllipse(GetPen(drawtype, LineDrawType.Ellipse), from.X, from.Y, xradius, yradius);
		}

        double ConvertRadToDeg(double rad)
        {
            double deg = -rad * 180.0 / Math.PI + 180.0;
            while (deg < 0)
                deg += 360;

            while (deg > 360)
                deg -= 360;

            return deg;
        }


        public void DrawArc(Command cmd, object param, DrawType drawtype, Point3D ptFrom, Point3D ptTo, Point3D pIJ, bool clockwise)
        {
            if (drawtype == DrawType.NoDraw) return;

            PaintEventArgs e = (PaintEventArgs)param;

            Point from = ToClient(ptFrom);
            Point to = ToClient(ptTo);

            //e.Graphics.DrawLine(_helpLine, from, to);

            double I = (double)pIJ.X.Value;
            double J = (double)pIJ.Y.Value;
            double R = Math.Sqrt(I * I + J * J);

            double cx = (double)ptFrom.X.Value + I;
            double cy = (double)ptFrom.Y.Value + J;

            double startAng = ConvertRadToDeg(Math.Atan2(J, I));
            double endAng = ConvertRadToDeg(Math.Atan2(cy - (double)ptTo.Y.Value, cx - (double)ptTo.X.Value));
            double diffAng = (endAng - startAng);
            if (startAng > endAng)
                diffAng += 360;

            if (clockwise == false)
            {
                startAng = endAng;
                diffAng = 360 - diffAng;
                while (diffAng > 360)
                    diffAng -= 360;

                while (diffAng < -360)
                    diffAng += 360;
            }

            Point rcfrom = new Point(ToClientXInt(cx - R* SignX), ToClientYInt(cy - R* SignY));
            int RR = ToClientSizeX(R * 2);
            Rectangle rec = new Rectangle(rcfrom, new Size(RR,RR));

            //e.Graphics.DrawRectangle(_helpLine, rec);

            if (rec.Width > 0 && rec.Height > 0)
            {
                try
                {
                    e.Graphics.DrawArc(GetPen(drawtype, LineDrawType.Line), rec.X, rec.Y, rec.Width, rec.Height, (float)startAng, (float)diffAng);
                }
                catch (OutOfMemoryException)
                {
                    // ignore this Exception
                }
            }
        }

        enum LineDrawType
        {
            Line=0,
            Dot=1,
            Ellipse=2
        };

		private Pen GetPen(DrawType moveType, LineDrawType drawtype)
		{
			switch (moveType)
			{
				default:
				case DrawType.NoMove: return _NoMove;
				case DrawType.Fast: return _fastLine;
				case DrawType.Cut: return _cut[(int) drawtype];
                case DrawType.LaserFast: return _laserFastLine;
                case DrawType.LaserCut: return _laserCutLine;
            }
        }

		#endregion
	}
}
