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

using System.IO;
using System.Text;
using static SPV3.CLI.Configuration.PostProcessingConfiguration;
using static SPV3.CLI.Configuration.PostProcessingConfiguration.ExperimentalPostProcessing;

namespace SPV3.CLI
{
  /// <summary>
  ///   Represents the loader configuration.
  /// </summary>
  public class Configuration : File
  {
    public KernelConfiguration         Kernel         { get; set; } = new KernelConfiguration();
    public PostProcessingConfiguration PostProcessing { get; set; } = new PostProcessingConfiguration();

    public void Save()
    {
      using (var fs = new FileStream(Paths.Files.Configuration, FileMode.OpenOrCreate, FileAccess.Write))
      using (var ms = new MemoryStream(256))
      using (var bw = new BinaryWriter(ms))
      {
        ms.Position = 0;

        /* signature */
        {
          bw.Write(Encoding.Unicode.GetBytes("~yumiris"));
        }

        /* padding */
        {
          bw.Write(new byte[16 - ms.Position]);
        }

        /* kernel */
        {
          bw.Write(Kernel.SkipVerifyMainAssets);
          bw.Write(Kernel.SkipInvokeCoreTweaks);
          bw.Write(Kernel.SkipResumeCheckpoint);
          bw.Write(Kernel.SkipSetShadersConfig);
          bw.Write(Kernel.SkipInvokeExecutable);
          bw.Write(Kernel.SkipPatchLargeAAware);
        }

        /* padding */
        {
          bw.Write(new byte[64 - ms.Position]);
        }

        /* post-processing */
        {
          bw.Write(PostProcessing.Internal);
          bw.Write(PostProcessing.External);
          bw.Write(PostProcessing.GBuffer);
          bw.Write(PostProcessing.DepthFade);
          bw.Write(PostProcessing.Bloom);
          bw.Write(PostProcessing.LensDirt);
          bw.Write(PostProcessing.DynamicLensFlares);
          bw.Write(PostProcessing.Volumetrics);
          bw.Write(PostProcessing.AntiAliasing);
          bw.Write(PostProcessing.HudVisor);

          bw.Write((byte) PostProcessing.MotionBlur);
          bw.Write((byte) PostProcessing.Mxao);
          bw.Write((byte) PostProcessing.Dof);
          bw.Write((byte) PostProcessing.Experimental.ThreeDimensional);
          bw.Write((byte) PostProcessing.Experimental.ColorBlindMode);
        }

        /* padding */
        {
          bw.Write(new byte[256 - ms.Position]);
        }

        ms.Position = 0;
        ms.CopyTo(fs);
      }
    }

    public void Load()
    {
      using (var fs = new FileStream(Paths.Files.Configuration, FileMode.Open, FileAccess.Read))
      using (var ms = new MemoryStream(256))
      using (var br = new BinaryReader(ms))
      {
        fs.CopyTo(ms);
        ms.Position = 0;

        /* padding */
        {
          ms.Position += 16 - ms.Position;
        }

        /* kernel */
        {
          Kernel.SkipVerifyMainAssets = br.ReadBoolean();
          Kernel.SkipInvokeCoreTweaks = br.ReadBoolean();
          Kernel.SkipResumeCheckpoint = br.ReadBoolean();
          Kernel.SkipSetShadersConfig = br.ReadBoolean();
          Kernel.SkipInvokeExecutable = br.ReadBoolean();
          Kernel.SkipPatchLargeAAware = br.ReadBoolean();
        }

        /* padding */
        {
          ms.Position += 64 - ms.Position;
        }

        /* post-processing */
        {
          PostProcessing.Internal          = br.ReadBoolean();
          PostProcessing.External          = br.ReadBoolean();
          PostProcessing.GBuffer           = br.ReadBoolean();
          PostProcessing.DepthFade         = br.ReadBoolean();
          PostProcessing.Bloom             = br.ReadBoolean();
          PostProcessing.LensDirt          = br.ReadBoolean();
          PostProcessing.DynamicLensFlares = br.ReadBoolean();
          PostProcessing.Volumetrics       = br.ReadBoolean();
          PostProcessing.AntiAliasing      = br.ReadBoolean();
          PostProcessing.HudVisor          = br.ReadBoolean();

          PostProcessing.MotionBlur                    = (MotionBlurOptions) br.ReadByte();
          PostProcessing.Mxao                          = (MxaoOptions) br.ReadByte();
          PostProcessing.Dof                           = (DofOptions) br.ReadByte();
          PostProcessing.Experimental.ThreeDimensional = (ThreeDimensionalOptions) br.ReadByte();
          PostProcessing.Experimental.ColorBlindMode   = (ColorBlindModeOptions) br.ReadByte();
        }
      }
    }


    /// <summary>
    ///   Represents the inbound object as a string.
    /// </summary>
    /// <param name="configuration">
    ///   Object to represent as string.
    /// </param>
    /// <returns>
    ///   String representation of the inbound object.
    /// </returns>
    public static implicit operator string(Configuration configuration)
    {
      return configuration.Path;
    }

    /// <summary>
    ///   Represents the inbound string as an object.
    /// </summary>
    /// <param name="path">
    ///   String to represent as object.
    /// </param>
    /// <returns>
    ///   Object representation of the inbound string.
    /// </returns>
    public static explicit operator Configuration(string path)
    {
      return new Configuration
      {
        Path = path
      };
    }

    /// <summary>
    ///   File-driven kernel configuration object.
    /// </summary>
    public class KernelConfiguration
    {
      public bool SkipVerifyMainAssets { get; set; }
      public bool SkipInvokeCoreTweaks { get; set; }
      public bool SkipResumeCheckpoint { get; set; }
      public bool SkipSetShadersConfig { get; set; }
      public bool SkipInvokeExecutable { get; set; }
      public bool SkipPatchLargeAAware { get; set; }
    }

    public class PostProcessingConfiguration
    {
      /// <summary>
      ///   Depth of Field values.
      /// </summary>
      public enum DofOptions
      {
        Off  = 0x0,
        Low  = 0x1,
        High = 0x2
      }

      /// <summary>
      ///   Motion Blur values.
      /// </summary>
      public enum MotionBlurOptions
      {
        Off      = 0x0,
        BuiltIn  = 0x1,
        PombLow  = 0x2,
        PombHigh = 0x3
      }

      /// <summary>
      ///   MXAO values.
      /// </summary>
      public enum MxaoOptions
      {
        Off  = 0x0,
        Low  = 0x1,
        High = 0x2
      }

      public bool                       Internal          { get; set; }
      public bool                       External          { get; set; }
      public bool                       GBuffer           { get; set; }
      public bool                       DepthFade         { get; set; }
      public bool                       Bloom             { get; set; }
      public bool                       LensDirt          { get; set; }
      public bool                       DynamicLensFlares { get; set; }
      public bool                       Volumetrics       { get; set; }
      public bool                       AntiAliasing      { get; set; }
      public bool                       HudVisor          { get; set; }
      public MotionBlurOptions          MotionBlur        { get; set; } = MotionBlurOptions.Off;
      public MxaoOptions                Mxao              { get; set; } = MxaoOptions.Off;
      public DofOptions                 Dof               { get; set; } = DofOptions.Off;
      public ExperimentalPostProcessing Experimental      { get; set; } = new ExperimentalPostProcessing();

      /// <summary>
      ///   Experimental overrides for SPV3.
      /// </summary>
      public class ExperimentalPostProcessing
      {
        public enum ColorBlindModeOptions
        {
          Off          = 0x0,
          Protanopia   = 0x1,
          Deuteranopes = 0x2,
          Tritanopes   = 0x3
        }

        public enum ThreeDimensionalOptions
        {
          Off          = 0x0,
          Anaglyphic   = 0x1,
          Interleaving = 0x2,
          SideBySide   = 0x3
        }

        public ThreeDimensionalOptions ThreeDimensional { get; set; } = ThreeDimensionalOptions.Off;
        public ColorBlindModeOptions   ColorBlindMode   { get; set; } = ColorBlindModeOptions.Off;
      }
    }
  }
}