/**
 * Copyright (C) 2018-2019 Emilian Roman
 * 
 * This file is part of HCE.HCE.BalsamV.
 * 
 * HCE.HCE.BalsamV is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * HCE.HCE.BalsamV is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with HCE.HCE.BalsamV.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace HCE.BalsamV.Settings
{
    /// <summary>
    ///     Connection types.
    /// </summary>
    public enum Connection
    {
        /// <summary>
        ///     56kbps setting - allows a maximum of 2 people to join a hosted server.
        /// </summary>
        Modem,

        /// <summary>
        ///     DSL/Cable (LOW) setting - allows a maximum of 4 people to join a hosted server.
        /// </summary>
        DslLow,

        /// <summary>
        ///     DSL/Cable (AVG) setting - allows a maximum of 8 people to join a hosted server.
        /// </summary>
        DslAverage,

        /// <summary>
        ///     DSL/Cable (HIGH) setting - allows a maximum of 10 people to join a hosted server.
        /// </summary>
        DslHigh,

        /// <summary>
        ///     Represents the 11/LAN setting - allows a maximum of 16 people to join a hosted server.
        /// </summary>
        Lan
    }
}