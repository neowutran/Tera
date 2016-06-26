using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Tera.Game.Messages
{
    public class S_GET_USER_GUILD_LOGO : ParsedMessage
    {
        internal S_GET_USER_GUILD_LOGO(TeraMessageReader reader) : base(reader)
        {
            Offset = reader.ReadUInt16();
            Size = reader.ReadUInt16();
            PlayerId = reader.ReadUInt32();
            GuildId = reader.ReadUInt32();

            Console.WriteLine("icon size:"+Size+";offset:"+Offset+";player:"+PlayerId);

            var logo = reader.ReadBytes(Size);

            GuildLogo = new Bitmap(64,64,PixelFormat.Format8bppIndexed);
            if (Size < 0x1318)
            {
                return; //seems there can be some other icon format, so return to avoid exception
            }
        
            var palette = GuildLogo.Palette;
            for (var i = 0; i <= 255; i++)
            {
                palette.Entries[i] = Color.FromArgb(logo[0x14 + i*3], logo[0x15 + i*3], logo[0x16 + i*3]);
            }
            var pixels = GuildLogo.LockBits(new Rectangle(0, 0, 64, 64), ImageLockMode.WriteOnly, GuildLogo.PixelFormat);
            Marshal.Copy(logo, 0x318, pixels.Scan0, 0x1000);
            GuildLogo.UnlockBits(pixels);
            GuildLogo.Palette = palette;
            //GuildLogo.Save($"q:\\{Time.Ticks}.bmp",ImageFormat.Bmp);
            //System.IO.File.WriteAllBytes($"q:\\{Time.Ticks}.bin", logo);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool DestroyIcon(IntPtr handle);

        public Icon GetIcon()
        { // Get an Hicon for myBitmap.
            var Hicon = GuildLogo.GetHicon();
            var newIcon = Icon.FromHandle(Hicon);
            DestroyIcon(newIcon.Handle);
            return newIcon;
        }

        public int Offset { get; }
        public int Size { get; }

        public uint GuildId { get; }
        public uint PlayerId { get; }
        public Bitmap GuildLogo { get; }
    }
}