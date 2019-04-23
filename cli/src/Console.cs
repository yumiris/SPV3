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
using static System.Console;

namespace SPV3.CLI
{
  public static class Console
  {
    public static void Error(string value)
    {
      var decoration = new string('*', value.Length);

      Output("!!!!", ConsoleColor.Red, decoration);
      Output("!!!!", ConsoleColor.Red, value);
      Output("!!!!", ConsoleColor.Red, decoration);
    }

    public static void Debug(string value)
    {
      Output("DBUG", ConsoleColor.Green, value, ConsoleColor.White);
    }

    public static void Info(string value)
    {
      Output("INFO", ConsoleColor.Cyan, value, ConsoleColor.White);
    }

    public static void Logs(string value)
    {
      Output("LOGS", ConsoleColor.Magenta, value);
    }

    public static void Warn(string value)
    {
      var decoration = new string('-', value.Length);

      Output("WARN", ConsoleColor.Yellow, decoration);
      Output("WARN", ConsoleColor.Yellow, value);
      Output("WARN", ConsoleColor.Yellow, decoration);
    }

    private static void Output(string prefix, ConsoleColor color, string message)
    {
      Output(prefix, color, message, color);
    }

    private static void Output(string prefix, ConsoleColor color, string message, ConsoleColor messageColor)
    {
      ForegroundColor = ConsoleColor.Gray;
      Write("> [ ");

      ForegroundColor = color;
      Write(prefix);

      ForegroundColor = ConsoleColor.Gray;
      Write(" ] - ");

      ForegroundColor = messageColor;
      WriteLine(message);
    }
  }
}