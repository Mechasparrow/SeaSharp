using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SeaSharp_UI
{
    static class ImageHelper
    {

        //Adapted from .NET Class Playland Image example.
        public static BitmapSource LoadBitmapImage(string path, int decodeWidth)
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

        public static BitmapSource LoadBitmapImage(string path, int decodeWidth, double rotation)
        {
            BitmapSource loadedBitmap = LoadBitmapImage(path, decodeWidth);

            TransformedBitmap transformedBitmap = new TransformedBitmap();

            transformedBitmap.BeginInit();
            transformedBitmap.Source = loadedBitmap;
            RotateTransform transform = new RotateTransform(rotation);
            transformedBitmap.Transform = transform;
            transformedBitmap.EndInit();

            return transformedBitmap;
        }
    }
}
