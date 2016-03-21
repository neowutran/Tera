using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tera.Game
{
    public struct CharmStatus
    {
        public uint Unk1 { get; internal set; }
        public uint Status { get; internal set; } // 0=idle, 1= active
        public uint CharmId { get; internal set; }
        public uint Duration { get; internal set; }
        public uint Unk2 { get; internal set; } //unk1=unk2, 0=supplementary , 29=attack, 42=defence
    }
}
