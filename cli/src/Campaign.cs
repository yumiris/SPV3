/**
 * Copyright (c) 2019 Emilian Roman
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

namespace SPV3.CLI
{
  /// <summary>
  ///   Object representation of the SPV3.2 campaign attributes.
  /// </summary>
  public static class Campaign
  {
    /// <summary>
    ///   Available SPV3.2 difficulties.
    /// </summary>
    public enum Difficulty
    {
      Normal,    // normal
      Heroic,    // hard
      Legendary, // impossible
      Noble      // easy
    }

    /// <summary>
    ///   Availabl3 SPV3.2 missions.
    /// </summary>
    public enum Mission
    {
      Spv3A10 = 1,
      Spv3A30,
      Spv3A50,
      Spv3B30,
      Spv3B30Evolved,
      Spv3B40,
      Spv3C10,
      Spv3C20,
      Spv3C40,
      Spv3D20,
      Spv3D25,
      Spv3D30,
      Spv3D30Evolved,
      Spv3D40,
      LumoriaA,
      LumoriaB,
      LumoriaCd
    }
  }
}