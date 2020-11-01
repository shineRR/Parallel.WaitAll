using System;
using System.Threading.Tasks;

namespace LogParallel
{
    public delegate void TaskDelegate();
    public interface ITaskQueue : IDisposable
    {
        void EnqueueTask(TaskDelegate task);
    }
}