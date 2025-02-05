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

        public static async Task ProcessVideoAsync(string rootPath, string inputVideoPath, Stream outputStream)
        {
            FFmpeg.SetExecutablesPath(Path.GetTempPath());

            //string tempOutputPath = Path.Combine(rootPath, "wwwroot", "videos", "output", $"{Guid.NewGuid()}.mp4");

            //var arguments = await FFMpegArguments
            //    .FromFileInput(inputVideoPath)
            //    .OutputToFile(tempOutputPath, true, options =>
            //    {
            //        options.ForceFormat("mp4");
            //        options.WithVideoCodec("libx264");
            //        options.WithAudioCodec("aac");
            //        options.WithConstantRateFactor(23);
            //        options.WithAudioBitrate(128000);
            //        options.WithVideoBitrate(1000000);
            //        options.WithVideoFilters(filter => filter.Scale(1280, 720));
            //    }).ProcessAsynchronously();

            //using (var tempFileStream = new FileStream(tempOutputPath, FileMode.Open))
            //{
            //    await tempFileStream.CopyToAsync(outputStream);
            //}

            //File.Delete(tempOutputPath);
        }
    }
}
