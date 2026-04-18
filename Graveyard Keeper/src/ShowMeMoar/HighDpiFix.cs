using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ShowMeMoar;

// Applies (and removes) the Windows high-DPI compatibility fix for Graveyard Keeper.exe.
// Two layers, both best-effort — succeeds if either one lands:
//   1. HKCU AppCompatFlags\Layers entry ("~ HIGHDPIAWARE") — no admin needed
//   2. Sidecar Graveyard Keeper.exe.manifest — only effective if the game has no embedded manifest
internal static class HighDpiFix
{
    private const string LayersKeyPath = @"Software\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers";
    private const string CompatFlag    = "~ HIGHDPIAWARE";
    private const string GameExeName   = "Graveyard Keeper.exe";

    private const string ManifestXml =
        "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n" +
        "<assembly xmlns=\"urn:schemas-microsoft-com:asm.v1\" manifestVersion=\"1.0\">\r\n" +
        "  <application xmlns=\"urn:schemas-microsoft-com:asm.v3\">\r\n" +
        "    <windowsSettings>\r\n" +
        "      <dpiAware xmlns=\"http://schemas.microsoft.com/SMI/2005/WindowsSettings\">True/PM</dpiAware>\r\n" +
        "      <dpiAwareness xmlns=\"http://schemas.microsoft.com/SMI/2016/WindowsSettings\">PerMonitorV2</dpiAwareness>\r\n" +
        "    </windowsSettings>\r\n" +
        "  </application>\r\n" +
        "</assembly>\r\n";

    [DllImport("user32.dll")]
    private static extern uint GetDpiForSystem();

    // Vista-era fallback if GetDpiForSystem isn't available (pre-Win10 1607).
    [DllImport("gdi32.dll")]
    private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    // Wine/Proton exports this function via its fake ntdll.dll. Real Windows does not.
    [DllImport("ntdll.dll", EntryPoint = "wine_get_version")]
    private static extern IntPtr wine_get_version();

    private const int LOGPIXELSX = 88;

    internal enum Host
    {
        Windows,          // real Windows — fix applies
        WineProton,       // Windows build running under Wine/Proton — fix is no-op
        NativeNonWindows  // native Linux or macOS Unity build — fix is no-op
    }

    internal static Host DetectHost()
    {
        if (Application.platform != RuntimePlatform.WindowsPlayer)
        {
            return Host.NativeNonWindows;
        }
        try
        {
            wine_get_version();
            return Host.WineProton;
        }
        catch
        {
            // DllNotFoundException or EntryPointNotFoundException → real Windows.
            return Host.Windows;
        }
    }

    internal static int DetectDpi()
    {
        try
        {
            return (int) GetDpiForSystem();
        }
        catch
        {
            // Pre-1607 Windows — fall back to the desktop DC's pixel density.
            try
            {
                var hdc = GetDC(IntPtr.Zero);
                if (hdc == IntPtr.Zero) return 96;
                try { return GetDeviceCaps(hdc, LOGPIXELSX); }
                finally { ReleaseDC(IntPtr.Zero, hdc); }
            }
            catch
            {
                return 96;
            }
        }
    }

    internal static int DpiToScalingPercent(int dpi) => (int) Math.Round(dpi * 100.0 / 96.0);

    internal static string GameExePath()
    {
        try
        {
            var dir = Directory.GetCurrentDirectory();
            var exe = Path.Combine(dir, GameExeName);
            return File.Exists(exe) ? exe : null;
        }
        catch
        {
            return null;
        }
    }

    internal static string ManifestPath()
    {
        var exe = GameExePath();
        return exe == null ? null : exe + ".manifest";
    }

    internal static bool IsRegistryFlagSet()
    {
        var exe = GameExePath();
        if (exe == null) return false;
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(LayersKeyPath);
            var v = key?.GetValue(exe) as string;
            return !string.IsNullOrEmpty(v) && v.IndexOf("HIGHDPIAWARE", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[HighDpiFix] Registry read failed: {ex.Message}");
            return false;
        }
    }

    internal static bool IsManifestPresent()
    {
        var path = ManifestPath();
        return path != null && File.Exists(path);
    }

    private static bool WriteRegistryFlag()
    {
        var exe = GameExePath();
        if (exe == null)
        {
            Plugin.Log.LogWarning($"[HighDpiFix] Game exe not found; skipping registry write.");
            return false;
        }
        try
        {
            using var key = Registry.CurrentUser.CreateSubKey(LayersKeyPath);
            if (key == null) return false;
            key.SetValue(exe, CompatFlag, RegistryValueKind.String);
            Plugin.Log.LogInfo($"[HighDpiFix] Wrote '{CompatFlag}' to HKCU Layers for {exe}");
            return true;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[HighDpiFix] Registry write failed: {ex.Message}");
            return false;
        }
    }

    private static bool RemoveRegistryFlagInternal()
    {
        var exe = GameExePath();
        if (exe == null) return false;
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(LayersKeyPath, true);
            if (key == null) return true; // nothing to remove
            if (key.GetValue(exe) == null) return true;
            key.DeleteValue(exe, false);
            Plugin.Log.LogInfo($"[HighDpiFix] Removed HKCU Layers entry for {exe}");
            return true;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[HighDpiFix] Registry remove failed: {ex.Message}");
            return false;
        }
    }

    private static bool WriteManifest()
    {
        var path = ManifestPath();
        if (path == null)
        {
            Plugin.Log.LogWarning($"[HighDpiFix] Game exe not found; skipping manifest write.");
            return false;
        }
        try
        {
            File.WriteAllText(path, ManifestXml);
            Plugin.Log.LogInfo($"[HighDpiFix] Wrote sidecar manifest to {path}");
            return true;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[HighDpiFix] Manifest write failed ({ex.GetType().Name}: {ex.Message}). Registry flag alone will still apply if it succeeded.");
            return false;
        }
    }

    private static bool RemoveManifestInternal()
    {
        var path = ManifestPath();
        if (path == null) return false;
        try
        {
            if (!File.Exists(path)) return true;
            File.Delete(path);
            Plugin.Log.LogInfo($"[HighDpiFix] Removed sidecar manifest at {path}");
            return true;
        }
        catch (Exception ex)
        {
            Plugin.Log.LogWarning($"[HighDpiFix] Manifest remove failed: {ex.Message}");
            return false;
        }
    }

    internal readonly struct ApplyResult
    {
        public readonly bool Registry;
        public readonly bool Manifest;
        public ApplyResult(bool registry, bool manifest) { Registry = registry; Manifest = manifest; }
        public bool AnySuccess => Registry || Manifest;
        public bool FullSuccess => Registry && Manifest;
    }

    internal static ApplyResult Apply() => new(WriteRegistryFlag(), WriteManifest());
    internal static ApplyResult Remove() => new(RemoveRegistryFlagInternal(), RemoveManifestInternal());
}
