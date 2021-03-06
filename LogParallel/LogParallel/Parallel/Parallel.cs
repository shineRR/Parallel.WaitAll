﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogParallel;

namespace LogParallel
{
    public static class Parallel
    {
        private static TaskQueue _taskQueue = new TaskQueue(16);
        public static void WaitAll(TaskDelegate[] taskDelegates)
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);
            int taskCounter = taskDelegates.Length;
            foreach (var func in taskDelegates)
            {
                _taskQueue.EnqueueTask(delegate
                {
                    func();
                    DecrementCounter(ref taskCounter, autoResetEvent);
                });
            }
            autoResetEvent.WaitOne();
        }

        private static void DecrementCounter(ref int counter, AutoResetEvent autoResetEvent)
        {
            if (Interlocked.Decrement(ref counter) == 0)
            {
                _taskQueue.Dispose();
                autoResetEvent.Set();
            }
        }
    }
}