/**
 * Copyright (c) 2019 Emilian Roman
 * Copyright (c) 2021 Noah Sherwin
 *
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using System;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace HXE.Common
{
    public static class ExtConvert
    {
        public static byte[] StringToByteArray(string hex)
        {
            // alternatively, long.Parse(hex, System.Globalization.NumberStyles.HexNumber)
            return SoapHexBinary.Parse(hex).Value;
        }

        public static uint ByteArrayToUInt(byte[] bytes)
        {
            uint result = 0;

            foreach (var b in bytes)
            {
                result += b;
            }

            return result;
        }

        /// <summary>
        /// reverse byte order (16-bit)/(UShort)
        /// </summary>
        /// <see cref="https://www.csharp-examples.net/reverse-bytes/"/>
        public static ushort ReverseBytes(ushort value)
        {
            return (ushort) ((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }
    }
}