/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Loader.
 * 
 * SPV3.Loader is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Loader is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Loader.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace SPV3.Loader
{
    /// <summary>
    ///     Creates Parameters-type instances.
    /// </summary>
    public class ParametersParser
    {
        /// <summary>
        ///     Initialise ExecutableParameter based on the inbound string value.
        /// </summary>
        /// <param name="parameters">
        ///     String value representing representing HCE-compliant startup parameters.
        /// </param>
        /// <returns>
        ///     Parameters instance based on the inbound string value.
        /// </returns>
        public Parameters Parse(string parameters)
        {
            var result = new Parameters();

            if (parameters.Contains("-nosound"))
                result.DisableSound = true;

            if (parameters.Contains("-novideo"))
                result.DisableVideo = true;

            if (parameters.Contains("-nojoystick"))
                result.DisableJoystick = true;

            if (parameters.Contains("-nogamma"))
                result.DisableGamma = true;

            if (parameters.Contains("-safemode"))
                result.EnableSafeMode = true;

            if (parameters.Contains("-window"))
                result.EnableWindowMode = true;

            if (parameters.Contains("-screenshot"))
                result.EnableScreenshot = true;

            if (parameters.Contains("-console"))
                result.EnableConsole = true;

            if (parameters.Contains("-devmode"))
                result.EnableDeveloperMode = true;

            if (parameters.Contains("-useff"))
                result.CardType = CardType.FixedFunction;

            if (parameters.Contains("-use11"))
                result.CardType = CardType.Shaders11Card;

            if (parameters.Contains("-use14"))
                result.CardType = CardType.Shaders14Card;

            if (parameters.Contains("-use20"))
                result.CardType = CardType.Shaders20Card;

            // TODO: Parse argument-type parameters!

            return result;
        }
    }
}