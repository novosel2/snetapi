using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using FFMpegCore.Pipes;
using FFMpegCore;
using Xabe.FFmpeg;

namespace Core.Helpers
{
    public static class FileHelper
    {
        public static void ProcessImage(Stream inputStream, Stream outputStream, int width, int height, int quality = 85)
        {
            using var image = Image.Load(inputStream);

            if (image.Size.Width > width || image.Size.Height > height)
            {
                image.Mutate(x => x.Resize(new ResizeOptions()
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(width, height)
                }));
            }

            image.Save(outputStream, new JpegEncoder { Quality = quality });
        }

        public static void ProcessVideo()
        {

        }
    }
}
