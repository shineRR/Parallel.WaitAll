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
            int _lockVar = 1;
            int _unlockVar = 0;
            Console.WriteLine(Interlocked.CompareExchange(ref _lockVar, 4, _lockVar));
        //     if (args.Count() != 3)
        //     {
        //         Console.WriteLine("Not enough arguments.");
        //         return;
        //     }
        //     string src = args[0];
        //     string dest = args[1];
        //     int threads;
        //     int.TryParse(args[2], out threads);
        //   
        //     if ((!Directory.Exists(src) || !Directory.Exists(Directory.GetDirectoryRoot(dest)) || 
        //          src == dest) || threads < 1) return;
        //     
        //     LogBuffer logBuffer = new LogBuffer(2000, 50);
        //     FileCopyService fileCopyService = new FileCopyService(logBuffer, threads);
        //     fileCopyService.StartCopying(src, dest);
        }
    }
}