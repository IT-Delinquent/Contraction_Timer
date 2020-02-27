using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Contraction_Timer.Helpers
{
    /// <summary>
    /// Class for interacting with the file system
    /// </summary>
    public static class IOHelpers
    {
        /// <summary>
        /// The file extension for the contraction files
        /// </summary>
        private const string _fileExtension = "*.contraction.txt";

        /// <summary>
        /// The folder path to save contractions to
        /// </summary>
        public static readonly string FolderPathLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

        /// <summary>
        /// Finds all the contraction files
        /// </summary>
        /// <returns>A list of file location strings</returns>
        public static List<string> EnumeratAllFiles()
        {
            return Directory.EnumerateFiles(App.FolderPath, '*' + _fileExtension).ToList();
        }

        /// <summary>
        /// Reads all the text inside a file
        /// </summary>
        /// <param name="file">The string location of the file</param>
        /// <returns>A string of all the file text</returns>
        public static string ReadAllFileText(string file)
        {
            return File.ReadAllText(file);
        }

        /// <summary>
        /// Created a unique file name + location
        /// </summary>
        /// <returns>A unique file name and location</returns>
        public static string GetUniqueFileName()
        {
            return Path.Combine(App.FolderPath, Path.GetRandomFileName() + _fileExtension);
        }

        /// <summary>
        /// Save the data to a file
        /// </summary>
        /// <param name="file">The file to save the data to</param>
        /// <param name="data">The data to save</param>
        public static void SaveData(string file, string data)
        {
            File.WriteAllText(file, data);
        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="file">The file location string</param>
        public static void DeleteFile(string file)
        {
            File.Delete(file);
        }

        /// <summary>
        /// Checks if a file exists
        /// </summary>
        /// <param name="file">The file location string to check</param>
        /// <returns>True if the file is found, false if not found</returns>
        public static bool FileExists(string file)
        {
            return File.Exists(file);
        }

        /// <summary>
        /// Deletes all the contractions
        /// </summary>
        public static void DeleteAllNotes()
        {
            foreach(string i in EnumeratAllFiles())
            {
                if (FileExists(i))
                {
                    File.Delete(i);
                }
            }
        }
    }
}