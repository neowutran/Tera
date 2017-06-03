using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Tera.Game.Messages
{
    public class S_GET_USER_GUILD_LOGO : ParsedMessage
    {
        internal S_GET_USER_GUILD_LOGO(TeraMessageReader reader) : base(reader)
        {
            var offset = reader.ReadUInt16();
            var size = reader.ReadUInt16();
            PlayerId = reader.ReadUInt32();
            GuildId = reader.ReadUInt32();
            //Debug.WriteLine("icon size:"+size+";offset:"+offset+";player:"+PlayerId+";GuildId:"+GuildId);

            if (offset==0||size < 20) {GuildLogo = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);return;}
            var logo = reader.ReadBytes(size);
            var width = BitConverter.ToInt32(logo, 8);
            var type = BitConverter.ToInt32(logo, 12);

            GuildLogo = new Bitmap(width, width ,PixelFormat.Format8bppIndexed);
            var palette = GuildLogo.Palette;
            int length;
            switch (type)
            {
                case 0:
                case 1:
                    var paletteSize = BitConverter.ToInt32(logo, 16);
                    if (paletteSize>=size-24) { Debug.WriteLine("palette size too big"); return;}
                    for (var i = 0; i < paletteSize; i++)
                    {
                        palette.Entries[i] = Color.FromArgb(logo[0x14 + i * 3], logo[0x15 + i * 3], logo[0x16 + i * 3]);
                    }
                    length = BitConverter.ToInt32(logo, paletteSize * 3 + 20);
                    break;
                case 2:
                case 3:
                    for (var i = 0; i < 255; i++)
                    {
                        palette.Entries[i] = Color.FromArgb(i, i, i);
                    }
                    length = BitConverter.ToInt32(logo, 16);
                    break;
                default:
                    Debug.WriteLine("Unknown guild logo format");
                    return;
            }
            if (length>=size+20 || length!=width*width) { Debug.WriteLine("length mismatch"); return; }
            var pixels = GuildLogo.LockBits(new Rectangle(0, 0, width, width), ImageLockMode.WriteOnly, GuildLogo.PixelFormat);
            Marshal.Copy(logo, size-length, pixels.Scan0, length);
            GuildLogo.UnlockBits(pixels);
            GuildLogo.Palette = palette;
            //GuildLogo.Save($"q:\\{Time.Ticks}.bmp",ImageFormat.Bmp);
            //System.IO.File.WriteAllBytes($"q:\\{Time.Ticks}.bin", logo);
        }
            
        public uint GuildId { get; }
        public uint PlayerId { get; }
        public Bitmap GuildLogo { get; }
    }
}