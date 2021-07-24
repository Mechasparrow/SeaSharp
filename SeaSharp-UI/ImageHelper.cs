using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SeaSharp_UI
{
    static class ImageHelper
    {

        //Adapted from .NET Class Playland Image example.
        public static BitmapImage LoadBitmapImage(string path, int decodeWidth)
        {
            string assetPath = @"Assets/" + path;
            string newPath = System.IO.Path.Combine(Environment.CurrentDirectory, assetPath);

            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(newPath);
            myBitmapImage.DecodePixelWidth = decodeWidth;
            myBitmapImage.EndInit();

            return myBitmapImage;
        }
    }
}
