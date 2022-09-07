namespace GamesFarming.MVVM.Models
{
    public class Resolution
    {
        public const int DefaultWidth = 300;
        public const int DefaultHeight = 300;

        public static Resolution Default = new Resolution(DefaultHeight, DefaultWidth);

        public readonly int Width;
        public readonly int Height;

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
