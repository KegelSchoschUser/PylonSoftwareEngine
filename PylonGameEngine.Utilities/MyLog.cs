using System;
using System.IO;
using System.Text;
using System.Threading;

namespace PylonGameEngine.Utilities
{
    public class MyLog
    {
        public static MyLog Default;
        public bool Initialized { get; private set; }

        private const int LongestEnum = 8;
        private Stream stream;
        private StreamWriter streamWriter;

        public bool WriteInConsole = true;
        public bool FireEvents = true;
        public string Output { get; private set; }
        public string FolderPath { get; private set; }

        public delegate void OnWriteText(string Text);
        public event OnWriteText OnWrite;

        public MyLog(string folderPath, string appName, string appversion)
        {
            OnWrite += FAKE_OnWrite;
            FolderPath = folderPath + @"\" + "Logs";
            Init(FolderPath, appName, appversion);
        }

        private void FAKE_OnWrite(string Text)
        {

        }

        private void Init(string FolderPath, string appName, string appversion)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FolderPath + @"\" + GetLogFileName(appName)));
            stream = File.Open(FolderPath + @"\" + GetLogFileName(appName), FileMode.Create, FileAccess.Write, FileShare.Read);
            streamWriter = new StreamWriter(stream, new UTF8Encoding(false, false));
            Initialized = true;
            Write("Log Started");
            Write(string.Format("Timezone (local - UTC): {0}h", GetLocalOffset()));
            Write("App Name: " + appName);
            Write("App Version: " + appversion);
        }

        public void Close()
        {
            Write("Log Closed");
            stream = null;
            streamWriter = null;
        }


        private static string GetLogFileName(string appName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(appName);
            sb.Append("_");
            sb.Append(GetDateTimeForFilename(DateTime.Now));
            sb.Append(".pylog");

            return sb.ToString();
        }

        public void Write(LogSeverity severity = LogSeverity.Info, bool newLine = true)
        {
            WriteInternal("", severity, newLine);
        }

        public void Write(string message, LogSeverity severity = LogSeverity.Info, bool newLine = true)
        {
            WriteInternal(message, severity, newLine);
        }

        public void Write(object message, LogSeverity severity = LogSeverity.Info, bool newLine = true)
        {
            WriteInternal(message.ToString(), severity, newLine);
        }


        public void Write(Exception exception, LogSeverity severity = LogSeverity.Error, bool newLine = true)
        {
            WriteInternal(exception.ToString(), severity, newLine);
        }

        private void WriteInternal(string text, LogSeverity severity, bool newLine)
        {
            if (!Initialized)
            {
                throw new MyExceptions.LogNotInitializedException();
            }

            StringBuilder sb = new StringBuilder();
            int prefixlength = AppendPrefix(sb, severity);
            sb.Append(text);

            string newlinecorrection = sb.ToString().Replace("\n", "\n" + new string(' ', prefixlength - 4 + 4) + "->  ");
            sb.Clear();
            sb.Append(newlinecorrection);

            if (newLine)
                sb.Append('\n');
            WriteToStream(sb.ToString());
            if (WriteInConsole)
            {
                switch (severity)
                {
                    case (LogSeverity.Info):
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            break;
                        }
                    case (LogSeverity.Warning):
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            break;
                        }
                    case (LogSeverity.Error):
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            break;
                        }
                    case (LogSeverity.Critical):
                        {
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            break;
                        }
                    case (LogSeverity.Crash):
                        {
                            Console.BackgroundColor = ConsoleColor.Magenta;
                            break;
                        }
                    default:
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            break;
                        }
                }

                Console.Write("   ");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" ");

                Console.Write(sb.ToString());
            }

            if (FireEvents)
            {
                OnWrite(sb.ToString());
            }

            Output += sb.ToString();

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private int AppendPrefix(StringBuilder sb, LogSeverity severity)
        {
            AppendDateTime(sb, severity);
            AppendThreadId(sb);
            AppendSeverity(sb, severity);
            sb.Append("] ->  ");
            return sb.ToString().Length;
        }

        private void AppendDateTime(StringBuilder sb, LogSeverity severity)
        {
            DateTime dt = DateTime.Now;
            sb.Append(dt.Day.ToString("00")).Append('.');
            sb.Append(dt.Month.ToString("00")).Append('.');
            sb.Append(dt.Year).Append(' ');
            sb.Append(dt.Hour.ToString("00")).Append(':');
            sb.Append(dt.Minute.ToString("00")).Append(':');
            sb.Append(dt.Second.ToString("00")).Append('.');
            sb.Append(dt.Millisecond.ToString("000")).Append(' ');
        }

        private void AppendThreadId(StringBuilder sb)
        {
            sb.Append(" - Thread: [");
            sb.Append(Thread.CurrentThread.ManagedThreadId.ToString("000"));
            sb.Append("] ");
        }

        private void AppendSeverity(StringBuilder sb, LogSeverity severity)
        {
            sb.Append('[');

            string SeverityName;
            switch (severity)
            {
                case (LogSeverity.Info):
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        SeverityName = "Info";
                        break;
                    }
                case (LogSeverity.Warning):
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        SeverityName = "Warning";
                        break;
                    }
                case (LogSeverity.Error):
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        SeverityName = "Error";
                        break;
                    }
                case (LogSeverity.Critical):
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        SeverityName = "Critical";
                        break;
                    }
                case (LogSeverity.Crash):
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        SeverityName = "Crash";
                        break;
                    }
                default:
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        SeverityName = "NONE";
                        break;
                    }
            }

            string prefix = new string(' ', (int)System.Math.Floor((LongestEnum - SeverityName.Length) / 2f));
            string prefix_severity = prefix + SeverityName;
            string prefix_severity_suffix = prefix_severity + new string(' ', LongestEnum - prefix_severity.Length);
            sb.Append(prefix_severity_suffix);
        }

        private void WriteToStream(string text)
        {
            if (stream == null || streamWriter == null)
            {
                throw new Exception("Log not initialized yet!");
            }

            streamWriter.Write(text);
            streamWriter.Flush();
        }

        public static string GetDateTimeForFilename(DateTime dt, bool ms = true)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(dt.Year);
            sb.Append(dt.Month.ToString("00"));
            sb.Append(dt.Day.ToString("00"));
            sb.Append("-");
            sb.Append(dt.Hour.ToString("00"));
            sb.Append(dt.Minute.ToString("00"));
            if (ms)
            {
                sb.Append(dt.Second.ToString("00")).Append("_").Append(dt.Millisecond.ToString("000"));
            }
            else
            {
                sb.Append(dt.Second.ToString("00"));
            }

            return sb.ToString();

        }

        public static int GetLocalOffset()
        {
            return (int)System.Math.Round((DateTime.Now - DateTime.UtcNow).TotalHours);
        }
    }
}
