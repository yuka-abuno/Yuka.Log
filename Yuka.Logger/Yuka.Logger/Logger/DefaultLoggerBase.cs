using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yuka.Logger.Logger
{
    /// <summary>
    ///     DefaultLoggerBase
    /// </summary>
    public abstract class DefaultLoggerBase : ILogger, IDisposable
    {
        private readonly object _lockobj = new object();

        /// <summary>LogWriters</summary>
        public abstract IList<ILogWriter> LogWriters { get; }


        /// <summary>MinLogLevel</summary>
        public abstract LogLevel MinLogLevel { get; set; }


        /// <summary>MaxLogLevel</summary>
        public abstract LogLevel MaxLogLevel { get; set; }


        /// <summary>
        ///     Create Trace Log
        /// </summary>
        /// <param name="messageFunc">Message Func</param>
        /// <returns></returns>
        public void Trace(Func<string> messageFunc) => Logging(messageFunc, LogLevel.Trace);
        /// <summary>
        ///     Create Debug Log
        /// </summary>
        /// <param name="messageFunc">Message Func</param>
        /// <returns></returns>
        public void Debug(Func<string> messageFunc) => Logging(messageFunc, LogLevel.Debug);
        /// <summary>
        /// Creaate Info Log
        /// </summary>
        /// <param name="messageFunc"></param>
        /// <returns></returns>
        public void Info(Func<string> messageFunc) => Logging(messageFunc, LogLevel.Info);
        /// <summary>
        ///     Create Warn Log
        /// </summary>
        /// <param name="messageFunc"></param>
        /// <returns></returns>
        public void Warning(Func<string> messageFunc) => Logging(messageFunc, LogLevel.Warnning);
        /// <summary>
        /// Create Error Log
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public void Error(Exception ex)
        {
            var timestamp = DateTime.Now;
            if (!ShouldWrite(LogLevel.Error))
            {
                return;
            }
            lock (_lockobj)
            {
                Writelog(CreateMessage(timestamp, ExceptionToMessage(ex), LogLevel.Error));
            }
        }
        /// <summary>
        ///     Create Error Log
        /// </summary>
        /// <param name="messageFunc"></param>
        /// <returns></returns>
        public void Error(Func<string> messageFunc) => Logging(messageFunc, LogLevel.Error);

        /// <summary>
        ///     Create Messafe
        /// </summary>
        /// <param name="time"></param>
        /// <param name="messagebody"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        protected virtual string CreateMessage(DateTime time, string messagebody, LogLevel level)
        {
            return $"{level.ToString().PadRight(10)}{GetTimeStamp(time)} {messagebody}";
        }

        /// <summary>
        ///     Exception Create Message
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected virtual string ExceptionToMessage(Exception ex) => ex.ToString();

        /// <summary>
        ///     Write Log
        /// </summary>
        /// <param name="logMessage"></param>
        protected virtual void Writelog(string logMessage)
        {
            if (LogWriters == null || LogWriters.Count == 0) return;

            Task.WaitAll(
                LogWriters
                    .Where(x => x != null)
                    .ToArray().Select(x =>
                    {
                        return Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                x.WriteLog(logMessage);
                            }
                            catch
                            {
                                LogWriters.Remove(x);
                                (x as IDisposable)?.Dispose();
                            }
                        });
                    }).ToArray()
                );
        }

        private void Logging(Func<string> messageFunc, LogLevel level)
        {
            var timestamp = DateTime.Now;
            if (!ShouldWrite(level))
            {
                return;
            }
            lock (_lockobj)
            {
                try
                {
                    Writelog(CreateMessage(timestamp, messageFunc?.Invoke(), level));
                }
                catch
                {
                    // ignored
                }
            }
        }
        private bool ShouldWrite(LogLevel level)
        {
            if (_disposedValue || LogWriters == null || !LogWriters.Any())
            {
                return false;
            }
            return level >= MinLogLevel && level <= MaxLogLevel;
        }

        private static string GetTimeStamp(DateTime time)
        {
            return time.ToString("yyyy/MM/dd HH:mm:ss.fff");
        }

        #region IDisposable Support

        private bool _disposedValue;

        /// <summary>
        ///     Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (LogWriters == null || LogWriters.Count == 0)
                        return;

                    foreach (var logger in LogWriters)
                    {
                        (logger as IDisposable)?.Dispose();
                    }
                }


                _disposedValue = true;
            }
        }

        /// <summary>
        ///     Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}