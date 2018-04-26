using System;
using System.IO;

namespace UnlocLoader.Core
{
    public class Workspace : IDisposable
    {
        public string WorkingFolder { get; }

        public Workspace()
        {
            WorkingFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(WorkingFolder);
        }

        public void Dispose()
        {
            Directory.Delete(WorkingFolder, true);
        }
    }
}