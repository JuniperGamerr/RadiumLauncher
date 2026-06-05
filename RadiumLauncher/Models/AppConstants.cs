using System;
using System.IO;

namespace RadiumLauncher.Models;

public static class AppConstants
{
    public static string AppDataDirectory
    {
        get
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "RadiumLauncher");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}