using System.Drawing;
using System.Drawing.Imaging;
using Humanizer;
using Humanizer.Localisation;

namespace CountdownBackground;

internal class Program
{
    private static string GenerateNewWallpaper(string imagePath, TimeSpan daysRemaining)
    {
        var folder = Path.GetDirectoryName(imagePath);
        if (string.IsNullOrEmpty(folder)) throw new Exception("path is not rooted");
        var extension = Path.GetExtension(imagePath);

        var text = "T-" + daysRemaining.Humanize(maxUnit: TimeUnit.Day);
        var newPath = Path.Combine(folder, $"{text}{extension}");
#pragma warning disable CA1416
        using var original = new Bitmap(imagePath);
        using var image = new Bitmap(original.Width, original.Height);
        using var graphics = Graphics.FromImage(image);
        graphics.DrawImage(original, 0, 0, original.Width, original.Height);
        using var font = new Font("Roboto", 120);
        graphics.DrawString(text, font, new SolidBrush(Color.Black), 710, 20);
        graphics.DrawString(text, font, new SolidBrush(Color.White), 700, 10);
        graphics.Flush();

        image.Save(newPath, ImageFormat.Png);
#pragma warning restore CA1416
        return newPath;
    }

    static void Main(string[] args)
    {
        const string imagePath = @"C:\Users\csut017\OneDrive - The University of Auckland\Pictures\Live-Fearlessly-desktop-wallpaper-en.png";
        var targetDate = new DateTime(2025, 7, 21);
        var daysRemaining = targetDate - DateTime.Today;
        Console.WriteLine("Generating wallpaper");
        var newPath = GenerateNewWallpaper(imagePath, daysRemaining);

        Console.WriteLine("Setting wallpaper");
        WallpaperUtility.SetWallpaper(newPath);
        Console.WriteLine("Wallpaper update completed");
    }
}