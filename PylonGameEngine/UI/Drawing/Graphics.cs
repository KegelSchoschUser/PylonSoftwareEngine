﻿using PylonGameEngine.GameWorld;
using PylonGameEngine.Mathematics;
using PylonGameEngine.Render11;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Vortice;
using Vortice.Direct2D1;
using Vortice.Direct2D1.Effects;
using Vortice.Direct3D11;
using Vortice.DirectWrite;
using Vortice.DXGI;
using Vortice.WIC;

namespace PylonGameEngine.UI.Drawing
{
    public class Graphics
    {
        public Texture Texture;
        private IDXGISurface DXGISurface;
        internal ID2D1RenderTarget RenderTarget;
        internal ID2D1DeviceContext DeviceContext;
        private IDWriteFactory7 WriteFactory;

        public Graphics(Texture texture)
        {
            Texture = texture;
            DXGISurface = Texture.InternalTexture.QueryInterface<IDXGISurface>();

            RenderTargetProperties renderTargetProperties = new RenderTargetProperties(new Vortice.DCommon.PixelFormat(Vortice.DXGI.Format.R32G32B32A32_Float, Vortice.DCommon.AlphaMode.Premultiplied));
            RenderTarget = D3D11GraphicsDevice.Factory2D.CreateDxgiSurfaceRenderTarget(DXGISurface, renderTargetProperties);
            DeviceContext = RenderTarget.QueryInterface<ID2D1DeviceContext>();
            WriteFactory = DWrite.DWriteCreateFactory<IDWriteFactory7>();
        }

        public Graphics(GUIObject guiobject)
        {
            Texture = new Texture((int)guiobject.Transform.Size.X, (int)guiobject.Transform.Size.Y);
            DXGISurface = Texture.InternalTexture.QueryInterface<IDXGISurface>();

            RenderTargetProperties renderTargetProperties = new RenderTargetProperties(new Vortice.DCommon.PixelFormat(Vortice.DXGI.Format.R32G32B32A32_Float, Vortice.DCommon.AlphaMode.Premultiplied));
            RenderTarget = D3D11GraphicsDevice.Factory2D.CreateDxgiSurfaceRenderTarget(DXGISurface, renderTargetProperties);
            DeviceContext = RenderTarget.QueryInterface<ID2D1DeviceContext>();
            WriteFactory = DWrite.DWriteCreateFactory<IDWriteFactory7>();
        }

        public bool Antialiasing
        {
            get
            {
                return Convert.ToBoolean((int)RenderTarget.AntialiasMode);
            }
            set
            {
                RenderTarget.AntialiasMode = (AntialiasMode)(value ? 1 : 0);
            }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(RenderTarget.Size.Width, RenderTarget.Size.Height);
            }
        }

        ~Graphics()
        {
            RenderTarget = null;
            DXGISurface = null;
            Texture = null;
            //Mesh = null;
            DeviceContext = null;
        }

        public SolidBrush CreateSolidBrush(RGBColor color)
        {
            return new SolidBrush(this, color);
        }

        public LinearGradientBrush CreateLinearGradientBrush(Vector2 StartPoint, Vector2 EndPoint, List<ValueTuple<float, RGBColor>> ColorStops)
        {
            return new LinearGradientBrush(this, StartPoint, EndPoint, ColorStops);
        }

        public Pen CreatePen(RGBColor color)
        {
            return new Pen(this, color, new StrokeStyle());
        }

        public Pen CreatePen(RGBColor color, float width)
        {
            return new Pen(this, color, width, new StrokeStyle());
        }

        public Pen CreatePen(RGBColor color, StrokeStyle style)
        {
            return new Pen(this, color, style);
        }

        public Pen CreatePen(RGBColor color, float width, StrokeStyle style)
        {
            return new Pen(this, color, width, style);
        }



        public void BeginDraw()
        {
            RenderTarget.BeginDraw();
        }

        public void EndDraw()
        {
            RenderTarget.Flush(out var tag1, out var tag2);
            RenderTarget.EndDraw();
        }

        public void Clear()
        {
            RenderTarget.Clear(RGBColor.Transparent.ToVorticeColor());
        }

        public void Clear(RGBColor color)
        {
            RenderTarget.Clear(color.ToVorticeColor());
        }

        public void FillRectangle(LinearGradientBrush brush)
        {
            RenderTarget.FillRectangle(new RawRectF(0, 0, this.RenderTarget.Size.Width, this.RenderTarget.Size.Height), brush.br);
        }

        public void FillRectangle(LinearGradientBrush brush, RectangleF rectangle)
        {
            RenderTarget.FillRectangle(new RawRectF(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom), brush.br);
        }

        public void FillRectangle(LinearGradientBrush brush, Vector2 Position, Vector2 Size)
        {
            RenderTarget.FillRectangle(new RawRectF(Position.X, Position.Y, Size.X, Size.Y), brush.br);
        }

        public void FillRectangle(SolidBrush brush)
        {
            RenderTarget.FillRectangle(new RawRectF(0, 0, this.RenderTarget.Size.Width, this.RenderTarget.Size.Height), brush.br);
        }

        public void FillRectangle(SolidBrush brush, RectangleF rectangle)
        {
            RenderTarget.FillRectangle(new RawRectF(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom), brush.br);
        }

        public void FillRectangle(SolidBrush brush, Vector2 Position, Vector2 Size)
        {
            RenderTarget.FillRectangle(new RawRectF(Position.X, Position.Y, Size.X, Size.Y), brush.br);
        }

        public void FillRoundedRectangle(SolidBrush brush, Vector2 Position, Vector2 Size, float radius)
        {
            RenderTarget.FillRoundedRectangle(new RoundedRectangle(new RawRectF(Position.X, Position.Y, Size.X, Size.Y), radius, radius), brush.br);
        }


        public void FillRoundedRectangle(SolidBrush brush, float radius)
        {
            RenderTarget.FillRoundedRectangle(new RoundedRectangle(new RawRectF(0, 0, this.RenderTarget.Size.Width, this.RenderTarget.Size.Height), radius, radius), brush.br);
        }

        public void FillRoundedRectangle(SolidBrush brush, Vector2 radius)
        {
            RenderTarget.FillRoundedRectangle(new RoundedRectangle(new RawRectF(0, 0, this.RenderTarget.Size.Width, this.RenderTarget.Size.Height), radius.X, radius.Y), brush.br);
        }

        public void FillRoundedRectangle(SolidBrush brush, RectangleF rectangle, Vector2 radius)
        {
            RenderTarget.FillRoundedRectangle(new RoundedRectangle(new RawRectF(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom), radius.X, radius.Y), brush.br);
        }

        public void FillRoundedRectangle(LinearGradientBrush brush, Vector2 radius)
        {
            RenderTarget.FillRoundedRectangle(new RoundedRectangle(new RawRectF(0, 0, this.RenderTarget.Size.Width, this.RenderTarget.Size.Height), radius.X, radius.Y), brush.br);
        }

        public void FillRoundedRectangle(LinearGradientBrush brush, RectangleF rectangle, Vector2 radius)
        {
            RenderTarget.FillRoundedRectangle(new RoundedRectangle(new RawRectF(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom), radius.X, radius.Y), brush.br);
        }



















        public void DrawPixel(Pen pen, Vector2 Position)
        {
            RenderTarget.FillRectangle(new Vortice.Mathematics.Rectangle(Position.X, Position.Y, 1, 1), pen.br);
        }


        public void DrawPixels(Pen pen, Vector2[] Positions)
        {
            for (int i = 0; i < Positions.Length; i++)
            {
                RenderTarget.FillRectangle(new Vortice.Mathematics.Rectangle(Positions[i].X, Positions[i].Y, 1, 1), pen.br);
            }
            
        }

        public void DrawRectangle(Pen pen)
        {
            RenderTarget.DrawRectangle(new Vortice.Mathematics.Rectangle(0, 0, this.RenderTarget.Size.Width, this.RenderTarget.Size.Height), pen.br, pen.Width, pen.StrokeStyle);
        }

        public void DrawRectangle(Pen pen, Vector2 Position, Vector2 Size)
        {
            RenderTarget.DrawRectangle(new Vortice.Mathematics.Rectangle(Position.X, Position.Y, Size.X, Size.Y), pen.br, pen.Width, pen.StrokeStyle);
        }

        public void DrawRectangle(Pen pen, RectangleF rectangle)
        {
            RenderTarget.DrawRectangle(new RawRectF(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom), pen.br, pen.Width, pen.StrokeStyle);
        }

        public void DrawRoundedRectangle(Pen pen, Vector2 radius)
        {
            RenderTarget.DrawRoundedRectangle(new RoundedRectangle(new RawRectF(0, 0, this.RenderTarget.Size.Width, this.RenderTarget.Size.Height), radius.X, radius.Y), pen.br, pen.Width, pen.StrokeStyle);
        }

        public void DrawRoundedRectangle(Pen pen, RectangleF rectangle, Vector2 radius)
        {
            RenderTarget.DrawRoundedRectangle(new RoundedRectangle(new RawRectF(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom), radius.X, radius.Y), pen.br, pen.Width, pen.StrokeStyle);
        }

        public void DrawLine(Pen pen, Vector2 p1, Vector2 p2)
        {
            RenderTarget.DrawLine(p1.ToVorticePoint(), p2.ToVorticePoint(), pen.br, pen.Width, pen.StrokeStyle);
        }

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            RenderTarget.DrawLine(new Vortice.Mathematics.Point(x1, y1), new Vortice.Mathematics.Point(x2, y2), pen.br, pen.Width, pen.StrokeStyle);
        }

        public void DrawArrow(Pen pen, Vector2 p1, Vector2 p2)
        {
            DrawArrow(pen, p1, p2, 90f, pen.Width * 2f);
        }

        public void DrawArrow(Pen pen, Vector2 p1, Vector2 p2, float ArrowAngle)
        {
            DrawArrow(pen, p1, p2, ArrowAngle, pen.Width * 3f);
        }

        public void DrawArrow(Pen pen, Vector2 p1, Vector2 p2, float ArrowAngle, float ArrowWidth)
        {
            float Angle = ArrowAngle / 2f * 0.0174532925f;

            Vector2 back = new Vector2(p1.X - p2.X, p1.Y - p2.Y);
            float length = (float)System.Math.Sqrt(back.X * back.X + back.Y * back.Y);
            Vector2 normalized = new Vector2(back.X / length, back.Y / length);

            float Sin = (float)System.Math.Sin(Angle);
            float Cos = (float)System.Math.Cos(Angle);

            float ax = (float)(normalized.X * Cos - normalized.Y * Sin);
            float ay = (float)(normalized.X * Sin + normalized.Y * Cos);
            float bx = (float)(normalized.X * Cos + normalized.Y * Sin);
            float by = (float)(-normalized.X * Sin + normalized.Y * Cos);

            float c = (float)(System.Math.Tan(Angle) * ArrowWidth);
            float ArrowLength = (float)System.Math.Sqrt(ArrowWidth * ArrowWidth + c * c);

            Vector2 Left = new Vector2(p2.X + ArrowLength * ax, p2.Y + ArrowLength * ay);
            Vector2 Right = new Vector2(p2.X + ArrowLength * bx, p2.Y + ArrowLength * by);

            Vector2 LineEnd = new Vector2((Left.X + Right.X) / 2f, (Left.Y + Right.Y) / 2f);

            DrawLine(pen, p1, LineEnd);
        }

        public void DrawArrowHead(Pen pen, Vector2 p1, Vector2 p2, float ArrowAngle)
        {
            DrawArrowHead(pen, p1, p2, ArrowAngle, pen.Width * 3f);
        }

        public void DrawArrowHead(Pen pen, Vector2 p1, Vector2 p2, float ArrowAngle, float ArrowWidth)
        {
            float Angle = ArrowAngle / 2f * 0.0174532925f;

            Vector2 back = new Vector2(p1.X - p2.X, p1.Y - p2.Y);
            float length = (float)System.Math.Sqrt(back.X * back.X + back.Y * back.Y);
            Vector2 normalized = new Vector2(back.X / length, back.Y / length);

            float Sin = (float)System.Math.Sin(Angle);
            float Cos = (float)System.Math.Cos(Angle);

            float ax = (float)(normalized.X * Cos - normalized.Y * Sin);
            float ay = (float)(normalized.X * Sin + normalized.Y * Cos);
            float bx = (float)(normalized.X * Cos + normalized.Y * Sin);
            float by = (float)(-normalized.X * Sin + normalized.Y * Cos);

            float c = (float)(System.Math.Tan(Angle) * ArrowWidth);
            float ArrowLength = (float)System.Math.Sqrt(ArrowWidth * ArrowWidth + c * c);

            Vector2 Left = new Vector2(p2.X + ArrowLength * ax, p2.Y + ArrowLength * ay);
            Vector2 Right = new Vector2(p2.X + ArrowLength * bx, p2.Y + ArrowLength * by);


            List<Vector2> points = new List<Vector2>();
            points.Add(p2);
            points.Add(Left);
            points.Add(Right);


            FillGeometry(pen.ToSolidBrush(), points.ToArray());
        }




        public class GIWBitmap
        {
            public ID2D1Bitmap InternalBitmap;

            public GIWBitmap(ID2D1Bitmap bitmap)
            {
                InternalBitmap = bitmap;
            }

            public void Release()
            {
                InternalBitmap.Release();
            }
        }

        public GIWBitmap GetBitmapFromFile(string FileName)
        {
            var WICImagingFactory = new Vortice.WIC.IWICImagingFactory();
            IWICBitmapDecoder bitmapDecoder = WICImagingFactory.CreateDecoderFromFileName(
               FileName,
               FileAccess.Read,
               DecodeOptions.CacheOnDemand
               );

            IWICFormatConverter result = WICImagingFactory.CreateFormatConverter();


            result.Initialize(
                bitmapDecoder.GetFrame(0),
                PixelFormat.Format32bppRGBA,
                BitmapDitherType.None,
                null,
                0.0,
                BitmapPaletteType.Custom);

            var bitmapProperties = new BitmapProperties(new Vortice.DCommon.PixelFormat(Vortice.DXGI.Format.R8G8B8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied));

            var internbitmap = RenderTarget.CreateBitmapFromWicBitmap(result, bitmapProperties);

            bitmapDecoder.Release();
            result.Release();

            GIWBitmap bitmap = new GIWBitmap(internbitmap);
            return bitmap;
        }

        public void DrawBitmap(GIWBitmap bitmap, float opacity = 1f, Enums.InterpolationMode interpolationMode = Enums.InterpolationMode.Linear)
        {
            DrawBitmap(bitmap, new Rectangle(Point.Empty, new System.Drawing.Size((int)RenderTarget.Size.Width, (int)RenderTarget.Size.Height)), opacity, interpolationMode);
        }

        public void DrawBitmap(GIWBitmap bitmap, RectangleF rectangle, float opacity = 1f, Enums.InterpolationMode interpolationMode = Enums.InterpolationMode.Linear)
        {
            RawRectF rect = new RawRectF(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            RenderTarget.DrawBitmap(bitmap.InternalBitmap, rect, opacity, (Vortice.Direct2D1.BitmapInterpolationMode)interpolationMode, new RawRectF(0, 0, bitmap.InternalBitmap.Size.Width, bitmap.InternalBitmap.Size.Height));
        }

        public void DrawBitmap(string FileName, float opacity = 1f, Enums.InterpolationMode interpolationMode = Enums.InterpolationMode.Linear)
        {
            DrawBitmap(FileName, new Rectangle(Point.Empty, new System.Drawing.Size((int)RenderTarget.Size.Width, (int)RenderTarget.Size.Height)), opacity, interpolationMode);
        }

        public void DrawBitmap(string FileName, RectangleF rectangle, float opacity = 1f, Enums.InterpolationMode interpolationMode = Enums.InterpolationMode.Linear)
        {
            var WICImagingFactory = new Vortice.WIC.IWICImagingFactory();
            IWICBitmapDecoder bitmapDecoder = WICImagingFactory.CreateDecoderFromFileName(
               FileName,
               FileAccess.Read,
               DecodeOptions.CacheOnDemand
               );

            IWICFormatConverter result = WICImagingFactory.CreateFormatConverter();


            result.Initialize(
                bitmapDecoder.GetFrame(0),
                PixelFormat.Format32bppRGBA,
                BitmapDitherType.None,
                null,
                0.0,
                BitmapPaletteType.Custom);

            var bitmapProperties = new BitmapProperties(new Vortice.DCommon.PixelFormat(Vortice.DXGI.Format.R8G8B8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied));

            var bitmap = RenderTarget.CreateBitmapFromWicBitmap(result, bitmapProperties);

            RawRectF rect = new RawRectF(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            RenderTarget.DrawBitmap(bitmap, rect, opacity, (Vortice.Direct2D1.BitmapInterpolationMode)interpolationMode, new RawRectF(0, 0, bitmap.Size.Width, bitmap.Size.Height));

            WICImagingFactory.Release();
            bitmapDecoder.Release();
            result.Release();
            bitmap.Release();
        }

        internal void DrawBitmap(ID2D1Bitmap1 Bitmap, float opacity = 1f, Enums.InterpolationMode interpolationMode = Enums.InterpolationMode.Linear)
        {
            DrawBitmap(Bitmap, new RectangleF(PointF.Empty, new System.Drawing.Size((int)RenderTarget.Size.Width, (int)RenderTarget.Size.Height)), opacity, interpolationMode);
        }

        internal void DrawBitmap(ID2D1Bitmap1 Bitmap, RectangleF rectangle, float opacity = 1f, Enums.InterpolationMode interpolationMode = Enums.InterpolationMode.Linear)
        {
            RawRectF rect = new RawRectF(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            RenderTarget.DrawBitmap(Bitmap, rect, opacity, (Vortice.Direct2D1.BitmapInterpolationMode)interpolationMode, new RawRectF(0, 0, Bitmap.Size.Width, Bitmap.Size.Height));
        }

        internal void DrawImage(ID2D1Effect Effect)
        {
            DeviceContext.DrawImage(Effect);
        }

        internal void DrawImage(ID2D1Image Image)
        {
            DeviceContext.DrawImage(Image);
        }


        public void FillGeometry(LinearGradientBrush brush, Vector2[] points)
        {
            if (points.Length < 3)
                throw new Exception("Must be more than 2 points!");

            var geometry = D3D11GraphicsDevice.Factory2D.CreatePathGeometry();
            var sink = geometry.Open();
            sink.BeginFigure(points[0].ToVorticePoint(), FigureBegin.Filled);

            List<Vortice.Mathematics.Point> Points = new List<Vortice.Mathematics.Point>();
            for (int i = 1; i < points.Length; i++)
            {
                Points.Add(points[i].ToVorticePoint());
            }
            sink.AddLines(Points.ToArray());
            sink.EndFigure(FigureEnd.Closed);
            sink.Close();

            RenderTarget.FillGeometry(geometry, brush.br);
        }

        public void FillGeometry(SolidBrush brush, Vector2[] points)
        {
            if (points.Length < 3)
                throw new Exception("Must be more than 2 points!");

            var geometry = D3D11GraphicsDevice.Factory2D.CreatePathGeometry();
            var sink = geometry.Open();
            sink.BeginFigure(points[0].ToVorticePoint(), FigureBegin.Filled);

            List<Vortice.Mathematics.Point> Points = new List<Vortice.Mathematics.Point>();
            for (int i = 1; i < points.Length; i++)
            {
                Points.Add(points[i].ToVorticePoint());
            }
            sink.AddLines(Points.ToArray());
            sink.EndFigure(FigureEnd.Closed);
            sink.Close();


            RenderTarget.FillGeometry(geometry, brush.br);
        }

        public void DrawGeometry(Pen pen, Vector2[] points, bool Closed = true)
        {
            if (points.Length < 3)
                throw new Exception("Must be more than 2 points!");

            var geometry = D3D11GraphicsDevice.Factory2D.CreatePathGeometry();

            var sink = geometry.Open();
            sink.BeginFigure(points[0].ToVorticePoint(), FigureBegin.Filled);

            List<Vortice.Mathematics.Point> Points = new List<Vortice.Mathematics.Point>();
            for (int i = 1; i < points.Length; i++)
            {
                Points.Add(points[i].ToVorticePoint());
            }
            sink.AddLines(Points.ToArray());
            sink.EndFigure((FigureEnd)(Closed ? 1 : 0));
            sink.Close();

            RenderTarget.DrawGeometry(geometry, pen.br, pen.Width);
        }

        public void DrawBezierCurve(Pen pen, BezierCurve2D Curve)
        {
            if (Curve.Points.Count < 3)
                throw new Exception("Must be more than 2 points!");

            var geometry = D3D11GraphicsDevice.Factory2D.CreatePathGeometry();

            var sink = geometry.Open();
            sink.BeginFigure(Curve.Points[0].ToVorticePoint(), FigureBegin.Filled);

            var CurveSegments = Curve.ToIndividualSegments();
            List<BezierSegment> segments = new List<BezierSegment>();
            foreach (var item in CurveSegments)
            {
                segments.Add(new BezierSegment(item.Point1.ToVorticePoint(), item.ControlPoint.ToVorticePoint(), item.Point2.ToVorticePoint()));
            }
            //  sink.AddQuadraticBeziers(segments.ToArray());
            sink.AddBeziers(segments.ToArray());

            sink.EndFigure(FigureEnd.Open);

            //var group = D3D11GraphicsDevice.Factory2D.CreateGeometryGroup(Vortice.Direct2D1.FillMode.Alternate, new ID2D1Geometry[] { geometry});
            //group.GetSourceGeometry
            sink.Close();

            RenderTarget.DrawGeometry(geometry, pen.br, pen.Width);
        }

        public void DrawArc(Pen pen, Vector2 StartPoint, Vector2 EndPoint, Vector2 Size, bool Closed = false)
        {
            var geometry = D3D11GraphicsDevice.Factory2D.CreatePathGeometry();

            var sink = geometry.Open();
            sink.BeginFigure(StartPoint.ToVorticePoint(), FigureBegin.Filled);

            sink.AddArc(new ArcSegment(EndPoint.ToVorticePoint(), new Vortice.Mathematics.Size(Size.X, Size.Y), 0f, SweepDirection.Clockwise, ArcSize.Large));
            sink.EndFigure((FigureEnd)(Closed ? 1 : 0));

            //var group = D3D11GraphicsDevice.Factory2D.CreateGeometryGroup(Vortice.Direct2D1.FillMode.Alternate, new ID2D1Geometry[] { geometry});
            //group.GetSourceGeometry
            sink.Close();
            RenderTarget.DrawGeometry(geometry, pen.br, pen.Width);
        }

        public void DrawEllipse(Pen pen, Vector2 Center, Vector2 Size)
        {
            RenderTarget.DrawEllipse(new Ellipse(Center.ToVorticePoint(), Size.X, Size.Y), pen.br, pen.Width);
        }


        public TextMeasureOutput MeasureText(string Text, Font f, Enums.TextAlignment XAlign = Enums.TextAlignment.Leading, Enums.ParagraphAlignment YAlign = Enums.ParagraphAlignment.Near, Enums.WordWrapping WordWrapping = Enums.WordWrapping.Wrap)
        {
            return MeasureText(Text, f, new Vector2(float.MaxValue), XAlign, YAlign, WordWrapping);
        }

        public TextMeasureOutput MeasureText(string Text, Font f, Vector2 Size, Enums.TextAlignment XAlign = Enums.TextAlignment.Leading, Enums.ParagraphAlignment YAlign = Enums.ParagraphAlignment.Near, Enums.WordWrapping WordWrapping = Enums.WordWrapping.Wrap)
        {
            Text = Text.Replace("\n", "#\n");
            var textFormat = WriteFactory.CreateTextFormat(f.FontFamilyName, (FontWeight)f.FontWeight, (Vortice.DirectWrite.FontStyle)f.FontStyle, f.FontSize);
            textFormat.TextAlignment = (TextAlignment)XAlign;
            textFormat.ParagraphAlignment = (ParagraphAlignment)YAlign;
            textFormat.WordWrapping = (WordWrapping)WordWrapping;
            var textLayout = WriteFactory.CreateTextLayout(Text, textFormat, Size.X, Size.Y);
            var Metrics = textLayout.Metrics;
            textLayout.HitTestTextPosition(Text.Length - 1, false, out float CaretX, out float CaretY, out HitTestMetrics hitTestMetrics);
            textFormat.Release();
            textLayout.Release();
            return new TextMeasureOutput(new Vector2(Metrics.Width, Metrics.Height), new Vector2(CaretX + hitTestMetrics.Width, CaretY), hitTestMetrics.Height);
        }

        public void DrawText(string Text, Font f, Enums.TextAlignment XAlign = Enums.TextAlignment.Leading, Enums.ParagraphAlignment YAlign = Enums.ParagraphAlignment.Near, Enums.WordWrapping WordWrapping = Enums.WordWrapping.Wrap)
        {
            DrawText(Text, f, Vector2.Zero, new Vector2(RenderTarget.Size.Width, RenderTarget.Size.Height), XAlign, YAlign, WordWrapping);
        }

        public void DrawText(string Text, Font f, Vector2 Position, Enums.TextAlignment XAlign = Enums.TextAlignment.Leading, Enums.ParagraphAlignment YAlign = Enums.ParagraphAlignment.Near, Enums.WordWrapping WordWrapping = Enums.WordWrapping.Wrap)
        {
            DrawText(Text, f, Position, new Vector2(RenderTarget.Size.Width, RenderTarget.Size.Height), XAlign, YAlign, WordWrapping);
        }

        public void DrawText(string Text, Font f, Vector2 Position, Vector2 Size, Enums.TextAlignment XAlign = Enums.TextAlignment.Leading, Enums.ParagraphAlignment YAlign = Enums.ParagraphAlignment.Near, Enums.WordWrapping WordWrapping = Enums.WordWrapping.Wrap)
        {
            var textFormat = WriteFactory.CreateTextFormat(f.FontFamilyName, (FontWeight)f.FontWeight, (Vortice.DirectWrite.FontStyle)f.FontStyle, f.FontSize);
            textFormat.TextAlignment = (TextAlignment)XAlign;
            textFormat.ParagraphAlignment = (ParagraphAlignment)YAlign;
            textFormat.WordWrapping = (WordWrapping)WordWrapping;
            var textLayout = WriteFactory.CreateTextLayout(Text, textFormat, Size.X, Size.Y);
            var brush = CreateSolidBrush(f.Color);
            RenderTarget.DrawTextLayout(Position, textLayout, brush.br, DrawTextOptions.EnableColorFont);
            var Metrics = textLayout.Metrics;
            textFormat.Release();
            textLayout.Release();
            brush.br.Release();
        }

        internal ID2D1Bitmap1 GetAsBitmapI()
        {
            var bitmapProperties = new BitmapProperties1(RenderTarget.PixelFormat);
            var bitmap = DeviceContext.CreateBitmap(new Vortice.Mathematics.SizeI((int)RenderTarget.Size.Width, (int)RenderTarget.Size.Height), IntPtr.Zero, 0, ref bitmapProperties);
            bitmap.CopyFromRenderTarget(RenderTarget);
            return bitmap;
        }

        public GIWBitmap GetAsBitmap()
        {
            var bitmapProperties = new BitmapProperties1(RenderTarget.PixelFormat);
            var bitmap = DeviceContext.CreateBitmap(new Vortice.Mathematics.SizeI((int)RenderTarget.Size.Width, (int)RenderTarget.Size.Height), IntPtr.Zero, 0, ref bitmapProperties);
            bitmap.CopyFromRenderTarget(RenderTarget);
            return new GIWBitmap(bitmap);
        }


        #region Effects
        public void ApplyGaussianBlur(float Deviation = 3.0f, bool HardBorder = false)
        {
            var Bitmap = GetAsBitmapI();
            GaussianBlur gaussianBlur = new GaussianBlur(DeviceContext);
            gaussianBlur.SetInput(0, Bitmap, new SharpGen.Runtime.RawBool(false));

            gaussianBlur.Optimization = GaussianBlurOptimization.Balanced;
            gaussianBlur.BorderMode = (BorderMode)(HardBorder ? 1 : 0);
            gaussianBlur.StandardDeviation = Deviation;

            Clear(RGBColor.Transparent);
            DrawImage(gaussianBlur);
            gaussianBlur.Release();
            Bitmap.Release();
        }

        public void ApplyGammaTransfer(Vector3 ColorAmplitude)
        {
            ApplyGammaTransfer(ColorAmplitude, Vector3.Unit);
        }

        public void ApplyGammaTransfer(Vector3 ColorAmplitude, Vector3 ColorExponent)
        {
            var Bitmap = GetAsBitmapI();
            GammaTransfer gammaTransfer = new GammaTransfer(DeviceContext);
            gammaTransfer.SetInput(0, Bitmap, new SharpGen.Runtime.RawBool(false));

            gammaTransfer.RedAmplitude = ColorAmplitude.X;
            gammaTransfer.GreenAmplitude = ColorAmplitude.Y;
            gammaTransfer.BlueAmplitude = ColorAmplitude.Z;

            gammaTransfer.RedExponent = ColorExponent.X;
            gammaTransfer.GreenExponent = ColorExponent.Y;
            gammaTransfer.BlueExponent = ColorExponent.Z;

            Clear(RGBColor.Transparent);
            DrawImage(gammaTransfer);
            gammaTransfer.Release();
            Bitmap.Release();
        }

        public void ApplyBrightness(Vector2 WhitePoint, Vector2 BlackPoint)
        {
            var Bitmap = GetAsBitmapI();
            Brightness brightness = new Brightness(DeviceContext);
            brightness.SetInput(0, Bitmap, new SharpGen.Runtime.RawBool(false));
            brightness.WhitePoint = WhitePoint.ToSystemNumerics();
            brightness.WhitePoint = BlackPoint.ToSystemNumerics();

            Clear(RGBColor.Transparent);
            DrawImage(brightness);
            brightness.Release();
            Bitmap.Release();
        }

        public void ApplySaturation(float value)
        {
            var Bitmap = GetAsBitmapI();
            Saturation saturation = new Saturation(DeviceContext);
            saturation.SetInput(0, Bitmap, new SharpGen.Runtime.RawBool(false));
            saturation.Value = value;

            Clear(RGBColor.Transparent);
            DrawImage(saturation);
            saturation.Release();
            Bitmap.Release();
        }

        public void ApplyContrast(float value)
        {
            var Bitmap = GetAsBitmapI();
            Contrast contrast = new Contrast(DeviceContext);
            contrast.SetInput(0, Bitmap, new SharpGen.Runtime.RawBool(false));
            contrast.Value = value;

            Clear(RGBColor.Transparent);
            DrawImage(contrast);
            contrast.Release();
            Bitmap.Release();
        }

        public void ApplyShadow(RGBColor color, float Deviation = 3.0f)
        {
            var Bitmap = GetAsBitmapI();
            Shadow shadow = new Shadow(DeviceContext);
            shadow.SetInput(0, Bitmap, new SharpGen.Runtime.RawBool(false));
            shadow.BlurStandardDeviation = Deviation;
            shadow.Color = new System.Numerics.Vector4(color.R, color.G, color.B, color.A);
            shadow.Optimization = ShadowOptimization.Balanced;


            Clear(RGBColor.Transparent);
            DrawImage(shadow);
            shadow.Release();
            Bitmap.Release();
        }

        public void ApplyTransform(Vector2 Translation, Vector2 Scale, float Rotation)
        {
            ApplyTransform(Translation, Scale, Rotation, new Vector2(RenderTarget.Size.Width / 2f, RenderTarget.Size.Height / 2f));
        }

        public void ApplyTransform(Vector2 Translation, Vector2 Scale, float Rotation, Vector2 Center)
        {
            var Bitmap = GetAsBitmapI();
            AffineTransform2D transform = new AffineTransform2D(DeviceContext);
            transform.SetInput(0, Bitmap, new SharpGen.Runtime.RawBool(false));
            var TranslationMatrix = System.Numerics.Matrix3x2.Identity;
            var ScaleMatrix       = System.Numerics.Matrix3x2.Identity;
            var RotationMatrix    = System.Numerics.Matrix3x2.Identity;
            var RotationMatrixCenter = System.Numerics.Matrix3x2.Identity;
            var RotationMatrixNegativeCenter = System.Numerics.Matrix3x2.Identity;

            TranslationMatrix.M31 = Translation.X;
            TranslationMatrix.M32 = Translation.Y;

            RotationMatrixCenter.M31 = Center.X;
            RotationMatrixCenter.M32 = Center.Y;

            RotationMatrixNegativeCenter.M31 = -Center.X;
            RotationMatrixNegativeCenter.M32 = -Center.Y;

            ScaleMatrix.M31 = Center.X - (Scale.X * Center.X);
            ScaleMatrix.M32 = Center.Y - (Scale.Y * Center.Y);

            ScaleMatrix.M11 = Scale.X;
            ScaleMatrix.M22 = Scale.Y;

            float cos = (float)Math.Cos(Rotation * Mathf.Deg2Rad);
            float sin = (float)Math.Sin(Rotation * Mathf.Deg2Rad);

            RotationMatrix.M11 = cos;
            RotationMatrix.M12 = sin;
            RotationMatrix.M21 = -sin;
            RotationMatrix.M22 = cos;

            transform.TransformMatrix = ScaleMatrix * (RotationMatrixNegativeCenter * RotationMatrix * RotationMatrixCenter) * TranslationMatrix;

            Clear(RGBColor.Transparent);
            DrawImage(transform);
            transform.Release();
            Bitmap.Release();
        }

        public void CreateClip(float x, float y, float x2, float y2)
        {
            RenderTarget.PushAxisAlignedClip(new RawRectF(x, y, x2, y2), AntialiasMode.PerPrimitive);
        }

        public void ApplyClip()
        {
            RenderTarget.PopAxisAlignedClip();
        }
        #endregion Effects

        public struct TextMeasureOutput
        {
            public Vector2 LayoutSize;
            public Vector2 CursorPosition;
            public float CursorSizeY;

            public TextMeasureOutput(Vector2 size, Vector2 caretPosition, float caretSizeY)
            {
                LayoutSize = size;
                CursorPosition = caretPosition;
                CursorSizeY = caretSizeY;
            }
        }
    }
}
