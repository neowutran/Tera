namespace Tera.Game
{
    public struct CharmStatus
    {
        public uint Status { get; internal set; } // 0=idle, 1= active
        public uint CharmId { get; internal set; }
        public uint Duration { get; internal set; }
    }
}