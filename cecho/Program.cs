using System;
using System.Collections.Generic;
using System.Text;

namespace il.co.sherer.cecho
{
    internal class Program
    {
        static ConsoleColor[] AnsiTable = new[] { 
            ConsoleColor.Black, ConsoleColor.DarkRed, ConsoleColor.DarkGreen, ConsoleColor.DarkYellow,
            ConsoleColor.DarkBlue, ConsoleColor.DarkMagenta, ConsoleColor.DarkCyan, ConsoleColor.DarkGray,
            ConsoleColor.Gray, ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Yellow,
            ConsoleColor.Blue, ConsoleColor.Magenta, ConsoleColor.Cyan, ConsoleColor.White};

        internal static void ShowUsage()
        {
#if (!PACKED)
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("cecho v1.0 by elisherer");
            Console.ResetColor();
            Console.WriteLine("Displays messages, with colors by using escape codes.\n");
            Console.WriteLine("    CECHO \"This is a demo:\\n \\07Normal\\m & \\09Blue\\m and A=\\u0041\"\n");
            Console.WriteLine("Escape Codes:");
            Console.WriteLine("    \\## - # is an hex digit [0-F] representing a color, 1st is the backcolor");
            Console.WriteLine("    \\m - Reset color (not NULL)");
            Console.WriteLine("    \\n - Line-feed    \\r - Carriage-return    \\t - (Horizontal-) Tab");
            Console.WriteLine("    \\u#### - A unicode character. #### - the hexadecimal value.");
            Console.WriteLine("    \\U######## - A UTF32 character. ######## - the hexadecimal value.");
            Console.WriteLine("    \\x1b[##m - ANSI color syntax - ## as specified in the table below");
            Console.WriteLine("    \\x1b[0m - Reset color");
            Console.WriteLine("    \\\" - The character \"    \\\\ - The character \\");
            Console.WriteLine("Color Table:  Regular                                ANSI*");
            for (var i = 0; i < 8; i++)
                Console.WriteLine("    {0,-13}{1}    {2,-13}{3}  |  {4,-13}{5}    {6,-13}{7};1", 
                    ((ConsoleColor)i).ToString(), i, ((ConsoleColor)(i + 8)).ToString(), (i + 8).ToString("X"), 
                    AnsiTable[i].ToString(), 30 + i, AnsiTable[i+8].ToString(), 30 + i);
            Console.WriteLine("* These values are used for foreground, for background add 10 to the 1st num.");
            Console.WriteLine("**The ^ sign escapes to insert other letters like & when not using parentheses.");
#endif
            //Environment.ExitCode = -1;
        }

        internal static bool IsHex(string s, int index)
        {
            return Char.IsDigit(s, index) || (s[index] >= 'A' && s[index] <= 'F') || (s[index] >= 'a' && s[index] <= 'f');
        }

        internal static int HexVal(int c)
        {   
            return (c > '9') ? (c + 10 - (c > 'Z' ? 'a' : 'A')) : (c - '0');
        }

        internal static void Main(string[] args)
        {
            Environment.ExitCode = 0;
            if (args.Length == 0 || args[0].StartsWith("/"))
            {
                ShowUsage();
                return;
            }
            var line = string.Join(" ", args);
            var lastStop = 0;
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] == '\\' && i != line.Length - 1) //escaped
                {
                    Console.Write(line.Substring(lastStop, i - lastStop));
                    
                    if (line[i + 1] == 't')
                    {
                        Console.Write((char)9);
                        i++;
                    }
                    else if (line[i + 1] == 'n')
                    {
                        Console.Write((char)10);
                        i++;
                    }
                    else if (line[i + 1] == 'r')
                    {
                        Console.Write((char)13);
                        i++;
                    }
                    else if (line[i + 1] == '"')
                    {
                        Console.Write('"');
                        i++;
                    }
                    else if (line[i + 1] == '\\')
                    {
                        Console.Write('\\');
                        i++;
                    }
                    else if (line[i + 1] == 'm')
                    {
                        Console.ResetColor();
                        i++;
                    }
                    else if (IsHex(line, i + 1) && i + 2 < line.Length && IsHex(line, i + 2))
                    {
                        Console.BackgroundColor = (ConsoleColor)HexVal(line[i + 1]);
                        Console.ForegroundColor = (ConsoleColor)HexVal(line[i + 2]);
                        i += 2;
                    }
#if (!PACKED)
                    else if (line[i + 1] == 'u' && i + 5 < line.Length && IsHex(line, i + 2) && IsHex(line, i + 3) && IsHex(line, i + 4) && IsHex(line, i + 5))
                    {
                        var unicode = Convert.ToInt32(line.Substring(i + 2, 4), 16);
                        Console.Write(Char.ConvertFromUtf32(unicode));
                        i += 5;
                    }
                    else if (line[i + 1] == 'U' && i + 9 < line.Length && //surrogate pair
                        IsHex(line, i + 2) && IsHex(line, i + 3) && IsHex(line, i + 4) && IsHex(line, i + 5) &&
                        IsHex(line, i + 6) && IsHex(line, i + 7) && IsHex(line, i + 8) && IsHex(line, i + 9))
                    {
                        var p0 = Convert.ToInt32(line.Substring(i + 2, 4), 16);
                        var p1 = Convert.ToInt32(line.Substring(i + 6, 4), 16);
                        Console.Write(Char.ConvertFromUtf32(p0) + Char.ConvertFromUtf32(p1));
                        i += 9;
                    }
#endif
                    else if (i + 6 < line.Length && line.Substring(i+1).StartsWith("x1b["))
                    {
                        if (line[i + 6] == 'm' && line[i + 5] == '0') //reset
                        {
                            Console.ResetColor();
                            i += 6;
                        }
                        else if (i + 7 < line.Length && Char.IsDigit(line, i + 5) && Char.IsDigit(line, i + 6))
                        {
                            var num = (line[i + 5] - '0') * 10 + (line[i + 6] - '0');
                            if ((num > 29 && num < 38) || (num > 39 && num < 48))
                            {
                                if (i + 9 < line.Length && line[i + 7] == ';' && line[i + 8] == '1' && line[i + 9] == 'm')
                                {
                                    if (num > 39) //background
                                    {
                                        Console.BackgroundColor = AnsiTable[num - 30];
                                    }
                                    else
                                        Console.ForegroundColor = AnsiTable[num - 20];
                                    i += 9;
                                }
                                else if (line[i + 7] == 'm')
                                {
                                    if (num > 39) //background
                                    {
                                        Console.BackgroundColor = AnsiTable[num - 40];
                                    }
                                    else
                                        Console.ForegroundColor = AnsiTable[num - 30];
                                    i += 7;
                                }
                            }
                        }
                    }
                    lastStop = i + 1;
                }
            }
            if (lastStop < line.Length)
                Console.Write(line.Substring(lastStop, line.Length - lastStop));
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
