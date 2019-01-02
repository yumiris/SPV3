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

using System.Collections.Generic;
using System.Text;

namespace SPV3.Loader
{
    public class ParametersSerialiser
    {
        /// <summary>
        ///     Serialises the instance to a string that complies with the HCE startup arguments.
        /// </summary>
        /// <returns>
        ///     HCE-compliant startup string representation of this instance.
        /// </returns>
        public string Serialise(Parameters parameters)
        {
            var builder = new StringBuilder();

            // append the string values for toggles if they're enabled
            foreach (var toggle in new Dictionary<string, bool>
            {
                // disable overrides
                {"-nosound", parameters.DisableSound},
                {"-novideo", parameters.DisableVideo},
                {"-nojoystick", parameters.DisableJoystick},
                {"-nogamma", parameters.DisableGamma},

                // enable overrides
                {"-safemode", parameters.EnableSafeMode},
                {"-window", parameters.EnableWindowMode},
                {"-screenshot", parameters.EnableScreenshot},
                {"-console", parameters.EnableConsole},
                {"-devmode", parameters.EnableDeveloperMode}
            })
                if (toggle.Value)
                    builder.Append($"{toggle.Key} ");

            // shader overrides
            switch (parameters.CardType)
            {
                case CardType.FixedFunction:
                    builder.Append("-useff ");
                    break;
                case CardType.Shaders11Card:
                    builder.Append("-use11 ");
                    break;
                case CardType.Shaders14Card:
                    builder.Append("-use14 ");
                    break;
                case CardType.Shaders20Card:
                    builder.Append("-use20 ");
                    break;
                case CardType.Default:
                    builder.Append(string.Empty);
                    break;
                default:
                    builder.Append(string.Empty);
                    break;
            }

            // -vidmode
            if (parameters.VideoWidth != null && parameters.VideoHeight != null && parameters.VideoRefreshRate != null)
                builder.Append(
                    $"-vidmode {parameters.VideoWidth},{parameters.VideoHeight},{parameters.VideoRefreshRate} "
                );

            // -adapter
            if (parameters.VideoAdapterIndex != null)
                builder.Append($"-adapter {parameters.VideoAdapterIndex} ");

            // -port
            if (parameters.ServerPort != null)
                builder.Append($"-port {parameters.ServerPort} ");

            // -cport
            if (parameters.ClientPort != null)
                builder.Append($"-cport {parameters.ClientPort} ");

            // -ip
            if (!string.IsNullOrWhiteSpace(parameters.IpAddress))
                builder.Append($"-ip {parameters.IpAddress} ");

            return builder.ToString().TrimEnd();
        }
    }
}