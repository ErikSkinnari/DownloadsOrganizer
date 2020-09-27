using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Syroot.Windows.IO;

namespace DownloadsOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the organizer...");

            string folderToOrganize = String.Empty;

            if (args.Length > 0)
            {
                try
                {
                    folderToOrganize = args[0];
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Given path is not valid. Closing Application");
                    Environment.Exit(-1);
                }
            }
            else folderToOrganize = new KnownFolder(KnownFolderType.Downloads).Path;

            Console.WriteLine(folderToOrganize);

            Dictionary<string, string> foldersToCreate = new Dictionary<string, string>
            {
                { "ExecutablesPath", Path.Combine(folderToOrganize, "Executables") },
                { "EbooksPath", Path.Combine(folderToOrganize, "E-books") },
                { "SoundsPath", Path.Combine(folderToOrganize, "Sounds") },
                { "DocumentsPath", Path.Combine(folderToOrganize, "Documents") },
                { "ImagesPath", Path.Combine(folderToOrganize, "Images") },
                { "CompressedPath", Path.Combine(folderToOrganize, "Compressed") },
                { "CodePath", Path.Combine(folderToOrganize, "Code") },
                { "TextPath", Path.Combine(folderToOrganize, "TextFiles") },
                { "Misc", Path.Combine(folderToOrganize, "Misc") }
            };

            string[] exeExtentions = new string[] { "exe", "jar" };
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
                Console.WriteLine(filename);

                string fileExtention = file[(file.LastIndexOf('.') + 1)..];

                Console.WriteLine(fileExtention);

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
            }

            Console.ReadLine();
        }
    }
}
