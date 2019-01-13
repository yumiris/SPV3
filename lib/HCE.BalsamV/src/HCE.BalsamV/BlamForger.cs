/**
 * Copyright (C) 2018-2019 Emilian Roman
 * 
 * This file is part of HCE.HCE.BalsamV.
 * 
 * HCE.HCE.BalsamV is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * HCE.HCE.BalsamV is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with HCE.HCE.BalsamV.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace HCE.BalsamV
{
    /// <summary>
    ///     Hash forger for a HCE configuration file (blam.sav).
    /// </summary>
    public class BlamForger
    {
        /// <summary>
        ///     Forges the inbound stream's checksum.
        /// </summary>
        /// <param name="stream">
        ///     Stream representing a blam.sav file.
        /// </param>
        public void Forge(Stream stream)
        {
            var amount = Blam.BlamLength - Blam.BlamHashLength;
            var buffer = new byte[amount];

            stream.Position = 0;
            stream.Read(buffer, 0, amount);

            var crc32 = new Crc32().ComputeHash(buffer);
            var forge = new Func<byte[], byte[]>(x =>
            {
                var y = new byte[x.Length];

                for (var i = 0; i < x.Length; i++)
                    y[i] = (byte) ~ x[i];

                return y;
            })(crc32);

            Array.Reverse(forge);

            var writer = new BinaryWriter(stream);
            writer.BaseStream.Seek(Blam.BlamHashOffset, SeekOrigin.Begin);
            writer.Write(forge);
        }

        // Copyright (c) Damien Guard.  All rights reserved.
        // Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
        // You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
        /// <summary>
        ///     Implements a 32-bit CRC hash algorithm compatible with Zip etc.
        /// </summary>
        /// <remarks>
        ///     Crc32 should only be used for backward compatibility with older file formats
        ///     and algorithms. It is not secure enough for new applications.
        ///     If you need to call multiple times for the same data either use the HashAlgorithm
        ///     interface or remember that the result of one Compute call needs to be ~ (XOR) before
        ///     being passed in as the seed for the next Compute call.
        /// </remarks>
        private sealed class Crc32 : HashAlgorithm
        {
            public const uint DefaultPolynomial = 0xedb88320u;
            public const uint DefaultSeed = 0xffffffffu;

            private static uint[] _defaultTable;

            private readonly uint _seed;
            private readonly uint[] _table;
            private uint _hash;

            public Crc32()
                : this(DefaultPolynomial, DefaultSeed)
            {
            }

            public Crc32(uint polynomial, uint seed)
            {
                if (!BitConverter.IsLittleEndian)
                    throw new PlatformNotSupportedException("Not supported on Big Endian processors");

                _table = InitializeTable(polynomial);
                _seed = _hash = seed;
            }

            public override int HashSize => 32;

            public override void Initialize()
            {
                _hash = _seed;
            }

            protected override void HashCore(byte[] array, int ibStart, int cbSize)
            {
                _hash = CalculateHash(_table, _hash, array, ibStart, cbSize);
            }

            protected override byte[] HashFinal()
            {
                var hashBuffer = UInt32ToBigEndianBytes(~_hash);
                HashValue = hashBuffer;
                return hashBuffer;
            }

            public static uint Compute(byte[] buffer)
            {
                return Compute(DefaultSeed, buffer);
            }

            public static uint Compute(uint seed, byte[] buffer)
            {
                return Compute(DefaultPolynomial, seed, buffer);
            }

            public static uint Compute(uint polynomial, uint seed, byte[] buffer)
            {
                return ~CalculateHash(InitializeTable(polynomial), seed, buffer, 0, buffer.Length);
            }

            private static uint[] InitializeTable(uint polynomial)
            {
                if (polynomial == DefaultPolynomial && _defaultTable != null)
                    return _defaultTable;

                var createTable = new uint[256];
                for (var i = 0; i < 256; i++)
                {
                    var entry = (uint) i;
                    for (var j = 0; j < 8; j++)
                        if ((entry & 1) == 1)
                            entry = (entry >> 1) ^ polynomial;
                        else
                            entry = entry >> 1;
                    createTable[i] = entry;
                }

                if (polynomial == DefaultPolynomial)
                    _defaultTable = createTable;

                return createTable;
            }

            private static uint CalculateHash(uint[] table, uint seed, IList<byte> buffer, int start, int size)
            {
                var hash = seed;
                for (var i = start; i < start + size; i++)
                    hash = (hash >> 8) ^ table[buffer[i] ^ (hash & 0xff)];
                return hash;
            }

            private static byte[] UInt32ToBigEndianBytes(uint uint32)
            {
                var result = BitConverter.GetBytes(uint32);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(result);

                return result;
            }
        }
    }
}