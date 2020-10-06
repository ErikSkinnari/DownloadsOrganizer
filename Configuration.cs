using System;
using System.Collections.Generic;

namespace DirectoryOrganizer
{
    public class Configuration
    {
        public string SourceDirectory { get; set; }
        public List<TargetDirectory> TargetDirectories { get; set; }
    }


    public class TargetDirectory
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fileextensions")]
        public IList<string> FileExtensions { get; set; }
    }
}