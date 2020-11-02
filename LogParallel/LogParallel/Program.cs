using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogParallel
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Count() != 2)
            { 
                Console.WriteLine("Not enough arguments.");
                return;
            }
            string src = args[0];
            string dest = args[1];

            if ((!Directory.Exists(src) || !Directory.Exists(Directory.GetDirectoryRoot(dest)) || 
                 src == dest)) return;
            LogBuffer logBuffer = new LogBuffer(300, 64);
            FileCopyService fileCopyService = new FileCopyService(logBuffer);
            fileCopyService.StartCopying(src, dest);
        }
    }
}