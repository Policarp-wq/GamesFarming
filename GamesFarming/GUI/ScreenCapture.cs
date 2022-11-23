using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GamesFarming.GUI
{
    public static class ScreenCapture
    {
        public static Color GetPixelColor(Point p)
        {
            using (Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                    Screen.PrimaryScreen.Bounds.Y,
                    0, 0,
                    Screen.PrimaryScreen.Bounds.Size,
                    CopyPixelOperation.SourceCopy);
                }
                return bmp.GetPixel(p.X, p.Y);
            }
        }
        public static bool PixelEquals(Color expect, Point p) {
            Color color = GetPixelColor(p);
            return expect.R == color.R && expect.G == color.G && expect.B == color.B;
        }
    }
}
