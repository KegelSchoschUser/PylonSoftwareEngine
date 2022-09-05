//using PylonSoftwareEngine.GUI.GUIObjects;
//using PylonSoftwareEngine.Mathematics;
//using PylonSoftwareEngine.UI;
//using PylonSoftwareEngine.UI.Drawing;
//using PylonSoftwareEngine.Utilities;

//namespace PylonSoftwareEngine.SceneManagement.Editor
//{
//    public class DirectController3D : RenderCanvas
//    {
//        public bool Active = true;
//        public bool Local = false;
//        public bool Drawname = false;
//        public ControllerMode Mode = ControllerMode.Transform;

//        [WorkInProgress]
//        public bool Grid = false;
//        [WorkInProgress]
//        public float GridSize = 10f;
//        [WorkInProgress]
//        public float GridDistance = 100f;

//        public SoftwareObject3D AttachedObject { get; private set; }
//        private Pen P;
//        private Font Font = new Font(color: RGBColor.From255Range(255, 0, 0));




//        public DirectController3D()
//        {
//            this.Transform.Size = new Vector2(MySoftware.MainWindow.Size.X, MySoftware.MainWindow.Size.Y);
//            P = Graphics.CreatePen(RGBColor.White, 5f, new StrokeStyle());
//        }

//        public void Attach(SoftwareObject3D Object, bool local = false, ControllerMode mode = ControllerMode.Transform, bool drawname = false)
//        {
//            AttachedObject = Object;
//            Local = local;
//            Mode = mode;
//            Drawname = drawname;
//        }


//        public override void OnDraw(Graphics g)
//        {
//            g.Clear();

//            P.Color = new RGBColor(0.25f, 0.25f, 0.25f);
//            P.Width = 1f;
//            DrawGrid(g);

//            P.Width = 5f;
//            DrawDirectController(g);
//        }

//        private void DrawGrid(Graphics g)
//        {
//            if (Grid == true)
//            {
//                if (GridSize > 0f || GridDistance > 0f)
//                {
//                    Vector3 CameraPosition = MySoftwareWorld.ActiveCamera.Transform.WorldPosition;

//                    for (float x = -GridDistance; x <= GridDistance; x += GridSize)
//                    {
//                        Vector3 P1 = new Vector3(x, 0, -GridDistance);
//                        Vector3 P2 = new Vector3(x, 0, GridDistance);
//                        P1 += new Vector3(CameraPosition.X, 0, CameraPosition.Z);
//                        P2 += new Vector3(CameraPosition.X, 0, CameraPosition.Z);
//                        Vector2 ScreenP1 = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(P1);
//                        Vector2 ScreenP2 = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(P2);

//                        if (MySoftwareWorld.ActiveCamera.PointInView(P1) && MySoftwareWorld.ActiveCamera.PointInView(P2))
//                            g.DrawLine(P, ScreenP1, ScreenP2);
//                    }

//                    for (float z = -GridDistance; z <= GridDistance; z += GridSize)
//                    {
//                        Vector3 P1 = new Vector3(-GridDistance, 0, z);
//                        Vector3 P2 = new Vector3(GridDistance, 0, z);
//                        P1 += new Vector3(CameraPosition.X, 0, CameraPosition.Z);
//                        P2 += new Vector3(CameraPosition.X, 0, CameraPosition.Z);
//                        Vector2 ScreenP1 = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(P1);
//                        Vector2 ScreenP2 = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(P2);

//                        if (MySoftwareWorld.ActiveCamera.PointInView(P1) && MySoftwareWorld.ActiveCamera.PointInView(P2))
//                            g.DrawLine(P, ScreenP1, ScreenP2);
//                    }
//                }
//            }
//        }

//        private void DrawDirectController(Graphics g)
//        {
//            if (AttachedObject == null || Active == false)
//                return;
//            if (MySoftwareWorld.ActiveCamera.PointInView(AttachedObject.Transform.WorldPosition) == false)
//                return;

//            float Length = Vector3.Distance(AttachedObject.Transform.WorldPosition, MySoftwareWorld.ActiveCamera.Transform.WorldPosition) / 4f;
//            switch (Mode)
//            {
//                case ControllerMode.Translation:
//                    DrawTranslation(g, AttachedObject.Transform, Length, Local, false);
//                    break;
//                case ControllerMode.Rotation:
//                    DrawRotation(g, AttachedObject.Transform, Length, Local);
//                    break;
//                case ControllerMode.Scale:
//                    DrawScale(g, AttachedObject.Transform, Length, Local);
//                    break;
//                case ControllerMode.Transform:
//                    DrawTranslation(g, AttachedObject.Transform, Length, Local, true);
//                    DrawRotation(g, AttachedObject.Transform, Length, Local);
//                    DrawScale(g, AttachedObject.Transform, Length, Local);
//                    break;
//            }

//            if (Drawname)
//                DrawName(g, AttachedObject.Transform, AttachedObject.Name);
//        }

//        private void DrawTranslation(Graphics g, Transform Transform, float Length, bool Local, bool TransformMode)
//        {
//            if (TransformMode)
//                Length = Length *= 1.3f;
//            Vector2 ScreenCenter = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(Transform.WorldPosition);

//            Vector3 X1;
//            Vector3 Y1;
//            Vector3 Z1;
//            if (Local)
//            {
//                X1 = Transform.WorldPosition + Transform.Left * new Vector3(Length);
//                Y1 = Transform.WorldPosition + Transform.Up * new Vector3(Length);
//                Z1 = Transform.WorldPosition + Transform.Forward * new Vector3(Length);
//            }
//            else
//            {
//                X1 = Transform.WorldPosition + Vector3.Right * new Vector3(Length, Length, Length);
//                Y1 = Transform.WorldPosition + Vector3.Up * new Vector3(Length, Length, Length);
//                Z1 = Transform.WorldPosition + Vector3.Forward * new Vector3(Length, Length, Length);
//            }

//            Vector2 ScreenX1 = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(X1);
//            Vector2 ScreenY1 = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(Y1);
//            Vector2 ScreenZ1 = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(Z1);

//            float ArrowAngle = 75f;
//            if (TransformMode)
//            {
//                P.Color = RGBColor.Red;
//                g.DrawArrowHead(P, ScreenCenter, ScreenX1, ArrowAngle);

//                P.Color = RGBColor.Green;
//                g.DrawArrowHead(P, ScreenCenter, ScreenY1, ArrowAngle);

//                P.Color = RGBColor.Blue;
//                g.DrawArrowHead(P, ScreenCenter, ScreenZ1, ArrowAngle);
//            }
//            else
//            {
//                P.Color = RGBColor.Red;
//                g.DrawArrow(P, ScreenCenter, ScreenX1, ArrowAngle);

//                P.Color = RGBColor.Green;
//                g.DrawArrow(P, ScreenCenter, ScreenY1, ArrowAngle);

//                P.Color = RGBColor.Blue;
//                g.DrawArrow(P, ScreenCenter, ScreenZ1, ArrowAngle);
//            }
//        }

//        private void DrawRotation(Graphics g, Transform Transform, float Radius, bool Local)
//        {
//            Vector2[] VertsX;
//            Vector2[] VertsY;
//            Vector2[] VertsZ;

//            if (Local)
//            {
//                VertsX = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(Primitves3D.CreateHollowCircle(32, Transform.WorldPosition, new Vector3(Radius), Transform.Rotation * Quaternion.FromEuler(0, 0, 90)));
//                VertsY = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(Primitves3D.CreateHollowCircle(32, Transform.WorldPosition, new Vector3(Radius), Transform.Rotation * Quaternion.Identity));
//                VertsZ = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(Primitves3D.CreateHollowCircle(32, Transform.WorldPosition, new Vector3(Radius), Transform.Rotation * Quaternion.FromEuler(90, 0, 0)));
//            }
//            else
//            {
//                VertsX = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(Primitves3D.CreateHollowCircle(32, Transform.WorldPosition, new Vector3(Radius), Quaternion.FromEuler(0, 0, 90)));
//                VertsY = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(Primitves3D.CreateHollowCircle(32, Transform.WorldPosition, new Vector3(Radius), Quaternion.Identity));
//                VertsZ = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(Primitves3D.CreateHollowCircle(32, Transform.WorldPosition, new Vector3(Radius), Quaternion.FromEuler(90, 0, 0)));
//            }

//            P.Color = RGBColor.Red;
//            g.DrawGeometry(P, VertsX);

//            P.Color = RGBColor.Green;
//            g.DrawGeometry(P, VertsY);

//            P.Color = RGBColor.Blue;
//            g.DrawGeometry(P, VertsZ);
//        }

//        private void DrawScale(Graphics g, Transform Transform, float Length, bool Local)
//        {
//            Vector2 ScreenCenter = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(Transform.WorldPosition);

//            Vector3 X1;
//            Vector3 Y1;
//            Vector3 Z1;
//            if (Local)
//            {
//                X1 = Transform.WorldPosition + Transform.Left * new Vector3(Length);
//                Y1 = Transform.WorldPosition + Transform.Up * new Vector3(Length);
//                Z1 = Transform.WorldPosition + Transform.Forward * new Vector3(Length);
//            }
//            else
//            {
//                X1 = Transform.WorldPosition + Vector3.Right * new Vector3(Length, Length, Length);
//                Y1 = Transform.WorldPosition + Vector3.Up * new Vector3(Length, Length, Length);
//                Z1 = Transform.WorldPosition + Vector3.Forward * new Vector3(Length, Length, Length);
//            }

//            Vector2 ScreenX1 = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(X1);
//            Vector2 ScreenY1 = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(Y1);
//            Vector2 ScreenZ1 = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(Z1);

//            if (Mode == ControllerMode.Translation)
//            {
//                float ArrowAngle = 75f;
//                P.Color = RGBColor.Red;
//                g.DrawArrow(P, ScreenCenter, ScreenX1, ArrowAngle);

//                P.Color = RGBColor.Green;
//                g.DrawArrow(P, ScreenCenter, ScreenY1, ArrowAngle);

//                P.Color = RGBColor.Blue;
//                g.DrawArrow(P, ScreenCenter, ScreenZ1, ArrowAngle);
//            }
//            else
//            {
//                float SquareSize = 20f;
//                P.Color = RGBColor.Red;
//                g.DrawLine(P, ScreenCenter, ScreenX1);
//                g.FillRectangle(P.ToSolidBrush(), ScreenX1 - new Vector2(SquareSize / 2f), new Vector2(SquareSize));

//                P.Color = RGBColor.Green;
//                g.DrawLine(P, ScreenCenter, ScreenY1);
//                g.FillRectangle(P.ToSolidBrush(), ScreenY1 - new Vector2(SquareSize / 2f), new Vector2(SquareSize));

//                P.Color = RGBColor.Blue;
//                g.DrawLine(P, ScreenCenter, ScreenZ1);
//                g.FillRectangle(P.ToSolidBrush(), ScreenZ1 - new Vector2(SquareSize / 2f), new Vector2(SquareSize));
//            }
//        }

//        private void DrawName(Graphics g, Transform Transform, string Name)
//        {
//            Vector2 TextLocation = MySoftwareWorld.ActiveCamera.WorldToScreenPoint2(Transform.WorldPosition);

//            var measure = g.MeasureText(Name, Font, new Vector2(400), Enums.TextAlignment.Center, Enums.ParagraphAlignment.Center);
//            TextLocation += new Vector2(measure.LayoutSize.X / 1.5f, 0);
//            float margin = 5f;

//            P.Color = new RGBColor(0, 0, 0);
//            g.FillRectangle(P.ToSolidBrush(), TextLocation - measure.LayoutSize / 2f - margin, measure.LayoutSize + margin * 2f);
//            P.Color = new RGBColor(255, 255, 255);
//            g.DrawRectangle(P, TextLocation - measure.LayoutSize / 2f - margin + 1f, measure.LayoutSize + margin * 2f - 2f);
//            g.DrawText(Name, Font, TextLocation - new Vector2(200), new Vector2(400, 400), Enums.TextAlignment.Center, Enums.ParagraphAlignment.Center);
//        }

//        public enum ControllerMode
//        {
//            Translation,
//            Rotation,
//            Scale,
//            Transform
//        }
//    }
//}
