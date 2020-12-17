using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        class Logger
        {
            FileSystemWatcher Watcher;
            private string _file;
            private bool _enabled = true;
            string sourceDir = @"C:\Users\User\source\SourceDir";
            string targetDir = @"C:\Users\User\source\TargetDir";
            string dearchiveDir = @"C:\Users\User\source\DearchiveDir";
            public Logger()
            {


                Watcher = new FileSystemWatcher(sourceDir);
                Watcher.IncludeSubdirectories = true;
                Watcher.EnableRaisingEvents = true;
                Watcher.Created += FSW_Created;
                Watcher.Deleted += FSW_Deleted;
                Watcher.Renamed += FSW_Renamed;
                Watcher.Changed += FSW_Changed;
            }
            private void FSW_Deleted(object sender, System.IO.FileSystemEventArgs e)
            {
                Logger.LogTxt("Deleted file " + e.Name + "\r\n");
            }

            private void FSW_Renamed(object sender, System.IO.RenamedEventArgs e)
            {
                Logger.LogTxt("old name:" + e.OldName + "\r\n to new:" + e.Name + "\r\n");
            }
            private void FSW_Created(object sender, System.IO.FileSystemEventArgs e)
            {
                Logger.LogTxt("Added new file " + e.Name + "\r\n");
                _file = Path.GetFileNameWithoutExtension(e.FullPath);
                string newfile = targetDir + $@"\{_file}.gz";
                Compress(e.FullPath, newfile);
                _file = Path.GetFileNameWithoutExtension(newfile);
                string decomprFile = dearchiveDir + $@"\{_file}.txt";
                Decompress(newfile, decomprFile);

            }
            private void FSW_Changed(object sender, FileSystemEventArgs e)
            {
                Logger.LogTxt("file changed " + e.Name + "\r\n");
                _file = Path.GetFileNameWithoutExtension(e.FullPath);
                string newfile = targetDir + $@"\{_file}.gz";
                Compress(e.FullPath, newfile);
                _file = Path.GetFileNameWithoutExtension(newfile);
                string decomprFile = dearchiveDir + $@"\{_file}.txt";
                Decompress(newfile, decomprFile);
            }

            public void Start()
            {
                Watcher.EnableRaisingEvents = true;
                while (_enabled)
                {
                    Thread.Sleep(1000);
                }
            }
            public void Stop()
            {
                Watcher.EnableRaisingEvents = false;
                _enabled = false;
            }
            public static void LogTxt(string Log)
            {
                using (StreamWriter writer = new StreamWriter((@"D:\temp.txt"), true))
                {
                    writer.WriteLine(Log);
                    writer.Flush();
                }
            }

        }
        public static void Compress(string sourceFile, string compressedFile)
        {
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open))
            {
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        public static void Decompress(string compressedFile, string targetFile)
        {
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.Open))
            {
                using (FileStream targetStream = File.Create(targetFile))
                {
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                    }
                }
            }
        }
    }

}
