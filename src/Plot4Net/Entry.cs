using System;
using SkiaSharp;

namespace Plot4Net
{
    public class Entry
    {
        public float X { get; set; }

        public float Y { get; set; }

        public string Label { get; set; }

        public SKColor Color { get; set; } = SKColors.Black;

        public SKColor TextColor { get; set; } = SKColors.Gray;
    }
}