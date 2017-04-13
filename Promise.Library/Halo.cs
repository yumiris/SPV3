﻿using System.Diagnostics;
using Promise.Library.Utilities;

namespace Promise.Library
{
    public class Halo
    {
        public const string ExeName = "haloce.exe";

        public void Launch(ConfigOperation configOperation)
        {
            Process.Start(ExeName, configOperation.ReadConfiguration());
        }
    }
}