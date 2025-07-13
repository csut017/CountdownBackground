using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace CountdownBackground;

internal static class WallpaperUtility
{
    private static readonly uint SPI_SETDESKWALLPAPER = 0x14;

    private static readonly uint SPIF_SENDWININICHANGE = 0x02;

    private static readonly uint SPIF_UPDATEINIFILE = 0x01;

    public static void SetWallpaper(string path)
    {
#pragma warning disable CA1416
        var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
        if (key != null)
        {
            key.SetValue(@"WallpaperStyle", 2.ToString());
            key.SetValue(@"TileWallpaper", 0.ToString());
        }
        else
        {
            throw new Exception("Could not open registry key");
        }
#pragma warning restore CA1416

        var result = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        if (result != 1)
        {
            throw new Exception("SystemParametersInfo failed");
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);
}