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
    ///     Interface dealing with a progress status. This should be implemented by a type that binds or outputs data
    ///     to the user. For example, a ViewModel can implement this interface by assigning the data to a property that
    ///     is bound to a view control.
    /// </summary>
    public interface IStatus
    {
        /// <summary>
        ///     Appends given progress to the current status.
        /// </summary>
        /// <param name="data">
        ///     Data to append the status.
        /// </param>
        void CommitStatus(string data);

        /// <summary>
        ///     Appends given progress to the current status, and handles the given status type.
        /// </summary>
        /// <param name="data">
        ///     Data to append the status.
        /// </param>
        /// <param name="type"></param>
        void CommitStatus(string data, StatusType type);
    }
}

