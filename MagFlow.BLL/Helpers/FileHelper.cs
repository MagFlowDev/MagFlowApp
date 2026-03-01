using SkiaSharp;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.BLL.Helpers
{
    public static class FileHelper
    {
        public static byte[] ResizeIfLarger(byte[] imageBytes, int maxSize = 384, int jpegQuality = 90)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                throw new ArgumentException("imageBytes is null or empty", nameof(imageBytes));

            using var input = new MemoryStream(imageBytes);
            using var codes = SKCodec.Create(input);
            if (codes == null)
                return imageBytes;

            var info = codes.Info;
            if(info.Width <= maxSize && info.Height <= maxSize)
                return imageBytes;

            float scale = Math.Min((float)maxSize / info.Width, (float)maxSize / info.Height);
            int newWidth = Math.Max(1, (int)Math.Round(info.Width * scale));
            int newHeight = Math.Max(1, (int)Math.Round(info.Height * scale));

            input.Position = 0;
            using var original = SKBitmap.Decode(input);
            if(original == null)
                return imageBytes;

            var sampling = SKSamplingOptions.Default;
            using var resizedBitmap = original.Resize(new SKImageInfo(newWidth, newHeight), sampling);
            if (resizedBitmap == null)
                return imageBytes;

            using var image = SKImage.FromBitmap(resizedBitmap);
                    
            var format = codes.EncodedFormat;
            SKEncodedImageFormat encodedFormat = format switch
            {
                SKEncodedImageFormat.Jpeg => SKEncodedImageFormat.Jpeg,
                SKEncodedImageFormat.Png => SKEncodedImageFormat.Png,
                SKEncodedImageFormat.Gif => SKEncodedImageFormat.Gif,
                SKEncodedImageFormat.Webp => SKEncodedImageFormat.Webp,
                SKEncodedImageFormat.Bmp => SKEncodedImageFormat.Bmp,
                SKEncodedImageFormat.Ico => SKEncodedImageFormat.Ico,
                _ => SKEncodedImageFormat.Jpeg
            };

            int quality = encodedFormat == SKEncodedImageFormat.Jpeg ? jpegQuality : 100;

            using var data = image.Encode(encodedFormat, quality);
            return data?.ToArray() ?? imageBytes;
        }
    }
}
