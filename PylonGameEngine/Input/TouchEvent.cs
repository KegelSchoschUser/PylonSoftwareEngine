using PylonGameEngine.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static PylonGameEngine.Utilities.Win32.User32;

namespace PylonGameEngine.Input
{
    public class TouchEvent
    {
        #region Properties

        /// <summary>
        /// Gets the x size of the contact area in pixels.
        /// </summary>
        public Vector2 ContactSize
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the total number of current touch points.
        /// </summary>
        public Int32 Count
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the given flags.
        /// </summary>
        public Int32 Flags
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the contact ID.
        /// </summary>
        public int Id
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the touch Loctaion client coordinate in pixels.
        /// </summary>
        /// 
        public Vector2 Location
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the mask which fields in the structure are valid.
        /// </summary>
        public Int32 Mask
        {
            get;
            internal set;
        }

        public CursorEvent.EventType Status
        {
            get;
            internal set;
        }

        public Vector2 Delta
        {
            get;
            internal set;
        }
        /// <summary>
        /// Gets the touch event time.
        /// </summary>
        public long Time
        {
            get;
            internal set;
        }

        #endregion Properties
    }
}
