using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace UI.Broadcast
{
    static class Screenshoter
    {
        public static Bitmap[] Screenshot(int fragments)
        {
            int sizeY = Screen.PrimaryScreen.Bounds.Height;
            int sizeX = Screen.PrimaryScreen.Bounds.Width;

            int fX = sizeX / fragments;

            using (var bmp = new Bitmap(sizeX, sizeY))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, new Size(sizeX, sizeY), CopyPixelOperation.SourceCopy);
                    g.Flush();
                }

                Bitmap[] bitmaps = new Bitmap[fragments];

                for (int i = 0; i < fragments; i++)
                    bitmaps[i] = bmp.Clone(new Rectangle(fX * i, 0, fX, sizeY), bmp.PixelFormat);


                return bitmaps;
            }
        }
    }
}
