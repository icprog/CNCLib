﻿using Framework.Tools.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNCLib.Tests.Drawing
{
    [TestClass]
    public class DitherTest
    {
        public class DitherTestClass : DitherBase
        {
            protected override void ConvertImage()
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        Color currentPixel = GetPixel(x, y);

                        if ((x + y)%2 == 0)
                        {
                            SetPixel(x, y, 255,255,255,255);
                        }
                        else
                        {
                            SetPixel(x, y, 0, 0, 0, 255);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void ReadWriteImage()
        {
            var dt = new DitherTestClass();

            const int xsize= 100;
            const int ysize = 100;

            var b1 = new Bitmap(xsize, ysize);
            var b2 = dt.Process(b1);

            for (int x = 0; x < xsize; x++)
            {
                for (int y = 0; y < ysize; y++)
                {
                    Color col = b2.GetPixel(x, y);

                    if ((x + y) % 2 == 0)
                    {
                        Assert.AreEqual(255, col.R);
                        Assert.AreEqual(255, col.G);
                        Assert.AreEqual(255, col.B);
                    }
                    else
                    {
                        Assert.AreEqual(0, col.R);
                        Assert.AreEqual(0, col.G);
                        Assert.AreEqual(0, col.B);
                    }
                }
            }
        }


        [TestMethod]
        public void FloydSteinberg1()
        {
            var dt = new FloydSteinbergDither();

            const int xsize = 100;
            const int ysize = 100;

            var b1 = new Bitmap(xsize, ysize);

            b1.SetPixel(50, 50, Color.FromArgb(255, 255, 255, 255));

            var b2 = dt.Process(b1);

            for (int x = 0; x < xsize; x++)
            {
                for (int y = 0; y < ysize; y++)
                {
                    Color col = b2.GetPixel(x, y);

                    if (col.R != 0)
                    {
                        Assert.AreEqual(255, col.R);
                        Assert.AreEqual(255, col.R);
                        Assert.AreEqual(255, col.R);
                        Assert.AreEqual(50, x);
                        Assert.AreEqual(50, y);
                    }
                    else
                    {
                        Assert.AreEqual(0, col.R);
                        Assert.AreEqual(0, col.G);
                        Assert.AreEqual(0, col.B);
                    }
                }
            }
        }

        [TestMethod]
        public void FloydSteinberg2()
        {
            var dt = new FloydSteinbergDither();

            const int xsize = 100;
            const int ysize = 100;

            var b1 = new Bitmap(xsize, ysize, PixelFormat.Format32bppArgb);

            b1.SetPixel(50, 50, Color.FromArgb(255, 127, 127, 127));
			b1.SetPixel(50, 51, Color.FromArgb(255, 127, 127, 127));
			b1.SetPixel(50, 52, Color.FromArgb(255, 127, 127, 127));
			b1.SetPixel(50, 53, Color.FromArgb(255, 127, 127, 127));

			var b2 = dt.Process(b1);

            for (int x = 0; x < xsize; x++)
            {
                for (int y = 0; y < ysize; y++)
                {
                    Color col = b2.GetPixel(x, y);

                    if (col.R != 0)
                    {
                        Assert.AreEqual(255, col.R);
                        Assert.AreEqual(255, col.R);
                        Assert.AreEqual(255, col.R);
                        Assert.AreEqual(50, x);
                        Assert.IsTrue(y==53 || y==51);
                    }
                    else
                    {
                        Assert.AreEqual(0, col.R);
                        Assert.AreEqual(0, col.G);
                        Assert.AreEqual(0, col.B);
                    }
                }
            }
        }
    }
}