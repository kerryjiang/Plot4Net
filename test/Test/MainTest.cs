using System;
using System.IO;
using Plot4Net;
using SkiaSharp;
using Xunit;

namespace Test
{
    public class MainTest
    {
        [Fact]
        public void TestBarChart()
        {
            var entries = new[]
            {
                new Entry()
                {
                    Y = 200,
                    Label = "January",
                    Color = SKColor.Parse("#266489")
                },
                new Entry()
                {
                    Y = 400,
                    Label = "February",
                    Color = SKColor.Parse("#68B9C0")
                },
                new Entry()
                {
                    Y = 600,
                    Label = "March",
                    Color = SKColor.Parse("#90D585")
                }
            };

            var chart = new BarChart();
            chart.Entries = entries;

            var dir = Path.Combine(AppContext.BaseDirectory, "images");

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var width = 500;
            var height = 300;

            using (var bitmap = new SKBitmap(width, height))
            using (var canvas = new SKCanvas(bitmap))
            {
                chart.Draw(canvas, width, height);
                var pngImage = SKImage.FromBitmap(bitmap).Encode(SKEncodedImageFormat.Png, 100);

                using (var fs = File.Create(Path.Combine(dir, Guid.NewGuid().ToString() + ".png")))
                {
                    pngImage.SaveTo(fs);
                    fs.Flush();
                    fs.Close();
                }
            }
        }
    }
}
