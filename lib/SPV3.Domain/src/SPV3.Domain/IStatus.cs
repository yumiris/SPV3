/**
 * Copyright (c) 2018 Emilian Roman
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
    }
}