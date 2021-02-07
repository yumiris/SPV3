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

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Serialization;
using static System.Console;
using static System.Environment;
using static System.IO.Compression.CompressionMode;
using static System.IO.FileMode;
using static System.IO.Path;
using static System.IO.SeekOrigin;
using static System.Reflection.Assembly;
using static System.Text.Encoding;
using static HXE.Console;

namespace HXE
{
	/**
	 * Class containing the data structure and logic for creating SFX binaries.
	 */
	public class SFX
	{
		/**
		 * Files to be compressed and extracted on the filesystem using the SFX system.
		 */
		public List<Entry> Entries { get; set; } = new List<Entry>();

		/**
		 * Creates an SFX of the given source, using the current HXE executable, to the given target.
		 */
		public static void Compile(Configuration configuration)
		{
			var source = configuration.Source;
			var target = configuration.Target;
			var filter = configuration.Filter;

			target.Create();

			/**
			 * We will create an SFX for all of the files residing in the given source directory. Our goal is to contain all
			 * of the data in a single file, and subsequently reproduce the directory tree & file contents.
			 *
			 * For a reasonable balance between space savings and process time, we will DEFLATE each file before including it
			 * in the output SFX binary.
			 *
			 * The structure of the output SFX is essentially:
			 *
			 * [pe executable] + [deflate data] + [sfx object] + [sfx object length]
			 * |-------------|   |------------|   |----------|   |-----------------|
			 *               |                |              |                     |
			 *               |                |              |                     +- used for seeking the start of this sfx
			 *               |                |              |                        object, by reading backwards from EOF
			 *               |                |              |
			 *               |                |              +----------------------- specifies the deflate offsets & output
			 *               |                |                                       file name + path on the filesystem
			 *               |                |
			 *               |                +-------------------------------------- deflate data for each discovered file in
			 *               |                                                        the source directory
			 *               |
			 *               +------------------------------------------------------- contents of this executable; essentially
			 *                                                                        we append the aforementioned data to the
			 *                                                                        end of this executable
			 */

			var sfx   = new SFX();
			var files = source.GetFiles(filter, SearchOption.AllDirectories);

			var sourceExe = configuration.Executable;
			var targetExe = new FileInfo(Combine(target.FullName, sourceExe.Name));

			Info($"Source: {source.FullName}");
			Info($"Target: {target.FullName}");

			Info($"Source EXE: {sourceExe.FullName}");
			Info($"Target SFX: {targetExe.FullName}");

			if (targetExe.Exists)
				targetExe.Delete();

			var sourceSize = files.Sum(x => x.Length);
			var sfxSize    = 0L;

			/**
			 * We create a copy of this executable for subsequent appending of the DEFLATE & SFX data. We refresh the FileInfo
			 * object to ensure that we have the real length of the executable on the fs.
			 */

			sourceExe.CopyTo(targetExe.FullName);
			targetExe.Refresh();

			Info($"Copied {targetExe.Length} bytes to: {targetExe.Length}");

			/**
			 * For each discovered file in the given source directory, we will:
			 *
			 * 1. Append a DEFLATE representation of it to the copied HXE executable;
			 * 2. Create an SFX entry specifying:
			 *    a) the file name & path on the fs
			 *    b) the file length on the fs
			 *    c) offset of the DEFLATE data in SFX
			 */
			using (var oStream = System.IO.File.Open(targetExe.FullName, Append))
			{
				for (var i = 0; i < files.Length; i++)
				{
					var file = files[i];
					Info($"Packaging file: {file.Name}");

					/**
					 * We append the DEFLATE data to the SFX binary. After the procedure is done, we will refresh the FileInfo
					 * for the SFX to retrieve its new length. This length will be used to determine length of the DEFALTE and thus
					 * its offset in the SFX binary.
					 */

					var  length = file.Length;
					long deflateLength;

					{
						using (var iStream = file.OpenRead())
						using (var dStream = new DeflateStream(oStream, Compress, true))
						{
							iStream.CopyTo(dStream);
							dStream.Close();
							oStream.Flush(true);
						}

						WriteLine(NewLine + new string('-', 80));

						var oldLength = targetExe.Length;
						targetExe.Refresh();
						var newLength = targetExe.Length;

						Info($"SFX increased from {oldLength} to {newLength} bytes.");

						deflateLength = newLength - oldLength;

						sfxSize += deflateLength;

						if (deflateLength < length)
							Info($"DEFLATE length is thus {(decimal) deflateLength / length * 100:##.##}% of {length} bytes.");
						else
							Warn($"DEFLATE length is higher by {deflateLength - length} bytes than the raw file length.");
					}

					/**
					 * We will add an entry for the current file and its DEFLATE representation to teach the SFX how to recreate
					 * the file down the line.
					 *
					 * The entries are designed to recreate the structure of the source directory, in a given arbitrary target
					 * directory. As such, we will avoid absoltue paths for the files and instead infer paths relative to the source
					 * directory.
					 *
					 * Each DEFLATE entry will be appended at the end of the SFX binary. To determine where each file's DEFLATE
					 * representation starts, we determine the offset by extracting the DEFLATE length, from the SFX binary
					 * length.
					 */

					{
						var name   = file.Name;
						var offset = targetExe.Length - deflateLength;
						var path = file.DirectoryName != null && file.DirectoryName.Equals(source.FullName)
							? string.Empty
							: file.DirectoryName?.Substring(source.FullName.Length + 1);

						Info($"Acknowledging new entry: {targetExe.Name} <= {path}\\{name}");
						Info($"DEFLATE starts at offset 0x{offset:x8} in the SFX binary.");

						sfx.Entries.Add(new Entry
						{
							Name   = name,
							Path   = path,
							Length = length,
							Offset = offset
						});

						targetExe.Refresh();
					}

					WriteLine(NewLine + new string('-', 80));
					Info($"Finished packaging file: {file.Name}");
					Info($"{files.Length - (i + 1)} files are currently remaining.");
					WriteLine(NewLine + new string('=', 80));
				}

				/**
				 * Once we have created & appended the DEFLATE data for each file, and also populated the SFX object, we will
				 * serialise it to a byte array which in turn gets appended to the SFX binary as well. This makes the SFX
				 * completely self-contained and portable.
				 *
				 * Because an SFX object's array representation is a variable length, we will append the said length to the binary
				 * as well. This will allow HXE to determine both the start and end of the SFX data when deserialising it, by
				 * seeking backwards from the EOF. By seeking as many bytes backwards as specified in the length value, the start
				 * of the SFX data can be determined.
				 */

				{
					var sfxData   = sfx.Serialise();
					var sfxOffset = targetExe.Length;

					Info($"Appending SFX length ({sfxData.Length}) at offset 0x{sfxOffset:x8}.");

					using (var binWriter = new BinaryWriter(oStream))
					{
						binWriter.Write(sfxData);
						binWriter.Write(sfxOffset);

						oStream.Flush(true);
						binWriter.Close();
					}
				}
			}

			{
				WriteLine(NewLine + new string('*', 80));
				Info($"Finished packaging {targetExe.Name} with {files.Length} files.");

				var percentage = (double) sfxSize / sourceSize * 100;

				Info($"Source directory size: {sourceSize} bytes");
				Info($"SFX DEFLATE data size: {sfxSize} bytes");
				Info($"Data has been compressed down to {percentage:##.##}%");
			}
		}

		/**
		 * Extracts the SFX contents of the current HXE executable to the given target.
		 */
		public static void Extract(Configuration configuration)
		{
			var target = configuration.Target;
			
			target.Create();

			/**
			 * We will assume that the current HXE executable contains SFX data to be extracted.
			 */

			var exe = configuration.Executable;
			var sfx = new SFX();

			Info($"Determining SFX structure in {exe.Name}");

			/**
			 * We first hydrate the SFX object with information from the SFX binary. The start of the SFX data is inferred
			 * from the length specified at the EOF. We also need to keep in mind the length of the variable that specifies
			 * the length of the SFX data. Since it's a long, the size would be 8.
			 *
			 * With the above confusion in mind:
			 * - Starting offset of the SFX data = (hxe length - sizeof(long) - sfx data length)
			 * - Ending offset of the SFX data   = (hxe length - sizeof(long))
			 */

			using (var binReader = new BinaryReader(exe.OpenRead()))
			{
				binReader.BaseStream.Seek(exe.Length - sizeof(long), Begin);
				binReader.BaseStream.Seek(binReader.ReadInt64(),     Begin);
				sfx.Deserialise
				(
					binReader.ReadBytes
					(
						(int) exe.Length - (int) binReader.BaseStream.Position - sizeof(long)
					)
				);
			}

			/**
			 * For each entry specified in the SFX object, we seek its DEFLATE data and extract it to the fs at the given path
			 * and filename. Because .NET doesn't support copying a specified amount of bytes from a stream to another, we are
			 * implementing our own method for copying the data using buffers.
			 */

			for (var i = 0; i < sfx.Entries.Count; i++)
			{
				var entry    = sfx.Entries[i];
				var original = new FileInfo(Combine(target.FullName, entry.Path, entry.Name));

				original.Directory?.Create();

				Info($"Found {entry.Length} bytes at 0x{entry.Offset:x8}: {entry.Name}");
				Info($"Extracting its DEFLATE data to: {original.FullName}");

				using (var iStream = exe.OpenRead())
				using (var oStream = original.Create())
				using (var dStream = new DeflateStream(iStream, Decompress))
				{
					dStream.BaseStream.Seek(entry.Offset, Begin);
					var bytes  = entry.Length;
					var buffer = new byte[0x8000];
					int read;
					while (bytes > 0 && (read = dStream.Read(buffer, 0, Math.Min(buffer.Length, (int) bytes))) > 0)
					{
						oStream.Write(buffer, 0, read);
						bytes -= read;
					}
				}

				WriteLine(NewLine + new string('-', 80));
				Info($"Finished extracting file: {entry.Name}");
				Info($"{sfx.Entries.Count - (i + 1)} files are currently remaining.");

				WriteLine(NewLine + new string('=', 80));
			}
		}

		/**
		 * Retrieves a byte array representing the object state which can hydrate an SFX instance using Deserialise().
		 */
		public byte[] Serialise()
		{
			var xml = new XmlSerializer(typeof(SFX));
			using (var sw = new StringWriter())
			{
				xml.Serialize(sw, this);
				return Unicode.GetBytes(sw.ToString());
			}
		}

		/**
		 * Hydrates the SFX instance properties with inbound data previously created by Serialise().
		 */
		public void Deserialise(byte[] data)
		{
			using (var sr = new StringReader(Unicode.GetString(data)))
			{
				var sfx = (SFX) new XmlSerializer(typeof(SFX)).Deserialize(sr);
				Entries = sfx.Entries;
			}
		}

		/**
		 * Entry for a file to be compressed and extracted using the SFX system.
		 */
		public class Entry
		{
			public string Name   { get; set; }                 /* original file name on the filesystem    */
			public string Path   { get; set; } = string.Empty; /* path relative to root source/target dir */
			public long   Offset { get; set; }                 /* offset in the SFX executable            */
			public long   Length { get; set; }                 /* file length on the filesystem           */
		}

		public class Configuration
		{
			public DirectoryInfo Source { get; set; } = new DirectoryInfo(CurrentDirectory);
			public DirectoryInfo Target { get; set; } = new DirectoryInfo(CurrentDirectory).Parent;
			public string        Filter { get; set; } = "*";

			public FileInfo Executable { get; set; } = new FileInfo(GetEntryAssembly()?.Location
			                                                        ?? throw new InvalidOperationException());
		}
	}
}