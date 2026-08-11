using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

internal static class Program
{
    private static int Main(string[] args)
    {
        if (args.Length < 1 || args.Length > 2) { Console.Error.WriteLine("Usage: IconBuilder <output.ico> [preview.png]"); return 64; }
        var output = Path.GetFullPath(args[0]); Directory.CreateDirectory(Path.GetDirectoryName(output));
        var frames = new List<byte[]>();
        foreach (var size in new[] { 16, 24, 32, 48, 64, 128, 256 })
        {
            using (var bitmap = DrawIcon(size))
            using (var memory = new MemoryStream())
            { bitmap.Save(memory, ImageFormat.Png); frames.Add(memory.ToArray()); }
        }
        using (var stream = File.Create(output))
        using (var writer = new BinaryWriter(stream))
        {
            writer.Write((ushort)0); writer.Write((ushort)1); writer.Write((ushort)frames.Count);
            var offset = 6 + frames.Count * 16;
            for (var i = 0; i < frames.Count; i++)
            {
                var size = new[] { 16, 24, 32, 48, 64, 128, 256 }[i];
                writer.Write((byte)(size == 256 ? 0 : size)); writer.Write((byte)(size == 256 ? 0 : size));
                writer.Write((byte)0); writer.Write((byte)0); writer.Write((ushort)1); writer.Write((ushort)32);
                writer.Write(frames[i].Length); writer.Write(offset); offset += frames[i].Length;
            }
            foreach (var frame in frames) writer.Write(frame);
        }
        if (args.Length == 2)
        {
            var preview = Path.GetFullPath(args[1]); Directory.CreateDirectory(Path.GetDirectoryName(preview));
            using (var bitmap = DrawIcon(256)) bitmap.Save(preview, ImageFormat.Png);
        }
        return 0;
    }

    private static Bitmap DrawIcon(int size)
    {
        var bitmap = new Bitmap(size, size, PixelFormat.Format32bppArgb);
        using (var g = Graphics.FromImage(bitmap))
        {
            g.SmoothingMode = size <= 24 ? SmoothingMode.AntiAlias : SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality; g.CompositingQuality = CompositingQuality.HighQuality;
            var s = size / 64f;
            if (size >= 24)
            {
                using (var shadow = new SolidBrush(Color.FromArgb(46, 0, 0, 0)))
                    g.FillEllipse(shadow, 8*s, 53*s, 48*s, 7*s);
            }

            var driveRect = new RectangleF(4*s, 31*s, 56*s, 27*s);
            using (var path = Rounded(driveRect, 5*s))
            using (var drive = new LinearGradientBrush(driveRect, Color.FromArgb(245, 247, 250),
                       Color.FromArgb(115, 145, 166), LinearGradientMode.Vertical))
            using (var outline = new Pen(Color.FromArgb(62, 87, 105), Math.Max(1, 1.2f*s)))
            {
                g.FillPath(drive, path); g.DrawPath(outline, path);
            }
            using (var highlight = new Pen(Color.FromArgb(210, 255, 255, 255), Math.Max(1, s)))
                g.DrawLine(highlight, 9*s, 35*s, 55*s, 35*s);
            using (var slot = new LinearGradientBrush(new RectangleF(10*s, 41*s, 35*s, 5*s),
                       Color.FromArgb(61, 91, 112), Color.FromArgb(178, 202, 216), LinearGradientMode.Vertical))
                g.FillRectangle(slot, 10*s, 41*s, 35*s, Math.Max(2, 4*s));
            using (var light = new SolidBrush(Color.FromArgb(55, 191, 69))) g.FillEllipse(light, 50*s, 47*s, Math.Max(2, 5*s), Math.Max(2, 5*s));

            DrawWindow(g, s, size <= 24);
            DrawArrow(g, s, size <= 24);
        }
        return bitmap;
    }

    private static void DrawWindow(Graphics g, float s, bool simple)
    {
        var panes = new[]
        {
            new RectangleF(7*s, 7*s, 13*s, 10*s), new RectangleF(22*s, 5*s, 14*s, 12*s),
            new RectangleF(7*s, 19*s, 13*s, 10*s), new RectangleF(22*s, 19*s, 14*s, 12*s)
        };
        var colors = simple
            ? new[] { Color.FromArgb(40, 128, 196), Color.FromArgb(40, 128, 196), Color.FromArgb(40, 128, 196), Color.FromArgb(40, 128, 196) }
            : new[] { Color.FromArgb(55, 146, 211), Color.FromArgb(32, 116, 187), Color.FromArgb(35, 124, 193), Color.FromArgb(18, 91, 158) };
        for (var i = 0; i < panes.Length; i++)
        {
            using (var brush = new LinearGradientBrush(panes[i], Color.FromArgb(130, 211, 247), colors[i], LinearGradientMode.Vertical))
                g.FillRectangle(brush, panes[i]);
            using (var pen = new Pen(Color.FromArgb(19, 78, 129), Math.Max(1, .7f*s))) g.DrawRectangle(pen, panes[i].X, panes[i].Y, panes[i].Width, panes[i].Height);
        }
    }

    private static void DrawArrow(Graphics g, float s, bool simple)
    {
        var points = new[] { new PointF(44*s, 10*s), new PointF(53*s, 10*s), new PointF(53*s, 19*s),
            new PointF(59*s, 19*s), new PointF(48.5f*s, 30*s), new PointF(38*s, 19*s), new PointF(44*s, 19*s) };
        using (var path = new GraphicsPath())
        {
            path.AddPolygon(points);
            using (var brush = simple ? (Brush)new SolidBrush(Color.FromArgb(34, 137, 207)) :
                       new LinearGradientBrush(new RectangleF(38*s, 10*s, 21*s, 20*s), Color.FromArgb(112, 211, 247), Color.FromArgb(22, 108, 181), LinearGradientMode.Vertical))
                g.FillPath(brush, path);
            using (var pen = new Pen(Color.FromArgb(14, 78, 135), Math.Max(1, s))) g.DrawPath(pen, path);
        }
    }

    private static GraphicsPath Rounded(RectangleF rectangle, float radius)
    {
        var path = new GraphicsPath(); var d = radius * 2;
        path.AddArc(rectangle.X, rectangle.Y, d, d, 180, 90); path.AddArc(rectangle.Right-d, rectangle.Y, d, d, 270, 90);
        path.AddArc(rectangle.Right-d, rectangle.Bottom-d, d, d, 0, 90); path.AddArc(rectangle.X, rectangle.Bottom-d, d, d, 90, 90); path.CloseFigure(); return path;
    }
}
