using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace APP.Helpers
{
    public static class BitmapHelper
    {
        #region Blur

        public static void Blur(this Bitmap sourceImage, int radius)
        {
            var rct = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);
            var dest = new int[rct.Width*rct.Height];
            var source = new int[rct.Width*rct.Height];
            var bits = sourceImage.LockBits(rct, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Marshal.Copy(bits.Scan0, source, 0, source.Length);
            sourceImage.UnlockBits(bits);

            if (radius < 1) return;

            int w = rct.Width;
            int h = rct.Height;
            int wm = w - 1;
            int hm = h - 1;
            int wh = w*h;
            int div = radius + radius + 1;
            var r = new int[wh];
            var g = new int[wh];
            var b = new int[wh];
            int rsum, gsum, bsum, x, y, i, p1, p2, yi;
            var vmin = new int[Max(w, h)];
            var vmax = new int[Max(w, h)];

            var dv = new int[256*div];
            for (i = 0; i < 256*div; i++)
            {
                dv[i] = (i/div);
            }

            int yw = yi = 0;

            for (y = 0; y < h; y++)
            {
                // blur horizontal
                rsum = gsum = bsum = 0;
                for (i = -radius; i <= radius; i++)
                {
                    int p = source[yi + Min(wm, Max(i, 0))];
                    rsum += (p & 0xff0000) >> 16;
                    gsum += (p & 0x00ff00) >> 8;
                    bsum += p & 0x0000ff;
                }
                for (x = 0; x < w; x++)
                {
                    r[yi] = dv[rsum];
                    g[yi] = dv[gsum];
                    b[yi] = dv[bsum];

                    if (y == 0)
                    {
                        vmin[x] = Min(x + radius + 1, wm);
                        vmax[x] = Max(x - radius, 0);
                    }
                    p1 = source[yw + vmin[x]];
                    p2 = source[yw + vmax[x]];

                    rsum += ((p1 & 0xff0000) - (p2 & 0xff0000)) >> 16;
                    gsum += ((p1 & 0x00ff00) - (p2 & 0x00ff00)) >> 8;
                    bsum += (p1 & 0x0000ff) - (p2 & 0x0000ff);
                    yi++;
                }
                yw += w;
            }

            for (x = 0; x < w; x++)
            {
                // blur vertical
                rsum = gsum = bsum = 0;
                int yp = -radius*w;
                for (i = -radius; i <= radius; i++)
                {
                    yi = Max(0, yp) + x;
                    rsum += r[yi];
                    gsum += g[yi];
                    bsum += b[yi];
                    yp += w;
                }
                yi = x;
                for (y = 0; y < h; y++)
                {
                    dest[yi] = (int) (0xff000000u | (uint) (dv[rsum] << 16) | (uint) (dv[gsum] << 8) | (uint) dv[bsum]);
                    if (x == 0)
                    {
                        vmin[y] = Min(y + radius + 1, hm)*w;
                        vmax[y] = Max(y - radius, 0)*w;
                    }
                    p1 = x + vmin[y];
                    p2 = x + vmax[y];

                    rsum += r[p1] - r[p2];
                    gsum += g[p1] - g[p2];
                    bsum += b[p1] - b[p2];

                    yi += w;
                }
            }

            // copy back to image
            var bits2 = sourceImage.LockBits(rct, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Marshal.Copy(dest, 0, bits2.Scan0, dest.Length);
            sourceImage.UnlockBits(bits);
        }

        private static int Min(int a, int b)
        {
            return Math.Min(a, b);
        }

        private static int Max(int a, int b)
        {
            return Math.Max(a, b);
        }

        #endregion


        public static void Clear(this Bitmap sourceImage, Color color)
        {
            Graphics.FromImage(sourceImage).Clear(color);
        }

        #region FloodFill

        public static void FloodFill(this Bitmap bitmap, Point node, int gapSize)
        {
            Bitmap gapBitmap;
            FloodFill(bitmap, node, gapSize, out gapBitmap);
        }

        public static void FloodFill(this Bitmap bitmap, Point node, int gapSize, out Bitmap gapBitmap)
        {
            FloodFill(bitmap, new[] {node}, gapSize, out gapBitmap);
        }

        public static void FloodFill(this Bitmap bitmap, IEnumerable<Point> nodes, int gapSize)
        {
            Bitmap gapBitmap;
            FloodFill(bitmap, nodes, gapSize, out gapBitmap);
        }

        public static void FloodFill(this Bitmap bitmap, IEnumerable<Point> nodes, int gapSize, out Bitmap gapBitmap)
        {
            Rectangle dimensionRectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            //gapBitmap = (Bitmap)bitmap.Clone();

            gapBitmap = (Bitmap) bitmap.Clone();

            gapBitmap.Blur(gapSize);

            Bitmap gp = gapBitmap;
            Queue<FloodFillValues> queue = new Queue<FloodFillValues>();
            foreach (var node in nodes.OrderBy(point => gp.GetPixel(point.X, point.Y).R))
            {
                queue.Enqueue(new FloodFillValues {Node = node, LastGapValue = 255});
            }


            while (queue.Count > 0)
            {
                FloodFillValues fillValues = queue.Dequeue();

                if (!dimensionRectangle.Contains(fillValues.Node))
                {
                    continue;
                }

                if (bitmap.GetPixel(fillValues.Node.X, fillValues.Node.Y).R == 0)
                {
                    continue;
                }
                bitmap.SetPixel(fillValues.Node.X, fillValues.Node.Y, Color.Black);
                int thisGapValue = gapBitmap.GetPixel(fillValues.Node.X, fillValues.Node.Y).R;
                if (thisGapValue > fillValues.LastGapValue)
                {
                    continue;
                }


                if (fillValues.Node.X + 1 < bitmap.Width)
                {
                    queue.Enqueue(new FloodFillValues
                    {
                        Node = new Point(fillValues.Node.X + 1, fillValues.Node.Y),
                        LastGapValue = thisGapValue
                    });
                }
                if (fillValues.Node.X - 1 >= 0)
                {
                    queue.Enqueue(new FloodFillValues
                    {
                        Node = new Point(fillValues.Node.X - 1, fillValues.Node.Y),
                        LastGapValue = thisGapValue
                    });
                }
                if (fillValues.Node.Y + 1 < bitmap.Height)
                {
                    queue.Enqueue(new FloodFillValues
                    {
                        Node = new Point(fillValues.Node.X, fillValues.Node.Y + 1),
                        LastGapValue = thisGapValue
                    });
                }
                if (fillValues.Node.Y - 1 >= 0)
                {
                    queue.Enqueue(new FloodFillValues
                    {
                        Node = new Point(fillValues.Node.X, fillValues.Node.Y - 1),
                        LastGapValue = thisGapValue
                    });
                }
            }
            //FloodFill(node, bitmap, gapBitmap, 255);
        }

        private class FloodFillValues
        {
            public Point Node { get; set; }
            public int LastGapValue { get; set; }
        }

        #endregion
    }
}