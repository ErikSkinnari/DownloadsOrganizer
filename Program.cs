using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Syroot.Windows.IO;

namespace DirectoryOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the organizer...");
            DateTime startTime = DateTime.Now;

            string folderToOrganize = String.Empty;

            string sourceDirectory = String.Empty;
            Configuration config;

            if (args.Length > 0)
            {
                try
                {
                    string configPath = args[0];
                    using (StreamReader reader = new StreamReader(configPath))
                    {
                        var json = reader.ReadToEnd();
                        config = JsonConvert.DeserializeObject<Configuration>(json);
                    }
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Given configuration file is not valid. Closing Application");
                    Environment.Exit(-1);
                }
            }
            else 
            {
                Console.WriteLine("To use this application a configuration file needs to be provided.");
            }

            // Dictionary<string, string> foldersToCreate = new Dictionary<string, string>
            // {
            //     { "ExecutablesPath", Path.Combine(folderToOrganize, "Executables") },
            //     { "EbooksPath", Path.Combine(folderToOrganize, "E-books") },
            //     { "SoundsPath", Path.Combine(folderToOrganize, "Sounds") },
            //     { "DocumentsPath", Path.Combine(folderToOrganize, "Documents") },
            //     { "ImagesPath", Path.Combine(folderToOrganize, "Images") },
            //     { "CompressedPath", Path.Combine(folderToOrganize, "Compressed") },
            //     { "CodePath", Path.Combine(folderToOrganize, "Code") },
            //     { "TextPath", Path.Combine(folderToOrganize, "TextFiles") },
            //     { "Misc", Path.Combine(folderToOrganize, "Misc") }
            // };

            string[] exeExtentions = new string[] { "exe", "jar", "msi" };
            string[] ebookExtentions = new string[] { "epub", "mobi" };
            string[] soundExtentions = new string[] { "wav", "mp3", "ogg" };
            string[] documentExtentions = new string[] { "doc", "docx", "pdf" };
            string[] imageExtentions = new string[] { "svg", "img", "png" };
            string[] archiveExtentions = new string[] { "rar", "zip" };
            string[] codeExtentions = new string[] { "js", "cs", "html", "css", "py" };
            string[] textExtentions = new string[] { "txt", "csv" };

            foreach (KeyValuePair<string, string> folderName in foldersToCreate)
            {
                if (!Directory.Exists(folderName.Value))
                {
                    Directory.CreateDirectory(folderName.Value);
                }
            }

            var files = Directory.EnumerateFiles(folderToOrganize);

            foreach (string file in files)
            {
                string filename = file[(folderToOrganize.Length + 1)..];
                string fileExtention = file[(file.LastIndexOf('.') + 1)..];
                string filePath = Path.Combine(folderToOrganize, filename);

                string targetDirectory;
                if (exeExtentions.Any(fileExtention.Contains)) targetDirectory = foldersToCreate["ExecutablesPath"];
                else if (ebookExtentions.Any(fileExtention.Contains)) targetDirectory = foldersToCreate["EbooksPath"];
                else if (soundExtentions.Any(fileExtention.Contains)) targetDirectory = foldersToCreate["SoundsPath"];
                else if (documentExtentions.Any(fileExtention.Contains)) targetDirectory = foldersToCreate["DocumentsPath"];
                else if (imageExtentions.Any(fileExtention.Contains)) targetDirectory = foldersToCreate["ImagesPath"];
                else if (archiveExtentions.Any(fileExtention.Contains)) targetDirectory = foldersToCreate["CompressedPath"];
                else if (codeExtentions.Any(fileExtention.Contains)) targetDirectory = foldersToCreate["CodePath"];
                else if (textExtentions.Any(fileExtention.Contains)) targetDirectory = foldersToCreate["TextPath"];
                else targetDirectory = foldersToCreate["Misc"];

                string targetPath = Path.Combine(targetDirectory, filename);
                File.Move(filePath, targetPath);
            }

            foreach (var folder in Directory.EnumerateDirectories(folderToOrganize))
            {
                if (Directory.EnumerateDirectories(folder).Count() < 1 && Directory.EnumerateFiles(folder).Count() < 1) 
                {
                    Directory.Delete(Path.Combine(folderToOrganize, folder));
                }
            }

            TimeSpan elapsedTime = DateTime.Now - startTime;

            Console.WriteLine($"The organizer has finished the cleaning in {elapsedTime.Seconds} seconds..");
        }
    }
}
