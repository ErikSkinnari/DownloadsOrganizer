using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
            Configuration config = null;

            if (args.Length > 0)
            {
                try
                {
                    string configPath = args[0];
                    Console.WriteLine(configPath);
                    using StreamReader reader = new(configPath);
                    var json = reader.ReadToEnd();
                    Console.WriteLine(json);

                    config = JsonSerializer.Deserialize<Configuration>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    Console.WriteLine("Source Directory: " + config.SourceDirectory);
                    Console.WriteLine("Target Directories:");

                    config.TargetDirectories.ForEach(td =>
                    {
                        string fileExtensions = String.Empty;
                        td.FileExtensions.ForEach(fe => fileExtensions += fe + ", ");
                        fileExtensions = fileExtensions.Length > 0 ? fileExtensions[..^2] : " ";

                        Console.WriteLine("\t\t" + td.FolderName + ": (" + fileExtensions + ")");
                    });

                    Console.WriteLine(config.SourceDirectory);
                    if (String.IsNullOrEmpty(config.SourceDirectory))
                    {
                        KnownFolder downloadsFolderPath = new(KnownFolderType.Downloads);
                        sourceDirectory = downloadsFolderPath.Path;
                        Console.WriteLine("Source path not provided. Automaticly set to: " + sourceDirectory);
                    }
                    else
                    {
                        sourceDirectory = config.SourceDirectory;
                        Console.WriteLine("Source dir set to: " + sourceDirectory);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Something went wrong. Maybe the given configuration file is not valid. Closing Application..");
                    Environment.Exit(-1);
                }
            }
            else 
            {
                Console.WriteLine("To use this application a configuration file needs to be provided.");
                Environment.Exit(-1);
            }

            foreach (var targetDirectory in config.TargetDirectories)
            {
                string directoryPath = Path.Combine(sourceDirectory, targetDirectory.FolderName);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }

            // Misc folder, where all unknown filetypes goes.
            string miscDirectoryPath = Path.Combine(sourceDirectory, config.MiscDirectoryName);
            if (!Directory.Exists(miscDirectoryPath))
            {
                Directory.CreateDirectory(miscDirectoryPath);
            }


            var files = Directory.EnumerateFiles(sourceDirectory);

            foreach (string file in files)
            {
                string fileName = file[(sourceDirectory.Length + 1)..];
                Console.WriteLine(fileName);
                string fileExtention = file[(file.LastIndexOf('.') + 1)..];
                Console.WriteLine(fileExtention);
                string filePath = Path.Combine(sourceDirectory, fileName);
                Console.WriteLine(filePath);

                string targetDirectoryName;

                targetDirectoryName = config.TargetDirectories.Where(td => td.FileExtensions.Contains(fileExtention)).FirstOrDefault()?.FolderName;
                targetDirectoryName = (String.IsNullOrEmpty(targetDirectoryName)) ? config.MiscDirectoryName : targetDirectoryName;

                string targetPath = Path.Combine(sourceDirectory, targetDirectoryName, fileName);
                Console.WriteLine("Moving: " + filePath + " => to: " + targetPath);
                File.Move(filePath, targetPath);
            }

            foreach (var folder in Directory.EnumerateDirectories(sourceDirectory))
            {
                if (!Directory.EnumerateDirectories(folder).Any() && !Directory.EnumerateFiles(folder).Any())
                {
                    Console.WriteLine("Deleting folder: " + Path.Combine(sourceDirectory, folder));
                    Directory.Delete(Path.Combine(folderToOrganize, folder));
                }
            }

            TimeSpan elapsedTime = DateTime.Now - startTime;

            Console.WriteLine($"The organizer has finished the cleaning in {elapsedTime.Milliseconds} milliseconds..");
        }
    }
}
