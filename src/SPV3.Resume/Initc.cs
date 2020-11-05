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
using System.Linq;
using File = SPV3.Domain.File;

namespace SPV3.Resume
{
    /// <summary>
    ///     Saves data to the provided initc text file.
    /// </summary>
    public class Initc
    {
        /// <summary>
        ///     Command used for declaring the Mission variable.
        /// </summary>
        private const string MissionSet = "set f3";

        /// <summary>
        ///     Command used for declaring the difficulty.
        /// </summary>
        private const string DifficultySet = "game_difficulty_set";

        /// <summary>
        ///     Initc text file.
        /// </summary>
        private readonly File _file;

        /// <summary>
        ///     Initc constructor.
        /// </summary>
        /// <param name="file">
        ///     Initc text file.
        /// </param>
        public Initc(File file)
        {
            _file = file;
        }

        /// <summary>
        ///     Serialises Initc instance to the provided text file.
        /// </summary>
        /// <param name="progress">
        ///     Instance to serialise to the text file.
        /// </param>
        public void Save(Progress progress)
        {
            /**
             * Infers the Difficulty string from the inbound enum member.
             */
            string GetDifficulty(Difficulty difficulty)
            {
                switch (difficulty)
                {
                    case Difficulty.Noble:
                        return "easy";
                    case Difficulty.Normal:
                        return "normal";
                    case Difficulty.Heroic:
                        return "hard";
                    case Difficulty.Legendary:
                        return "impossible";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
                }
            }

            /**
             * Infers the Mission string from the inbound enum member.
             */
            int GetMission(Mission mission)
            {
                switch (mission)
                {
                    case Mission.Spv3a10:
                        return 1;
                    case Mission.Spv3a30:
                        return 2;
                    case Mission.Spv3a50:
                        return 3;
                    case Mission.Spv3b30:
                        return 4;
                    case Mission.Spv3b40:
                        return 5;
                    case Mission.Spv3c10:
                        return 6;
                    case Mission.Spv3c20:
                        return 7;
                    case Mission.Spv3c40:
                        return 8;
                    case Mission.Spv3d20:
                        return 9;
                    case Mission.Spv3d30Evolved:
                        return 10;
                    case Mission.Spv3d40:
                        return 11;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mission), mission, null);
                }
            }

            /**
             * Save initc current state, excluding the commands that will be written. 
             */
            var state = System.IO.File.Exists(_file)
                ? System.IO.File.ReadAllLines(_file)
                    .Where(line => !line.Contains(DifficultySet))
                    .Where(line => !line.Contains(MissionSet))
                : new[] {string.Empty};

            System.IO.File.Delete(_file);

            using (var writer = new StreamWriter(System.IO.File.Open(_file, FileMode.OpenOrCreate)))
            {
                /**
                 * Re-introduce the previous state's lines.
                 */
                foreach (var s in state)
                    writer.WriteLine(s);

                /**
                 * Build the mission & difficulty declarations & write them.
                 */
                var difficulty = $"{DifficultySet} {GetDifficulty(progress.Difficulty)}";
                var mission = $"{MissionSet} {GetMission(progress.Mission)}";

                writer.WriteLine(difficulty);
                writer.WriteLine(mission);
            }
        }
    }
}