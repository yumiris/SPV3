/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Resume.
 * 
 * SPV3.Resume is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Resume is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Resume.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using File = SPV3.Domain.File;

namespace SPV3.Resume
{
    /// <summary>
    ///     Loads data from the provided savegame binary.
    /// </summary>
    public class SavegameRepository
    {
        /// <summary>
        ///     Offset of the difficulty value in the savegame binary.
        /// </summary>
        private const int DifficultyOffset = 0x1E2;

        /// <summary>
        ///     Offset of the mission value in the savegame binary.
        /// </summary>
        private const int MissionOffset = 0x1E8;

        /// <summary>
        ///     Maximum characters a mission can be as decided by this library.
        /// </summary>
        private const int MissionLength = 0x20;

        /// <summary>
        ///     Savegame binary file.
        /// </summary>
        private readonly File _file;

        /// <summary>
        ///     SavegameRepository constructor.
        /// </summary>
        /// <param name="file">
        ///    Savegame binary file.
        /// </param>
        public SavegameRepository(File file)
        {
            _file = file;
        }

        /// <summary>
        ///     Deserialises Savegame instance from the provided binary.
        /// </summary>
        /// <returns>
        ///     Savegame instance representing the provided binary.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Invalid difficulty numeric value.
        ///     - or -
        ///     Invalid SPV3 mission value.
        /// </exception>
        public Savegame Load()
        {
            /**
             * Infers the Difficulty enum member from the inbound integer.
             */
            Difficulty GetDifficulty(int value)
            {
                switch (value)
                {
                    case 0x0:
                        return Difficulty.Noble;
                    case 0x1:
                        return Difficulty.Normal;
                    case 0x2:
                        return Difficulty.Heroic;
                    case 0x3:
                        return Difficulty.Legendary;
                    default:
                        throw new ArgumentException("Invalid difficulty numeric value.");
                }
            }

            /**
             * Infers the Mission enum member from the inbound string.
             */
            Mission GetMission(string value)
            {
                switch (value)
                {
                    case "spv3a10":
                        return Mission.Spv3a10;
                    case "spv3a30":
                        return Mission.Spv3a30;
                    case "spv3a50":
                        return Mission.Spv3a50;
                    case "spv3b30":
                        return Mission.Spv3b30;
                    case "spv3b40":
                        return Mission.Spv3b40;
                    case "spv3c10":
                        return Mission.Spv3c10;
                    case "spv3c20":
                        return Mission.Spv3c20;
                    case "spv3c40":
                        return Mission.Spv3c40;
                    case "spv3d20":
                        return Mission.Spv3d20;
                    case "spv3d30":
                        return Mission.Spv3d30;
                    case "spv3d40":
                        return Mission.Spv3d40;
                    case "spv3d30_evolved":
                        return Mission.Spv3d30Evolved;
                    default:
                        throw new ArgumentException("Invalid SPV3 mission value.");
                }
            }

            /**
             * Read the mission & difficulty data from the provided savegame binary path.
             */
            using (var reader = new BinaryReader(System.IO.File.Open(_file, FileMode.Open)))
            {
                reader.BaseStream.Seek(DifficultyOffset, SeekOrigin.Begin);
                var difficulty = reader.ReadInt16();

                reader.BaseStream.Seek(MissionOffset, SeekOrigin.Begin);
                var mission = new string(reader.ReadChars(MissionLength))
                    .TrimEnd('\0');

                return new Savegame
                {
                    Difficulty = GetDifficulty(difficulty),
                    Mission = GetMission(mission)
                };
            }
        }
    }
}