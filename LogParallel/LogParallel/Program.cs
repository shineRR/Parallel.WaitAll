using LogParallel.Log;

namespace LogParallel
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            LogBuffer logBuffer = new LogBuffer(32);
            for (int i = 0; i < 10; ++i)
            {
                logBuffer.Add("LOGGGING");
            }
        }
    }
}