using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public static class ImageHelper
    {
        public static byte[] ProcessImage(byte[] imageBytes, int width, int height, int quality = 85)
        {
            using var image = Image.Load(imageBytes);

            if (image.Size.Width > width || image.Size.Height > height)
            {
                image.Mutate(x => x.Resize(new ResizeOptions()
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(width, height)
                }));
            }

            using var outputStream = new MemoryStream();
            image.Save(outputStream, new JpegEncoder { Quality = quality });

            return outputStream.ToArray();
        }
    }
}
