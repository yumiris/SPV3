/**
 * Copyright (c) 2019 Emilian Roman
 * Copyright (c) 2020 Noah Sherwin
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

using System.Collections.Generic;
using static HXE.OpenSauce.OpenSauceObjects.ObjectsWeapon;
using static SPV3.Kernel;

namespace SPV3
{
  public partial class Configuration
  {
    // See SPV3.Kernel class
    public ConfigurationLoader    Loader    { get; set; } = spv3;
    public ConfigurationShaders   Shaders   { get; set; } = new ConfigurationShaders();
    public ConfigurationOpenSauce OpenSauce { get; set; } = new ConfigurationOpenSauce();
    public ConfigurationChimera   Chimera   { get; set; } = new ConfigurationChimera();

    public void Load()
    {
      Kernel.Load();    // kernel/loader bin
      Loader = spv3;    // loader bin. Copy static instance to local instance. Deprecate local instance?
      Shaders.Load();   // kernel bin, load from Kernel.hxe
      OpenSauce.Load(); // OS_Settings.user.xml
      Chimera.Load();   // chimera bin
    }

    public void Save()
    {
      spv3 = Loader;    // loader bin. Copy local instance to static instance
      Shaders.Save();   // kernel bin, load from Kernel.hxe
      Kernel.Save();    // kernel/loader bin.
      OpenSauce.Save(); // OS_Settings.user.xml
      Chimera.Save();   // chimera bin
    }

    public void CalculateFOV()
    {
      OpenSauce.FieldOfView = OpenSauce.Configuration.Camera.CalculateFOV(Loader.Width, Loader.Height);
    }

    public void ResetWeaponPositions()
    {
      OpenSauce.Configuration.Objects.Weapon.Positions = new List<PositionWeapon>();
    }

    public void ShowHxeSettings()
    {
      Save(); // Save all settings, copy Loader to Kernel equivalents

      var Settings = new HXE.Settings(hxe); // Pass modified Kernel instance to Settings.
      Settings.ShowDialog();
      if (Settings.DialogResult == true)
      {
        hxe.Load();
        spv3.Load();
        CopyKernelToLoader();
        Loader = spv3;
      }
    }

    public void ShowHxeWepPositions()
    {
      new HXE.Positions().ShowDialog();
    }
  }
}