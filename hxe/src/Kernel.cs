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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HXE.Exceptions;
using static System.Environment;
using static System.IO.Path;
using static System.Windows.Forms.Screen;
using static HXE.Console;
using static HXE.Paths;
using static HXE.Paths.Files;
using static HXE.Profile.ProfileVideo;

namespace HXE
{
  /// <summary>
  ///   Class used for bootstrapping the HCE loading procedure.
  /// </summary>
  public static class Kernel
  {
    /**
     * Inic.txt can be located across multiple locations on the filesystem; however, HCE only deals with the one in
     * the working directory -- hence the name!
     */
    private static readonly Initiation RootInitc = (Initiation) Combine(CurrentDirectory, Files.Initiation);

    /// <summary>
    ///   Invokes the HCE loading procedure.
    /// </summary>
    public static void Bootstrap(Executable executable)
    {
      var configuration = (Configuration) Files.Configuration;

      if (!configuration.Exists())
        configuration.Save(); /* gracefully create new configuration */

      configuration.Load();

      if (!configuration.Kernel.SkipVerifyMainAssets)
        VerifyMainAssets();
      else
        Info("Skipping Kernel.VerifyMainAssets");

      if (!configuration.Kernel.SkipInvokeCoreTweaks)
        InvokeCoreTweaks(executable);
      else
        Info("Skipping Kernel.InvokeCoreTweaks");

      if (!configuration.Kernel.SkipResumeCheckpoint)
        ResumeCheckpoint(executable);
      else
        Info("Skipping Kernel.ResumeCheckpoint");

      if (!configuration.Kernel.SkipSetShadersConfig)
        SetShadersConfig(configuration);
      else
        Info("Skipping Kernel.SkipSetShadersConfig");

      if (!configuration.Kernel.SkipPatchLargeAAware)
        PatchLargeAAware(executable);
      else
        Info("Skipping Kernel.SkipPatchLargeAAware");

      if (!configuration.Kernel.SkipInvokeExecutable)
        InvokeExecutable(executable);
      else
        Info("Skipping Kernel.InvokeExecutable");
    }

    /// <summary>
    ///   Invokes the HCE data verification routines.
    /// </summary>
    private static void VerifyMainAssets()
    {
      /**
       * It is preferable to whitelist the type of files we would like to verify. The focus would be to skip any files
       * which are expected to be changed.
       *
       * For example, if HCE were to be distributed with a configuration file, then changing its contents would
       * result in an asset verification error. Additionally, changing HXE executable by updating it could result in
       * the same error.
       */

      var whitelist = new List<string>
      {
        ".map",      /* map resources */
        "haloce.exe" /* game executable */
      };

      /**
       * Through the use of the manifest that was copied to the installation directory, the loader can infer the list of
       * HCE files on the filesystem that it must verify. Verification is done through a simple size comparison between
       * the size of the file on the filesystem and the one declared in the manifest.
       *
       * This routine relies on combining...
       *
       * - the path of the working directory; with
       * - the package's declared relative path; with
       * - the filename of the respective file
       *
       * ... to determine the absolute path of the file on the filesystem:
       * 
       * X:\Installations\HCE\gallery\Content\editbox1024.PNG
       * |----------+---------|-------+-------|-------+-------|
       *            |                 |               |
       *            |                 |               + - File on the filesystem
       *            |                 + ----------------- Package relative path
       *            + ----------------------------------- Working directory
       */

      var manifest = (Manifest) Combine(CurrentDirectory, Files.Manifest);

      /**
       * This shouldn't be an issue in conventional HCE installations; however, for existing/current SPV3 installations
       * OR installations that weren't conducted by the installer, the manifest will likely not be present. As such, we
       * have no choice but to skip the verification mechanism.
       */
      if (!manifest.Exists()) return;

      Info("Deserialising found manifest file");
      manifest.Load();

      Info("Verifying assets recorded in the manifest file");
      foreach (var package in manifest.Packages)
      foreach (var entry in package.Entries)
      {
        if (!whitelist.Any(entry.Name.Contains)) /* skip verification if current file isn't in the whitelist */
          continue;

        var absolutePath = Combine(CurrentDirectory, package.Path, entry.Name);
        var expectedSize = entry.Size;
        var actualSize   = new FileInfo(absolutePath).Length;

        if (expectedSize == actualSize) continue;

        Info($"Size mismatch {entry.Name} (expect: {expectedSize}, actual: {actualSize}).");
        throw new AssetException($"Size mismatch {entry.Name} (expect: {expectedSize}, actual: {actualSize}).");
      }
    }

    /// <summary>
    ///   Invokes core improvements to the auto-detected profile, such as auto max resolution and gamma fixes. This is
    ///   NOT done when a profile does not exist/cannot be found!
    /// </summary>
    /// <param name="executable"></param>
    private static void InvokeCoreTweaks(Executable executable)
    {
      try
      {
        var lastprof = (LastProfile) Combine(executable.Profile.Path, Files.LastProfile);

        if (!lastprof.Exists()) return;

        Info("Deserialising found lastprof file");
        lastprof.Load();

        var profile = (Profile) Combine(
          executable.Profile.Path,
          Directories.Profiles,
          lastprof.Profile,
          Files.Profile
        );

        if (!profile.Exists()) return;

        Info("Deserialising inferred HCE profile");
        profile.Load();

        Info("Applying profile video settings");
        profile.Video.Resolution.Width  = (ushort) PrimaryScreen.Bounds.Width;
        profile.Video.Resolution.Height = (ushort) PrimaryScreen.Bounds.Height;
        profile.Video.FrameRate         = VideoFrameRate.VsyncOff; /* ensure no FPS locking */
        profile.Video.Particles         = VideoParticles.High;
        profile.Video.Quality           = VideoQuality.High;
        profile.Video.Effects.Specular  = true;
        profile.Video.Effects.Shadows   = true;
        profile.Video.Effects.Decals    = true;

        Info("Saving profile data to the filesystem");
        profile.Save();

        Debug("Patched video resolution width  - " + profile.Video.Resolution.Width);
        Debug("Patched video resolution height - " + profile.Video.Resolution.Height);
        Debug("Patched video frame rate        - " + profile.Video.FrameRate);
        Debug("Patched video quality           - " + profile.Video.Particles);
        Debug("Patched video texture           - " + profile.Video.Quality);
        Debug("Patched video effect - specular - " + profile.Video.Effects.Specular);
        Debug("Patched video effect - shadows  - " + profile.Video.Effects.Shadows);
        Debug("Patched video effect - decals   - " + profile.Video.Effects.Decals);
      }
      catch (Exception e)
      {
        Error(e.Message + " -- CORE TWEAKS WILL NOT BE APPLIED");
      }
    }

    /// <summary>
    ///   Invokes the profile & campaign auto-detection mechanism.
    /// </summary>
    private static void ResumeCheckpoint(Executable executable)
    {
      try
      {
        var lastprof = (LastProfile) Combine(executable.Profile.Path, Files.LastProfile);

        if (!lastprof.Exists()) return;

        Info("Deserialising found lastprof file");
        lastprof.Load();

        var playerDat = (Progress) Combine(
          executable.Profile.Path,
          Directories.Profiles,
          lastprof.Profile,
          Files.Progress
        );

        if (!playerDat.Exists()) return;

        Info("Deserialising inferred progress binary");
        playerDat.Load();

        Info("Updating the initiation file with campaign progress");
        RootInitc.Mission    = playerDat.Mission;
        RootInitc.Difficulty = playerDat.Difficulty;

        Info("Saving campaign progress to the initiation file");
        RootInitc.Save();

        Debug("Resumed campaign mission    - " + playerDat.Mission);
        Debug("Resumed campaign difficulty - " + playerDat.Difficulty);
      }
      catch (UnauthorizedAccessException e)
      {
        Error(e.Message + " -- CAMPAIGN WILL NOT RESUME");
      }
    }

    /// <summary>
    ///   Applies the post-processing settings.
    /// </summary>
    private static void SetShadersConfig(Configuration configuration)
    {
      try
      {
        Info("Updating the initiation file with the post-processing settings");
        RootInitc.PostProcessing = configuration.PostProcessing;

        Info("Saving post-processing settings to the initiation file");
        RootInitc.Save();

        Info("Applied PP settings for MXAO        - " + RootInitc.PostProcessing.Mxao);
        Info("Applied PP settings for DOF         - " + RootInitc.PostProcessing.Dof);
        Info("Applied PP settings for Motion Blur - " + RootInitc.PostProcessing.MotionBlur);
        Info("Applied PP settings for Lens Flares - " + RootInitc.PostProcessing.DynamicLensFlares);
        Info("Applied PP settings for Volumetrics - " + RootInitc.PostProcessing.Volumetrics);
        Info("Applied PP settings for Lens Dirt   - " + RootInitc.PostProcessing.LensDirt);
      }
      catch (UnauthorizedAccessException e)
      {
        Error(e.Message + " -- POST PROCESSING WILL NOT BE APPLIED");
      }
    }

    /// <summary>
    ///   Patches HCE executable for Large Address Aware.
    /// </summary>
    private static void PatchLargeAAware(Executable executable)
    {
      try
      {
        Info("Patching HCE executable with LAA flag");
        using (var fs = new FileStream(executable.Path, FileMode.Open, FileAccess.ReadWrite))
        using (var bw = new BinaryWriter(fs))
        {
          fs.Position = 0x136;
          bw.Write((byte) 0x2F);
        }

        Info("Applied LAA patch to the HCE executable");
      }
      catch (Exception e)
      {
        Error(e.Message + " -- LAA PATCH WILL NOT BE APPLIED");
      }
    }

    /// <summary>
    ///   Invokes the HCE executable.
    /// </summary>
    private static void InvokeExecutable(Executable executable)
    {
      executable.Start();
    }
  }
}