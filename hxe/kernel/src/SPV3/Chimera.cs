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

using System.IO;

namespace HXE.SPV3
{
    /// <summary>
    ///   Object representation of Chimera settings.
    /// </summary>
    public class Chimera : File
    {
        private const int Length = 2056;

        public byte Interpolation { get; set; } = 8;
        public bool AnisotropicFiltering { get; set; } = false;
        public bool UncapCinematic { get; set; } = true;
        public bool BlockLOD { get; set; } = false;

        /// <summary>
        ///   Saves object state to the inbound file.
        /// </summary>
        public void Save()
        {
            using (var fs = new FileStream(Path, FileMode.Create, FileAccess.Write))
            using (var ms = new MemoryStream(Length)) /* chimera.bin size */
            using (var bw = new BinaryWriter(ms))
            {
                ms.Position = 0;

                ms.Position = 0x0F;
                bw.Write(Interpolation);

                ms.Position = 0x02;
                bw.Write(AnisotropicFiltering ? (byte) 1 : (byte) 0);

                ms.Position = 0x1E;
                bw.Write(UncapCinematic ? (byte) 1 : (byte) 0);

                ms.Position = 0x1F;
                bw.Write(BlockLOD ? (byte) 1 : (byte) 0);

                bw.Write(new byte[Length - ms.Position]); /* padding */

                ms.Position = 0;
                ms.CopyTo(fs);
            }
        }

        /// <summary>
        ///   Loads object state from the inbound file.
        /// </summary>
        public void Load()
        {
            using (var fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
            using (var ms = new MemoryStream(Length))
            using (var br = new BinaryReader(ms))
            {
                fs.CopyTo(ms);

                ms.Position = 0x0F;
                Interpolation = br.ReadByte();
                ms.Position = 0x02;
                AnisotropicFiltering = br.ReadByte() == 1;
                ms.Position = 0x1E;
                UncapCinematic = br.ReadByte() == 1;
                ms.Position = 0x1F;
                BlockLOD = br.ReadByte() == 1;
            }
        }

        /// <summary>
        ///   Represents the inbound object as a string.
        /// </summary>
        /// <param name="chimera">
        ///   Object to represent as string.
        /// </param>
        /// <returns>
        ///   String representation of the inbound object.
        /// </returns>
        public static implicit operator string(Chimera chimera)
        {
            return chimera.Path;
        }

        /// <summary>
        ///   Represents the inbound string as an object.
        /// </summary>
        /// <param name="name">
        ///   String to represent as object.
        /// </param>
        /// <returns>
        ///   Object representation of the inbound string.
        /// </returns>
        public static explicit operator Chimera(string name)
        {
            return new Chimera
            {
                Path = name
            };
        }
    }
}