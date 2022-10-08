using System.Windows.Forms;

namespace GamesFarming.MVVM.Models
{
    public class Resolution
    {
        public const int DefaultWidth = 300;
        public const int DefaultHeight = 300;

        public static Resolution Default = new Resolution(DefaultHeight, DefaultWidth);
        public static Resolution GetUserResolution() 
        {
            var rect = Screen.PrimaryScreen.Bounds;
            return new Resolution(rect.Width, rect.Height);
        }

        public readonly int Width;
        public readonly int Height;

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
