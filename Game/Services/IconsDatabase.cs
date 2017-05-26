using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Packaging;
using System.Windows.Media.Imaging;

namespace Tera.Game
{
    public class IconsDatabase
    {
        private readonly Dictionary<string, Bitmap> _bitmaps = new Dictionary<string, Bitmap>();
        private readonly Package _icons;
        private readonly Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();
        private readonly BitmapImage _emptyBitmap;

        public IconsDatabase(string resourceDirectory)
        {
//            IconsDirectory = Path.Combine(resourceDirectory, "icons/");
            _icons = Package.Open(resourceDirectory + "icons.zip", FileMode.Open, FileAccess.Read, FileShare.Read);
            using (MemoryStream memory = new MemoryStream())
            {
                (new Bitmap(1,1)).Save(memory,ImageFormat.Bmp);
                memory.Position = 0;
                _emptyBitmap = new BitmapImage();
                _emptyBitmap.BeginInit();
                _emptyBitmap.StreamSource = memory;
                _emptyBitmap.CacheOption=BitmapCacheOption.OnLoad;
                _emptyBitmap.EndInit();
                _emptyBitmap.Freeze();
            }
        }

        public BitmapImage GetImage(string iconName)
        {
            if (string.IsNullOrEmpty(iconName)) return _emptyBitmap;
            BitmapImage image;
            if (_images.TryGetValue(iconName, out image))
            {
                return image;
            }
            var ur = new Uri("/" + iconName + ".png", UriKind.Relative);
            if (_icons.PartExists(ur))
            {
                image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = _icons.GetPart(ur).GetStream();
                image.EndInit();
                image.Freeze();
            }
            else { image = _emptyBitmap; }
            //var filename = IconsDirectory + iconName + ".png";
            //image = new BitmapImage(new Uri(filename));
            _images.Add(iconName, image);
            return image;
        }

        public Bitmap GetBitmap(string iconName)
        {
            Bitmap image;
            if (_bitmaps.TryGetValue(iconName, out image) || string.IsNullOrEmpty(iconName))
            {
                return image;
            }
            var ur = new Uri("/" + iconName + ".png", UriKind.Relative);
            image = _icons.PartExists(ur) ? new Bitmap(_icons.GetPart(ur).GetStream()) : new Bitmap(1, 1);
            _bitmaps.Add(iconName, image);
            return image;
        }
    }
}