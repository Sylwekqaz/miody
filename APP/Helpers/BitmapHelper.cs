//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace APP.Helpers
//{
//    static class BitmapBlurHelper
//    {
//        public static Bitmap Blur(this Bitmap image, Int32 blurSize)
//        {
//            Rectangle rectangle = new Rectangle(0,0,image.Width,image.Height);
//            return Blur(image, rectangle, blurSize);
//        }

//        public static Bitmap Blur(this Bitmap image, Rectangle rectangle, Int32 blurSize)
//        {
//            Bitmap blurred = new Bitmap(image.Width, image.Height);

//            // make an exact copy of the bitmap provided
//            using (Graphics graphics = Graphics.FromImage(blurred))
//                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
//                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

//            // look at every pixel in the blur rectangle
//            for (Int32 xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
//            {
//                for (Int32 yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
//                {
//                    Int32 avgR = 0, avgG = 0, avgB = 0;
//                    Int32 blurPixelCount = 0;

//                    // average the color of the red, green and blue for each pixel in the
//                    // blur size while making sure you don't go outside the image bounds
//                    for (Int32 x = xx; (x < xx + blurSize && x < image.Width); x++)
//                    {
//                        for (Int32 y = yy; (y < yy + blurSize && y < image.Height); y++)
//                        {
//                            Color pixel = blurred.GetPixel(x, y);

//                            avgR += pixel.R;
//                            avgG += pixel.G;
//                            avgB += pixel.B;

//                            blurPixelCount++;
//                        }
//                    }

//                    avgR = avgR / blurPixelCount;
//                    avgG = avgG / blurPixelCount;
//                    avgB = avgB / blurPixelCount;

//                    // now that we know the average for the blur size, set each pixel to that color
//                    for (Int32 x = xx; x < xx + blurSize && x < image.Width && x < rectangle.Width; x++)
//                        for (Int32 y = yy; y < yy + blurSize && y < image.Height && y < rectangle.Height; y++)
//                            blurred.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
//                }
//            }

//            return blurred;
//        }
//    }
//}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace APP.Helpers
{
    public static class BitmapHelper
    {
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
            var vmin = new int[max(w, h)];
            var vmax = new int[max(w, h)];

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
                    int p = source[yi + min(wm, max(i, 0))];
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
                        vmin[x] = min(x + radius + 1, wm);
                        vmax[x] = max(x - radius, 0);
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
                    yi = max(0, yp) + x;
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
                        vmin[y] = min(y + radius + 1, hm)*w;
                        vmax[y] = max(y - radius, 0)*w;
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

        public static void Reduce(this Bitmap sourceImage, int radius)
        {
            throw new NotImplementedException();
        }


        public static void Clear(this Bitmap sourceImage, Color color)
        {
            for (int x = 0; x < sourceImage.Width; x++)
            {
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    sourceImage.SetPixel(x,y,color);
                }
            }
        }

        public static void FloodFill(this Bitmap bitmap,  Point node, int gapSize)
        {
#if DEBUG
            Bitmap debugBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            debugBitmap.SetPixel(node.X,node.Y,Color.DarkOrange);
            
#endif
            Bitmap gapBitmap = new Bitmap(bitmap.Width, bitmap.Height);
            gapBitmap.Clear(Color.White);
            //gapBitmap = (Bitmap)bitmap.Clone();
            Graphics gapGraphics = Graphics.FromImage(gapBitmap);
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    if (bitmap.GetPixel(x, y).ToArgb() == Color.Black.ToArgb())
                    {
                        gapBitmap.SetPixel(x, y, Color.Black);
                        //var rec = new Rectangle(new Point(x - gapSize, y - gapSize), new Size(2 * gapSize, 2 * gapSize));
                        //gapGraphics.FillEllipse(new SolidBrush(Color.Black), rec);
                    }
                }
            }
            gapBitmap.Blur(gapSize);


            Queue<FloodFillValues> queue = new Queue<FloodFillValues>();
            queue.Enqueue(new FloodFillValues {Node = node, LastGapValue = 255});

            while (queue.Count > 0)
            {
                FloodFillValues fillValues = queue.Dequeue();
                if (bitmap.GetPixel(fillValues.Node.X, fillValues.Node.Y).ToArgb() == Color.Black.ToArgb())
                {
                    continue;
                }
                int thisGapValue = gapBitmap.GetPixel(fillValues.Node.X, fillValues.Node.Y).R;
                if (thisGapValue > fillValues.LastGapValue)
                {
#if DEBUG
                    debugBitmap.SetPixel(fillValues.Node.X, fillValues.Node.Y,Color.Chartreuse);
#endif
                    continue;
                }
                bitmap.SetPixel(fillValues.Node.X, fillValues.Node.Y, Color.Black);

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


        private static int min(int a, int b)
        {
            return Math.Min(a, b);
        }

        private static int max(int a, int b)
        {
            return Math.Max(a, b);
        }
    }
}