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
using static System.Environment;

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
			var processCandidate = new Candidate();
			try
			{
				processCandidate = Candidates
					.FirstOrDefault(x => DeeperCheck(GetProcesses()
						.FirstOrDefault(Processname => Processname.ProcessName == x.Name), x.Name));
			}
			catch(System.Exception e)
      {
				var msg = $" -- Process Inference failed{NewLine}Error:  { e }{NewLine}";
				var log = (File) Paths.Exception;
				log.AppendAllText(msg);
				Console.Info(msg);
				throw;
			}

			return processCandidate?.Type ?? Type.Unknown;
		}

		static bool DeeperCheck(System.Diagnostics.Process process, string candidateName)
		{
			/** Check for NullReferenceException (no processes match current candidate) */
			try
			{
				bool check = process.ProcessName == candidateName;
			}
			catch(System.NullReferenceException)
      {
				return false;
      }

			switch(process.ProcessName)
			{
				case "halo":
				case "haloce":
					{
						try
						{
							return process.MainModule.FileVersionInfo.FileVersion == "01.00.10.0621";
						}
						catch (System.Exception e)
						{
							ErrorOutput(e, "Failed to assess Halo/HaloCE process.");
							return false;
						}
					}

				case "MCC-Win64-Shipping-WinStore":
				case "MCC-Win64-Shipping":
					{
						try
						{
							return process.Modules
								.Cast<System.Diagnostics.ProcessModule>()
								.Any(module => module.ModuleName == Paths.MCC.Halo1dll);
						}
						catch (System.Exception e)
						{
							var msg2 = string.Empty;
							msg2 += Is64BitProcess ? "Current process is 64-bit." : "Current process is not 32-bit.";
							msg2 += NewLine;
							msg2 += Is64BitOperatingSystem ? "Operating system is 64-bit." : "Operating system is NOT 64-bit.";
							ErrorOutput(e, msg2);
							return false;
						}
					}

				default:
					return false;
			}
			void ErrorOutput(System.Exception e, string msg2 = "")
			{
				var msg = $" -- Process Inference failed{NewLine}{msg2}{NewLine}Error:  { e }{NewLine}";
				var log = (File) Paths.Exception;
				log.AppendAllText(msg);
				Console.Error(msg);;
			}
		}

		public class Candidate
		{
			public Type   Type { get; set; }
			public string Name { get; set; }
		}
	}
}