using System;
using System.Linq;
using SkiaSharp;

namespace Plot4Net
{
    public abstract class Chart
    {
        public AxisStyle AxisXStyle { get; set; } = new AxisStyle
        {
            Visible = true,
            FontSize = 3f,
            ValueFormat = "0"
        };

        public AxisStyle AxisYStyle { get; set; } = new AxisStyle
        {
            Visible = true,
            FontSize = 3f,
            ValueFormat = "0"
        };

        public Entry[] Entries { get; set; }

        public float Margin { get; set; } = 20;

        public SKColor BackgroundColor { get; set; } = SKColors.White;

        public string Title { get; set; }

        public void Draw(SKCanvas canvas, int width, int height)
        {
            canvas.Clear(this.BackgroundColor);

            if (Margin > 0)
            {
                canvas.Translate(Margin, Margin);
                width -= (int)Margin * 2;
                height -= (int)Margin * 2;
            }

            var chartAreaWidth = width;
            var chartAreaHeight = height;
            var footerHeight = 25;
            var axisYAreaWidth = 30;

            if (AxisYStyle.Visible)
            {
                var yScalars = CalculateAxisYScalars();
                chartAreaWidth -= axisYAreaWidth;
                DrawAxisYArea(canvas, yScalars, axisYAreaWidth, height - footerHeight);             
            }

            if (AxisXStyle.Visible)
            {
                var xScalars = CalculateAxisXScalars();
                using (new SKAutoCanvasRestore(canvas))
                {
                    canvas.Translate(axisYAreaWidth, height - footerHeight);
                    DrawAxisXArea(canvas, xScalars, chartAreaWidth, footerHeight);
                }           
            }
        }

        protected void DrawAxisYArea(SKCanvas canvas, float[] scalars, int width, int height)
        {
            using (var paint = new SKPaint())
            {
                paint.IsStroke = true;
                paint.Color = SKColors.Black;
                paint.StrokeWidth = 1f;
                paint.Style = SKPaintStyle.Stroke;
                paint.IsAntialias = true;
                canvas.DrawLine(width, height, width, 0, paint);

                var unitLen = (float)height / scalars.Length;

                float y = 0f;

                for (var i = 0; i < scalars.Length; i++)
                {
                    canvas.DrawLine(width, y, width - 5, y, paint);
                    y += unitLen;
                }
            }
        }

        protected void DrawAxisXArea(SKCanvas canvas, float[] scalars, int width, int height)
        {
            using (var paint = new SKPaint())
            {
                paint.IsStroke = true;
                paint.Color = SKColors.Black;
                paint.StrokeWidth = 1f;
                paint.Style = SKPaintStyle.Stroke;
                paint.IsAntialias = true;
                canvas.DrawLine(0, 0, width, 0, paint);

                var unitLen = (float)width / scalars.Length;

                float x = unitLen;

                for (var i = 0; i < scalars.Length - 1; i++)
                {
                    canvas.DrawLine(x, 0, x, 5, paint);
                    x += unitLen;
                }
            }
        }

        private float[] CalculateAxisScalars(Func<Entry, float> valueGetter)
        {
            var max = Entries.Max(valueGetter);
            var min = Entries.Min(valueGetter);

            var cell = Math.Pow(10, max.ToString("0").Length - 1);
            var lowerCells = max / cell;
            var highCells = lowerCells + 1;

            max = (float)(highCells * cell);

            if (min > 0)
                min = 0;
            else
            {
                lowerCells = min / cell;
                min = (float)((lowerCells - 1) * cell);
            }

            var scalars = new float[(int)((max - min) / cell) + 1];

            var value = min;

            for (var i = 0; i < scalars.Length; i++)
            {
                scalars[i] = value;
                value += (float)cell;
            }

            return scalars; 
        }

        protected float[] CalculateAxisYScalars()
        {
            return CalculateAxisScalars(e => e.Y);
        }

        protected float[] CalculateAxisXScalars()
        {
            var hasXValues = Entries.Any(e => e.X != 0);

            if (hasXValues)
                return CalculateAxisScalars(e => e.X);

            return Enumerable.Range(1, Entries.Length + 1)
                .Select(e => e * 1f)
                .ToArray();
        }
    }
}