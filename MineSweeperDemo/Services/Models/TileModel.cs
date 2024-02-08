using TileType = Services.Enums.TileEnum.TileType;

namespace Services.Models
{

    public class TileModel
    {
        public TileType Type { get; set; }
        public bool IsRevealed { get; set; }
        public bool HasFlag { get; set; }

        public TileModel(TileType type)
        {
            Type = type;
            IsRevealed = false;
            HasFlag = false;
        }
    }
}
