using System;
using System.IO;
using System.Text;

namespace TekkenInputMethod.Core
{
    public static class Logger
    {
        private static string LogDirectory {
            get {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(appDataPath, "TekkenInputMethod", "Logs");
            }
        }
        
        private static string LogFilePath {
            get {
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                return Path.Combine(LogDirectory, $"log_{date}.txt");
            }
        }
        
        static Logger()
        {
            // 确保日志目录存在
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
        }
        
        public static void Info(string message)
        {
            WriteLog("INFO", message);
        }
        
        public static void Warning(string message)
        {
            WriteLog("WARNING", message);
        }
        
        public static void Error(string message, Exception ex = null)
        {
            string fullMessage = message;
            if (ex != null)
            {
                fullMessage += $"\nException: {ex.Message}\nStackTrace: {ex.StackTrace}";
            }
            WriteLog("ERROR", fullMessage);
        }
        
        public static void Debug(string message)
        {
            WriteLog("DEBUG", message);
        }
        
        private static void WriteLog(string level, string message)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string logEntry = $"[{timestamp}] [{level}] {message}";
                
                using (StreamWriter writer = new StreamWriter(LogFilePath, true, Encoding.UTF8))
                {
                    writer.WriteLine(logEntry);
                }
                
                // 同时输出到控制台
                Console.WriteLine(logEntry);
            }
            catch (Exception ex)
            {
                // 如果日志写入失败，至少输出到控制台
                Console.WriteLine($"Error writing log: {ex.Message}");
            }
        }
        
        public static void CleanOldLogs(int daysToKeep = 7)
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                    return;
                
                DateTime cutoffDate = DateTime.Now.AddDays(-daysToKeep);
                
                foreach (string file in Directory.GetFiles(LogDirectory, "log_*.txt"))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (fileInfo.LastWriteTime < cutoffDate)
                    {
                        fileInfo.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cleaning old logs: {ex.Message}");
            }
        }
        
        public static string GetLogPath()
        {
            return LogFilePath;
        }
    }
}
