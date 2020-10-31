namespace LogParallel
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            LogBuffer.LogBuffer logBuffer = new LogBuffer.LogBuffer(10, 56);
            
            for (int i = 0; i < 123123; ++i)
            { 
                logBuffer.Add(i.ToString());
            }
            
            while (true)
            {
            }
        }
    }
}