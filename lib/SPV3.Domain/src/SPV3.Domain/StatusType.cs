/**
 * Copyright (C) 2019 Emilian Roman
 * 
 * This file is part of SPV3.Domain.
 * 
 * SPV3.Domain is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Domain is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Domain.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace SPV3.Domain
{
    /// <summary>
    ///     Status types handled by an <see cref="IStatus"/> implementer.
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        ///     Successful step or execution.
        /// </summary>
        Success,
        
        /// <summary>
        ///     Non-fatal warning during execution.
        /// </summary>
        Warning,
        
        /// <summary>
        ///     Execution failure.
        /// </summary>
        Failure,
        
        /// <summary>
        ///     Require user inut.
        /// </summary>
        Require
    }
}