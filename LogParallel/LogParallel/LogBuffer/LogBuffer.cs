using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace LogParallel
{
    public class LogBuffer: ILogBuffer
    {
        private static List<string> _messages = new List<string>();
        private readonly int _maxMessages;
        private readonly object _obj = new object();
        private static readonly string _path = "C:\\Users\\shine\\Desktop\\Dev\\Parallel.WaitAll\\LogParallel\\LogParallel\\log\\log.txt";
        private Timer _timer = null;

        public LogBuffer(int timerInterval, int maxMessages)
        {
            _maxMessages = maxMessages;
            CreateFileForLogs();
            ConfigureTimer(timerInterval, true);
        }

        ~LogBuffer()
        {
            ClearBuffer(true);
        }
        
        private void CreateFileForLogs()
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(_path, FileMode.Create);
            }
            catch
            {
                // ignored
            }
            finally
            {
                fs?.Close();
            }
        }

        private void ConfigureTimer(int timerInterval, bool allMessages)
        {
            _timer = new Timer(timerInterval);
            _timer.Elapsed += async(sender, e) => await ClearBuffer(allMessages);
            _timer.Enabled = true;
        }

        private Task ClearBuffer(bool auto)
        {
            var tempMessages = new List<string>();
            lock (_obj)
            {
                int count = auto || _messages.Count < _maxMessages ? _messages.Count : _maxMessages;
                tempMessages.AddRange(_messages.GetRange(0, count));
                _messages.RemoveRange(0, count);
            }
            lock (this)
            {
                using (var sw = File.AppendText(_path))
                {
                    foreach (var message in tempMessages)
                    {
                        sw.WriteLine(message);
                    }
                }
                Console.WriteLine("Logging " + tempMessages.Count + " messages");
            }

            return Task.CompletedTask;
        }

        private async void IsMaxSize()
        {
            if (_messages.Count > _maxMessages - 1)
                await ClearBuffer(false);
        }

        public void Add(string item)
        {
            lock (_obj)
            {
                _messages.Add("[LOG " + DateTime.Now + "] " + item);
                IsMaxSize();
            }
        }
    }
}