/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using System;
using System.Collections.Generic;
using Vortice.Direct2D1;

namespace PylonSoftwareEngine.UI.Drawing
{
    public class LinearGradientBrush
    {
        internal ID2D1LinearGradientBrush br;
        public LinearGradientBrush(Graphics g, Vector2 StartPoint, Vector2 EndPoint, List<ValueTuple<float, RGBColor>> ColorStops)
        {
            GradientStop[] gradientStops = new GradientStop[ColorStops.Count];
            for (int i = 0; i < ColorStops.Count; i++)
            {
                gradientStops[i] = new GradientStop(ColorStops[i].Item1, ColorStops[i].Item2.ToVorticeColor());
            }

            var collection = g.RenderTarget.CreateGradientStopCollection(gradientStops, Gamma.Linear, ExtendMode.Mirror);

            br = g.RenderTarget.CreateLinearGradientBrush(new LinearGradientBrushProperties(StartPoint.ToSystemNumerics(), EndPoint.ToSystemNumerics()), collection);
        }

        public void Release()
        {
            br.Release();
        }

        public void SetStartPoint(Vector2 P)
        {
            br.StartPoint = P;
        }

        public void SetEndPoint(Vector2 P)
        {
            br.EndPoint = P;
        }
    }
}
