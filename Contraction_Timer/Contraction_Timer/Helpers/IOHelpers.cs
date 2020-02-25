using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contraction_Timer.Helpers
{
    public static class IOHelpers
    {
        private const string _fileExtension = "*.contraction.txt";
        public static readonly string FolderPathLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

        public static List<string> EnumeratAllFiles()
        {
            return Directory.EnumerateFiles(App.FolderPath, '*' + _fileExtension).ToList();
        }

        public static string ReadAllFileText(string file)
        {
            return File.ReadAllText(file);
        }

        public static string GetUniqueFileName()
        {
            return Path.Combine(App.FolderPath, Path.GetRandomFileName() + _fileExtension);
        }

        public static void SaveData(string file, string data)
        {
            File.WriteAllText(file, data);
        }

        public static void DeleteFile(string file)
        {
            File.Delete(file);
        }

        public static bool FileExists(string file)
        {
            return File.Exists(file);
        }
    }
}
