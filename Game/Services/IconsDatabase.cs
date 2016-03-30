using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
namespace Tera.Game
{
    public class IconsDatabase
    {
        private readonly Dictionary<string, BitmapImage> _images = new Dictionary<string, BitmapImage>();
        private readonly string IconsDirectory;
        public IconsDatabase(string ResourceDirectory)
        {
            IconsDirectory = Path.Combine(ResourceDirectory, "icons/");
        }

        public BitmapImage GetImage(string iconName)
        {
            BitmapImage image = new BitmapImage();
            if (_images.TryGetValue(iconName,out image)||string.IsNullOrEmpty(iconName))
            {
                return image;
            }
            var filename = IconsDirectory + iconName + ".png";
            image = new BitmapImage(new Uri(filename));
            _images.Add(iconName, image);
            return image;
        }
    }
}
