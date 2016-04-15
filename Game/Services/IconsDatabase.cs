using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace Tera.Game
{
    public class IconsDatabase
    {
        private readonly Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();
        private readonly Dictionary<string, Bitmap> _bitmaps = new Dictionary<string, Bitmap>();
        private readonly Package _icons;
        public IconsDatabase(string ResourceDirectory)
        {
//            IconsDirectory = Path.Combine(ResourceDirectory, "icons/");
            _icons = Package.Open(ResourceDirectory + "icons.zip", FileMode.Open, FileAccess.Read);
        }

        public BitmapImage GetImage(string iconName)
        {
            BitmapImage image;
            if (_images.TryGetValue(iconName,out image)||string.IsNullOrEmpty(iconName))
            {
                return image;
            }
            image = new BitmapImage();
            Uri ur = new Uri("/" + iconName + ".png",UriKind.Relative);
            if (_icons.PartExists(ur))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = _icons.GetPart(ur).GetStream();
                image.EndInit();
            }
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
            Uri ur = new Uri("/" + iconName + ".png", UriKind.Relative);
            if (_icons.PartExists(ur)) image = new Bitmap(_icons.GetPart(ur).GetStream());
            else image = new Bitmap(1, 1);
            _bitmaps.Add(iconName, image);
            return image;
        }
    }
}
