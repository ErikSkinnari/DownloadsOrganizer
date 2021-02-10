using System.Collections.Generic;

namespace DirectoryOrganizer
{
    public class Configuration
    {
        public string SourceDirectory { get; set; }
        public List<TargetDirectory> TargetDirectories { get; set; }
        public string MiscDirectoryName { get; set; }
    }

    public class TargetDirectory
    {
        public string FolderName { get; set; }
        public List<string> FileExtensions { get; set; }
    }
}