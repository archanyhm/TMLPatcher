﻿using System;
using System.IO;
using System.Reflection;
using Consolation;
using TMLPatcher.Common;
using TMLPatcher.Common.Framework;
using TMLPatcher.Common.Options;

namespace TMLPatcher
{
    public static class Program
    {
        public static string EXEPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public const string Line = "-----------------------------------------------------------------";

        public static ConfigurationFile Configuration;
        public static ConsoleOptions StartOptions;
        public static ConsoleOptions SelectedOptions;

        public static void Main(string[] args)
        {
            ConsoleAPI.Initialize();
            ConsoleAPI.ParseParameters(args);

            InitializeConsoleOptions();
            InitializeProgramOptions();

            WriteStaticText(false);
            //CheckForUndefinedPath();
            SelectedOptions.ListForOption();
        }

        /// <summary>
        /// Writes text that will always show at the beginning, and should persist after clears.
        /// </summary>
        public static void WriteStaticText(bool withMessage)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;

            string[] whatCanThisDoLol =
            {
                "Unpack .tmod files",
                "Repack .tmod files",
                "Decompile stored assembles in .tmod files",
                "Patch and recompile assemblies"
            };

            Console.WriteLine(" Welcome to TMLPatcher!");
            Console.WriteLine(" This is a program that allows you to:");

            for (int i = 0; i < whatCanThisDoLol.Length; i++)
                Console.WriteLine($"  [{i + 1}] {whatCanThisDoLol[i]}");
            Console.WriteLine(Line);
            Console.WriteLine(" Loaded with configuration options:");
            Console.WriteLine($"  {nameof(Configuration.ModsPath)}: {Configuration.ModsPath}");

            if (!withMessage)
                Console.WriteLine();
        }

        public static void CheckForUndefinedPath()
        {
            while (true)
            {
                if (!Configuration.ModsPath.Equals("undefined"))
                    return;

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($" {nameof(Configuration.ModsPath)} is undefined!");
                Console.WriteLine(" Please enter the directory of your tModLoader Mods folder:");
                string modsPath = Console.ReadLine();

                if (Directory.Exists(modsPath))
                {
                    Configuration.ModsPath = modsPath;
                    ConfigurationFile.Save();
                }
                else
                {
                    WriteAndClear("Whoops! The specified path does not exist! Please enter a valid directory.");
                    continue;
                }

                break;
            }
        }

        public static void InitializeConsoleOptions()
        {
            StartOptions = new ConsoleOptions("TEST", null, new TestOption());
            SelectedOptions = StartOptions;
        }

        public static void InitializeProgramOptions() => Configuration = ConfigurationFile.Load(EXEPath + Path.DirectorySeparatorChar + "configuration.json");

        public static void Clear(bool withMessage)
        {
            Console.Clear();
            WriteStaticText(withMessage);
        }

        public static void WriteAndClear(string message, ConsoleColor color = ConsoleColor.Red)
        {
            Clear(true);
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
