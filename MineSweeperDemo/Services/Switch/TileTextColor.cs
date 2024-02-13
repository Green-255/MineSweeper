using System.Drawing;
using System.Windows.Media;

namespace Services.Switch
{
    internal class TileTextColor
    {
        public static System.Windows.Media.Color GetTileTextColor(String tileValue)
        {
            switch (tileValue)
            {
                case "1":
                    return Colors.Blue;
                case "2":
                    return Colors.Green;
                case "3":
                    return Colors.Red;
                case "4":
                    return Colors.Purple;
                case "5":
                    return Colors.Maroon;
                case "6":
                    return Colors.Turquoise;
                case "7":
                    return Colors.Cyan;
                case "8":
                    return Colors.Yellow;
                case "X":
                    return Colors.OrangeRed;
                default:
                    return Colors.Black;
            }
        }
    }
}
