using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using LogParallel;

namespace LogParallel
{
    public class FileCopyService : IFileCopyService
    {
        private static int _copiedFiles = 0;
        private LogBuffer _logBuffer;
        private int _threads;

        public FileCopyService(LogBuffer logBuffer, int threads)
        {
            _logBuffer = logBuffer;
            _threads = threads;
        }

        private static List<string> GetDirs(string path)
        {
            string searchPattern = "*.*";
            List<string> list = new List<string>();
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] directories =
                di.GetDirectories(searchPattern, SearchOption.AllDirectories);

            foreach (var dir in directories)
            {
                list.Add(dir.FullName);
            }
            return list;
        } 
        
        private static List<string> GetFiles(string path)
        {
            string searchPattern = "*.*";
            List<string> list = new List<string>();
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files =
                di.GetFiles(searchPattern, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                list.Add(file.FullName);
            }
            return list;
        }

        private static void PrintCopiedFiles()
        {
            Console.WriteLine("Total Copied: " + _copiedFiles);
        }
        public void CreateMissingDirs(string src, string dest)
        {
            foreach (var dir in GetDirs(src))
            {
                try
                {
                    Directory.CreateDirectory(dir.Replace(src, dest));

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                } 
            }
        }

        public void StartCopying(string src, string dest)
        {
            Parallel parallel = new Parallel(_logBuffer, new TaskQueue(_threads));
            List<TaskDelegate> taskDelegates = new List<TaskDelegate>();
            List<String> dirFileList = GetFiles(src);
            FileAttributes attr = File.GetAttributes(dest);
            
            if (!attr.HasFlag(FileAttributes.Directory)) return;
            Directory.CreateDirectory(dest);
            CreateMissingDirs(src, dest);
            
            
            foreach (var file in dirFileList)
            {
                string fileName = file.Replace(src, dest);

                taskDelegates.Add(delegate {
                    try
                    {
                        File.Copy(file, fileName, true);
                        Interlocked.Increment(ref _copiedFiles);
                        _logBuffer.Add("from: " + file + ", to: " + fileName + " by " + Thread.CurrentThread.Name);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
                
            }
            Parallel.WaitAll(taskDelegates.ToArray());
     
            PrintCopiedFiles();
        }
    }
}