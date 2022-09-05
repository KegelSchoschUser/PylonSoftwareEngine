/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.Mathematics;
using PylonSoftwareEngine.Utilities.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Input;
using static PylonSoftwareEngine.Utilities.Win32.User32;

namespace PylonSoftwareEngine.Input
{
    public static class Touchscreen
    {
        private static object LOCK = new object();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        private const int SM_DIGITIZER = 94;
        public static bool IsTouchEnabled()
        {
            var value = (SM_DIGITIZER_FLAG)GetSystemMetrics(SM_DIGITIZER);
            return value.HasFlag(SM_DIGITIZER_FLAG.NID_EXTERNAL_TOUCH)
                || value.HasFlag(SM_DIGITIZER_FLAG.NID_INTEGRATED_TOUCH)
                || value.HasFlag(SM_DIGITIZER_FLAG.NID_EXTERNAL_PEN)
                || value.HasFlag(SM_DIGITIZER_FLAG.NID_INTEGRATED_PEN);
        }

        private static Dictionary<int, TouchEvent> TouchPointsBuffer = new Dictionary<int, TouchEvent>();
        public static Dictionary<int, TouchEvent> TouchPoints = new Dictionary<int, TouchEvent>();
        public static Vector2 Delta
        {
            get { return TouchPoints.Count > 0 ? TouchPoints.First().Value.Delta : Vector2.Zero; }
        }


        internal static void ProcessTouchs(TOUCHINPUT[] inputs, int count)
        {
            //var updated = new HashSet<int>();
            for (int i = 0; i < count; i++)
            {
                var TOUCHINPUT = inputs[i];

                //updated.Add(input.dwID);

                // Assign a handler to this message.
                //Action<TouchArgs> handler = null;
                bool validFlag = false;
                CursorEvent.EventType status = CursorEvent.EventType.UP;
                #region convertStatus

                if ((TOUCHINPUT.dwFlags & 0x0007) == 0x0005)
                {
                    validFlag = true;
                    status = CursorEvent.EventType.UP;
                }
                else if ((TOUCHINPUT.dwFlags & 0x0007) == 0x0004)
                {
                    validFlag = true;
                    status = CursorEvent.EventType.UP;
                }
                else if ((TOUCHINPUT.dwFlags & 0x0007) == 0x0002)
                {
                    validFlag = true;
                    status = CursorEvent.EventType.DOWN;
                }
                else if ((TOUCHINPUT.dwFlags & 0x0007) == 0x0001)
                {
                    validFlag = true;
                    status = CursorEvent.EventType.MOVE;
                }
                #endregion convertStatus
                long time = Stopwatch.GetTimestamp();

                // Convert message parameters into touch event arguments and handle the event.
                if (!validFlag)
                    continue;
                switch (status)
                {
                    case CursorEvent.EventType.DOWN:
                        {
                            var e = new TouchEvent
                            {
                                // TOUCHINFO point coordinates and contact size is in 1/100 of a pixel; convert it to pixels.
                                ContactSize = new Vector2(TOUCHINPUT.cyContact * 0.01f, TOUCHINPUT.cxContact * 0.01f),
                                Id = TOUCHINPUT.dwID,
                                Location = new Vector2(TOUCHINPUT.x * 0.01, TOUCHINPUT.y * 0.01),
                                Time = time,
                                Mask = TOUCHINPUT.dwMask,
                                Flags = TOUCHINPUT.dwFlags,
                                Count = count,
                                Status = status
                            };
                            TouchPointsBuffer.Add(e.Id, e);
                        }
                        break;

                    case CursorEvent.EventType.MOVE:
                        {
                            var e = new TouchEvent
                            {
                                // TOUCHINFO point coordinates and contact size is in 1/100 of a pixel; convert it to pixels.
                                ContactSize = new Vector2(TOUCHINPUT.cyContact * 0.01f, TOUCHINPUT.cxContact * 0.01f),
                                Id = TOUCHINPUT.dwID,
                                Location = new Vector2(TOUCHINPUT.x * 0.01, TOUCHINPUT.y * 0.01),
                                Time = time,
                                Mask = TOUCHINPUT.dwMask,
                                Flags = TOUCHINPUT.dwFlags,
                                Count = count,
                                Status = status,
                                Delta = TouchPointsBuffer[TOUCHINPUT.dwID].Location - new Vector2(TOUCHINPUT.x * 0.01, TOUCHINPUT.y * 0.01),
                            };


                            TouchPointsBuffer[TOUCHINPUT.dwID] = e;
                        }
                        break;

                    case CursorEvent.EventType.UP:
                        {
                            TouchPointsBuffer.Remove(TOUCHINPUT.dwID);
                        }
                        break;
                }
            }
        }

        public static void Cycle()
        {
            lock (LOCK)
            {
                TouchPoints.Clear();
                for (int i = 0; i < TouchPointsBuffer.Count; i++)
                {
                    TouchPoints.Add(TouchPointsBuffer.ToArray()[i].Key, TouchPointsBuffer.ToArray()[i].Value);
                }
                //TouchPoints.Add(TouchPointsBuffer);
                //TouchPointsBuffer.Clear();
            }
        }


        internal static void DisableWPFTabletSupport()
        {
            // Get a collection of the tablet devices for this window.
            var devices = Tablet.TabletDevices;

            if (devices.Count > 0)
            {
                // Get the Type of InputManager.
                var inputManagerType = typeof(System.Windows.Input.InputManager);

                // Call the StylusLogic method on the InputManager.Current instance.
                var stylusLogic = inputManagerType.InvokeMember("StylusLogic",
                            BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                            null, System.Windows.Input.InputManager.Current, null);

                if (stylusLogic != null)
                {
                    //  Get the type of the device class.
                    var devicesType = devices.GetType();

                    // Loop until there are no more devices to remove.
                    var count = devices.Count + 1;

                    while (devices.Count > 0)
                    {
                        // Remove the first tablet device in the devices collection.
                        devicesType.InvokeMember("HandleTabletRemoved", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null, devices, new object[] { (uint)0 });

                        count--;

                        if (devices.Count != count)
                        {
                            throw new Win32Exception("Unable to remove real-time stylus support.");
                        }
                    }
                }
            }
        }

        private static bool _touchRegistered = false;
        internal static bool RegisterTouchEvent(IntPtr handle)
        {
            if (!IsTouchEnabled())
            {
                Debug.WriteLine("no Touch device available");
                if (_touchRegistered)
                {
                    CloseTouchInputHandle(handle);
                    _touchRegistered = false;
                    Debug.WriteLine("unregister touch event");
                }
                //OnTouchNotAvailable();
                return false;
            }
            if (_touchRegistered)
                return true;

            if (!RegisterTouchWindow(handle, RegisterTouchFlags.TWF_WANTPALM))
            {
                var err = Marshal.GetLastWin32Error();
                Debug.WriteLine("cant register touch window. error " + err);
                return false;
            }
            else
                Debug.WriteLine("Win Touch source initialysed");
            _touchRegistered = true;

            return true;
        }
    }
}
