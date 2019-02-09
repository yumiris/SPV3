/**
 * Copyright (C) 2018-2019 Emilian Roman
 * 
 * This file is part of SPV3.Bbkpify.
 * 
 * SPV3.Bbkpify is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * SPV3.Bbkpify is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with SPV3.Bbkpify.  If not, see <http://www.gnu.org/licenses/>.
 */

﻿namespace SPV3.Bbkpify
{
    public static class Ascii
    {
        public static string Banner => @"
    __    __    __         _ ____     
   / /_  / /_  / /______  (_) __/_  __
  / __ \/ __ \/ //_/ __ \/ / /_/ / / /
 / /_/ / /_/ / ,< / /_/ / / __/ /_/ / 
/_.___/_.___/_/|_/ .___/_/_/  \__, /  
                /_/          /____/   
======================================
                           // Yu:YuMi
";

        public static string Progress(int index, int arrayLength)
        {
            return $"[{index + 1}/{arrayLength}]";
        }
    }
}