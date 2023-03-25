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
                //bmp.SetPixel(p.X, p.Y, Color.Red);
                //bmp.SetPixel(p.X + 1, p.Y, Color.Red);
                //bmp.SetPixel(p.X - 1, p.Y, Color.Red);
                //bmp.SetPixel(p.X, p.Y + 1, Color.Red);
                //bmp.SetPixel(p.X, p.Y - 1, Color.Red);
                bmp.Save("B.jpg");
                return bmp.GetPixel(p.X, p.Y);
            }
        }
        public static bool PixelEquals(Color expect, Point p) 
        {
            p = new Point(p.X, p.Y);
            Color color = GetPixelColor(p);
            //MessageBox.Show(color.ToString());
            return expect.R == color.R && expect.G == color.G && expect.B == color.B;
        }
    }
}
