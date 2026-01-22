using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace DuckSimulatorApp.Services;

public static class AudioService
{
    public static void PlaySound(string fileName)
    {
        try
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Assets", fileName);
            if (!File.Exists(path)) return;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                // macOS: built-in audio player
                StartProcess("afplay", Quote(path));
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Windows fallback (PowerShell)
                var ps = $"-c (New-Object Media.SoundPlayer {Quote(path)}).PlaySync()";
                StartProcess("powershell", ps);
                return;
            }

            // Linux fallback (if available)
            StartProcess("aplay", Quote(path));
        }
        catch
        {
            // no crash if audio fails
        }
    }

    private static void StartProcess(string fileName, string args)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = args,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process.Start(psi);
    }

    private static string Quote(string s) => $"\"{s}\"";
}
