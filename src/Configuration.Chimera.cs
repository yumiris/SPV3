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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using HXE.SPV3;
using SPV3.Annotations;
using static HXE.Paths.Custom;

namespace SPV3
{
  public class ConfigurationChimera : INotifyPropertyChanged
  {
    private bool _anisotropicFiltering = true;
    private bool _blockLOD             = false;
    private int  _interpolation        = 8;
    private bool _uncapCinematic       = true;

    public Chimera Configuration { get; } = (Chimera) Chimera(Paths.Directory);

    public int Interpolation
    {
      get => _interpolation;
      set
      {
        if (value == _interpolation) return;
        _interpolation = value;
        OnPropertyChanged();
      }
    }

    public bool AnisotropicFiltering
    {
      get => _anisotropicFiltering;
      set
      {
        if (value == _anisotropicFiltering) return;
        _anisotropicFiltering = value;
        OnPropertyChanged();
      }
    }

    public bool UncapCinematic
    {
      get => _uncapCinematic;
      set
      {
        if (value == _uncapCinematic) return;
        _uncapCinematic = value;
        OnPropertyChanged();
      }
    }

    public bool BlockLOD
    {
      get => _blockLOD;
      set
      {
        if (value == _blockLOD) return;
        _blockLOD = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void Load()
    {
      if (!Configuration.Exists()) return;

      Configuration.Load();

      Interpolation        = Configuration.Interpolation;
      AnisotropicFiltering = Configuration.AnisotropicFiltering;
      UncapCinematic       = Configuration.UncapCinematic;
      BlockLOD             = Configuration.BlockLOD;
    }

    public void Save()
    {
      Configuration.Interpolation        = (byte) Interpolation;
      Configuration.AnisotropicFiltering = AnisotropicFiltering;
      Configuration.UncapCinematic       = UncapCinematic;
      Configuration.BlockLOD             = BlockLOD;

      Configuration.Save();
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}