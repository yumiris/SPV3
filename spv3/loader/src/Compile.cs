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

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HXE;
using SPV3.Annotations;
using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.File;
using static System.IO.Path;
using static System.Reflection.Assembly;

namespace SPV3
{
  public class Compile : INotifyPropertyChanged
  {
    private bool   _canCompile = true;
    private string _status     = "Awaiting user input...";
    private string _target     = Combine(GetFolderPath(Personal), "My Games", "Halo SPV3 Compiled");

    public bool CanCompile
    {
      get => _canCompile;
      set
      {
        if (value == _canCompile) return;
        _canCompile = value;
        OnPropertyChanged();
      }
    }

    public string Status
    {
      get => _status;
      set
      {
        if (value == _status) return;
        _status = value;
        OnPropertyChanged();
      }
    }

    public string Target
    {
      get => _target;
      set
      {
        if (value == _target) return;
        _target = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public async void Commit()
    {
      try
      {
        CanCompile = false;

        /**
         * Back up the file which permits us to compile.
         *
         * After all, we wouldn't want our end-users compiling. ^_^
         */

        var sourceCompileFile = Combine(CurrentDirectory,               Paths.Compile);
        var backupCompileFile = Combine(GetFolderPath(ApplicationData), Paths.Compile);

        if (Exists(backupCompileFile))
          Delete(backupCompileFile);

        if (Exists(sourceCompileFile))
          Move(sourceCompileFile, backupCompileFile);

        /**
         * And now... we lift off!
         */

        Status = "Currently compiling SPV3, please wait ...";
        
        await Task.Run(() => { SFX.Compile(new SFX.Configuration
        {
          Source     = new DirectoryInfo(CurrentDirectory),
          Target     = new DirectoryInfo(Paths.Packages(Target)),
          Executable = new FileInfo(GetExecutingAssembly().Location)
        }); });

        /**
         * Copy data... 
         */

        Copy(GetExecutingAssembly().Location, Combine(Target, "spv3.exe"));
        Copy(Combine(CurrentDirectory,                        "hxe.exe"), Combine(Target, "hxe.exe"));

        /**
         * Restore the file which permits us to compile!
         */

        if (Exists(backupCompileFile))
          Move(backupCompileFile, sourceCompileFile);

        var redundantHxeExecutable = Combine(Paths.Packages(Target), HXE.Paths.Executable);

        if (Exists(redundantHxeExecutable))
          Delete(redundantHxeExecutable);

        Status     = "Compilation has successfully finished!";
        CanCompile = true;
      }
      catch (Exception e)
      {
        var msg = "Compilation failed.\n Error:  " + e.ToString() + "\n";
        var log = (HXE.File)Paths.Exception;
        log.AppendAllText(msg);
        Status     = msg;
        CanCompile = true;
      }
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}