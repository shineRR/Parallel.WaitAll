using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogParallel;

namespace LogParallel
{
    public class Parallel
    {
        private LogBuffer _logBuffer;
        public static TaskQueue _taskQueue;

        public Parallel(LogBuffer logBuffer, TaskQueue taskQueue)
        {
            _logBuffer = logBuffer;
            _taskQueue = taskQueue;
        }

        public static void WaitAll(TaskDelegate[] taskDelegates)
        {
            AutoResetEvent mre = new AutoResetEvent(false);
            int taskCounter = taskDelegates.Length;
            foreach (var func in taskDelegates)
            {
                _taskQueue.EnqueueTask(delegate
                {
                    func();
                    DecrementCoounter(ref taskCounter, mre);
                });
            }
            mre.WaitOne();
        }

        private static void DecrementCoounter(ref int counter, AutoResetEvent mre)
        {
            if (Interlocked.Decrement(ref counter) == 0)
            {
                _taskQueue.Dispose();
                mre.Set();
            }
        }
    }
}