/**
 * Copyright (c) 2021 Emilian Roman
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
using System.Linq;
using static System.Diagnostics.Process;

namespace HXE
{
	public class Process
	{
		public enum Type
		{
			Unknown,
			Retail, /* Halo: Combat Evolved */
			HCE,    /* Halo: Custom Edition */
			Steam,  /* MCC (Steam)          */
			Store   /* MCC (Windows Store)  */
		}

		public static IEnumerable<Candidate> Candidates { get; } = new List<Candidate>
		{
			new Candidate { Type = Type.Retail, Name = "halo"                        },
			new Candidate { Type = Type.HCE,    Name = "haloce"                      },
			new Candidate { Type = Type.Steam,  Name = "MCC-Win64-Shipping"          },
			new Candidate { Type = Type.Store,  Name = "MCC-Win64-Shipping-WinStore" }
		};

		/// <summary>
		/// Infers the running Halo executable, with support for HCE, HCE and MCC (Steam & Windows Store).
		/// </summary>
		/// <returns>Type of Platform</returns>
		public static Type Infer()
		{
            var processCandidate = Candidates
                .FirstOrDefault(x => DeeperCheck(GetProcesses()
                    .FirstOrDefault(Processname => Processname.ProcessName == x.Name)));

            return processCandidate?.Type ?? Type.Unknown;
		}

		static bool DeeperCheck(System.Diagnostics.Process process)
        {
			if (process == null)
				return false;
			else if (process.ProcessName == "MCC-Win64-Shipping-WinStore")
			{
				using (System.Diagnostics.Process checkerProcess = new System.Diagnostics.Process())
				{
					checkerProcess.StartInfo = new System.Diagnostics.ProcessStartInfo(@"Validator.exe", process.Id.ToString());
					checkerProcess.StartInfo.CreateNoWindow = true;
					checkerProcess.StartInfo.ErrorDialog = false;
					checkerProcess.StartInfo.RedirectStandardError = true;
					checkerProcess.StartInfo.RedirectStandardInput = true;
					checkerProcess.StartInfo.RedirectStandardOutput = true;
					checkerProcess.StartInfo.UseShellExecute = false;
					checkerProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
					checkerProcess.Start();
					if (!checkerProcess.HasExited)
					{
						checkerProcess.WaitForExit(30000);
						if (!checkerProcess.HasExited)
						{
							checkerProcess.Kill();
						}
					}

					bool output = bool.Parse(checkerProcess.StandardOutput.ReadToEnd().Trim());
					if (output)
						return true;
					else
						return false;
				}
			}
			else if (process.MainModule.FileVersionInfo.FileVersion == "01.00.10.0621")
				return true;
			else
				return false;
        }

		public class Candidate
		{
			public Type   Type { get; set; }
			public string Name { get; set; }
		}
	}
}