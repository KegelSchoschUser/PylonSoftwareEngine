/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using System;
using System.Runtime.CompilerServices;

namespace PylonSoftwareEngine.Utilities
{
    internal static class MemoryHelper
    {
        /// <summary>
        /// Native memcpy.
        /// </summary>
        /// <param name="dest">The destination memory location.</param>
        /// <param name="src">The source memory location.</param>
        /// <param name="sizeInBytesToCopy">The byte count.</param>
        public static void CopyMemory(IntPtr dest, IntPtr src, int sizeInBytesToCopy)
        {
            unsafe
            {
                Unsafe.CopyBlockUnaligned((void*)dest, (void*)src, (uint)sizeInBytesToCopy);
            }
        }

        /// <summary>
        /// Native memcpy.
        /// </summary>
        /// <param name="dest">The destination memory location.</param>
        /// <param name="src">The source memory location.</param>
        /// <param name="sizeInBytesToCopy">The byte count.</param>
        public static void CopyMemory<T>(IntPtr dest, ReadOnlySpan<T> src)
            where T : struct
        {
            unsafe
            {
                src.CopyTo(new Span<T>((void*)dest, src.Length));
            }
        }
    }
}
