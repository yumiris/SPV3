using LoaderConf = SPV3.Configuration.ConfigurationLoader;
using KernelConf = HXE.Kernel.Configuration;
using static System.IO.File;

namespace SPV3
{
  /**
   * Object representing the HXE Kernel.
   */
  public static class Kernel
  {
    public static KernelConf hxe  = new KernelConf(Paths.Kernel);
    public static LoaderConf spv3 = new LoaderConf();

    /**
     * Available HXE kernel modes.
     */
    public enum Mode
    {
      HCE,   /* vanilla hce mode -- won't ever be used by SPV3        */
      SPV32, /* <=SPV3.2 "Legacy" mode, without post-processing       */
      SPV33  /* >=SPV3.3 mode, with post-processing and other goodies */
    }

    /**
     * Infer the mode that the HXE kernel will use in the current environment.
     */
    public static Mode Infer()
    {
      return Exists(HXE.Paths.Legacy)
        ? Mode.SPV32
        : Mode.SPV33;
    }

    public static void CopyLoaderToKernel()
    {
      hxe.Mode = (HXE.Kernel.Configuration.ConfigurationMode) Infer();
      hxe.Tweaks.Sensor            = !(spv3.Photo && Exists(Paths.Photo)); /* forcefully enable motion sensor   */
      hxe.Main.Reset               = true;                   /* improve loading stability         */
      hxe.Main.Patch               = true;                   /* improve loading stability         */
      hxe.Main.Resume              = true;                   /* improve loading stability         */
      hxe.Main.Start               = true;                   /* improve loading stability         */
      hxe.Video.Quality            = true;                   /* enforce in-game quality settings  */
      hxe.Video.GammaOn            = spv3.GammaOn;
      hxe.Video.Gamma              = spv3.Gamma;
      hxe.Video.Bless              = spv3.DisplayMode == (byte) LoaderConf.DisplayModes.Borderless;
      hxe.Audio.Enhancements       = spv3.EAX;
      hxe.Input.Override           = spv3.Preset;
      hxe.Tweaks.CinemaBars        = spv3.CinemaBars;
      hxe.Tweaks.Unload            = !spv3.Shaders;
      if (!hxe.Video.Bless)
      {
        hxe.Main.Elevated           = spv3.Elevated;          /* prevent certain crashes           */
        hxe.Video.ResolutionEnabled = spv3.ResolutionEnabled; /* permit custom resolution override */
        hxe.Video.Uncap             = spv3.Vsync == false;    /* sync fps to refresh rate          */
      }
    }

    public static void CopyKernelToLoader()
    {
      spv3.Photo             = Exists(Paths.Photo) && hxe.Tweaks.Sensor == false;
      spv3.GammaOn           = hxe.Video.GammaOn;
      spv3.Gamma             = hxe.Video.Gamma;
      spv3.EAX               = hxe.Audio.Enhancements;
      spv3.Preset            = hxe.Input.Override;
      spv3.CinemaBars        = hxe.Tweaks.CinemaBars;
      spv3.Shaders           = hxe.Tweaks.Unload == false;

      if (hxe.Video.Bless)
      {
        spv3.DisplayMode = (byte) LoaderConf.DisplayModes.Borderless;
          //spv3.ResolutionEnabled = false;
          //spv3.Window     = true;
          //spv3.Borderless = true;
          //spv3.Vsync      = false;
          //spv3.Elevated   = false;
      }
      else
      {
        spv3.ResolutionEnabled = hxe.Video.ResolutionEnabled;
        spv3.Vsync             = hxe.Video.Uncap == false;
        spv3.Elevated          = hxe.Main.Elevated;
      }
    }

    public static void Load()
    {
      hxe.Load();
      spv3.Load();
      CopyLoaderToKernel();
    }

    /// <summary>
    /// Apply SPV3 Loader settings to the Kernel state 
    /// and save the Kernel state to the file system.
    /// </summary>
    public static void Save()
    {
      CopyLoaderToKernel();
      spv3.Save();
      hxe.Save();
    }
  }
}