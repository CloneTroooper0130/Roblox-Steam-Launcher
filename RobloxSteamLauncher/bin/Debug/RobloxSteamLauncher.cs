using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RobloxSteamLauncher
{
    internal static class Program
    {
        // OPTIONAL:
        // Replace this with a specific placeId if you want
        // Example: 606849621 (Jailbreak)
        private const long PlaceId = 0;

        static void Main(string[] args)
        {
            try
            {
                EnsureSteamIsRunning();
                LaunchRoblox();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Launcher error:");
                Console.Error.WriteLine(ex.Message);
            }
        }

        private static void EnsureSteamIsRunning()
        {
            if (Process.GetProcessesByName("steam").Length > 0)
                return;

            string steamPath = GetSteamPath();
            if (string.IsNullOrWhiteSpace(steamPath))
                throw new InvalidOperationException("Steam installation not found.");

            Process.Start(new ProcessStartInfo
            {
                FileName = steamPath,
                UseShellExecute = true
            });

            // Give Steam a moment to initialize
            System.Threading.Thread.Sleep(4000);
        }

        private static void LaunchRoblox()
        {
            string robloxUri;

            if (PlaceId > 0)
            {
                robloxUri = $"roblox-player:launch?placeId={PlaceId}";
            }
            else
            {
                // Opens Roblox Home if no placeId is specified
                robloxUri = "roblox-player:";
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = robloxUri,
                UseShellExecute = true
            });
        }

        private static string GetSteamPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                        @"SOFTWARE\WOW6432Node\Valve\Steam"
                    );
                    return key?.GetValue("InstallPath") as string + "\\steam.exe";
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }
    }
}
