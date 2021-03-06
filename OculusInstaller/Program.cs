﻿/*
*
* Made by github.com/Arc0re
*
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace OculusInstaller
{
    class Program
    {
        static string RuntimeInstaller = "oculus_runtime_sdk_0.7.0.win.exe";
        static string URLRuntimeInstaller = "https://static.oculus.com/sdk-downloads/0.7.0.0/Public/1440610361/oculus_runtime_sdk_0.7.0.0_win.exe";

        /// <summary>
        /// Prints a line of dashes in the current stdout.
        /// </summary>
        static void PutLine()
        {
            Console.WriteLine(@"-------------------------------------------------------------------");
        }

        /// <summary>
        /// Parses then prints a list of the tested games from the "games.txt" file.
        /// If its not present, exits with -1.
        /// </summary>
        static void ListGames()
        {
            Console.WriteLine("List of working games and demos:");

            string listPath = Path.GetFullPath("games.txt");

            if (!File.Exists("games.txt"))
            {
                Console.WriteLine("Please add the file'games.txt' in the application directory.");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            else
            {
                string[] lines = File.ReadAllLines(listPath);

                foreach (string line in lines)
                {
                    Console.WriteLine("\t" + line);
                }

                Console.WriteLine("Press a key to exit.");
            }
        }

        /// <summary>
        /// Gets the Oculus Rift 0.7.0 Runtime from the official server.
        /// </summary>
        /// <returns>Returns a boolean indicating success or failure.</returns>
        static bool DownloadRuntime()
        {
            Console.WriteLine("It will now proceed to the download and the installation.");
            Console.WriteLine("It should only take a minute, the installer is 40~ mb in total.");

            if (!File.Exists(RuntimeInstaller))
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(URLRuntimeInstaller, RuntimeInstaller);
                }

                if (!File.Exists(RuntimeInstaller))
                {
                    Console.WriteLine("The download failed.");
                    Console.ReadKey();
                    Environment.Exit(-1);
                    return false;
                }
                else
                {
                    Console.WriteLine("File successfully downloaded.");
                    Console.WriteLine("Press a key to continue.");
                    return true;
                }
            }
            else
            {
                PutLine();
                Console.WriteLine("The file is already present in the folder. We will now install the runtime.");
                return true;
            }
        }

        /// <summary>
        /// Launches the downloaded runtime setup.
        /// Throws a Win32Exception in case it fails.
        /// </summary>
        static void InstallRuntime()
        {
            try
            {
                Process.Start(RuntimeInstaller);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                Console.WriteLine("Exception: " + e);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Oculus Rift SDK/Runtime 0.7.0 for Windows 8.1+ Installer");

            PutLine();

            Console.WriteLine(@"This utility will download and install everything necessary to run Oculus Rift DK2 games and demos.
Use the '--list' command line argument to list the compatible (tested) games/demos.");

            PutLine();

            if (args.Length == 0)
            {
                if (DownloadRuntime())
                {
                    PutLine();
                    Console.WriteLine("Installing runtime...");
                    InstallRuntime();

                    PutLine();
                    Console.WriteLine("Follow the setup instructions and you'll be able to use the Oculus Rift!");
                }
                else
                {
                    Console.WriteLine("Something went wrong with the download. Please start again.");
                }
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string cargs = args[i];
                    if (cargs == "--list")
                    {
                        ListGames();
                    }
                    else
                    {
                        PutLine();
                        Console.WriteLine(@"Available command line arguments:
'--list': Lists the current available games from the 'games.txt' file.");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                }
            }

            // Final keyboard input that ends the program.
            Console.ReadKey();
        }
    }
}
