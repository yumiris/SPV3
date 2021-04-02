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

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using static HXE.Paths.MCC;

namespace HXE.Steam
{
  public partial class MCC
  {
    public static class Halo1
    {
      /// <summary>
      /// Set a new path for Halo1.dll
      /// </summary>
      public static void SetHalo1Path(Platform platform)
      {
        switch(platform)
        {
          case Platform.Steam:
            var libraries = new Libraries();
            var mccH1 = Path.Combine(HTMCC, Halo1dir, Halo1dll);
            libraries.ParseLibraries();
            Halo1Path = libraries.FindInLibraries(mccH1).First();
            break;
          case Platform.WinStore:
            // TODO
            throw new NotImplementedException("TODO: Add function to find WinStore MCC files.");
            //break;
          default:
            throw new ArgumentOutOfRangeException(
              $"Cannot set Halo1.dll path: Specified platform is invalid." + Environment.NewLine +
              $"How did you do that?");
        }

        if (Halo1Path == null)
          throw new FileNotFoundException("Halo1.dll not found");

        if (!VerifyHalo1DLL())
          throw new Exception("Halo1.dll is invalid.");
      }

      /// <summary>
      /// Check if the inferred Halo1.dll is probably legitimate.
      /// </summary>
      /// <returns>
      /// true if the file's Digital Certificate is valid.
      /// </returns>
      public static bool VerifyHalo1DLL()
      {
        /// Known issue: This doesn't verify the file is a real Halo1.dll
        /// It merely checks if the file's certificate is valid.

        /** Convert embedded resource to X509Certificate */
        X509Certificate P7B_Fallback;
        {
          var ms = new MemoryStream();
          var assembly = Assembly.GetExecutingAssembly();
          assembly.GetManifestResourceStream(@"HXE.Assets.343I_DER.cer").CopyTo(ms);
          P7B_Fallback = new X509Certificate(ms.ToArray());
        }

        /** Get 343 Industries' Public Key from internet source. */
        // TODO: Add OFFICIAL URI for web-accessible public key
        var uri = "https://github.com/HaloSPV3/HCE/releases/download/updates/343I_DER.exp2022-04-27.cer";
        X509Certificate remoteCert = new X509Certificate();
        if (!string.IsNullOrEmpty(uri))
          remoteCert = new X509Certificate(GetWebStream(uri).ToArray());

        /** Get Public Key of a reference Certificate.
         * If we failed to get the remote certificate,
         * fallback to the embedded resource.
         * This allows for offline validation.
         */
        var publicKey_ref = string.IsNullOrEmpty(uri) ?
            P7B_Fallback.GetPublicKey() :
            remoteCert.GetPublicKey();

        /** Get certificate from local Halo1.dll. */
        var publicKey_file = new X509Certificate(Halo1Path).GetPublicKey();

        /** Return whether or not Halo1.dll is valid.
         * If the Halo1.dll certificate's public key is equal to
         * the cached PublicKey, return True. */
        return publicKey_file.SequenceEqual(publicKey_ref);
      }

      public enum Platform
      {
        Steam,
        WinStore
      }

      public static MemoryStream GetWebStream(string uri)
      {
        MemoryStream dataStream = new MemoryStream();
        var webRequest = WebRequest.Create(uri);
        webRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

        using (var wr = (HttpWebResponse) webRequest.GetResponse())
        using (var rs = wr.GetResponseStream())
        using (var sr = new StreamReader(rs ?? throw new NullReferenceException("No response.")))
        {
          sr.BaseStream.CopyTo(dataStream);
        }

        return dataStream;
      }
    }
  }
}
