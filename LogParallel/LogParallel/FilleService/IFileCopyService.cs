﻿namespace LogParallel
{
    public interface IFileCopyService
    {
        void CreateMissingDirs(string src, string dest);
        void StartCopying(string src, string path);
    }
}