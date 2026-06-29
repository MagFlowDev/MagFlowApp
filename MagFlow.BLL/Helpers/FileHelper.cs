using PhotoSauce.MagicScaler;

namespace MagFlow.BLL.Helpers
{
    public static class FileHelper
    {
        public static byte[] ResizeIfLarger(byte[] imageBytes, int maxSize = 384, int jpegQuality = 90)
        {
            try
            {
                if (imageBytes == null || imageBytes.Length == 0)
                    throw new ArgumentException("imageBytes is null or empty", nameof(imageBytes));

                using var input = new MemoryStream(imageBytes);
                var fileInfo = ImageFileInfo.Load(input);

                if (fileInfo.Frames[0].Width <= maxSize && fileInfo.Frames[0].Height <= maxSize)
                    return imageBytes;

                var settings = new ProcessImageSettings
                {
                    Width = maxSize,
                    Height = maxSize,
                    ResizeMode = CropScaleMode.Max,
                    EncoderOptions = new JpegEncoderOptions(Quality: jpegQuality, Subsample: ChromaSubsampleMode.Default)
                };
                settings.TrySetEncoderFormat(ImageMimeTypes.Jpeg);

                input.Position = 0;
                using var output = new MemoryStream();
                MagicImageProcessor.ProcessImage(input, output, settings);

                return output.ToArray();
            }
            catch(Exception ex)
            {
                return imageBytes;
            }
        }
    }
}
